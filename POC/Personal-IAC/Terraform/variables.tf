variable "location" {
  default = "East US"
}

variable "resource_owner" {
  type = object({
    value = string
    abbr  = string
  })
  default = {
    value = "Nathan Thompson"
    abbr  = "NT"
  }
}

variable "region" {
  type = object({
    value = string
    abbr  = string
  })
  default = {
    value = "Azure East"
    abbr  = "AE"
  }
}

variable "environment" {
  default = "Lowers"
}

variable "project" {
  default = "LearningProject"
}