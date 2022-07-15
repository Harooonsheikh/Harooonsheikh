using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class UpdateParameterRequest
    {
        public Asset asset { get; set; } = new Asset();
    }

    public class Asset
    {
        public List<Parameter> @params { get; set; } = new List<Parameter>();
    }

    public class Parameter
    {
        public Parameter(string id = "", string valueError = "")
        {
            this.id = id;
            value_error = valueError;
        }
        

        public string id { get; set; }
        
        public string value_error { get; set; }

    }
}
