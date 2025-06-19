namespace Admin.NETCore.Core.ViewModels.Base
{
    public class PagedFilterBaseModel : PagingBaseModel
    {
        public string KeyWord { get; set; } = "";
    }
    public class PagedFilterWithTimeRangeBaseModel : PagedFilterBaseModel
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}