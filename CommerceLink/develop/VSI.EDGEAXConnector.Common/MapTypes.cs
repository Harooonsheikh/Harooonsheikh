using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Common
{
    public enum MapTypes
    {
        None = 1,
        SimpleToSimple = 2,
        ObjectToObject = 3,
        CollectionToCollection = 4,
        ObjectToCollection = 5,
        CollectionToObject = 6,
        FieldUsingDefault = 7,
        FieldUsingConstant = 8,
        CustomExpression = 9,
        KeyMapping = 10,
        BooleanValues = 11

    }
    public enum MapDirection
    {
        ErpToEcom,
        EcomToErp
    }
}
