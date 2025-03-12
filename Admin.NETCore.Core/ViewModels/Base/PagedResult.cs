namespace Admin.NETCore.Core.ViewModels.Base
{
    public class PagedResult<T> : ApiResult<List<T>>
    {
        public int Total { get; set; }

        public static PagedResult<T> SuccessResult(List<T> data, int total)
        {
            return new PagedResult<T>
            {
                Code = ApiStatusCode.Success,
                Msg = "success",
                Data = data,
                Total = total
            };
        }
    }

}