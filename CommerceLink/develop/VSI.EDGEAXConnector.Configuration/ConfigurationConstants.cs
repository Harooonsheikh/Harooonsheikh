using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Configuration
{
    public static class ConfigurationConstants
    {
        public const bool isMFF = true;
        public const bool IsDefaultInventoryFlow = true;
        public const bool IsdefaultPricingFlow = true;
        public const bool ProcessProductWithCategory = true;
        public const bool ProcessImage = false;
        public const bool LoadAdditionalProductImageFiles = false;
        public const bool GetOrderShipments = false;
        public const string SaleOrderParamID = "MF_";


    }
}
