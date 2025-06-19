namespace Admin.NETCore.Core.ViewModels.Base
{
    public class BaseModel
    {
        public DateTime CreateTime { get; set; }
        public DateTime UpdateDate { get; set; }

        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public bool IsDelete { get; set; } // 是否删除
    }
}