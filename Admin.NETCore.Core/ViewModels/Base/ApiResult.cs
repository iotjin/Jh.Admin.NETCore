namespace Admin.NETCore.Core.ViewModels.Base
{
    public class ApiResult<T>
    {
        public ApiStatusCode Code { get; set; }
        public string Msg { get; set; } = "";
        public bool Success => Code == ApiStatusCode.Success;
        public T? Data { get; set; }

        public static ApiResult<T> SuccessResult(T data, string message = "")
        {
            return new ApiResult<T> { Code = ApiStatusCode.Success, Msg = message, Data = data };
        }

        public static ApiResult<T> FailResult(string message, ApiStatusCode code = ApiStatusCode.Fail)
        {
            return new ApiResult<T> { Code = code, Msg = message, Data = default };
        }
    }

    public enum ApiStatusCode
    {
        Success = 200,
        Fail = 201,
        Unauthorized = 401,
        InternalError = 500
    }
}


