namespace VSI.EDGEAXConnector.Mapper.TransformationAdapter
{
    public interface ITransform<S,T>
    {
        T Transform(S source);      
    }
}
