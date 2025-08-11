namespace Admin.NETCore.Core.ViewModels.Base
{
    public class ApiResult<T>
    {
        public ApiStatusCode Code { get; set; }
        public string Msg { get; set; } = "";
        public bool IsSuccess => Code == ApiStatusCode.Success;
        public T? Data { get; set; }

        // 静态方法
        public static ApiResult<T> SuccessResult(T data, string message = "")
        {
            return new ApiResult<T> { Code = ApiStatusCode.Success, Msg = message, Data = data };
        }

        public static ApiResult<T> FailResult(string message, ApiStatusCode code = ApiStatusCode.Fail)
        {
            return new ApiResult<T> { Code = code, Msg = message, Data = default };
        }

        // 实例方法
        public ApiResult<T> Success(T data, string message = "")
        {
            Code = ApiStatusCode.Success;
            Data = data;
            Msg = message;
            return this;
        }

        public ApiResult<T> Fail(string message, ApiStatusCode code = ApiStatusCode.Fail)
        {
            Code = code;
            Msg = message;
            Data = default;
            return this;
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


