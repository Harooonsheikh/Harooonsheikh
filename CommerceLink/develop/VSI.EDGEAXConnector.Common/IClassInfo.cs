using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Common
{
    public interface IClassInfo
    {
        List<Type> ClassesToExpose { get; }
        List<Type> GetClassesInfo();
        List<Type> FilterClasses(List<Type> classes);
        List<Type> GetComplexProperties(Type type);
    }
}
