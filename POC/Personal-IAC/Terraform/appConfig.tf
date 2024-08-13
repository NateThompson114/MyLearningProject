resource "azurerm_app_configuration" "main" {
  name                = local.app_config_name
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = "standard"

  identity {
    type = "SystemAssigned"
  }

  tags = local.tags
}

resource "azurerm_role_assignment" "app_config_data_owner" {
  scope        = azurerm_app_configuration.main.id
  role_definition_name = "App Configuration Data Owner"
  principal_id = data.azurerm_client_config.current.object_id  
}

resource "azurerm_role_assignment" "app_config_data_reader" {
  scope        = azurerm_user_assigned_identity.global_reader.id
  principal_id = azurerm_role_assignment.app_config_data_owner.principal_id
  role_definition_name = "App Configuration Data Reader"
}

# resource "azurerm_app_configuration_key" "test" {
#   configuration_store_id = azurerm_app_configuration.main.id
#   key                    = "test"
#   value                  = "hello world"
# 
#   depends_on = [
#     azurerm_role_assignment.app_config_data_owner
#   ]
# }

# resource "azurerm_app_configuration_feature" "foo_bar" {
#   configuration_store_id = azurerm_app_configuration.main.id
#   name                   = "foo-bar"
#   description            = "Sample feature flag"
#   enabled                = true
# 
#   tags = local.tags
# }

resource "azurerm_app_configuration_key" "eventhub_name" {
  configuration_store_id = azurerm_app_configuration.main.id
  key                    = "EventHubName"
  value                  = local.event_hub_name
}

resource "azurerm_app_configuration_key" "eventhub_consumer_name" {
  configuration_store_id = azurerm_app_configuration.main.id
  key                    = "EventHubConsumerName"
  value                  = azurerm_eventhub_consumer_group.bssso.name
}

resource "azurerm_app_configuration_key" "eventhub_connection_string" {
  configuration_store_id = azurerm_app_configuration.main.id
  key                    = "EventHubConnectionString"
  vault_key_reference    = azurerm_key_vault_secret.eventhub_connection_string.versionless_id
  type                   = "vault"

  depends_on = [
    azurerm_key_vault.main,
    azurerm_key_vault_secret.eventhub_connection_string
  ]
}

resource "azurerm_app_configuration_key" "storage_container_name" {
  configuration_store_id = azurerm_app_configuration.main.id
  key                    = "StorageContainerName"
  value                  = azurerm_storage_container.main.name

  depends_on = [
    azurerm_storage_container.main
  ]
}

resource "azurerm_app_configuration_key" "storage_account_key" {
  configuration_store_id = azurerm_app_configuration.main.id
  key                    = "StorageAccountKey"
  vault_key_reference    = azurerm_key_vault_secret.storage_account_connection_string.versionless_id
  type                   = "vault"

  depends_on = [
    azurerm_key_vault.main,
    azurerm_key_vault_secret.storage_account_connection_string,
  ]
}