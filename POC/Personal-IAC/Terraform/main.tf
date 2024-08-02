data "azurerm_client_config" "current" {}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

locals {
  common_prefix                = "${var.resource_owner.abbr}-${var.region.abbr}-${var.project}"
  compact_common_prefix        = lower("${var.resource_owner.abbr}${var.region.abbr}lp")
  resource_group_name          = lower("${local.common_prefix}-RG")
  key_vault_name               = lower("${local.common_prefix}-KV")
  event_hub_namespace_name     = lower("${local.common_prefix}-EHUB-NS")
  event_hub_name               = lower("${local.common_prefix}-EHUB")
  app_config_name              = lower("${local.common_prefix}-APP-CONFIG")
  reader_managed_identity_name = lower("${local.common_prefix}-Reader-MI")
  storage_account_name         = lower("${local.compact_common_prefix}stgacct")
  tags = {
    Environment = var.environment
    ResourceOwner = var.resource_owner.value
  }
}

resource "azurerm_resource_group" "main" {
  name     = local.resource_group_name
  location = var.location

  tags = local.tags
}

resource "azurerm_user_assigned_identity" "global_reader" {
  name                = local.reader_managed_identity_name
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  
  tags = local.tags
}


