namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class AosJsonServiceRequest<T>
    {
        public T _jsonObject { get; set; }

        public AosJsonServiceRequest(T data)
        {
            _jsonObject = data;
        }
    }
}
