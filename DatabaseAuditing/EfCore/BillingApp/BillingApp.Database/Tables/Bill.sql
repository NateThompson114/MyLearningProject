﻿CREATE TABLE [dbo].[Bill]
(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
    CustomerName NVARCHAR(100) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    BillingDate DATETIME2 NOT NULL
)