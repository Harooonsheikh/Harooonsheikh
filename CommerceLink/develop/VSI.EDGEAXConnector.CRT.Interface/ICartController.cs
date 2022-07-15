using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    /// <summary>
    /// ICartController interface defines features to cart
    /// </summary>
    public interface ICartController
    {

        //ErpCartResponse CreateCartWithLinesAndDelivery(string cartId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, IEnumerable<string> couponCodes, bool isLegacyDiscountCode);
        ErpCartResponse CreateMergedCart(string cartId, long affiliationId, ErpCalculationModes calculationModes, IEnumerable<ErpCartLine> cartLines, ErpDeliverySpecification deliverySpecification, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string requestId);

        ErpCartResponse GetCart(string cartId);
        ErpCartResponse CreateOrUpdateCart(ErpCart cart, ErpCalculationModes calculationModes);
        ErpCartResponse AddCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes, long cartVersion);
        ErpCartResponse VoidCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes);
        ErpCartResponse RemoveCartLines(string cartId, IEnumerable<string> lineIds, ErpCalculationModes calculationModes, string requestId);
        ErpCartResponse UpdateCartLines(string cartId, IEnumerable<ErpCartLine> cartLines, ErpCalculationModes calculationModes);
        ErpCartResponse AddCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode,string requestId);
        ErpCartResponse RemoveCouponsFromCart(string cartId, IEnumerable<string> couponCodes);
        ErpCartResponse AddDiscountCodesToCart(string cartId, IEnumerable<string> dicountCodes);
        ErpCartResponse RemoveDiscountCodesFromCart(string cartId, IEnumerable<string> dicountCodes);
        ErpCartResponse AddTenderLine(string cartId, ErpCartTenderLine cartTenderLine, long cartVersion);
        ErpCartResponse AddPreprocessedTenderLine(string cartId, ErpTenderLine preprocessedTenderLine, long cartVersion);
        ErpCartCheckoutResponse Checkout(string cartId, string receiptEmail, string receiptNumberSequence, long cartVersion);
        ErpCartResponse RecalculateCustomerOrder(string cartId);
        ErpCartResponse UpdateDeliverySpecification(string cartId, ErpDeliverySpecification deliverySpecification);
        List<ErpCartResponse> GetCarts(List<string> cartIds);
        ErpCartResponse DeleteCarts(List<string> cartIds);
    }
}
