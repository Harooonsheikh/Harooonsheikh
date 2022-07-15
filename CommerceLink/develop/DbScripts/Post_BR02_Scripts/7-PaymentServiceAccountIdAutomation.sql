-- Create new table PaymentConnector
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PaymentConnector](
	[PaymentConnectorId] [int] NOT NULL,
	[PaymentConnectorName] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_PaymentConnector] PRIMARY KEY CLUSTERED 
(
	[PaymentConnectorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Add data for new table PaymentConnector
INSERT [dbo].[PaymentConnector] ([PaymentConnectorId], [PaymentConnectorName], [IsActive]) VALUES (1, N'TestConnector', 1)
GO
INSERT [dbo].[PaymentConnector] ([PaymentConnectorId], [PaymentConnectorName], [IsActive]) VALUES (2, N'MasterCard Simplify Connector', 1)
GO
INSERT [dbo].[PaymentConnector] ([PaymentConnectorId], [PaymentConnectorName], [IsActive]) VALUES (3, N'PayPalPaymentProcessor', 1)
GO
INSERT [dbo].[PaymentConnector] ([PaymentConnectorId], [PaymentConnectorName], [IsActive]) VALUES (4, N'WirecardPaymentProcessor', 1)
GO
INSERT [dbo].[PaymentConnector] ([PaymentConnectorId], [PaymentConnectorName], [IsActive]) VALUES (5, N'VerifoneConnector', 1)
GO

-- Add new column in PaymentMethod table
ALTER TABLE [dbo].[PaymentMethod] ADD PaymentConnectorId int NULL;
GO

-- Set values for new column in PaymentMethod table
update PaymentMethod set PaymentConnectorId = 4 where ECommerceValue = 'BASIC_CREDIT'
GO
update PaymentMethod set PaymentConnectorId = 3 where ECommerceValue = 'PAYPAL_EXPRESS'
GO

