using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VSI.EDGEAXConnector.UI.ClassGenerator
{
    public class ClassGeneratorHelper
    {
        public void GenerateClasses()
        {
            //var assembly = Assembly.GetAssembly(typeof(Microsoft.Dynamics.Commerce.Runtime.DataModel.Customer));
            //var classes = assembly.GetTypes().Where(t => t.IsClass &&
            //  (
            //   t == typeof(ProductDimensionValueDictionary) ||
            //   t == typeof(ProductDimensionSet) ||
            //   t == typeof(ProductDimensionValueSet) ||
            //   t == typeof(ProductPropertyTranslation) ||
            //   t == typeof(ProductPropertyTranslationDictionary)
            //   )).ToList();
            //classes.ForEach(c =>
            //{
            //    DataModelGenerator.GenerateClasses(c, "VSI.EDGEAXConnector.ERPDataModels");
            //    //var types = c.GetProperties();
            //    //types.ToList().ForEach(t =>
            //    //{
            //    //    if (t.PropertyType.Namespace == "Microsoft.Dynamics.Commerce.Runtime.DataModel")
            //    //    {
            //    //        DataModelGenerator.GenerateClasses(t.PropertyType, "VSI.EDGEAXConnector.ERPDataModels");
            //    //    }
            //    //});

            //});
        }
    }
}