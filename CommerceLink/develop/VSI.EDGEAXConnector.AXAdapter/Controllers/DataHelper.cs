//// Decompiled with JetBrains decompiler
//// Type: Microsoft.Dynamics.Commerce.Runtime.Data.SalesOrderDataManager
//// Assembly: Microsoft.Dynamics.Commerce.Runtime.DataManagers, Version=6.3.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
//// MVID: 5AAD47CE-1BE7-487F-8E53-08EB9AE00D8A
//// Assembly location: C:\Users\zubair.sharafat\Desktop\Commerce Run-time\References\Microsoft.Dynamics.Commerce.Runtime.DataManagers.dll

//using Microsoft.Dynamics.Commerce.Runtime;
//using Microsoft.Dynamics.Commerce.Runtime.Data.Types;
//using Microsoft.Dynamics.Commerce.Runtime.DataModel;
//using Microsoft.Dynamics.Commerce.Runtime.DataServices.Messages;
//using Microsoft.Dynamics.Commerce.Runtime.Helpers;
//using Microsoft.Dynamics.Commerce.Runtime.Messages;
////using Microsoft.Dynamics.Retail.Diagnostics;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Text;

//namespace Microsoft.Dynamics.Commerce.Runtime.Data
//{
//    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Unfortunately, a sales order does reference a multitude of types.")]
//    public class SalesOrderDataManager : DatabaseAccessor
//    {
//        private static readonly Decimal OrderLevelReasonCodeLineNumber = new Decimal(-1);
//        private static readonly Decimal OrderLevelChargeLineNumber = new Decimal(0);
//        protected const int TransactionIdLength = 44;
//        protected const string DataAreaIdColumn = "DATAAREAID";
//        protected const string StoreIdColumn = "STOREID";
//        protected const string TaxCodeColumn = "TAXCODE";
//        protected const string TerminalIdColumn = "TERMINALID";
//        protected const string TaxItemGroupColumn = "TAXITEMGROUP";
//        protected const string TransactionIdColumn = "TRANSACTIONID";
//        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Reviewed", MessageId = "Num")]
//        protected const string SaleLineNumColumn = "SALELINENUM";
//        private const string PurgeSalesTransactionsSpProcName = "PURGESALESONTERMINAL";
//        private const string RetailTransactionSalesTransView = "RETAILTRANSACTIONSALESTRANSVIEW";
//        private const string RetailTransactionPaymentTransView = "RETAILTRANSACTIONPAYMENTTRANSVIEW";
//        private const string RetailTransactionInfoCodeTransView = "RETAILTRANSACTIONINFOCODETRANSVIEW";
//        private const string RetailTransactionAttributeTransView = "RETAILTRANSACTIONATTRIBUTETRANSVIEW";
//        private const string RetailTransactionAddressTransView = "RETAILTRANSACTIONADDRESSTRANSVIEW";
//        private const string RetailTransactionAffiliationTransView = "RETAILTRANSACTIONAFFILIATIONTRANSVIEW";
//        private const string RetailTransactionMarkupTransView = "RETAILTRANSACTIONMARKUPTRANSVIEW";
//        private const string RetailTransactionOrderInvoiceTransView = "RETAILTRANSACTIONORDERINVOICETRANSVIEW";
//        private const string RetailTransactionIncomeExpenseTransView = "RETAILTRANSACTIONINCOMEEXPENSETRANSVIEW";
//        private const string SearchSalesOrdersFunctionTemplate = "GETSALESORDER(@nvc_CustomerName)";
//        private const string ReceiptMaskView = "RECEIPTMASKVIEW";
//        private const string ReceiptProfilesView = "RECEIPTPROFILESVIEW";
//        private const string ReceiptPrintersView = "RECEIPTPRINTERSVIEW";
//        private const string ReceiptInfoView = "RECEIPTINFOVIEW";
//        private const string TenderLinesView = "RETAILTRANSACTIONPAYMENTTRANSVIEW";
//        private const string TransactionPropertiesView = "TRANSACTIONPROPERTIESVIEW";
//        private const string StoreColumn = "STORE";
//        private const string TerminalColumn = "TERMINAL";
//        private const string SaleIdColumn = "SALESID";
//        private const string DeliverNameColumn = "DELIVERYNAME";
//        private const string ZipCodeColumn = "ZIPCODE";
//        private const string CountyRegionIdColumn = "COUNTRYREGIONID";
//        private const string StateColumn = "STATE";
//        private const string CityColumn = "CITY";
//        private const string CountyColumn = "COUNTY";
//        private const string StreetColumn = "STREET";
//        private const string EmailColumn = "EMAIL";
//        private const string EmailContentColumn = "EMAILCONTENT";
//        private const string PhoneColumn = "PHONE";
//        private const string StreetNumberColumn = "STREETNUMBER";
//        private const string DistrictNameColumn = "DISTRICTNAME";
//        private const string SalesNameColumn = "SALESNAME";
//        private const string TextValueColumn = "TEXTVALUE";
//        private const string NameColumn = "NAME";
//        private const string IsIncludedInPriceColumn = "ISINCLUDEDINPRICE";
//        private const string AmountColumn = "AMOUNT";
//        private const string CorrencyCodeColumn = "CORRENCYCODE";
//        private const string MarkUpCodeColumn = "MARKUPCODE";
//        private const string MarkUpLineNumColumn = "MARKUPLINENUM";
//        private const string TaxGroupColumn = "TAXGROUP";
//        private const string ValueColumn = "VALUE";
//        private const string CreatedDateTimeColumn = "CREATEDDATETIME";
//        private const string VariantIdColumn = "VARIANTID";
//        private const string UnitColumn = "UNIT";
//        private const string TaxAmountColumn = "TAXAMOUNT";
//        private const string ShippingDateRequestedColumn = "SHIPPINGDATEREQUESTED";
//        private const string ReceiptDateRequestedColumn = "RECEIPTDATEREQUESTED";
//        private const string QtyColumn = "QTY";
//        private const string PriceColumn = "PRICE";
//        private const string NetAmountColumn = "NETAMOUNT";
//        private const string NetAmountWithAllInclusiveTaxColumn = "NETAMOUNTINCLTAX";
//        private const string LogisticsPostalAddressColumn = "LOGISTICSPOSTALADDRESS";
//        private const string ListingIdColumn = "LISTINGID";
//        private const string LineNumColumn = "LINENUM";
//        private const string ItemIdColumn = "ITEMID";
//        private const string BarcodeColumn = "BARCODE";
//        private const string InventSiteIdColumn = "INVENTSITEID";
//        private const string InventSerialIdColumn = "INVENTSERIALID";
//        private const string InventLocationIdColumn = "INVENTLOCATIONID";
//        private const string InventDimIdColumn = "INVENTDIMID";
//        private const string DlvModeColumn = "DLVMODE";
//        private const string DiscAmountColumn = "DISCAMOUNT";
//        private const string CatalogIdColumn = "CATALOG";
//        private const string TotalDiscAmountColumn = "TOTALDISCAMOUNT";
//        private const string TotalDiscPctColumn = "TOTALDISCPCT";
//        private const string LineDscAmountColumn = "LINEDSCAMOUNT";
//        private const string LineManualDiscountAmountColumn = "LINEMANUALDISCOUNTAMOUNT";
//        private const string LineManualDiscountPercentageColumn = "LINEMANUALDISCOUNTPERCENTAGE";
//        private const string PeriodicDiscAmountColumn = "PERIODICDISCAMOUNT";
//        private const string PeriodicDiscPctColumn = "PERIODICPERCENTAGEDISCOUNT";
//        private const string ChannelReferenceIdColumn = "CHANNELREFERENCEID";
//        private const string ChannelColumn = "CHANNEL";
//        private const string TypeColumn = "TYPE";
//        private const string PeriodicDiscountOfferIdColumn = "PERIODICDISCOUNTOFFERID";
//        private const string DiscountCodeColumn = "DISCOUNTCODE";
//        private const string ChargeMethodColumn = "METHOD";
//        private const string CalculatedAmountColumn = "CALCULATEDAMOUNT";
//        private const string ReceiptIdColumn = "RECEIPTID";
//        private const string StaffColumn = "STAFF";
//        private const string StaffIdColumn = "STAFFID";
//        private const string TransTimeColumn = "TRANSTIME";
//        private const string TransDateColumn = "TRANSDATE";
//        private const string TransactionStatusColumn = "TRANSACTIONSTATUS";
//        private const string ReturnNoSaleColumn = "RETURNNOSALE";
//        private const string ReturnTransactionIdColumn = "RETURNTRANSACTIONID";
//        private const string ReturnLineNumberColumn = "RETURNLINENUM";
//        private const string ReturnStoreColumn = "RETURNSTORE";
//        private const string ReturnTerminalIdColumn = "RETURNTERMINALID";
//        private const string ReasonCodeIdColumn = "INFOCODEID";
//        private const string InformationColumn = "INFORMATION";
//        private const string InformationAmountColumn = "INFOAMOUNT";
//        private const string ItemTenderColumn = "ITEMTENDER";
//        private const string InputTypeColumn = "INPUTTYPE";
//        private const string SubReasonCodeIdColumn = "SUBINFOCODEID";
//        private const string StatementCodeColumn = "STATEMENTCODE";
//        private const string SourceCodeColumn = "SOURCECODE";
//        private const string SourceCode2Column = "SOURCECODE2";
//        private const string SourceCode3Column = "SOURCECODE3";
//        private const string ParentLineNumColumn = "PARENTLINENUM";
//        private const string OrderPlacedDateColumn = "ORDERPLACEDDATE";
//        private const string CommentColumn = "COMMENT";
//        private const string GiftCardColumn = "GIFTCARD";
//        private const string DiscountLineNumColumn = "LINENUM";
//        private const string DiscountOriginTypeColumn = "DISCOUNTORIGINTYPE";
//        private const string ManualDiscountTypeColumn = "MANUALDISCOUNTTYPE";
//        private const string CustomerDiscountTypeColumn = "CUSTOMERDISCOUNTTYPE";
//        private const string DealPriceColumn = "DEALPRICE";
//        private const string DiscountAmountColumn = "DISCOUNTAMOUNT";
//        private const string DiscountPercentageColumn = "PERCENTAGE";
//        private const string AffiliationColumn = "AFFILIATION";
//        private const string CardNumberColumn = "CARDNUMBER";
//        private const string CustAccountColumn = "CUSTACCOUNT";
//        private const string EntryDateColumn = "ENTRYDATE";
//        private const string EntryTimeColumn = "ENTRYTIME";
//        private const string EntryTypeColumn = "ENTRYTYPE";
//        private const string ExpirationDateColumn = "EXPIRATIONDATE";
//        private const string LoyaltyTierColumn = "LOYALTYTIER";
//        private const string RewardPointColumn = "REWARDPOINT";
//        private const string RewardPointAmountQtyColumn = "REWARDPOINTAMOUNTQTY";
//        private const string ReceiptEmailColumn = "RECEIPTEMAIL";
//        private const string IncomeExpenseAccountColumn = "INCOMEEXPENSEACCOUNT";
//        private const string AccountTypeColumn = "ACCOUNTTYPE";
//        private const string ElectronicDeliveryEmailColumn = "ELECTRONICDELIVERYEMAIL";
//        private const string ElectronicDeliveryEmailContentColumn = "ELECTRONICDELIVERYEMAILCONTENT";
//        private const string EntryStatusColumn = "ENTRYSTATUS";
//        private const string InvoiceIdColumn = "INVOICEID";
//        private const string InvoiceAmountColumn = "AMOUNTCUR";
//        private const string TaxAmountInclusiveColumn = "TAXAMOUNTINCLUSIVE";
//        private const string TaxAmountExclusiveColumn = "TAXAMOUNTEXCLUSIVE";
//        private readonly CustomerDataManager customerDataManager;

//        public SalesOrderDataManager(RequestContext context)
//            : base(DataStoreManager.DataStores[DataStoreType.Database], context)
//        {
//            this.customerDataManager = new CustomerDataManager(context);
//        }

//        public static void BuildSearchOrderWhereClause(SalesOrderSearchCriteria criteria, SqlPagedQuery query, IList<string> whereClauses)
//        {
//            ThrowIf.Null<SalesOrderSearchCriteria>(criteria, "criteria");
//            ThrowIf.Null<SqlPagedQuery>(query, "query");
//            ThrowIf.Null<IList<string>>(whereClauses, "whereClauses");
//            if (!string.IsNullOrEmpty(criteria.SalesId))
//            {
//                whereClauses.Add(string.Format("{0} = @saleId", (object)"SALESID"));
//                query.Parameters["@saleId"] = (object)criteria.SalesId.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.ReceiptId))
//            {
//                whereClauses.Add(string.Format("{0} = @receiptId", (object)"RECEIPTID"));
//                query.Parameters["@receiptId"] = (object)criteria.ReceiptId.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.ChannelReferenceId))
//            {
//                whereClauses.Add(string.Format("{0} = @channelReferenceId", (object)"CHANNELREFERENCEID"));
//                query.Parameters["@channelReferenceId"] = (object)criteria.ChannelReferenceId.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.CustomerAccountNumber))
//            {
//                whereClauses.Add(string.Format("{0} = @custAccount", (object)"CUSTOMERID"));
//                query.Parameters["@custAccount"] = (object)criteria.CustomerAccountNumber.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.StoreId))
//            {
//                whereClauses.Add(string.Format("{0} = @storeId", (object)"STORE"));
//                query.Parameters["@storeId"] = (object)criteria.StoreId.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.TerminalId))
//            {
//                whereClauses.Add(string.Format("{0} = @terminalId", (object)"TERMINAL"));
//                query.Parameters["@terminalId"] = (object)criteria.TerminalId.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.StaffId))
//            {
//                whereClauses.Add(string.Format("{0} = @staffId", (object)"STAFF"));
//                query.Parameters["@staffId"] = (object)criteria.StaffId.Trim();
//            }
//            if (criteria.StartDateTime.HasValue)
//            {
//                whereClauses.Add(string.Format("{0} >= @startDate", (object)"ORDERPLACEDDATE"));
//                query.Parameters["@startDate"] = (object)criteria.StartDateTime.Value.UtcDateTime;
//            }
//            if (criteria.EndDateTime.HasValue)
//            {
//                whereClauses.Add(string.Format("{0} <= @endDate", (object)"ORDERPLACEDDATE"));
//                whereClauses.Add(string.Format("{0} < @endDate", (object)"ORDERPLACEDDATE"));
//                query.Parameters["@endDate"] = (object)criteria.EndDateTime.Value.UtcDateTime;
//            }
//            if (!IEnumerableExtensions.IsNullOrEmpty<SalesTransactionType>(criteria.SalesTransactionTypes) && Enumerable.All<SalesTransactionType>(criteria.SalesTransactionTypes, (Func<SalesTransactionType, bool>)(transactionType => transactionType != SalesTransactionType.None)))
//                SalesOrderDataManager.AddInClause<SalesTransactionType>(criteria.SalesTransactionTypes, "TYPE", whereClauses, query);
//            if (!IEnumerableExtensions.IsNullOrEmpty<TransactionStatus>(criteria.TransactionStatusTypes))
//                SalesOrderDataManager.AddInClause<TransactionStatus>(criteria.TransactionStatusTypes, "ENTRYSTATUS", whereClauses, query);
//            if (criteria.TransactionIds != null && Enumerable.Any<string>(criteria.TransactionIds))
//                SalesOrderDataManager.AddInClause<string>(criteria.TransactionIds, "TRANSACTIONID", whereClauses, query);
//            if (!string.IsNullOrEmpty(criteria.ReceiptEmailAddress))
//            {
//                whereClauses.Add(string.Format("({0} = @receiptEmailAddress OR {1} = @receiptEmailAddress)", new object[2]
//        {
//          (object) "RECEIPTEMAIL",
//          (object) "EMAIL"
//        }));
//                query.Parameters["@receiptEmailAddress"] = (object)criteria.ReceiptEmailAddress.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.SearchIdentifiers))
//            {
//                StringBuilder stringBuilder = new StringBuilder();
//                stringBuilder.AppendFormat("({0} = @searchIdentifiers", (object)"TRANSACTIONID");
//                stringBuilder.AppendFormat(" OR {0} = @searchIdentifiers", (object)"RECEIPTID");
//                stringBuilder.AppendFormat(" OR {0} = @searchIdentifiers", (object)"SALESID");
//                stringBuilder.AppendFormat(" OR {0} = @searchIdentifiers", (object)"CUSTOMERID");
//                stringBuilder.AppendFormat(" OR {0} = @searchIdentifiers)", (object)"CHANNELREFERENCEID");
//                whereClauses.Add(stringBuilder.ToString());
//                query.Parameters["@searchIdentifiers"] = (object)criteria.SearchIdentifiers.Trim();
//            }
//            query.Where = string.Join(" AND ", (IEnumerable<string>)whereClauses);
//        }

//        public IEnumerable<SalesOrder> SearchSalesOrders(QueryResultSettings settings, SalesOrderSearchCriteria criteria)
//        {
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            ThrowIf.Null<SalesOrderSearchCriteria>(criteria, "criteria");
//            SqlPagedQuery query = new SqlPagedQuery(new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, //KAR
//                From = "GETSALESORDER(@nvc_CustomerName)",
//                OrderBy = settings.Sorting.ToString()
//            };
//            if (settings.Sorting == null || settings.Sorting.Count == 0)
//                query.OrderBy = new SortingInfo("ORDERPLACEDDATE", true).ToString();
//            List<string> list = new List<string>();
//            this.BuildWhereClauseForItemIdBarcodeSerialNumber(criteria, query, (IList<string>)list);
//            SalesOrderDataManager.BuildSearchOrderWhereClause(criteria, query, (IList<string>)list);
//            query.Where = string.Join(" AND ", (IEnumerable<string>)list);
//            string str1 = "\"\"";
//            if (!string.IsNullOrEmpty(criteria.CustomerFirstName) || !string.IsNullOrEmpty(criteria.CustomerLastName))
//            {
//                string str2;
//                if (criteria.CustomerFirstName != null)
//                    str2 = "\"" + criteria.CustomerFirstName + "*" + (criteria.CustomerLastName ?? string.Empty) + "\"";
//                else
//                    str2 = "\"" + criteria.CustomerLastName + "\"";
//                str1 = str2;
//            }
//            query.Parameters["@nvc_CustomerName"] = (object)str1;
//            //IEnumerable<SalesOrder> source = this.ExecuteSelect<SalesOrder>(query);
//            //return (IEnumerable<SalesOrder>)settings.Paging.Paginate<SalesOrder>((ICollection<SalesOrder>)Enumerable.ToList<SalesOrder>(source));
//            //KAR
//            return null;
//        }

//        public ReadOnlyCollection<TenderLine> GetTenderLinesForSalesOrder(string transactionId, QueryResultSettings settings)
//        {
//            ThrowIf.Null<string>(transactionId, "salesOrderId");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords ))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, //KAR
//                From = "RETAILTRANSACTIONPAYMENTTRANSVIEW",
//                Where = "TRANSACTIONID = @TransactionId"
//            };
//            sqlPagedQuery.Parameters["@TransactionId"] = (object)transactionId;
//            //return ExecuteSelect<TenderLine>(sqlPagedQuery) ?? new ReadOnlyCollection<TenderLine>(new List<TenderLine>());//KAR
//            return null;
//        }

