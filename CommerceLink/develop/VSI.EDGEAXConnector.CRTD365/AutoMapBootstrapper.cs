using System;
using Mapster;
using MapsterMapper;

namespace VSI.EDGEAXConnector.CRTD365
{
    /// <summary>
    /// Singleton class to get instance of AutoMapper
    /// </summary>
    public sealed class AutoMapBootstrapper
    {
        private static readonly Lazy<IMapper>
            MapsterLazyInstance = new Lazy<IMapper>
            (() =>
            {
                var config = MapsterConfig.GetCrtConfig();
                config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
                config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);

                return new Mapper(config);
            });

        /// <summary>
        /// Gets the instance of Mapster
        /// </summary>
        public static IMapper MapsterInstance => MapsterLazyInstance.Value;

        /// <inheritdoc />
        private AutoMapBootstrapper()
        {

        }
    }
}
