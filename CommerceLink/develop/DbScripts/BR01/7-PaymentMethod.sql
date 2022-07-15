
DROP TABLE  [dbo].[PaymentMethod]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 1/15/2018 6:05:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[PaymentMethodId] [int] IDENTITY(1,1) NOT NULL,
	[ParentPaymentMethodId] [int] NULL,
	[ECommerceValue] [varchar](100) NULL,
	[ErpValue] [varchar](100) NULL,
	[HasSubMethod] [bit] NULL CONSTRAINT [DF_PaymentMethod_HasSubMethod]  DEFAULT ((0)),
	[ErpCode] [varchar](100) NULL,
	[IsPrepayment] [bit] NOT NULL,
	[IsCreditCard] [bit] NULL,
	[UsePaymentConnector] [bit] NULL,
	[ServiceAccountId] [nvarchar](50) NULL,
 CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
(
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO


-------------------------------------
GO
SET IDENTITY_INSERT [dbo].[PaymentMethod] ON 

INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (1, NULL, N'BASIC_CREDIT', N'3', 1, N'CreditCard', 0, 1, 1, N'5e2ee49f-06a3-492c-b777-cf8218f0a767')
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (2, 1, N'AE', N'3', 0, N'AMEX', 0, 1, 1, NULL)
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (3, 1, N'VISA', N'3', 0, N'Visa', 0, 1, 1, NULL)
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (4, 1, N'MC', N'3', 0, N'MasterCard', 0, 1, 1, NULL)
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (5, 1, N'DI', N'3', 0, N'Discover', 0, 1, 1, NULL)
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (6, NULL, N'PAYPAL_EXPRESS', N'50', 0, N'Visa', 0, 1, 1, N'41e1a80a-dee3-49c5-b4bc-a1ec9d1af4d4')
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (7, NULL, N'PURCHASEORDER', N'4', 0, N'Invoice', 0, 0, 0, NULL)
INSERT [dbo].[PaymentMethod] ([PaymentMethodId], [ParentPaymentMethodId], [ECommerceValue], [ErpValue], [HasSubMethod], [ErpCode], [IsPrepayment], [IsCreditCard], [UsePaymentConnector], [ServiceAccountId]) VALUES (9, NULL, N'TESTCONNECTOR', N'3', 1, N'CrediCard', 0, 1, 1, N'5e2ee49f-06a3-492c-b777-cf8218f0a767')
SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF





