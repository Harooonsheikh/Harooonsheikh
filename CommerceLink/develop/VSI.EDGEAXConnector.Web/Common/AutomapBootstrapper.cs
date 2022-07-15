using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Mapster;
using VSI.CommerceLink.EcomDataModel;
using VSI.CommerceLink.EcomDataModel.Enum;
using VSI.CommerceLink.EcomDataModel.Request;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Web.Controllers;
using static VSI.EDGEAXConnector.Web.Controllers.DiscountController;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// Singleton class to get instance of AutoMapper
    /// </summary>
    public sealed class AutoMapBootstrapper
    {
        private static readonly Lazy<IMapper>
            LazyInstance = new Lazy<IMapper>
                    (() => 
            {
                var config = GetConfig();
                config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
                config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
                return new MapsterMapper.Mapper(config);
            });

        /// <summary>
        /// Gets the instance of Automapper
        /// </summary>
        public static IMapper MapperInstance => LazyInstance.Value;

        /// <inheritdoc />
        private AutoMapBootstrapper()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private static TypeAdapterConfig GetConfig()
        {
            TypeAdapterConfig config = new TypeAdapterConfig();
            config.ForType<EcomAddress, ErpAddress>();
            config.ForType<EcomAddressType, ErpAddressType>();
            config.ForType<EcomCommerceProperty, ErpCommerceProperty>();
            config.ForType<EcomCommercePropertyValue, ErpCommercePropertyValue>();
            config.ForType<EcomCustomer, ErpCustomer>();
            config.ForType<EcomContactPerson, ErpContactPerson>();
            config.ForType<EcomCustomerType, ErpCustomerType>();
            config.ForType<EcomRichMediaLocations, ErpRichMediaLocations>();
            config.ForType<EcomRichMediaLocationsRichMediaLocation, ErpRichMediaLocationsRichMediaLocation>();
            config.ForType<EcomCart, ErpCart>();
            config.ForType<EcomCalculationModes, ErpCalculationModes>();
            config.ForType<EcomCartLine, ErpCartLine>();
            config.ForType<EcomAddUpdateCartLine, ErpCartLine>();
            config.ForType<EcomDeliverySpecification, ErpDeliverySpecification>();

            config.ForType<EcomCustomerAttribute, ErpCustomerAttribute>();
            config.ForType<EcomCommercePropertyValue, ErpCommercePropertyValue>();
            config.ForType<EcomAffiliation, ErpAffiliationLoyaltyTier>();

            config.ForType<EcomCustomerUpdateRequest, CustomerController.CustomerCreateRequest>();
            config.ForType<EcomCustomerCreateRequest, CustomerController.CustomerCreateRequest>();
            config.ForType<EcomCartRequest, CartController.CartRequest>();
            config.ForType<EcomCartLinesRequest, CartController.CartLinesRequest>();
            config.ForType<EcomAddUpdateCartLinesRequest, CartController.CartLinesRequest>();
            config.ForType<EcomAddUpdateCartLine, EcomCartLine>();
            config.ForType<MergeCustomerResellerRequest, EcomCustomerCreateRequest>();

            config.ForType<ErpContactPerson, ContactPersonController.eComContactPerson>();
            config.ForType<ErpAddress, ContactPersonController.eComAddress>();
            config.ForType<ErpContactPersonNAL, ContactPersonController.ContactPersonInfo>();
            config.ForType<ErpContactPerson, ContactPersonController.ContactPersonRequest>();
            config.ForType<ErpAddress, ContactPersonController.Address>();
            config.ForType<ContactPersonController.eComContactPerson, ErpContactPerson>();
            config.ForType<ErpContactPerson, EcomContactPerson>();
            config.ForType<ErpAddress, EcomAddress>();
            config.ForType<ErpSalesOrder, ErpSalesOrderSmallResponse>();
            config.ForType<ErpSalesLine, ErpSalesLineSmallResponse>();
            config.ForType<ErpCreditCardCust, ErpCreditCardCustSmallResponse>();
            config.ForType<EcomProcessContractTerminateRequest, ErpProcessContractTerminateRequest>()
                .Map(d => d.SalesLineRecIds,
                    s => s.SalesLineRecIds == null ? "" :
                            String.Join(",", s.SalesLineRecIds.Select(x => x.RecId.ToString()).ToArray())
                    );
            config.ForType<EcomProcessContractReactivateRequest, ErpProcessContractReactivateRequest>()
                .Map(d => d.SalesLineRecIds,
                    s => s.SalesLineRecIds == null ? "" :
                            String.Join(",", s.SalesLineRecIds.Select(x => x.RecId.ToString()).ToArray())
                    );
            config.ForType<EcomSalesOrder, ErpSalesOrder>();
            config.ForType<EcomSalesLine, ErpSalesLine>();
            config.ForType<EcomDiscountLine, ErpDiscountLine>();
            config.ForType<EcomCustomerOrderInfo, ErpCustomerOrderInfo>();
            config.ForType<EcomItemInfo, ErpItemInfo>();
            config.ForType<EcomTaxInfo, ErpTaxInfo>();
            config.ForType<EcomDiscountInfo, ErpDiscountInfo>();

            config.ForType<ContactPersonCrm, ErpContactPersonNAL>()
                .Map(d => d.AddressName, s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().AddressName)
                .Map(d => d.City, s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().City)
                .Map(d => d.Country, s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().Country)
                .Map(d => d.State, s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().State)
                .Map(d => d.Street, s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().Street)
                .Map(d => d.Street2, s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().Street2)
                .Map(d => d.ZipCode, s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().ZipCode);

            config.ForType<ContactPersonController.CreateUpdateContactPersonRequest, ContactPersonController.SaveContactPersonRequest>();

            config.ForType<ErpContactPersonNAL, ContactPersonCrm>()
                .Map(d => d.Addresses, (s => new List<ContactPersonAddress>()
                    {
                        new ContactPersonAddress
                        {
                            AddressName = s.AddressName,
                            City = s.City,
                            Country = s.Country,
                            State = s.State,
                            Street = s.Street,
                            Street2 = s.Street2,
                            ZipCode = s.ZipCode
                        }
                    }
                ));

            config.ForType<PaymentLinkController.AddPaymentLinkForInvoiceRequest, ErpAddPaymentLinkForInvoiceBoletoRequest>();
            config.ForType<QuotationController.PaymentDetail, ErpPaymentDetail>();
            config.ForType<QuotationController.ConfirmQuotationRequest, ErpConfirmQuotationRequest>();

            config.ForType<ErpProductDiscountWithAffiliation, ProductDiscountWithAffiliation>();

            return config;
        }
    }
}