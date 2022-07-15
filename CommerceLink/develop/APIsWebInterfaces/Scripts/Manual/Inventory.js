var AjaxCallDelegateObj;

$(function () {
    AjaxCallDelegateObj = new AjaxCallDelegate();
});

function InventoryLookupByItem() {
    var itemId = $("itemId").val();
    var varientId = $("varientId").val();

    //alert("1");

    
}


function InventoryLookupByBarcode() {
    var barcodeIds = $("barcodes").val();

    var JsonData = { barcodes: barcodeIds };
    
    AjaxCallDelegateObj.AjaxCallDelegateCall(AjaxCallDelegateObj.RequestTypeVariables.Get, AjaxCallDelegateObj.DataTypeVariables.Json, "https://localhost:8222/Fabrikam/api/store/InventoryLookupByBarCode", JsonData, "InventoryLookupByBarcodeSuccess", "InventoryLookupByBarcodeError");
}

function InventoryLookupByBarcodeSuccess(Response) {
    debugger;
    alert(Response);
}

function InventoryLookupByBarcodeError(Response) {
    debugger;
    if (Response.Success == false) {
        alert(Response.ErrorMessage);
    }
}

function BindInventory(dataObj){

}
