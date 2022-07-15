using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ERPOfferTypeGroup
    {

        #region Properties

        public decimal DISPLAYORDER
        {
            get; set;
        }
        public string NOINBARCODE
        {
            get; set;
        }
        public string STYLE
        {
            get; set;
        }
        public string STYLEGROUP
        {
            get; set;
        }
        public int WEIGHT
        {
            get; set;
        }
        public int TMVMAINOFFERTYPE
        {
            get; set;
        }
        public int TMVFREELICENSE
        {
            get; set;
        }
        #endregion Properties
    }
}
