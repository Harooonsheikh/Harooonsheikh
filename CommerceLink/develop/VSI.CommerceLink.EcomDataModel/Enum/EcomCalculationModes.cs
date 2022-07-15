using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Enum
{
    public enum EcomCalculationModes
    {
        None = 0,
        Prices = 1,
        Discounts = 2,
        Charges = 4,
        Taxes = 8,
        Totals = 16,
        Deposit = 32,
        AmountDue = 64,
        All = 127
    }
}
