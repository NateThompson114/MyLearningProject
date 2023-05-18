-- terraform init
-- terraform plan
-- terraform apply
-- THIS IS NOT PROD READY

provider "azurerm" {
  features {}
}

data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "rg" {
  name     = "example-resources"
  location = "West Europe"
}

resource "azurerm_storage_account" "sa" {
  name                     = "examplestorageacc"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "GRS"
}

resource "azurerm_storage_container" "sc" {
  name                  = "examplecontainer"
  storage_account_name  = azurerm_storage_account.sa.name
  container_access_type = "blob"
}

resource "azurerm_key_vault" "kv" {
  name                        = "examplevault"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"
}

resource "azurerm_key_vault_key" "key" {
  name         = "example-key"
  key_vault_id = azurerm_key_vault.kv.id
  key_type     = "RSA"
  key_size     = 2048

  key_opts = [
    "decrypt",
    "encrypt",
    "sign",
    "unwrapKey",
    "verify",
    "wrapKey",
  ]
}

resource "azurerm_storage_account_customer_managed_key" "cmk" {
  storage_account_id = azurerm_storage_account.sa.id
  key_vault_id       = azurerm_key_vault.kv.id
  key_name           = azurerm_key_vault_key.key.name
  key_version        = azurerm_key_vault_key.key.version
}

resource "azurerm_application_insights" "ai" {
  name                = "example-appinsights"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
}

-- MANAGED IDENTITY

resource "azurerm_user_assigned_identity" "qaautomation" {
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  name                = "qaautomation_mid"
}

-- MID ASSIGNMENTS

data "azurerm_role_definition" "appinsights_reader" {
  name = "Monitoring Reader"
}

data "azurerm_role_definition" "storage_blob_data_reader" {
  name = "Storage Blob Data Reader"
}

resource "azurerm_role_assignment" "appinsights_role_assignment" {
  scope                = azurerm_application_insights.ai.id
  role_definition_name = data.azurerm_role_definition.appinsights_reader.name
  principal_id         = azurerm_user_assigned_identity.qaautomation.principal_id
}

resource "azurerm_role_assignment" "storage_blob_role_assignment" {
  scope                = azurerm_storage_account.sa.id
  role_definition_name = data.azurerm_role_definition.storage_blob_data_reader.name
  principal_id         = azurerm_user_assigned_identity.qaautomation.principal_id
}