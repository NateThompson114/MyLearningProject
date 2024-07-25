data "azurerm_client_config" "current" {}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

locals {
  common_prefix                = "${var.resource_owner.abbr}-${var.region.abbr}"
  resource_group_name          = "${local.common_prefix}-RG"
  key_vault_name               = "${local.common_prefix}-KV"
  event_hub_namespace_name     = "${local.common_prefix}-EHUB-NS"
  event_hub_name               = "${local.common_prefix}-EHUB"
  app_config_name              = "${local.common_prefix}-APP-CONFIG"
  reader_managed_identity_name = "${local.common_prefix}-Reader-MI"
}

resource "azurerm_resource_group" "main" {
  name     = local.resource_group_name
  location = var.location
}

resource "azurerm_key_vault" "main" {
  name                        = local.key_vault_name
  location                    = azurerm_resource_group.main.location
  resource_group_name         = azurerm_resource_group.main.name
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  sku_name                    = "standard"
  purge_protection_enabled    = true
}

resource "azurerm_key_vault_access_policy" "current_user" {
  key_vault_id = azurerm_key_vault.main.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  secret_permissions = [
    "Get", "List", "Set", "Delete", "Recover", "Backup", "Restore"
  ]
}

resource "azurerm_eventhub_namespace" "main" {
  name                = local.event_hub_namespace_name
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "Standard"
}

resource "azurerm_eventhub" "main" {
  name                = local.event_hub_name
  namespace_name      = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
  partition_count     = 2
  message_retention   = 1
}

data "azurerm_eventhub_namespace" "main" {
  name                = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
}

resource "azurerm_key_vault_secret" "eventhub_connection_string" {
  name         = "EventHubConnectionString"
  value        = data.azurerm_eventhub_namespace.main.default_primary_connection_string
  key_vault_id = azurerm_key_vault.main.id
}

resource "azurerm_app_configuration" "main" {
  name                = local.app_config_name
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = "standard"
}

resource "azurerm_user_assigned_identity" "main" {
  name                = local.reader_managed_identity_name
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
}

resource "azurerm_key_vault_access_policy" "managed_identity" {
  key_vault_id = azurerm_key_vault.main.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_user_assigned_identity.main.principal_id

  secret_permissions = [
    "Get",
  ]
}

resource "azurerm_role_assignment" "main" {
  principal_id         = azurerm_user_assigned_identity.main.principal_id
  role_definition_name = "Reader"
  scope                = azurerm_key_vault.main.id
}

resource "azurerm_app_configuration_key" "eventhub_connection_string" {
  configuration_store_id = azurerm_app_configuration.main.id
  key                    = "EventHubConnectionString"
  value                  = "@Microsoft.KeyVault(VaultName=${azurerm_key_vault.main.name};SecretName=${azurerm_key_vault_secret.eventhub_connection_string.name})"
  content_type           = "text/plain"
  tags = {
    Environment = var.environment
  }

  depends_on = [azurerm_role_assignment.main, azurerm_key_vault_access_policy.managed_identity]
}