//        public virtual void SaveSalesOrder(SalesTransaction transaction)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "retailTransaction");
//            using (DataTable dataTable1 = new DataTable("RETAILTRANSACTIONTABLETYPE"))
//            {
//                using (DataTable dataTable2 = new DataTable("RETAILTRANSACTIONPAYMENTTRANSTABLETYPE"))
//                {
//                    using (DataTable dataTable3 = new DataTable("RETAILTRANSACTIONSALESTRANSTABLETYPE"))
//                    {
//                        using (DataTable dataTable4 = new DataTable("RETAILINCOMEEXPENSETABLETYPE"))
//                        {
//                            using (DataTable dataTable5 = new DataTable("RETAILTRANSACTIONMARKUPTRANSTABLETYPE"))
//                            {
//                                using (DataTable dataTable6 = new DataTable("RETAILTRANSACTIONTAXTRANSTABLETYPE"))
//                                {
//                                    using (DataTable dataTable7 = new DataTable("RETAILTRANSACTIONATTRIBUTETRANSTABLETYPE"))
//                                    {
//                                        using (DataTable dataTable8 = new DataTable("RETAILTRANSACTIONADDRESSTRANSTABLETYPE"))
//                                        {
//                                            using (DataTable dataTable9 = new DataTable("RETAILTRANSACTIONDISCOUNTTRANSTABLETYPE"))
//                                            {
//                                                using (DataTable dataTable10 = new DataTable("RETAILTRANSACTIONINFOCODETRANSTABLETYPE"))
//                                                {
//                                                    using (DataTable dataTable11 = new DataTable("RETAILTRANSACTIONPROPERTIESTABLETYPE"))
//                                                    {
//                                                        using (DataTable dataTable12 = new DataTable("RETAILTRANSACTIONAFFILIATIONTRANSTABLETYPE"))
//                                                        {
//                                                            using (DataTable dataTable13 = new DataTable("RETAILTRANSACTIONLOYALTYREWARDPOINTTRANSTABLETYPE"))
//                                                            {
//                                                                using (DataTable dataTable14 = new DataTable("CUSTOMERORDERTRANSACTIONTABLETYPE"))
//                                                                {
//                                                                    using (DataTable dataTable15 = new DataTable("RETAILTRANSACTIONORDERINVOICETRANSTABLETYPE"))
//                                                                    {
//                                                                        RetailTransactionTableSchema.PopulateSchema(dataTable1);
//                                                                        RetailTransactionPaymentSchema.PopulatePaymentSchema(dataTable2);
//                                                                        SalesOrderDataManager.PopulateSaleLineSchema(dataTable3);
//                                                                        SalesOrderDataManager.PopulateIncomeExpenseLineSchema(dataTable4);
//                                                                        SalesOrderDataManager.PopulateMarkupSchema(dataTable5);
//                                                                        SalesOrderDataManager.PopulateTaxSchema(dataTable6);
//                                                                        SalesOrderDataManager.PopulateAttributeSchema(dataTable7);
//                                                                        SalesOrderDataManager.PopulateAddressSchema(dataTable8);
//                                                                        SalesOrderDataManager.PopulateDiscountSchema(dataTable9);
//                                                                        SalesOrderDataManager.PopulateInvoiceSchema(dataTable15);
//                                                                        SalesOrderDataManager.PopulateReasonCodeSchema(dataTable10);
//                                                                        SalesOrderDataManager.PopulatePropertiesSchema(dataTable11);
//                                                                        SalesOrderDataManager.PopulateRewardPointSchema(dataTable13);
//                                                                        SalesOrderDataManager.PopulateAffiliationsSchema(dataTable12);
//                                                                        CustomerOrderTransactionTableSchema.PopulateSchema(dataTable14);
//                                                                        DateTimeOffset inChannelTimeZone = RequestContextExtensions.GetNowInChannelTimeZone(this.Context);
//                                                                        this.FillOrderHeader(transaction, inChannelTimeZone, dataTable1, dataTable5, dataTable7, dataTable8, dataTable10, dataTable11, dataTable13);
//                                                                        this.FillOrderLines(transaction, inChannelTimeZone, dataTable3, dataTable4, dataTable5, dataTable6, dataTable8, dataTable9, dataTable10, dataTable11, dataTable12, dataTable15);
//                                                                        if (transaction.TenderLines != null)
//                                                                            this.FillOrderPayments(transaction, inChannelTimeZone, (IEnumerable<TenderLine>)transaction.TenderLines, dataTable2, dataTable10);
//                                                                        if (transaction.CartType == CartType.CustomerOrder)
//                                                                            this.FillCustomerOrder(transaction, dataTable14);
//                                                                        this.Context.Runtime.Execute<NullResponse>((Request)new InsertSalesTransactionTablesDataRequest(dataTable1, dataTable5, dataTable6, dataTable2, dataTable3, dataTable4, dataTable7, dataTable8, dataTable9, dataTable10, dataTable11, dataTable13, dataTable12, dataTable14, dataTable15), this.Context);
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        public string GetReceiptLayoutId(ReceiptType receiptType, string receiptProfileId, QueryResultSettings settings)
//        {
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings( PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, //KAR
//                From = "RECEIPTPROFILESVIEW",
//                Where = "RECEIPTTYPE = @ReceiptType AND PROFILEID = @ProfileId",
//                IsQueryByPrimaryKey = true
//            };
//            sqlPagedQuery.Parameters["@ProfileId"] = (object)receiptProfileId;
//            sqlPagedQuery.Parameters["@ReceiptType"] = (object)receiptType;
//            ReceiptProfile receiptProfile = Enumerable.SingleOrDefault<ReceiptProfile>((IEnumerable<ReceiptProfile>)this.ExecuteSelect<ReceiptProfile>(sqlPagedQuery));
//            if (receiptProfile != null)
//                return receiptProfile.ReceiptLayoutId;
//            return (string)null;
//        }

//        public ReadOnlyCollection<Printer> GetPrintersByReceiptType(string terminalId, ReceiptType receiptType, QueryResultSettings settings)
//        {
//            ThrowIf.Null<string>(terminalId, "terminalId");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery(new QueryResultSettings(PagingInfo.AllRecords) )
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging,
//                From = "RECEIPTPRINTERSVIEW",
//                Where = "TERMINALID = @TerminalId AND RECEIPTTYPE = @ReceiptType",
//                IsQueryByPrimaryKey = false
//            };
//            sqlPagedQuery.Parameters["@TerminalId"] = (object)terminalId;
//            sqlPagedQuery.Parameters["@ReceiptType"] = (object)receiptType;
//            return this.ExecuteSelect<Printer>(sqlPagedQuery);
//        }

//        public ReadOnlyCollection<Printer> GetPrintersByLayoutId(string terminalId, string layoutId, QueryResultSettings settings)
//        {
//            ThrowIf.Null<string>(terminalId, "terminalId");
//            ThrowIf.Null<string>(layoutId, "layoutId");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery(new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging,
//                From = "RECEIPTPRINTERSVIEW",
//                Where = "TERMINALID = @TerminalId AND FORMLAYOUTID = @LayoutId",
//                IsQueryByPrimaryKey = false
//            };
//            sqlPagedQuery.Parameters["@TerminalId"] = (object)terminalId;
//            sqlPagedQuery.Parameters["@LayoutId"] = (object)layoutId;
//            return this.ExecuteSelect<Printer>(sqlPagedQuery);
//        }

//        public ReadOnlyCollection<Printer> GetPrintersByTerminal(string terminalId, QueryResultSettings settings)
//        {
//            ThrowIf.Null<string>(terminalId, "terminalId");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery(new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging,
//                From = "RECEIPTPRINTERSVIEW",
//                Where = "TERMINALID = @TerminalId",
//                IsQueryByPrimaryKey = false
//            };
//            sqlPagedQuery.Parameters["@TerminalId"] = (object)terminalId;
//            return this.ExecuteSelect<Printer>(sqlPagedQuery);
//        }

//        public ReceiptMask GetReceiptMask(string functionalityProfileId, ReceiptTransactionType receiptTransactionType)
//        {
//            ThrowIf.NullOrWhiteSpace(functionalityProfileId, "functionalityProfileId");
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                From = "RECEIPTMASKVIEW",
//                Where = "FUNCPROFILEID = @FuncProfileId AND RECEIPTTRANSTYPE = @ReceiptTransType",
//                IsQueryByPrimaryKey = true
//            };
//            sqlPagedQuery.Parameters["@FuncProfileId"] = (object)functionalityProfileId;
//            sqlPagedQuery.Parameters["@ReceiptTransType"] = (object)receiptTransactionType;
//            ReadOnlyCollection<ReceiptMask> readOnlyCollection = this.ExecuteSelect<ReceiptMask>(sqlPagedQuery);
//            if (Enumerable.Any<ReceiptMask>((IEnumerable<ReceiptMask>)readOnlyCollection))
//                return Enumerable.SingleOrDefault<ReceiptMask>((IEnumerable<ReceiptMask>)readOnlyCollection);
//            return (ReceiptMask)null;
//        }

//        public ReceiptInfo GetReceiptInfo(bool copyReceipt, string layoutId, QueryResultSettings settings)
//        {
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            ThrowIf.Null<string>(layoutId, "layoutId");
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                ////Paging = settings.Paging, KAR KAR
//                From = "RECEIPTINFOVIEW",
//                Where = "FORMLAYOUTID = @LayoutId",
//                IsQueryByPrimaryKey = true
//            };
//            sqlPagedQuery.Parameters["@LayoutId"] = (object)layoutId;
//            ReceiptInfo receiptInfo = Enumerable.SingleOrDefault<ReceiptInfo>((IEnumerable<ReceiptInfo>)this.ExecuteSelect<ReceiptInfo>(sqlPagedQuery));
//            if (receiptInfo == null)
//                return (ReceiptInfo)null;
//            receiptInfo.IsCopy = copyReceipt;
//            return receiptInfo;
//        }

//        public void UpdateReturnQuantities(IEnumerable<SalesLine> salesLines)
//        {
//            ThrowIf.Null<IEnumerable<SalesLine>>(salesLines, "salesLines");
//            foreach (SalesLine salesLine in salesLines)
//            {
//                if (salesLine.IsReturnByReceipt)
//                {
//                    ParameterSet parameters = new ParameterSet();
//                    parameters["@bi_ChannelId"] = (object)salesLine.ReturnChannelId;
//                    parameters["@nvc_StoreNumber"] = (object)salesLine.ReturnStore;
//                    parameters["@nvc_TerminalId"] = (object)salesLine.ReturnTerminalId;
//                    parameters["@nvc_TransactionId"] = (object)salesLine.ReturnTransactionId;
//                    parameters["@nu_LineNumber"] = (object)salesLine.ReturnLineNumber;
//                    parameters["@nu_Quantity"] = (object)salesLine.Quantity;
//                    this.ExecuteStoredProcedureNonQuery("UpdateReturnQuantity", parameters);
//                }
//            }
//        }

//        public virtual void FillSalesOrderMembers(IEnumerable<SalesOrder> salesOrders, bool includeDetails)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(salesOrders, "salesOrders");
//            if (includeDetails)
//            {
//                QueryResultSettings settings = new QueryResultSettings(PagingInfo.AllRecords);// KAR
//                this.FillAddresses((IEnumerable<CommerceEntity>)salesOrders, settings);
//                this.FillSalesLines(salesOrders, settings);
//                this.FillInvoiceLines(salesOrders, settings);
//                this.FillIncomeExpenseLines(salesOrders, settings);
//                this.FillTenderLines(salesOrders, settings);
//                this.FillOrderAttributes(salesOrders, settings);
//                this.FillTransactionProperties(salesOrders, settings);
//                this.FillOrderAffiliations(salesOrders, settings);
//                this.FillOrderChargeLines(salesOrders, settings);
//                this.FillLoyaltyRewardPointLines(salesOrders, settings);
//                this.FillReasonCodeLines(salesOrders, settings);
//            }
//            foreach (SalesOrder salesOrder in salesOrders)
//            {
//                string str1 = (string)salesOrder.GetProperty("EMAIL") ?? string.Empty;
//                salesOrder.ContactInformationCollection.Add(new ContactInformation()
//                {
//                    ContactInformationType = ContactInformationType.Email,
//                    Value = str1
//                });
//                string str2 = (string)salesOrder.GetProperty("PHONE") ?? string.Empty;
//                salesOrder.ContactInformationCollection.Add(new ContactInformation()
//                {
//                    ContactInformationType = ContactInformationType.Phone,
//                    Value = str2
//                });
//                salesOrder.LineDiscount = Enumerable.Sum<SalesLine>((IEnumerable<SalesLine>)salesOrder.ActiveSalesLines, (Func<SalesLine, Decimal>)(s => s.LineDiscount));
//                salesOrder.PeriodicDiscountAmount = Enumerable.Sum<SalesLine>((IEnumerable<SalesLine>)salesOrder.ActiveSalesLines, (Func<SalesLine, Decimal>)(s => s.PeriodicDiscount));
//                salesOrder.TotalDiscount = Enumerable.Sum<SalesLine>((IEnumerable<SalesLine>)salesOrder.ActiveSalesLines, (Func<SalesLine, Decimal>)(s => s.TotalDiscount));
//                salesOrder.NetAmountWithNoTax = Enumerable.Sum<SalesLine>((IEnumerable<SalesLine>)salesOrder.ActiveSalesLines, (Func<SalesLine, Decimal>)(s => SalesTransactionExtensions.NetAmountWithNoTax(s)));
//                salesOrder.TaxAmountInclusive = Enumerable.Sum<SalesLine>((IEnumerable<SalesLine>)salesOrder.ActiveSalesLines, (Func<SalesLine, Decimal>)(s => s.TaxAmountInclusive));
//                salesOrder.SubtotalAmount = salesOrder.NetAmountWithNoTax + salesOrder.TaxAmountInclusive;
//                salesOrder.OrderPlacedDate = new DateTimeOffset(salesOrder.OrderPlacedDate.DateTime, new TimeSpan(0L));
//            }
//        }

//        public void PurgeSalesTransactions(long channelId, string terminalId, int retentionDays)
//        {
//            if (retentionDays <= 0)
//                return;
//            try
//            {
//                ParameterSet parameters = new ParameterSet()
//        {
//          {
//            "@bi_ChannelId",
//            (object) channelId
//          },
//          {
//            "@vc_TerminalId",
//            (object) terminalId
//          },
//          {
//            "@i_RetentionDays",
//            (object) retentionDays
//          },
//          {
//            "@f_PurgeOrder",
//            (object) 1
//          }
//        };
//                this.ExecuteStoredProcedureNonQuery("PURGESALESONTERMINAL", parameters);
//                parameters["@f_PurgeOrder"] = (object)0;
//                this.ExecuteStoredProcedureNonQuery("PURGESALESONTERMINAL", parameters);
//            }
//            catch (Exception)
//            {
//                // RetailLogger.Log.GenericErrorEvent("A problem occurred while purging outdated transactions.", ex.ToString());
//            }
//        }

//        protected static void SetField(DataRow row, string field, object value)
//        {
//            ThrowIf.Null<DataRow>(row, "row");
//            ThrowIf.Null<string>(field, "field");
//            row[field] = value;
//        }

//        protected string GetStoreId(SalesTransaction transaction)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            return transaction.StoreId ?? string.Empty;
//        }

//        private static void PopulateAddressSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINAL", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("SALELINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("DELIVERYNAME", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("SALESNAME", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("ZIPCODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("COUNTRYREGIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STATE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("CITY", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("COUNTY", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STREET", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("EMAIL", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("EMAILCONTENT", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("PHONE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STREETNUMBER", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("DISTRICTNAME", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateAttributeSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINAL", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TEXTVALUE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("NAME", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateTaxSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("AMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("ISINCLUDEDINPRICE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("SALELINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("STOREID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TAXCODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateMarkupSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("CORRENCYCODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("MARKUPCODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("MARKUPLINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("SALELINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("STORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TAXGROUP", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TAXITEMGROUP", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("VALUE", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("CALCULATEDAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("METHOD", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("TAXAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("TAXAMOUNTINCLUSIVE", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("TAXAMOUNTEXCLUSIVE", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//        }

//        private static void PopulateIncomeExpenseLineSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("RECEIPTID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("INCOMEEXPENSEACCOUNT", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("STORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINAL", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STAFF", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONSTATUS", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("AMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("ACCOUNTTYPE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("TRANSDATE", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("TRANSTIME", typeof(int)).DefaultValue = (object)(int)DateTime.UtcNow.TimeOfDay.TotalSeconds;
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("CHANNEL", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateSaleLineSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("CUSTACCOUNT", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("CREATEDDATETIME", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("LINEMANUALDISCOUNTAMOUNT", typeof(string)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("LINEMANUALDISCOUNTPERCENTAGE", typeof(string)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("DISCAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("TOTALDISCAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("TOTALDISCPCT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("LINEDSCAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("PERIODICDISCAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("PERIODICPERCENTAGEDISCOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("DLVMODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("INVENTDIMID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("INVENTLOCATIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("INVENTSERIALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("INVENTSITEID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("ITEMID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("BARCODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("LINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("LISTINGID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("LOGISTICSPOSTALADDRESS", typeof(long)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("NETAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("NETAMOUNTINCLTAX", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("PRICE", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("QTY", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("RECEIPTDATEREQUESTED", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("SHIPPINGDATEREQUESTED", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("STORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TAXAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("TAXGROUP", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TAXITEMGROUP", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STAFFID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("UNIT", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("VARIANTID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("RETURNNOSALE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("RETURNTRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("RETURNLINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("RETURNSTORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("RETURNTERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("RECEIPTID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSDATE", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("TRANSTIME", typeof(int)).DefaultValue = (object)(int)DateTime.UtcNow.TimeOfDay.TotalSeconds;
//            tableSchema.Columns.Add("TRANSACTIONSTATUS", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("COMMENT", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("GIFTCARD", typeof(bool)).DefaultValue = (object)false;
//            tableSchema.Columns.Add("CATALOG", typeof(long)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("ELECTRONICDELIVERYEMAIL", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("ELECTRONICDELIVERYEMAILCONTENT", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateDiscountSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("AMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("DEALPRICE", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("DISCOUNTAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("DISCOUNTCODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("LINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("PERCENTAGE", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("PERIODICDISCOUNTOFFERID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("SALELINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("STOREID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("DISCOUNTORIGINTYPE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("CUSTOMERDISCOUNTTYPE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("MANUALDISCOUNTTYPE", typeof(int)).DefaultValue = (object)0;
//        }

//        private static void PopulateReasonCodeSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("LINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TYPE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("INFOCODEID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("INFORMATION", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("INFOAMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("TRANSDATE", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("TRANSTIME", typeof(int)).DefaultValue = (object)(int)DateTime.UtcNow.TimeOfDay.TotalSeconds;
//            tableSchema.Columns.Add("STORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINAL", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STAFF", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("ITEMTENDER", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("AMOUNT", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("INPUTTYPE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("SUBINFOCODEID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STATEMENTCODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("SOURCECODE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("SOURCECODE2", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("SOURCECODE3", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("PARENTLINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//        }

//        private static void PopulatePropertiesSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STORE", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("SALELINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("NAME", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("VALUE", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateRewardPointSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("AFFILIATION", typeof(long)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("CARDNUMBER", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("CUSTACCOUNT", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("ENTRYDATE", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("ENTRYTIME", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("ENTRYTYPE", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("EXPIRATIONDATE", typeof(DateTime)).DefaultValue = (object)DateTime.UtcNow;
//            tableSchema.Columns.Add("LINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("LOYALTYTIER", typeof(long)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("RECEIPTID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("REWARDPOINT", typeof(long)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("REWARDPOINTAMOUNTQTY", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("STAFF", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STOREID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateAffiliationsSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("AFFILIATION", typeof(long)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("LOYALTYTIER", typeof(long)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("RECEIPTID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STAFF", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STOREID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//        }

//        private static void PopulateInvoiceSchema(DataTable tableSchema)
//        {
//            ThrowIf.Null<DataTable>(tableSchema, "tableSchema");
//            tableSchema.Columns.Add("DATAAREAID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("STOREID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TERMINALID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("TRANSACTIONID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("LINENUM", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//            tableSchema.Columns.Add("TRANSACTIONSTATUS", typeof(int)).DefaultValue = (object)0;
//            tableSchema.Columns.Add("INVOICEID", typeof(string)).DefaultValue = (object)string.Empty;
//            tableSchema.Columns.Add("AMOUNTCUR", typeof(Decimal)).DefaultValue = (object)new Decimal(0);
//        }

//        private static long GetAddressRecordId(Address shippingAddress)
//        {
//            Address address = shippingAddress;
//            if (address == null)
//                return 0L;
//            return address.RecordId;
//        }



//        private static SqlPagedQuery AddInClause<T>(IEnumerable<T> values, string columnName, IList<string> whereClauses, SqlPagedQuery query)
//        {
//            string parameterName = "";//"@" + columnName;
//            string[] strArray = Enumerable.ToArray<string>(Enumerable.Select<T, string>(values, (Func<T, int, string>)((s, i) => parameterName + i.ToString())));
//            string str = string.Join(",", strArray);
//            whereClauses.Add(string.Format("{0} IN ({1})", new object[2]
//      {
//        (object) columnName,
//        (object) str
//      }));
//            int index = 0;
//            foreach (T obj in values)
//            {
//                query.Parameters[strArray[index]] = (object)obj;
//                ++index;
//            }
//            return query;
//        }

//        private void BuildWhereClauseForItemIdBarcodeSerialNumber(SalesOrderSearchCriteria criteria, SqlPagedQuery query, IList<string> whereClauses)
//        {
//            if (string.IsNullOrEmpty(criteria.ItemId) && string.IsNullOrEmpty(criteria.Barcode) && string.IsNullOrEmpty(criteria.SerialNumber))
//                return;
//            if (!string.IsNullOrEmpty(criteria.ItemId))
//            {
//                whereClauses.Add(string.Format("{0} = @itemId", (object)"ITEMID"));
//                query.Parameters["@itemId"] = (object)criteria.ItemId.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.Barcode))
//            {
//                whereClauses.Add(string.Format("{0} = @barcode", (object)"BARCODE"));
//                query.Parameters["@barcode"] = (object)criteria.Barcode.Trim();
//            }
//            if (!string.IsNullOrEmpty(criteria.SerialNumber))
//            {
//                whereClauses.Add(string.Format("{0} = @serial", (object)"INVENTSERIALID"));
//                query.Parameters["@serial"] = (object)criteria.SerialNumber.Trim();
//            }
//            string str1 = string.Join(" AND ", (IEnumerable<string>)whereClauses);
//            whereClauses.Clear();
//            string str2 = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = new ColumnSet(new string[1]
//        {
//          "TRANSACTIONID"
//        }),
//                From = "RETAILTRANSACTIONSALESTRANSVIEW",
//                Where = str1,
//                //Paging = PagingInfo.NoPagination,
//                IsQueryByPrimaryKey = false
//            }.BuildQuery(this.GetCurrentDatabaseQueryBuilder());
//            whereClauses.Add(string.Format("{0} IN ({1})", new object[2]
//      {
//        (object) "TRANSACTIONID",
//        (object) str2
//      }));
//        }

//        private void FillOrderHeader(SalesTransaction transaction, DateTimeOffset transactionDate, DataTable transactionTable, DataTable markupTable, DataTable attributeTable, DataTable addressTable, DataTable reasonCodeTable, DataTable propertiesTable, DataTable rewardPointTable)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(transactionTable, "transactionTable");
//            ThrowIf.Null<DataTable>(markupTable, "markupTable");
//            ThrowIf.Null<DataTable>(attributeTable, "attributeTable");
//            DataRow row = transactionTable.NewRow();
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(transaction));
//            SalesOrderDataManager.SetField(row, "TERMINAL", (object)(transaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//            SalesOrderDataManager.SetField(row, "SALESORDERID", (object)StringDataHelper.TruncateString(transaction.SalesId, 20));
//            SalesOrderDataManager.SetField(row, "CHANNELREFERENCEID", (object)StringDataHelper.TruncateString(transaction.ChannelReferenceId, 50));
//            SalesOrderDataManager.SetField(row, "TYPE", (object)transaction.TransactionType);
//            SalesOrderDataManager.SetField(row, "CURRENCY", (object)(this.ChannelConfiguration.Currency ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "EXCHRATE", (object)transaction.ChannelCurrencyExchangeRate);
//            SalesOrderDataManager.SetField(row, "ENTRYSTATUS", (object)transaction.EntryStatus);
//            SalesOrderDataManager.SetField(row, "INVENTLOCATIONID", (object)StringDataHelper.TruncateString(transaction.InventoryLocationId, 10));
//            SalesOrderDataManager.SetField(row, "INVENTSITEID", (object)string.Empty);
//            SalesOrderDataManager.SetField(row, "INCOMEEXPENSEAMOUNT", (object)(transaction.IncomeExpenseTotalAmount * new Decimal(-1)));
//            SalesOrderDataManager.SetField(row, "CUSTACCOUNT", (object)StringDataHelper.TruncateString(transaction.CustomerId, 20));
//            SalesOrderDataManager.SetField(row, "DLVMODE", (object)StringDataHelper.TruncateString(transaction.DeliveryMode, 10));
//            SalesOrderDataManager.SetField(row, "RECEIPTDATEREQUESTED", (object)DateTimeOffsetDataHelper.GetDateOrDefaultSqlDate(transaction.RequestedDeliveryDate));
//            SalesOrderDataManager.SetField(row, "SHIPPINGDATEREQUESTED", (object)DateTimeOffsetDataHelper.GetDateOrDefaultSqlDate(transaction.RequestedDeliveryDate));
//            SalesOrderDataManager.SetField(row, "LOYALTYCARDID", (object)StringDataHelper.TruncateString(transaction.LoyaltyCardId, 30));
//            SalesOrderDataManager.SetField(row, "RECEIPTEMAIL", (object)StringDataHelper.TruncateString(transaction.ReceiptEmail, 80));
//            SalesOrderDataManager.SetField(row, "RECEIPTID", (object)StringDataHelper.TruncateString(transaction.ReceiptId, 18));
//            SalesOrderDataManager.SetField(row, "STAFF", (object)StringDataHelper.TruncateString(transaction.StaffId, 25));
//            SalesOrderDataManager.SetField(row, "NUMBEROFPAYMENTLINES", (object)(transaction.TenderLines == null ? 0 : transaction.TenderLines.Count));
//            SalesOrderDataManager.SetField(row, "SALEISRETURNSALE", (object)(transaction.IsReturnByReceipt ? 1 : 0));
//            SalesOrderDataManager.SetField(row, "DISCAMOUNT", (object)transaction.DiscountAmount);
//            SalesOrderDataManager.SetField(row, "TOTALDISCAMOUNT", (object)transaction.TotalDiscount);
//            SalesOrderDataManager.SetField(row, "CUSTDISCAMOUNT", (object)transaction.LineDiscount);
//            SalesOrderDataManager.SetField(row, "TOTALMANUALDISCOUNTAMOUNT", (object)transaction.TotalManualDiscountAmount);
//            SalesOrderDataManager.SetField(row, "TOTALMANUALDISCOUNTPERCENTAGE", (object)transaction.TotalManualDiscountPercentage);
//            SalesOrderDataManager.SetField(row, "BATCHID", (object)transaction.ShiftId);
//            if (transaction.CartType == CartType.AccountDeposit)
//            {
//                SalesOrderDataManager.SetField(row, "NUMBEROFITEMS", (object)new Decimal(0));
//                SalesOrderDataManager.SetField(row, "NUMBEROFITEMLINES", (object)new Decimal(0));
//                SalesOrderDataManager.SetField(row, "NETAMOUNT", (object)new Decimal(0));
//                SalesOrderDataManager.SetField(row, "PAYMENTAMOUNT", (object)new Decimal(0));
//                SalesOrderDataManager.SetField(row, "SALESPAYMENTDIFFERENCE", (object)new Decimal(0));
//                SalesOrderDataManager.SetField(row, "GROSSAMOUNT", (object)new Decimal(0));
//            }
//            else
//            {
//                SalesOrderDataManager.SetField(row, "GROSSAMOUNT", (object)Decimal.Negate(transaction.TotalAmount));
//                SalesOrderDataManager.SetField(row, "NETAMOUNT", (object)Decimal.Negate(transaction.NetAmountWithNoTax));
//                SalesOrderDataManager.SetField(row, "PAYMENTAMOUNT", (object)transaction.AmountPaid);
//                SalesOrderDataManager.SetField(row, "SALESPAYMENTDIFFERENCE", (object)transaction.SalesPaymentDifference);
//                SalesOrderDataManager.SetField(row, "NUMBEROFITEMS", (object)transaction.NumberOfItems);
//                SalesOrderDataManager.SetField(row, "NUMBEROFITEMLINES", (object)(transaction.SalesLines == null ? new Decimal(0) : (Decimal)transaction.SalesLines.Count));
//            }
//            string str = string.Empty;
//            if (!string.IsNullOrEmpty(transaction.ShiftTerminalId))
//                str = transaction.ShiftTerminalId;
//            else if (this.Context != null && RequestContextExtensions.GetTerminal(this.Context) != null)
//                str = RequestContextExtensions.GetTerminal(this.Context).TerminalId;
//            SalesOrderDataManager.SetField(row, "BATCHTERMINALID", (object)str);
//            SalesOrderDataManager.SetField(row, "COMMENT", (object)StringDataHelper.TruncateString(transaction.Comment, 60));
//            SalesOrderDataManager.SetField(row, "INVOICECOMMENT", (object)StringDataHelper.TruncateString(transaction.InvoiceComment, 60));
//            SalesOrderDataManager.SetField(row, "DESCRIPTION", (object)string.Empty);
//            SalesOrderDataManager.SetField(row, "TRANSDATE", (object)transactionDate.Date);
//            SalesOrderDataManager.SetField(row, "TRANSTIME", (object)(int)transactionDate.TimeOfDay.TotalSeconds);
//            SalesOrderDataManager.SetField(row, "BUSINESSDATE", (object)(transaction.BusinessDate.HasValue ? transaction.BusinessDate.Value.DateTime.Date : transactionDate.Date));
//            SalesOrderDataManager.SetField(row, "STATEMENTCODE", (object)(transaction.StatementCode ?? string.Empty));
//            long addressRecordId = SalesOrderDataManager.GetAddressRecordId(transaction.ShippingAddress);
//            SalesOrderDataManager.SetField(row, "LOGISTICSPOSTALADDRESS", (object)addressRecordId);
//            transactionTable.Rows.Add(row);
//            this.FillChargesForHeader((IEnumerable<ChargeLine>)transaction.ChargeLines, transaction, markupTable, new Decimal(0));
//            this.FillAttributesForHeader(transaction, attributeTable);
//            this.SaveReasonCodesForHeader(transaction, transactionDate, reasonCodeTable);
//            this.SavePropertySet(transaction, transaction.PersistentProperties, propertiesTable, new Decimal(0));
//            if (addressRecordId == 0L)
//                this.FillAddress(transaction.ShippingAddress, transaction, addressTable, new Decimal(0));
//            this.FillRewardPointLines(transaction, rewardPointTable);
//        }

//        private void FillAddress(Address address, SalesTransaction transaction, DataTable addressTable, Decimal lineNumber)
//        {
//            this.FillAddress(address, transaction, addressTable, lineNumber, this.GetStoreId(transaction));
//        }

//        private void FillAddress(Address address, SalesTransaction transaction, DataTable addressTable, Decimal lineNumber, string storeNumber)
//        {
//            if (address == null)
//                return;
//            DataRow row = addressTable.NewRow();
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            SalesOrderDataManager.SetField(row, "STORE", (object)(storeNumber ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TERMINAL", (object)(transaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//            SalesOrderDataManager.SetField(row, "SALELINENUM", (object)lineNumber);
//            SalesOrderDataManager.SetField(row, "DELIVERYNAME", (object)(address.Name ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "SALESNAME", (object)(transaction.Name ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "ZIPCODE", (object)(address.ZipCode ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "COUNTRYREGIONID", (object)(address.ThreeLetterISORegionName ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "STATE", (object)(address.State ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "CITY", (object)(address.City ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "COUNTY", (object)(address.County ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "STREET", (object)(address.Street ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "STREETNUMBER", (object)(address.StreetNumber ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "DISTRICTNAME", (object)(address.DistrictName ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "EMAIL", (object)(address.Email ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "EMAILCONTENT", (object)(address.EmailContent ?? string.Empty));
//            ContactInformation contactInformation1 = Enumerable.FirstOrDefault<ContactInformation>((IEnumerable<ContactInformation>)transaction.ContactInformationCollection, (Func<ContactInformation, bool>)(c => c.ContactInformationType == ContactInformationType.Email));
//            if (contactInformation1 == null)
//            {
//                string str1 = string.Empty;
//            }
//            else
//            {
//                string str2 = contactInformation1.Value;
//            }
//            ContactInformation contactInformation2 = Enumerable.FirstOrDefault<ContactInformation>((IEnumerable<ContactInformation>)transaction.ContactInformationCollection, (Func<ContactInformation, bool>)(c => c.ContactInformationType == ContactInformationType.Phone));
//            string str3 = contactInformation2 != null ? contactInformation2.Value : string.Empty;
//            SalesOrderDataManager.SetField(row, "PHONE", (object)(str3 ?? string.Empty));
//            addressTable.Rows.Add(row);
//        }

//        private void FillAttributesForHeader(SalesTransaction transaction, DataTable attributeTable)
//        {
//            foreach (AttributeValueBase attributeValueBase in transaction.AttributeValues)
//            {
//                AttributeTextValue attributeTextValue = attributeValueBase as AttributeTextValue;
//                if (attributeTextValue != null)
//                {
//                    DataRow row = attributeTable.NewRow();
//                    SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//                    SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(transaction));
//                    SalesOrderDataManager.SetField(row, "TERMINAL", (object)(transaction.TerminalId ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//                    SalesOrderDataManager.SetField(row, "NAME", (object)(attributeTextValue.Name ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "TEXTVALUE", (object)(attributeTextValue.TextValue ?? string.Empty));
//                    attributeTable.Rows.Add(row);
//                }
//            }
//        }

//        private void FillChargesForHeader(IEnumerable<ChargeLine> charges, SalesTransaction transaction, DataTable markupTable, Decimal lineNumber)
//        {
//            ThrowIf.Null<IEnumerable<ChargeLine>>(charges, "charges");
//            int num = 0;
//            foreach (ChargeLine chargeLine in charges)
//                this.FillChargeLine(chargeLine, transaction, num++, markupTable, lineNumber);
//        }

//        private void FillChargesForItemLine(IEnumerable<ChargeLine> charges, SalesTransaction transaction, SalesLine lineItem, DataTable markupTable, Decimal lineNumber)
//        {
//            ThrowIf.Null<IEnumerable<ChargeLine>>(charges, "charges");
//            ThrowIf.Null<SalesLine>(lineItem, "lineItem");
//            int num = 0;
//            foreach (ChargeLine chargeLine in Enumerable.Where<ChargeLine>(charges, (Func<ChargeLine, bool>)(c => c.ChargeType != ChargeType.PriceCharge)))
//                this.FillChargeLine(chargeLine, transaction, num++, markupTable, lineNumber);
//        }

//        private void FillChargeLine(ChargeLine chargeLine, SalesTransaction transaction, int markupLineNumber, DataTable markupTable, Decimal lineNumber)
//        {
//            ThrowIf.Null<ChargeLine>(chargeLine, "ChargeLine");
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(markupTable, "markupTable");
//            DataRow row = markupTable.NewRow();
//            SalesOrderDataManager.SetField(row, "CORRENCYCODE", (object)(this.ChannelConfiguration.Currency ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "MARKUPCODE", (object)(chargeLine.ChargeCode ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "MARKUPLINENUM", (object)markupLineNumber);
//            SalesOrderDataManager.SetField(row, "SALELINENUM", (object)lineNumber);
//            SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(transaction));
//            SalesOrderDataManager.SetField(row, "TAXGROUP", (object)(chargeLine.SalesTaxGroupId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TAXITEMGROUP", (object)(chargeLine.ItemTaxGroupId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//            SalesOrderDataManager.SetField(row, "VALUE", (object)chargeLine.Value);
//            SalesOrderDataManager.SetField(row, "METHOD", (object)chargeLine.ChargeMethod);
//            SalesOrderDataManager.SetField(row, "CALCULATEDAMOUNT", (object)chargeLine.CalculatedAmount);
//            SalesOrderDataManager.SetField(row, "TAXAMOUNT", (object)chargeLine.TaxAmount);
//            SalesOrderDataManager.SetField(row, "TAXAMOUNTINCLUSIVE", (object)chargeLine.TaxAmountInclusive);
//            SalesOrderDataManager.SetField(row, "TAXAMOUNTEXCLUSIVE", (object)chargeLine.TaxAmountExclusive);
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            markupTable.Rows.Add(row);
//        }

//        private void FillOrderPayments(SalesTransaction transaction, DateTimeOffset transactionDate, IEnumerable<TenderLine> tenderLines, DataTable paymentTable, DataTable reasonCodeTable)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<IEnumerable<TenderLine>>(tenderLines, "tenderLines");
//            ThrowIf.Null<DataTable>(paymentTable, "paymentTable");
//            int num = 1;
//            DateTimeOffset inChannelTimeZone = RequestContextExtensions.GetNowInChannelTimeZone(this.Context);
//            foreach (TenderLine tenderLine in tenderLines)
//            {
//                if (tenderLine.Status != TenderLineStatus.Historical)
//                {
//                    tenderLine.LineNumber = (Decimal)num++;
//                    DataRow row = paymentTable.NewRow();
//                    SalesOrderDataManager.SetField(row, "AMOUNTCUR", (object)tenderLine.AmountInTenderedCurrency);
//                    SalesOrderDataManager.SetField(row, "CURRENCY", (object)(tenderLine.Currency ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "EXCHRATE", (object)tenderLine.ExchangeRate);
//                    SalesOrderDataManager.SetField(row, "AMOUNTTENDERED", (object)tenderLine.Amount);
//                    SalesOrderDataManager.SetField(row, "EXCHRATEMST", (object)tenderLine.CompanyCurrencyExchangeRate);
//                    SalesOrderDataManager.SetField(row, "AMOUNTMST", (object)tenderLine.AmountInCompanyCurrency);
//                    SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//                    SalesOrderDataManager.SetField(row, "LINENUM", (object)tenderLine.LineNumber);
//                    SalesOrderDataManager.SetField(row, "PAYMENTAUTHORIZATION", (object)(tenderLine.Authorization ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(transaction));
//                    SalesOrderDataManager.SetField(row, "TENDERTYPE", (object)(tenderLine.TenderTypeId ?? string.Empty));
//                    //SalesOrderDataManager.SetField(row, "CHANGELINE", (object)(bool)(tenderLine.IsChangeLine ? 1 : 0));
//                    SalesOrderDataManager.SetField(row, "TERMINAL", (object)(transaction.TerminalId ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "STAFF", (object)StringDataHelper.TruncateString(transaction.StaffId, 25));
//                    SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//                    SalesOrderDataManager.SetField(row, "SIGCAPDATA", (object)tenderLine.SignatureData);
//                    SalesOrderDataManager.SetField(row, "TRANSACTIONSTATUS", (object)tenderLine.TransactionStatus);
//                    SalesOrderDataManager.SetField(row, "RECEIPTID", (object)StringDataHelper.TruncateString(transaction.ReceiptId, 18));
//                    SalesOrderDataManager.SetField(row, "GIFTCARDID", (object)(tenderLine.GiftCardId ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "LOYALTYCARDID", (object)(tenderLine.LoyaltyCardId ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "CARDORACCOUNT", (object)(tenderLine.CardOrAccount ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "CARDTYPEID", (object)(tenderLine.CardTypeId ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "CREDITVOUCHERID", (object)(tenderLine.CreditMemoId ?? string.Empty));
//                    SalesOrderDataManager.SetField(row, "TRANSDATE", (object)tenderLine.TenderDate.GetValueOrDefault(inChannelTimeZone).Date);
//                    SalesOrderDataManager.SetField(row, "TRANSTIME", (object)(int)tenderLine.TenderDate.GetValueOrDefault(inChannelTimeZone).TimeOfDay.TotalSeconds);
//                    paymentTable.Rows.Add(row);
//                    this.SaveReasonCodesForTenderLine(tenderLine, transaction, transactionDate, reasonCodeTable);
//                }
//            }
//        }

//        private void FillCustomerOrder(SalesTransaction transaction, DataTable customerOrderTable)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(customerOrderTable, "customerOrderTable");
//            DataRow row = customerOrderTable.NewRow();
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(transaction));
//            SalesOrderDataManager.SetField(row, "TERMINAL", (object)(transaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "CANCELLATIONCHARGE", (object)transaction.CancellationCharge);
//            SalesOrderDataManager.SetField(row, "DEPOSITOVERRIDE", (object)(transaction.IsDepositOverridden ? transaction.OverriddenDepositAmount : new Decimal?()));
//            SalesOrderDataManager.SetField(row, "PREPAYMENTINVOICED", (object)transaction.PrepaymentAmountInvoiced);
//            SalesOrderDataManager.SetField(row, "CALCULATEDDEPOSIT", (object)transaction.CalculatedDepositAmount);
//            SalesOrderDataManager.SetField(row, "PREPAYMENTPAID", (object)transaction.PrepaymentAmountPaid);
//            SalesOrderDataManager.SetField(row, "REQUIREDDEPOSIT", (object)transaction.RequiredDepositAmount);
//            SalesOrderDataManager.SetField(row, "QUOTEEXPIRATIONDATE", (object)DateTimeOffsetDataHelper.GetDateOrDefaultSqlDate(transaction.QuotationExpiryDate));
//            SalesOrderDataManager.SetField(row, "CUSTOMERORDERTYPE", (object)transaction.CustomerOrderType);
//            customerOrderTable.Rows.Add(row);
//        }

//        private void FillOrderLines(SalesTransaction salesTransaction, DateTimeOffset transactionDate, DataTable lineTable, DataTable incomeExpenseTable, DataTable markupTable, DataTable taxTable, DataTable addressTable, DataTable discountTable, DataTable reasonCodeTable, DataTable propertiesTable, DataTable affiliationsTable, DataTable invoiceLinesTable)
//        {
//            ThrowIf.Null<SalesTransaction>(salesTransaction, "salesTransaction");
//            ThrowIf.Null<DataTable>(lineTable, "lineTable");
//            foreach (SalesLine salesLine in salesTransaction.SalesLines)
//                this.FillItemLine(salesLine, salesTransaction, transactionDate, lineTable, markupTable, taxTable, addressTable, discountTable, reasonCodeTable, propertiesTable, invoiceLinesTable);
//            foreach (IncomeExpenseLine incomeExpenseLine in salesTransaction.IncomeExpenseLines)
//                this.FillIncomeExpenseLine(salesTransaction, transactionDate, incomeExpenseLine, incomeExpenseTable);
//            foreach (SalesAffiliationLoyaltyTier salesAffiliationLoyaltyTier in salesTransaction.AffiliationLoyaltyTierLines)
//                this.FillAffiliation(salesAffiliationLoyaltyTier, salesTransaction, transactionDate, affiliationsTable, reasonCodeTable);
//        }

//        private void FillIncomeExpenseLine(SalesTransaction salesTransaction, DateTimeOffset transactionDate, IncomeExpenseLine incomeExpenseLine, DataTable incomeExpenseTable)
//        {
//            ThrowIf.Null<SalesTransaction>(salesTransaction, "salesTransaction");
//            ThrowIf.Null<IncomeExpenseLine>(incomeExpenseLine, "incomeExpenseLine");
//            ThrowIf.Null<DataTable>(incomeExpenseTable, "incomeExpenseTable");
//            DataRow row = incomeExpenseTable.NewRow();
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)salesTransaction.Id);
//            SalesOrderDataManager.SetField(row, "RECEIPTID", (object)StringDataHelper.TruncateString(salesTransaction.ReceiptId, 18));
//            SalesOrderDataManager.SetField(row, "INCOMEEXPENSEACCOUNT", (object)incomeExpenseLine.IncomeExpenseAccount);
//            SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(salesTransaction));
//            SalesOrderDataManager.SetField(row, "TERMINAL", (object)(salesTransaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "STAFF", (object)StringDataHelper.TruncateString(salesTransaction.StaffId, 25));
//            SalesOrderDataManager.SetField(row, "TRANSACTIONSTATUS", (object)incomeExpenseLine.TransactionStatus);
//            SalesOrderDataManager.SetField(row, "AMOUNT", (object)(incomeExpenseLine.AccountType == IncomeExpenseAccountType.Income ? incomeExpenseLine.Amount * new Decimal(-1) : incomeExpenseLine.Amount));
//            SalesOrderDataManager.SetField(row, "ACCOUNTTYPE", (object)incomeExpenseLine.AccountType);
//            SalesOrderDataManager.SetField(row, "TRANSDATE", (object)transactionDate.Date);
//            SalesOrderDataManager.SetField(row, "TRANSTIME", (object)(int)transactionDate.TimeOfDay.TotalSeconds);
//            SalesOrderDataManager.SetField(row, "CHANNEL", (object)this.ChannelId);
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            incomeExpenseTable.Rows.Add(row);
//        }

//        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "To be refactored.")]
//        private void FillItemLine(SalesLine salesLine, SalesTransaction transaction, DateTimeOffset transactionDate, DataTable lineTable, DataTable markupTable, DataTable taxTable, DataTable addressTable, DataTable discountTable, DataTable reasonCodeTable, DataTable propertiesTable, DataTable invoiceTable)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<SalesLine>(salesLine, "saleItem");
//            ThrowIf.Null<DataTable>(lineTable, "lineTable");
//            if (salesLine.IsInvoiceLine)
//            {
//                DataRow row = invoiceTable.NewRow();
//                SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//                SalesOrderDataManager.SetField(row, "STOREID", (object)((salesLine.Store ?? transaction.StoreId) ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//                SalesOrderDataManager.SetField(row, "LINENUM", (object)salesLine.LineNumber);
//                SalesOrderDataManager.SetField(row, "TRANSACTIONSTATUS", (object)salesLine.Status);
//                SalesOrderDataManager.SetField(row, "INVOICEID", (object)salesLine.InvoiceId);
//                SalesOrderDataManager.SetField(row, "AMOUNTCUR", (object)salesLine.InvoiceAmount);
//                invoiceTable.Rows.Add(row);
//            }
//            else
//            {
//                DataRow row = lineTable.NewRow();
//                SalesOrderDataManager.SetField(row, "CUSTACCOUNT", (object)(transaction.CustomerId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//                SalesOrderDataManager.SetField(row, "LINEMANUALDISCOUNTAMOUNT", (object)salesLine.LineManualDiscountAmount);
//                SalesOrderDataManager.SetField(row, "LINEMANUALDISCOUNTPERCENTAGE", (object)salesLine.LineManualDiscountPercentage);
//                SalesOrderDataManager.SetField(row, "DISCAMOUNT", (object)(salesLine.LineDiscount + salesLine.TotalDiscount + salesLine.PeriodicDiscount));
//                SalesOrderDataManager.SetField(row, "TOTALDISCAMOUNT", (object)salesLine.TotalDiscount);
//                SalesOrderDataManager.SetField(row, "TOTALDISCPCT", (object)salesLine.TotalPercentageDiscount);
//                SalesOrderDataManager.SetField(row, "LINEDSCAMOUNT", (object)salesLine.LineDiscount);
//                SalesOrderDataManager.SetField(row, "PERIODICDISCAMOUNT", (object)salesLine.PeriodicDiscount);
//                SalesOrderDataManager.SetField(row, "PERIODICPERCENTAGEDISCOUNT", (object)salesLine.PeriodicPercentageDiscount);
//                SalesOrderDataManager.SetField(row, "DLVMODE", (object)(salesLine.DeliveryMode ?? transaction.DeliveryMode ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "INVENTLOCATIONID", (object)StringDataHelper.TruncateString(salesLine.InventoryLocationId ?? transaction.InventoryLocationId ?? string.Empty, 10));
//                SalesOrderDataManager.SetField(row, "INVENTSERIALID", (object)(salesLine.SerialNumber ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "INVENTSITEID", (object)string.Empty);
//                SalesOrderDataManager.SetField(row, "ITEMID", (object)(salesLine.ItemId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "BARCODE", (object)(salesLine.Barcode ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "LINENUM", (object)salesLine.LineNumber);
//                SalesOrderDataManager.SetField(row, "LISTINGID", (object)salesLine.ProductId.ToString());
//                DateTimeOffset transactionDate1 = salesLine.SalesDate ?? transactionDate;
//                SalesOrderDataManager.SetField(row, "TRANSDATE", (object)transactionDate1.Date);
//                SalesOrderDataManager.SetField(row, "TRANSTIME", (object)(int)transactionDate1.TimeOfDay.TotalSeconds);
//                Address address = salesLine.ShippingAddress ?? transaction.ShippingAddress;
//                long addressRecordId = SalesOrderDataManager.GetAddressRecordId(address);
//                SalesOrderDataManager.SetField(row, "LOGISTICSPOSTALADDRESS", (object)addressRecordId);
//                SalesOrderDataManager.SetField(row, "NETAMOUNT", (object)(salesLine.NetAmount * new Decimal(-1)));
//                SalesOrderDataManager.SetField(row, "NETAMOUNTINCLTAX", (object)(salesLine.NetAmountWithAllInclusiveTax * new Decimal(-1)));
//                SalesOrderDataManager.SetField(row, "TAXAMOUNT", (object)(salesLine.TaxAmount * new Decimal(-1)));
//                SalesOrderDataManager.SetField(row, "PRICE", (object)salesLine.Price);
//                SalesOrderDataManager.SetField(row, "QTY", (object)(salesLine.Quantity * new Decimal(-1)));
//                DateTime orDefaultSqlDate = DateTimeOffsetDataHelper.GetDateOrDefaultSqlDate(!string.IsNullOrWhiteSpace(salesLine.DeliveryMode) ? salesLine.RequestedDeliveryDate : transaction.RequestedDeliveryDate);
//                SalesOrderDataManager.SetField(row, "RECEIPTDATEREQUESTED", (object)orDefaultSqlDate);
//                SalesOrderDataManager.SetField(row, "SHIPPINGDATEREQUESTED", (object)orDefaultSqlDate);
//                SalesOrderDataManager.SetField(row, "STORE", (object)((salesLine.Store ?? transaction.StoreId) ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//                SalesOrderDataManager.SetField(row, "UNIT", (object)(salesLine.SalesOrderUnitOfMeasure ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TAXGROUP", (object)(salesLine.SalesTaxGroupId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TAXITEMGROUP", (object)(salesLine.ItemTaxGroupId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "STAFFID", (object)StringDataHelper.TruncateString(transaction.StaffId, 25));
//                SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//                string str = (string)null;
//                if (salesLine.Variant != null)
//                    str = salesLine.Variant.VariantId;
//                SalesOrderDataManager.SetField(row, "VARIANTID", (object)(str ?? string.Empty));
//                // SalesOrderDataManager.SetField(row, "RETURNNOSALE", (object)(bool)(salesLine.IsReturnByReceipt ? 1 : 0));
//                SalesOrderDataManager.SetField(row, "RETURNTRANSACTIONID", (object)(salesLine.ReturnTransactionId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "RETURNLINENUM", (object)salesLine.ReturnLineNumber);
//                SalesOrderDataManager.SetField(row, "RETURNSTORE", (object)(salesLine.ReturnStore ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "RETURNTERMINALID", (object)(salesLine.ReturnTerminalId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "RECEIPTID", (object)(transaction.ReceiptId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TRANSACTIONSTATUS", (object)salesLine.Status);
//                SalesOrderDataManager.SetField(row, "COMMENT", (object)StringDataHelper.TruncateString(salesLine.Comment, 60));
//                SalesOrderDataManager.SetField(row, "GIFTCARD", (object)(salesLine.IsGiftCardLine ? 1 : 0));
//                SalesOrderDataManager.SetField(row, "CATALOG", (object)salesLine.CatalogId);
//                SalesOrderDataManager.SetField(row, "ELECTRONICDELIVERYEMAIL", (object)salesLine.ElectronicDeliveryEmailAddress);
//                SalesOrderDataManager.SetField(row, "ELECTRONICDELIVERYEMAILCONTENT", (object)salesLine.ElectronicDeliveryEmailContent);
//                lineTable.Rows.Add(row);
//                this.FillChargesForItemLine((IEnumerable<ChargeLine>)salesLine.ChargeLines, transaction, salesLine, markupTable, salesLine.LineNumber);
//                if (addressRecordId == 0L)
//                    this.FillAddress(address, transaction, addressTable, salesLine.LineNumber, salesLine.Store);
//                this.FillItemDiscountLines(transaction, salesLine, discountTable, salesLine.LineNumber);
//                this.FillReasonCodesForSalesLine(salesLine, transaction, transactionDate1, reasonCodeTable);
//            }
//            this.FillItemTaxLines(transaction, salesLine, taxTable, salesLine.LineNumber);
//            this.SavePropertySet(transaction, salesLine.PersistentProperties, propertiesTable, salesLine.LineNumber);
//        }

//        private void FillItemTaxLines(SalesTransaction transaction, SalesLine saleItem, DataTable taxTable, Decimal lineNumber)
//        {
//            ThrowIf.Null<SalesLine>(saleItem, "saleItem");
//            ThrowIf.Null<DataTable>(taxTable, "taxTable");
//            foreach (TaxLine taxItem in saleItem.TaxLines)
//                this.FillTaxLine(transaction, saleItem, taxItem, taxTable, lineNumber);
//        }

//        private void FillTaxLine(SalesTransaction transaction, SalesLine saleItem, TaxLine taxItem, DataTable taxTable, Decimal lineNumber)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<SalesLine>(saleItem, "saleItem");
//            ThrowIf.Null<TaxLine>(taxItem, "taxItem");
//            ThrowIf.Null<DataTable>(taxTable, "taxTable");
//            DataRow row = taxTable.NewRow();
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            SalesOrderDataManager.SetField(row, "STOREID", (object)this.GetStoreId(transaction));
//            SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//            SalesOrderDataManager.SetField(row, "SALELINENUM", (object)lineNumber);
//            SalesOrderDataManager.SetField(row, "TAXCODE", (object)(taxItem.TaxCode ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "AMOUNT", (object)taxItem.Amount);
//            // SalesOrderDataManager.SetField(row, "ISINCLUDEDINPRICE", (object)(bool)(transaction.IsTaxIncludedInPrice ? 1 : 0));
//            taxTable.Rows.Add(row);
//        }

//        private void FillItemDiscountLines(SalesTransaction transaction, SalesLine saleItem, DataTable discountTable, Decimal lineNumber)
//        {
//            ThrowIf.Null<SalesLine>(saleItem, "saleItem");
//            ThrowIf.Null<DataTable>(discountTable, "discountTable");
//            Decimal discountLineNumber = new Decimal(1);
//            foreach (DiscountLine discount in saleItem.DiscountLines)
//            {
//                this.FillDiscountLine(transaction, saleItem, discount, discountTable, lineNumber, discountLineNumber);
//                ++discountLineNumber;
//            }
//        }

//        private void FillDiscountLine(SalesTransaction transaction, SalesLine saleItem, DiscountLine discount, DataTable discountTable, Decimal lineNumber, Decimal discountLineNumber)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<SalesLine>(saleItem, "saleItem");
//            ThrowIf.Null<DiscountLine>(discount, "discount");
//            ThrowIf.Null<DataTable>(discountTable, "discountTable");
//            DataRow row = discountTable.NewRow();
//            SalesOrderDataManager.SetField(row, "AMOUNT", (object)discount.EffectiveAmount);
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            SalesOrderDataManager.SetField(row, "DEALPRICE", (object)discount.DealPrice);
//            SalesOrderDataManager.SetField(row, "DISCOUNTAMOUNT", (object)discount.Amount);
//            SalesOrderDataManager.SetField(row, "DISCOUNTCODE", (object)(discount.DiscountCode ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "SALELINENUM", (object)lineNumber);
//            SalesOrderDataManager.SetField(row, "PERCENTAGE", (object)discount.Percentage);
//            SalesOrderDataManager.SetField(row, "PERIODICDISCOUNTOFFERID", (object)(discount.OfferId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "STOREID", (object)this.GetStoreId(transaction));
//            SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//            SalesOrderDataManager.SetField(row, "LINENUM", (object)discountLineNumber);
//            SalesOrderDataManager.SetField(row, "DISCOUNTORIGINTYPE", (object)discount.DiscountLineTypeValue);
//            SalesOrderDataManager.SetField(row, "CUSTOMERDISCOUNTTYPE", (object)discount.CustomerDiscountTypeValue);
//            SalesOrderDataManager.SetField(row, "MANUALDISCOUNTTYPE", (object)discount.ManualDiscountTypeValue);
//            discountTable.Rows.Add(row);
//        }

//        private void SavePropertySet(SalesTransaction transaction, ParameterSet propertySet, DataTable propertiesTable, Decimal lineNumber)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<ParameterSet>(propertySet, "propertySet");
//            ThrowIf.Null<DataTable>(propertiesTable, "propertiesTable");
//            foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>)propertySet)
//            {
//                DataRow row = propertiesTable.NewRow();
//                SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//                SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(transaction));
//                SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)transaction.Id);
//                SalesOrderDataManager.SetField(row, "SALELINENUM", (object)lineNumber);
//                SalesOrderDataManager.SetField(row, "NAME", (object)keyValuePair.Key);
//                SalesOrderDataManager.SetField(row, "VALUE", (object)keyValuePair.Value.ToString());
//                propertiesTable.Rows.Add(row);
//            }
//        }

//        private void SaveReasonCodesForHeader(SalesTransaction transaction, DateTimeOffset transactionDate, DataTable reasonCodeTable)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(reasonCodeTable, "reasonCodeTable");
//            if (transaction.ReasonCodeLines == null || !Enumerable.Any<ReasonCodeLine>((IEnumerable<ReasonCodeLine>)transaction.ReasonCodeLines))
//                return;
//            this.FillReasonCodeLines((IEnumerable<ReasonCodeLine>)transaction.ReasonCodeLines, transaction, transactionDate, reasonCodeTable, new Decimal(-1));
//        }

//        private void SaveReasonCodesForTenderLine(TenderLine tenderLine, SalesTransaction transaction, DateTimeOffset transactionDate, DataTable reasonCodeTable)
//        {
//            ThrowIf.Null<TenderLine>(tenderLine, "tenderLine");
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(reasonCodeTable, "reasonCodeTable");
//            if (tenderLine.ReasonCodeLines == null || !Enumerable.Any<ReasonCodeLine>((IEnumerable<ReasonCodeLine>)tenderLine.ReasonCodeLines))
//                return;
//            this.FillReasonCodeLines((IEnumerable<ReasonCodeLine>)tenderLine.ReasonCodeLines, transaction, transactionDate, reasonCodeTable, tenderLine.LineNumber);
//        }

//        private void FillReasonCodesForSalesLine(SalesLine salesLine, SalesTransaction transaction, DateTimeOffset transactionDate, DataTable reasonCodeTable)
//        {
//            ThrowIf.Null<SalesLine>(salesLine, "salesLine");
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(reasonCodeTable, "reasonCodeTable");
//            if (salesLine.ReasonCodeLines == null || !Enumerable.Any<ReasonCodeLine>((IEnumerable<ReasonCodeLine>)salesLine.ReasonCodeLines))
//                return;
//            this.FillReasonCodeLines((IEnumerable<ReasonCodeLine>)salesLine.ReasonCodeLines, transaction, transactionDate, reasonCodeTable, salesLine.LineNumber);
//        }

//        private void FillReasonCodesForAffiliationLine(SalesAffiliationLoyaltyTier salesAffiliationLoyaltyTier, SalesTransaction transaction, DateTimeOffset transactionDate, DataTable reasonCodeTable)
//        {
//            ThrowIf.Null<SalesAffiliationLoyaltyTier>(salesAffiliationLoyaltyTier, "salesAffiliationLoyaltyTier");
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(reasonCodeTable, "reasonCodeTable");
//            if (!Enumerable.Any<ReasonCodeLine>((IEnumerable<ReasonCodeLine>)salesAffiliationLoyaltyTier.ReasonCodeLines))
//                return;
//            this.FillReasonCodeLines((IEnumerable<ReasonCodeLine>)salesAffiliationLoyaltyTier.ReasonCodeLines, transaction, transactionDate, reasonCodeTable, (Decimal)salesAffiliationLoyaltyTier.AffiliationId);
//        }

//        private void FillReasonCodeLines(IEnumerable<ReasonCodeLine> reasonCodeLines, SalesTransaction transaction, DateTimeOffset transactionDate, DataTable reasonCodeTable, Decimal parentLineNumber)
//        {
//            ThrowIf.Null<IEnumerable<ReasonCodeLine>>(reasonCodeLines, "reasonCodeLines");
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(reasonCodeTable, "reasonCodeTable");
//            foreach (ReasonCodeLine reasonCodeLine in reasonCodeLines)
//            {
//                DataRow row = reasonCodeTable.NewRow();
//                SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//                SalesOrderDataManager.SetField(row, "TRANSDATE", (object)transactionDate.Date);
//                SalesOrderDataManager.SetField(row, "TRANSTIME", (object)(int)transactionDate.TimeOfDay.TotalSeconds);
//                // SalesOrderDataManager.SetField(row, "LINENUM", (object)SalesOrderDataManager.GetNextLineNumber(reasonCodeTable));
//                SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//                SalesOrderDataManager.SetField(row, "TYPE", (object)reasonCodeLine.LineType);
//                SalesOrderDataManager.SetField(row, "INFOCODEID", (object)StringDataHelper.TruncateString(reasonCodeLine.ReasonCodeId, 10));
//                SalesOrderDataManager.SetField(row, "INFORMATION", (object)StringDataHelper.TruncateString(reasonCodeLine.Information, 100));
//                SalesOrderDataManager.SetField(row, "INFOAMOUNT", (object)reasonCodeLine.InformationAmount);
//                SalesOrderDataManager.SetField(row, "STORE", (object)this.GetStoreId(transaction));
//                SalesOrderDataManager.SetField(row, "TERMINAL", (object)(transaction.TerminalId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "STAFF", (object)StringDataHelper.TruncateString(transaction.StaffId, 25));
//                SalesOrderDataManager.SetField(row, "ITEMTENDER", (object)StringDataHelper.TruncateString(reasonCodeLine.ItemTender, 10));
//                SalesOrderDataManager.SetField(row, "AMOUNT", (object)reasonCodeLine.Amount);
//                SalesOrderDataManager.SetField(row, "INPUTTYPE", (object)reasonCodeLine.InputType);
//                SalesOrderDataManager.SetField(row, "SUBINFOCODEID", (object)StringDataHelper.TruncateString(reasonCodeLine.SubReasonCodeId, 10));
//                SalesOrderDataManager.SetField(row, "STATEMENTCODE", (object)StringDataHelper.TruncateString(reasonCodeLine.StatementCode, 25));
//                SalesOrderDataManager.SetField(row, "SOURCECODE", (object)StringDataHelper.TruncateString(reasonCodeLine.SourceCode, 20));
//                SalesOrderDataManager.SetField(row, "SOURCECODE2", (object)StringDataHelper.TruncateString(reasonCodeLine.SourceCode2, 20));
//                SalesOrderDataManager.SetField(row, "SOURCECODE3", (object)StringDataHelper.TruncateString(reasonCodeLine.SourceCode3, 20));
//                SalesOrderDataManager.SetField(row, "PARENTLINENUM", (object)parentLineNumber);
//                reasonCodeTable.Rows.Add(row);
//            }
//        }

//        private void FillRewardPointLines(SalesTransaction transaction, DataTable rewardPointTable)
//        {
//            ThrowIf.Null<SalesTransaction>(transaction, "transaction");
//            ThrowIf.Null<DataTable>(rewardPointTable, "rewardPointTable");
//            foreach (LoyaltyRewardPointLine loyaltyRewardPointLine in transaction.LoyaltyRewardPointLines)
//            {
//                DataRow row = rewardPointTable.NewRow();
//                SalesOrderDataManager.SetField(row, "AFFILIATION", (object)loyaltyRewardPointLine.LoyaltyGroupRecordId);
//                SalesOrderDataManager.SetField(row, "CARDNUMBER", (object)loyaltyRewardPointLine.LoyaltyCardNumber);
//                SalesOrderDataManager.SetField(row, "CUSTACCOUNT", (object)(loyaltyRewardPointLine.CustomerAccount ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "ENTRYDATE", (object)loyaltyRewardPointLine.EntryDate.Date);
//                SalesOrderDataManager.SetField(row, "ENTRYTIME", (object)loyaltyRewardPointLine.EntryTime);
//                SalesOrderDataManager.SetField(row, "ENTRYTYPE", (object)loyaltyRewardPointLine.EntryType);
//                SalesOrderDataManager.SetField(row, "EXPIRATIONDATE", (object)DateTimeOffsetDataHelper.GetDateOrDefaultSqlDate(loyaltyRewardPointLine.ExpirationDate));
//                SalesOrderDataManager.SetField(row, "LINENUM", (object)loyaltyRewardPointLine.LineNumber);
//                SalesOrderDataManager.SetField(row, "LOYALTYTIER", (object)loyaltyRewardPointLine.LoyaltyTierRecordId);
//                SalesOrderDataManager.SetField(row, "RECEIPTID", (object)(transaction.ReceiptId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "REWARDPOINT", (object)loyaltyRewardPointLine.RewardPointRecordId);
//                SalesOrderDataManager.SetField(row, "REWARDPOINTAMOUNTQTY", (object)loyaltyRewardPointLine.RewardPointAmountQuantity);
//                SalesOrderDataManager.SetField(row, "STAFF", (object)StringDataHelper.TruncateString(transaction.StaffId, 25));
//                SalesOrderDataManager.SetField(row, "STOREID", (object)this.GetStoreId(transaction));
//                SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//                SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//                SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//                rewardPointTable.Rows.Add(row);
//            }
//        }

//        private IEnumerable<Address> GetOneTimeAddress(IEnumerable<string> transactionIds, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<string>>(transactionIds, "transactionIds");
//            if (!Enumerable.Any<string>(transactionIds))
//                return Enumerable.Empty<Address>();
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONADDRESSTRANSVIEW"
//            };
//            using (StringIdTableType stringIdTableType = new StringIdTableType(transactionIds, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@tvp_transactionIds"] = (object)stringIdTableType;
//                return (IEnumerable<Address>)this.ExecuteSelect<Address>(sqlPagedQuery);
//            }
//        }

//        private void FillSalesLines(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary1 = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            SalesLinesQueryCriteria criteria = new SalesLinesQueryCriteria();
//            criteria.TransactionIds = (IEnumerable<string>)dictionary1.Keys;
//            QueryResultSettings queryResultSettings = new QueryResultSettings(settings.ColumnSet, settings.Paging, new SortingInfo("LINENUM", false));
//            ReadOnlyCollection<SalesLine> entityCollection = this.Context.Runtime.Execute<EntityDataServiceResponse<SalesLine>>((Request)new GetSalesLinesDataRequest(criteria, queryResultSettings), this.Context).EntityCollection;
//            this.FillAddresses((IEnumerable<CommerceEntity>)entityCollection, settings);
//            Collection<ItemVariantInventoryDimension> collection = new Collection<ItemVariantInventoryDimension>();
//            foreach (SalesLine salesLine in entityCollection)
//            {
//                if (!string.IsNullOrEmpty(salesLine.ItemId) && !string.IsNullOrEmpty(salesLine.InventoryDimensionId))
//                    collection.Add(new ItemVariantInventoryDimension(salesLine.ItemId, salesLine.InventoryDimensionId));
//            }
//            Dictionary<ItemVariantInventoryDimension, ProductVariant> dictionary2 = new Dictionary<ItemVariantInventoryDimension, ProductVariant>();
//            if (Enumerable.Any<ItemVariantInventoryDimension>((IEnumerable<ItemVariantInventoryDimension>)collection))
//                dictionary2 = Enumerable.ToDictionary<ProductVariant, ItemVariantInventoryDimension>((IEnumerable<ProductVariant>)this.Context.Runtime.Execute<EntityDataServiceResponse<ProductVariant>>((Request)new GetProductVariantsDataRequest((IEnumerable<ItemVariantInventoryDimension>)collection), this.Context).EntityCollection, (Func<ProductVariant, ItemVariantInventoryDimension>)(key => new ItemVariantInventoryDimension(key.ItemId, key.InventoryDimensionId)));
//            foreach (SalesLine line in entityCollection)
//            {
//                string index = (string)line.GetProperty("TRANSACTIONID");
//                dictionary1[index].SalesLines.Add(line);
//                this.FillDiscountLines(line, settings);
//                if (!string.IsNullOrEmpty(line.ItemId) && !string.IsNullOrEmpty(line.InventoryDimensionId))
//                {
//                    ItemVariantInventoryDimension key = new ItemVariantInventoryDimension(line.ItemId, line.InventoryDimensionId);
//                    ProductVariant productVariant;
//                    if (dictionary2.TryGetValue(key, out productVariant))
//                        line.Variant = productVariant;
//                }
//                if (line.LineDiscount != new Decimal(0) && line.Quantity != new Decimal(0) && line.Price != new Decimal(0))
//                    line.LinePercentageDiscount = Math.Round(line.LineDiscount / line.Price * line.Quantity * new Decimal(100), 2);
//            }
//        }

//        private void FillInvoiceLines(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONORDERINVOICETRANSVIEW",
//                OrderBy = "LINENUM"
//            };
//            ReadOnlyCollection<SalesLine> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<SalesLine>(sqlPagedQuery);
//            }
//            ItemDataManager itemDataManager = new ItemDataManager(this.Context);
//            foreach (SalesLine salesLine in readOnlyCollection)
//            {
//                string index = (string)salesLine.GetProperty("TRANSACTIONID");
//                salesLine.ItemId = string.Empty;
//                salesLine.NetAmount = salesLine.Price = salesLine.TotalAmount = salesLine.InvoiceAmount;
//                salesLine.IsInvoiceLine = true;
//                salesLine.Quantity = new Decimal(1);
//                dictionary[index].SalesLines.Add(salesLine);
//            }
//        }

//        private void FillIncomeExpenseLines(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(order => order.Id), (Func<SalesOrder, SalesOrder>)(order => order));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONINCOMEEXPENSETRANSVIEW",
//                OrderBy = "LINENUM"
//            };
//            ReadOnlyCollection<IncomeExpenseLine> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<IncomeExpenseLine>(sqlPagedQuery);
//            }
//            foreach (IncomeExpenseLine incomeExpenseLine in readOnlyCollection)
//            {
//                if (incomeExpenseLine.AccountType == IncomeExpenseAccountType.Income)
//                    incomeExpenseLine.Amount = Decimal.Negate(incomeExpenseLine.Amount);
//                string index = Convert.ToString(incomeExpenseLine.GetProperty("TRANSACTIONID"));
//                dictionary[index].IncomeExpenseLines.Add(incomeExpenseLine);
//            }
//        }

//        private void FillTenderLines(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(order => order.Id), (Func<SalesOrder, SalesOrder>)(order => order));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONPAYMENTTRANSVIEW",
//                OrderBy = "LINENUM"
//            };
//            ReadOnlyCollection<TenderLine> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<TenderLine>(sqlPagedQuery);
//            }
//            foreach (TenderLine tenderLine in readOnlyCollection)
//            {
//                string index = Convert.ToString(tenderLine.GetProperty("TRANSACTIONID"));
//                dictionary[index].TenderLines.Add(tenderLine);
//            }
//        }

//        private void FillAddresses(IEnumerable<CommerceEntity> entities, QueryResultSettings settings)
//        {
//            Dictionary<long, bool> dictionary1 = new Dictionary<long, bool>();
//            List<string> list = new List<string>();
//            foreach (CommerceEntity commerceEntity in entities)
//            {
//                long key = (long)(commerceEntity.GetProperty("LOGISTICSPOSTALADDRESS") ?? (object)0);
//                if (key != 0L)
//                {
//                    if (!dictionary1.ContainsKey(key))
//                        dictionary1.Add(key, true);
//                }
//                else
//                {
//                    string str = (string)commerceEntity.GetProperty("TRANSACTIONID");
//                    list.Add(str);
//                }
//            }
//            Dictionary<long, Address> dictionary2 = Enumerable.ToDictionary<Address, long, Address>(this.customerDataManager.GetAddresses((IEnumerable<long>)dictionary1.Keys), (Func<Address, long>)(address => address.RecordId), (Func<Address, Address>)(address => address));
//            Dictionary<string, Address> dictionary3 = Enumerable.ToDictionary<Address, string, Address>(this.GetOneTimeAddress((IEnumerable<string>)list, settings), (Func<Address, string>)(address => string.Format("{0}_{1:00.000}", new object[2]
//      {
//        address.GetProperty("TRANSACTIONID"),
//        address.GetProperty("SALELINENUM")
//      })), (Func<Address, Address>)(address => address));
//            foreach (CommerceEntity commerceEntity in entities)
//            {
//                long key1 = (long)(commerceEntity.GetProperty("LOGISTICSPOSTALADDRESS") ?? (object)0);
//                Address address;
//                if (key1 != 0L)
//                {
//                    dictionary2.TryGetValue(key1, out address);
//                }
//                else
//                {
//                    string key2 = string.Format("{0}_{1:00.000}", new object[2]
//          {
//            commerceEntity.GetProperty("TRANSACTIONID"),
//            (object) (Decimal) (commerceEntity.GetProperty("LINENUM") ?? (object) new Decimal(0, 0, 0, false, (byte) 1))
//          });
//                    dictionary3.TryGetValue(key2, out address);
//                }
//                commerceEntity.SetProperty("SHIPPINGADDRESS",address);
//            }
//        }

//        private void FillReasonCodeLines(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONINFOCODETRANSVIEW",
//                OrderBy = "TRANSACTIONID, PARENTLINENUM, LINENUM"
//            };
//            ReadOnlyCollection<ReasonCodeLine> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<ReasonCodeLine>(sqlPagedQuery);
//            }
//            foreach (ReasonCodeLine reasonCodeLine in readOnlyCollection)
//            {
//                string transactionId = reasonCodeLine.TransactionId;
//                SalesOrder salesOrder = dictionary[transactionId];
//                Decimal parentLineNumber = Decimal.Parse(reasonCodeLine.ParentLineId);
//                if (parentLineNumber == new Decimal(-1))
//                {
//                    salesOrder.ReasonCodeLines.Add(reasonCodeLine);
//                }
//                else
//                {
//                    switch (reasonCodeLine.LineType)
//                    {
//                        case ReasonCodeLineType.Sales:
//                            SalesLine salesLine = Enumerable.SingleOrDefault<SalesLine>(Enumerable.Where<SalesLine>((IEnumerable<SalesLine>)salesOrder.SalesLines, (Func<SalesLine, bool>)(o => o.LineNumber == parentLineNumber)));
//                            if (salesLine == null)
//                                throw new DataValidationException(DataValidationErrors.IdMismatch, string.Format("Failed to map reason code to sales line. Line number {0} not found.", (object)reasonCodeLine.ParentLineId), new object[0]);
//                            salesLine.ReasonCodeLines.Add(reasonCodeLine);
//                            continue;
//                        case ReasonCodeLineType.Affiliation:
//                            AffiliationLoyaltyTier affiliationLoyaltyTier = (AffiliationLoyaltyTier)Enumerable.SingleOrDefault<SalesAffiliationLoyaltyTier>(Enumerable.Where<SalesAffiliationLoyaltyTier>((IEnumerable<SalesAffiliationLoyaltyTier>)salesOrder.AffiliationLoyaltyTierLines, (Func<SalesAffiliationLoyaltyTier, bool>)(a => (Decimal)a.AffiliationId == parentLineNumber)));
//                            if (affiliationLoyaltyTier == null)
//                                throw new DataValidationException(DataValidationErrors.IdMismatch, string.Format("Failed to map reason code to affiliationLoyaltyTier line. AffiliationLoyaltyTier Idd {0} not found.", (object)reasonCodeLine.ParentLineId), new object[0]);
//                            affiliationLoyaltyTier.ReasonCodeLines.Add(reasonCodeLine);
//                            continue;
//                        default:
//                            continue;
//                    }
//                }
//            }
//        }

//        private void FillDiscountLines(SalesLine line, QueryResultSettings settings)
//        {
//            ThrowIf.Null<SalesLine>(line, "salesLine");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            DiscountLinesQueryCriteria criteria = new DiscountLinesQueryCriteria();
//            criteria.TransactionId = Convert.ToString(line.GetProperty("TRANSACTIONID"));
//            criteria.LineNumber = line.LineNumber;
//            QueryResultSettings queryResultSettings = new QueryResultSettings(settings.ColumnSet, settings.Paging, new SortingInfo("LINENUM", false));
//            foreach (DiscountLine discountLine in this.Context.Runtime.Execute<EntityDataServiceResponse<DiscountLine>>((Request)new GetDiscountLinesDataRequest(criteria, queryResultSettings), this.Context).EntityCollection)
//                line.DiscountLines.Add(discountLine);
//        }

//        private void FillOrderAttributes(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONATTRIBUTETRANSVIEW",
//                OrderBy = "NAME",
//                IsQueryByPrimaryKey = false
//            };
//            ReadOnlyCollection<AttributeTextValue> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<AttributeTextValue>(sqlPagedQuery);
//            }
//            foreach (AttributeTextValue attributeTextValue in readOnlyCollection)
//            {
//                string index = (string)attributeTextValue.GetProperty("TRANSACTIONID");
//                dictionary[index].AttributeValues.Add((AttributeValueBase)attributeTextValue);
//            }
//        }

//        private void FillTransactionProperties(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "TRANSACTIONPROPERTIESVIEW",
//                OrderBy = "SALELINENUM"
//            };
//            ReadOnlyCollection<TransactionProperty> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<TransactionProperty>(sqlPagedQuery);
//            }
//            foreach (TransactionProperty transactionProperty in readOnlyCollection)
//            {
//                SalesOrder salesOrder = dictionary[transactionProperty.TransactionId];
//                if (transactionProperty.IsHeaderProperty)
//                    salesOrder.PersistentProperties[transactionProperty.Name] = (object)transactionProperty.Value;
//                else
//                    salesOrder.SalesLines[(int)transactionProperty.SalesLineNumber - 1].PersistentProperties[transactionProperty.Name] = (object)transactionProperty.Value;
//            }
//        }

//        private void FillAffiliation(SalesAffiliationLoyaltyTier salesAffiliationLoyaltyTier, SalesTransaction transaction, DateTimeOffset transactionDate, DataTable affiliationsTable, DataTable reasonCodeTable)
//        {
//            DataRow row = affiliationsTable.NewRow();
//            SalesOrderDataManager.SetField(row, "AFFILIATION", (object)salesAffiliationLoyaltyTier.AffiliationId);
//            SalesOrderDataManager.SetField(row, "LOYALTYTIER", (object)salesAffiliationLoyaltyTier.LoyaltyTierId);
//            SalesOrderDataManager.SetField(row, "TRANSACTIONID", (object)StringDataHelper.TruncateString(transaction.Id, 44));
//            SalesOrderDataManager.SetField(row, "TERMINALID", (object)(transaction.TerminalId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "RECEIPTID", (object)(transaction.ReceiptId ?? string.Empty));
//            SalesOrderDataManager.SetField(row, "STAFF", (object)StringDataHelper.TruncateString(transaction.StaffId, 25));
//            SalesOrderDataManager.SetField(row, "STOREID", (object)this.GetStoreId(transaction));
//            SalesOrderDataManager.SetField(row, "DATAAREAID", (object)this.ChannelConfiguration.InventLocationDataAreaId);
//            affiliationsTable.Rows.Add(row);
//            this.FillReasonCodesForAffiliationLine(salesAffiliationLoyaltyTier, transaction, transactionDate, reasonCodeTable);
//        }

//        private void FillOrderAffiliations(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONAFFILIATIONTRANSVIEW",
//                IsQueryByPrimaryKey = false
//            };
//            ReadOnlyCollection<SalesAffiliationLoyaltyTier> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<SalesAffiliationLoyaltyTier>(sqlPagedQuery);
//            }
//            foreach (SalesAffiliationLoyaltyTier affiliationLoyaltyTier in readOnlyCollection)
//            {
//                string index = (string)affiliationLoyaltyTier.GetProperty("TRANSACTIONID");
//                dictionary[index].AffiliationLoyaltyTierLines.Add(affiliationLoyaltyTier);
//            }
//        }

//        private void FillOrderChargeLines(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            SqlPagedQuery sqlPagedQuery = new SqlPagedQuery( new QueryResultSettings(PagingInfo.AllRecords))
//            {
//                Select = settings.ColumnSet,
//                //Paging = settings.Paging, KAR
//                From = "RETAILTRANSACTIONMARKUPTRANSVIEW",
//                OrderBy = "TRANSACTIONID, SALELINENUM, MARKUPLINENUM"
//            };
//            ReadOnlyCollection<ChargeLine> readOnlyCollection;
//            using (StringIdTableType stringIdTableType = new StringIdTableType((IEnumerable<string>)dictionary.Keys, "TRANSACTIONID"))
//            {
//                sqlPagedQuery.Parameters["@TVP_TRANSACTIONIDTABLETYPE"] = (object)stringIdTableType;
//                readOnlyCollection = this.ExecuteSelect<ChargeLine>(sqlPagedQuery);
//            }
//            foreach (ChargeLine chargeLine in readOnlyCollection)
//            {
//                string transactionId = chargeLine.TransactionId;
//                SalesOrder salesOrder = dictionary[transactionId];
//                Decimal saleLineNumber = chargeLine.SaleLineNumber;
//                if (saleLineNumber != new Decimal(0))
//                {
//                    SalesLine salesLine = Enumerable.FirstOrDefault<SalesLine>((IEnumerable<SalesLine>)salesOrder.SalesLines, (Func<SalesLine, bool>)(o => o.LineNumber == saleLineNumber));
//                    if (salesLine == null)
//                        throw new DataValidationException(DataValidationErrors.IdMismatch, string.Format("Failed to map charge to sales line. Line number {0} not found.", (object)chargeLine.SaleLineNumber), new object[0]);
//                    salesLine.ChargeLines.Add(chargeLine);
//                }
//            }
//        }

//        private void FillLoyaltyRewardPointLines(IEnumerable<SalesOrder> orders, QueryResultSettings settings)
//        {
//            ThrowIf.Null<IEnumerable<SalesOrder>>(orders, "orders");
//            ThrowIf.Null<QueryResultSettings>(settings, "settings");
//            if (!Enumerable.Any<SalesOrder>(orders))
//                return;
//            Dictionary<string, SalesOrder> dictionary = Enumerable.ToDictionary<SalesOrder, string, SalesOrder>(orders, (Func<SalesOrder, string>)(x => x.Id), (Func<SalesOrder, SalesOrder>)(x => x));
//            LoyaltyRewardPointLinesQueryCriteria criteria = new LoyaltyRewardPointLinesQueryCriteria();
//            criteria.TransactionIds = (IEnumerable<string>)dictionary.Keys;
//            QueryResultSettings queryResultSettings = new QueryResultSettings(settings.ColumnSet, settings.Paging);
//            foreach (LoyaltyRewardPointLine loyaltyRewardPointLine in this.Context.Runtime.Execute<EntityDataServiceResponse<LoyaltyRewardPointLine>>((Request)new GetLoyaltyRewardPointLinesDataRequest(criteria, queryResultSettings), this.Context).EntityCollection)
//            {
//                string index = (string)loyaltyRewardPointLine.GetProperty("TRANSACTIONID");
//                dictionary[index].LoyaltyRewardPointLines.Add(loyaltyRewardPointLine);
//            }
//        }
//    }
//}
