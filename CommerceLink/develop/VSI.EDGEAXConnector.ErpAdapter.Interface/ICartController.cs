using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    /// <summary>
    /// ICartController interface defines features to cart
    /// </summary>
    public interface ICartController
    {
        ErpCartResponse CreateMergedCart(string cartId, long affiliationId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, ErpDeliverySpecification deliverySpecification, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string requestId);
        //ErpCartResponse CreateCartWithItemsAndCoupon(string cartId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, IEnumerable<string> couponCodes, bool isLegacyDiscountCode);
        ErpCartResponse GetCart(string cartId);
        ErpCartResponse CreateOrUpdateCart(ErpCart cart, ErpCalculationModes calculationModes, bool isUpdate);
        ErpCartResponse AddCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes, long cartVersion);
        ErpCartResponse VoidCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes);
        ErpCartResponse RemoveCartLines(string cartId, IEnumerable<string> lineIds, ErpCalculationModes calculationModes,string requestId);
        ErpCartResponse UpdateCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes,string requestId);
        ErpCartResponse AddCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string requestId);
        ErpCartResponse RemoveCouponsFromCart(string cartId, IEnumerable<string> couponCodes);
        ErpCartResponse AddTenderLine(string cartId, ErpCartTenderLine cartTenderLine, long cartVersion);
        ErpCartResponse AddPreprocessedTenderLine(string cartId, ErpTenderLine preprocessedTenderLine, long cartVersion);
        ErpCartResponse UpdateDeliverySpecification(string cartId, ErpDeliverySpecification deliverySpecification);

        List<ErpCart> GetAbandonedCarts();
    }
}
