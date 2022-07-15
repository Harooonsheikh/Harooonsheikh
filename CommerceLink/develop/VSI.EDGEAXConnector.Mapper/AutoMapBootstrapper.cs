using System;
using Mapster;
using MapsterMapper;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.Mapper
{
    /// <summary>
    /// Singleton class to get instance of AutoMapper
    /// </summary>
    public sealed class AutoMapBootstrapper
    {
        private static readonly Lazy<IMapper>
            LazyInstance = new Lazy<IMapper>
            (() => {
                var config = GetConfig();
                config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
                config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
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
            config.ForType<ErpCustomer, EcomcustomerCustomerEntity>();
            config.ForType<EcomcustomerCustomerEntity, ErpCustomer>();
            config.ForType<ErpAddress, EcomcustomerAddressEntityItem>();
            config.ForType<EcomcustomerCustomerEntity, ErpCustomer>();
            return config;
        }
    }
}
