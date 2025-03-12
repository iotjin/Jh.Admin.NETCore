using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels.Base
{
    public class BaseRequest
    {
        [Required(ErrorMessage = "用户ID不能为空")]
        [StringLength(36, ErrorMessage = "用户ID格式不正确")]
        public string Id { get; set; } = default!;
    }
}