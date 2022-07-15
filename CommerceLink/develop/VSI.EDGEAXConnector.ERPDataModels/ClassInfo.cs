using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.Common;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ClassInfo : IClassInfo
    {
        public List<Type> ClassesToExpose
        {
            get { throw new NotImplementedException(); }
        }

        public List<Type> GetClassesInfo()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t=>t.IsClass).ToList();
        }

        public List<Type> FilterClasses(List<Type> classes)
        {
            throw new NotImplementedException();
        }

        public List<Type> GetComplexProperties(Type type)
        {
            throw new NotImplementedException();
        }
    }
}