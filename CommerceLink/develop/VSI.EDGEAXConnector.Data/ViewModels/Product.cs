using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class Product
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ItemId { get; set; }
        public string ProductId { get; set; }
        public string eComProductId { get; set; }
        public string Name { get; set; }
        public Nullable<int> CustomAttributes { get; set; }
        public Nullable<int> StoreId { get; set; }
    }
}
