using System;
using Mapster;
using MapsterMapper;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter
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
                var cfg = new TypeAdapterConfig();
                cfg.ForType<ErpCustomer, ErpCustomer>();
                cfg.ForType<ErpAddress, ErpAddress>()
                    .Map(x => x.EntityName, src => "Address");
                cfg.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
                cfg.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
                return new Mapper(cfg);
            });

        /// <summary>
        /// Gets the instance of AutoMapper
        /// </summary>
        public static IMapper MapperInstance => LazyInstance.Value;

        /// <inheritdoc />
        private AutoMapBootstrapper()
        {

        }
    }
}
