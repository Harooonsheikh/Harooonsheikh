namespace VSI.EDGEAXConnector.Mapper.TransformationAdapter
{
    public class TransformationAdapter<TSource, TDestination> : ITransform<TSource, TDestination>
    {
        public TDestination Transform(TSource source)
        {
            return AutoMapBootstrapper.MapperInstance.Map<TSource, TDestination>(source);
        }
    }
}
