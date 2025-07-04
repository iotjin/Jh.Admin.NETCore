using Admin.NETCore.Core.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels
{
    public class DictTypeVModel : BaseModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Name不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "Name长度必须在1-32个字符范围内")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Code不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "Code长度必须在1-32个字符范围内")]
        public string Code { get; set; } = null!;

        [Range(0, 999, ErrorMessage = "Sort必须是0-999之间的整数")]
        public int Sort { get; set; }


        [Range(0, 1, ErrorMessage = "Status值只能是 0 或 1")]
        public int Status { get; set; }

        [Range(0, 1, ErrorMessage = "Builtin值只能是 0 或 1")]
        public int Builtin { get; set; }

        public string? Notes { get; set; }
    }

    public class DictTypeFilterModel
    {
        // Name / Code
        public string? Keyword { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

}
