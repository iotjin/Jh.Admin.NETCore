using Admin.NETCore.Common.Validators;
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels.Base
{
    public class BaseRequest
    {
        [Required(ErrorMessage = "Id不能为空")]
        public string Id { get; set; } = default!;
    }

    /// <summary>
    /// 36位Id
    /// </summary>

    public class IdRequest
    {
        [Required(ErrorMessage = "Id不能为空")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "Id格式不正确,Id长度必须36位")]
        public string Id { get; set; } = default!;
    }

    /// <summary>
    /// 36位Id 字符串数组
    /// </summary>
    public class IdsRequest
    {
        //[Required(ErrorMessage = "Ids不能为空")]
        //[MinLength(1, ErrorMessage = "至少需要一个Id")]
        //public List<string> Ids { get; set; } = new();


        [Required(ErrorMessage = "Id 列表不能为空")]
        // 自定义校验
        [IdListValidator(36, ErrorMessage = "Ids数组的每个 Id 不能为空，且必须是 36 位字符")]
        public List<string> Ids { get; set; } = new();
    }
}