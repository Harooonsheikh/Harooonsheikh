var AjaxCallDelegate = function () {

    //variables
    this.RequestTypeVariables = {
        Get: "GET",
        Post: "POST"
    };

    this.DataTypeVariables = {
        Json: "json",
        Xml: "xml",
        Html: "html",
        Script: "script",
        Jsonp: "jsonp",
        Text: "text"
    };

    //generic ajax call
    this.AjaxCallDelegateCall = function (RequestType, DataType, URL, paramArray, SuccessFunctionName, ErrorFunctionName) {

        $.ajax({
            dataType: DataType,
            type: RequestType,
            url: URL,
            data: paramArray,
            contentType: "application/json",
            success: eval(SuccessFunctionName),
            error: eval(ErrorFunctionName)
        });
    }
}