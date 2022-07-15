namespace VSI.CommerceLink.EcomDataModel.Response
{
    /// <summary>
    /// Generic Response for Web API Controllers
    /// </summary>
    /// <typeparam name="T">Type of Data E-G Sales Order</typeparam>
    public class BaseResponse<T> where T : class
    {
        /// <summary>
        /// Initializes Response with status true or false
        /// </summary>
        /// <param name="status">true for Success and false for Error</param>
        public BaseResponse(bool status)
        {
            Status = status;
            Message = string.Empty;
        }

        public BaseResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        public BaseResponse(bool status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public static BaseResponse<T> Success(T data)
        {
            return new BaseResponse<T>(true, string.Empty, data);
        }

        public static BaseResponse<T> Error(string message)
        {
            return new BaseResponse<T>(false, message);
        }

        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}