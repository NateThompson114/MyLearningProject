resource "azurerm_eventhub_namespace" "main" {
  name                = local.event_hub_namespace_name
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "Standard"

  tags = local.tags
}

resource "azurerm_eventhub" "main" {
  name                = local.event_hub_name
  namespace_name      = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
  partition_count     = 2
  message_retention   = 1
}

resource "azurerm_eventhub_authorization_rule" "main" {
  name                = "RootManageSharedAccessKey"
  namespace_name      = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
  eventhub_name       = azurerm_eventhub.main.name

  listen              = true
  send                = true
  manage              = true
}

data "azurerm_eventhub_namespace" "main" {
  name                = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
}

data "azurerm_eventhub_authorization_rule" "main" {
  name                = azurerm_eventhub_authorization_rule.main.name
  namespace_name      = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name
  eventhub_name       = azurerm_eventhub.main.name
}

resource "azurerm_eventhub_consumer_group" "bssso" {
  name                = "BsssO"
  namespace_name      = azurerm_eventhub_namespace.main.name
  eventhub_name       = azurerm_eventhub.main.name
  resource_group_name = azurerm_resource_group.main.name

  tags = local.tags
}

resource "azurerm_eventhub_namespace_authorization_rule" "bssso_access_key" {
  name                = "BsssOAccessKey"
  namespace_name      = azurerm_eventhub_namespace.main.name
  resource_group_name = azurerm_resource_group.main.name

  listen = true
  send   = true

  tags = local.tags
}