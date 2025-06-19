using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels.Base
{
    public class BaseRequest
    {
        [Required(ErrorMessage = "ID不能为空")]
        public string Id { get; set; } = default!;
    }

    /// <summary>
    /// 36位ID
    /// </summary>

    public class IDRequest
    {
        [Required(ErrorMessage = "ID不能为空")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ID格式不正确,ID长度必须36位")]
        public string Id { get; set; } = default!;
    }
}