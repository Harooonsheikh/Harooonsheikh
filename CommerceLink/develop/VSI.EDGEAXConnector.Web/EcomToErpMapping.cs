using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using VSI.CommerceLink.EcomDataModel;
using VSI.CommerceLink.EcomDataModel.Enum;
using VSI.CommerceLink.EcomDataModel.Request;
using VSI.CommerceLink.EcomDataModel.Response;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Web.Controllers;

namespace VSI.EDGEAXConnector.Web
{
    public class EcomToErpMappingConfiguration : Profile
    {
        public EcomToErpMappingConfiguration()
        {
            Configure();
        }

        protected override void Configure()
        {
            #region Ecom To Erp mapping

            AutoMapper.Mapper.CreateMap<EcomAddress, ErpAddress>();
            AutoMapper.Mapper.CreateMap<EcomAddressType, ErpAddressType>();
            AutoMapper.Mapper.CreateMap<EcomCommerceProperty, ErpCommerceProperty>();
            AutoMapper.Mapper.CreateMap<EcomCommercePropertyValue, ErpCommercePropertyValue>();
            AutoMapper.Mapper.CreateMap<EcomCustomer, ErpCustomer>();
            AutoMapper.Mapper.CreateMap<EcomContactPerson, ErpContactPerson>();
            AutoMapper.Mapper.CreateMap<EcomCustomerType, ErpCustomerType>();
            AutoMapper.Mapper.CreateMap<EcomRichMediaLocations, ErpRichMediaLocations>();
            AutoMapper.Mapper.CreateMap<EcomRichMediaLocationsRichMediaLocation, ErpRichMediaLocationsRichMediaLocation>();
            AutoMapper.Mapper.CreateMap<EcomCart, ErpCart>();
            AutoMapper.Mapper.CreateMap<EcomCalculationModes, ErpCalculationModes>();
            AutoMapper.Mapper.CreateMap<EcomCartLine, ErpCartLine>();
            AutoMapper.Mapper.CreateMap<EcomAddUpdateCartLine, ErpCartLine>();
            AutoMapper.Mapper.CreateMap<EcomDeliverySpecification, ErpDeliverySpecification>();

            AutoMapper.Mapper.CreateMap<EcomCustomerAttribute, ErpCustomerAttribute>();
            AutoMapper.Mapper.CreateMap<EcomCommercePropertyValue, ErpCommercePropertyValue>();
            AutoMapper.Mapper.CreateMap<EcomAffiliation, ErpAffiliationLoyaltyTier>();

            #endregion

            AutoMapper.Mapper.CreateMap<EcomCustomerUpdateRequest, CustomerController.CustomerCreateRequest>();
            AutoMapper.Mapper.CreateMap<EcomCustomerCreateRequest, CustomerController.CustomerCreateRequest>();
            AutoMapper.Mapper.CreateMap<EcomCartRequest, CartController.CartRequest>();
            AutoMapper.Mapper.CreateMap<EcomCartLinesRequest, CartController.CartLinesRequest>();
            AutoMapper.Mapper.CreateMap<EcomAddUpdateCartLinesRequest, CartController.CartLinesRequest>();
            AutoMapper.Mapper.CreateMap<EcomAddUpdateCartLine, EcomCartLine>();
            AutoMapper.Mapper.CreateMap<MergeCustomerResellerRequest, EcomCustomerCreateRequest>();
            AutoMapper.Mapper.CreateMap<EcomUpdateSubscriptionContractRequest, ErpUpdateSubscriptionContractRequest>();
            AutoMapper.Mapper.CreateMap<EcomCreateContractNewPaymentMethodRequest, ErpCreateContractNewPaymentMethod>();
            AutoMapper.Mapper.CreateMap<EcomCreateContractNewPaymentMethodCustomer, ErpCreateContractNewPaymentMethodCustomer>();
            AutoMapper.Mapper.CreateMap<EcomTenderLine, ErpTenderLine>();
            AutoMapper.Mapper.CreateMap<EcomAddress, ErpAddress>();
            AutoMapper.Mapper.CreateMap<EcomUpdateSubscriptionContractRequest, ErpUpdateSubscriptionContractRequest>();
            AutoMapper.Mapper.CreateMap<EcomCoupon, ErpCoupon>();
            AutoMapper.Mapper.CreateMap<EcomContractLine, ErpContractLine>();
            AutoMapper.Mapper.CreateMap<ErpUpdateSubscriptionContractResponse, UpdateSubscriptionContractResponse>();
            AutoMapper.Mapper.CreateMap<EcomProcessContractTerminateRequest, ErpProcessContractTerminateRequest>()
                .ForMember(d => d.SalesLineRecIds,
                    a => a.MapFrom(
                        s => s.SalesLineRecIds == null ? "" :
                            String.Join(",", s.SalesLineRecIds.Select(x => x.RecId.ToString()).ToArray())
                    ));

            AutoMapper.Mapper.CreateMap<EcomSalesOrder, ErpSalesOrder>();
            AutoMapper.Mapper.CreateMap<EcomSalesLine, ErpSalesLine>();
            AutoMapper.Mapper.CreateMap<EcomDiscountLine, ErpDiscountLine>();

            AutoMapper.Mapper.CreateMap<EcomCustomerOrderInfo, ErpCustomerOrderInfo>();
            AutoMapper.Mapper.CreateMap<EcomItemInfo, ErpItemInfo>();
            AutoMapper.Mapper.CreateMap<EcomTaxInfo, ErpTaxInfo>();
            AutoMapper.Mapper.CreateMap<EcomDiscountInfo, ErpDiscountInfo>();

            AutoMapper.Mapper.CreateMap<ContactPersonCrm, ErpContactPersonNAL>()
                .ForMember(d => d.AddressName, a => a.MapFrom(s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().AddressName))
                .ForMember(d => d.City, a => a.MapFrom(s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().City))
                .ForMember(d => d.Country, a => a.MapFrom(s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().Country))
                .ForMember(d => d.State, a => a.MapFrom(s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().State))
                .ForMember(d => d.Street, a => a.MapFrom(s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().Street))
                .ForMember(d => d.Street2, a => a.MapFrom(s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().Street2))
                .ForMember(d => d.ZipCode, a => a.MapFrom(s =>
                    s.Addresses == null ? null : s.Addresses.FirstOrDefault().ZipCode));

            AutoMapper.Mapper.CreateMap<ErpContactPersonNAL, ContactPersonCrm>()
                .ForMember(d => d.Addresses, a => a.MapFrom(s => new List<ContactPersonAddress>()
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

            AutoMapper.Mapper.CreateMap<ContactPersonController.CreateUpdateContactPersonRequest, ContactPersonController.SaveContactPersonRequest>();
        }

    }
}

