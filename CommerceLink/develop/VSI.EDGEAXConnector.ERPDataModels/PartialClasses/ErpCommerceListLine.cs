﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpCommerceListLine
    {
        [Required]
        public string ItemId { get; set; }//;
    }
}
