using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    class AutoIncrement
    {

    }
    public class Value
    {
        public string collectionname { get; set; }
        public string CounterName { get; set; }
        public int CounterValue { get; set; }
        public int CounterSeed { get; set; }
        public string CounterPrefix { get; set; }
    }

    public class RootObject
    {
        public string Name { get; set; }
        public Value Value { get; set; }
    }
}
