using Mapster;
using MapsterMapper;
using System;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.WindowService
{
    public sealed class AutoMapBootstrapper
    {
        private static readonly Lazy<IMapper>
            MapsterLazyInstance = new Lazy<IMapper>
            (() =>
            {
                var config = GetConfig();
                config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
                config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);

                return new MapsterMapper.Mapper(config);
            });

        /// <summary>
        /// Gets the instance of Mapster
        /// </summary>
        public static IMapper MapsterInstance => MapsterLazyInstance.Value;

        /// <inheritdoc />
        private AutoMapBootstrapper()
        {

        }

        public static TypeAdapterConfig GetConfig()
        {
            TypeAdapterConfig config = new TypeAdapterConfig();

            config.ForType<ErpSalesOrder, IngramSalesOrder>();
            config.ForType<ErpSalesLine, IngramSalesLine>();

            return config;
        }
    }
}
