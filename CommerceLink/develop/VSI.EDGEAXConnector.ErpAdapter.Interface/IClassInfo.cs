using System;
using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IClassInfo
    {
        List<Type> ClassesToExpose { get; }
        List<Type> GetClassesInfo();
        List<Type> FilterClasses(List<Type> classes);
        List<Type> GetComplexProperties(Type type);
    }
}