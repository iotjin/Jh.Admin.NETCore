using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB.Entities;
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels
{
    public class UserVModel : BaseModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "LoginName不能为空")]
        public string LoginName { get; set; } = null!;

        //[MaxLength(11, ErrorMessage = "Phone必须为11位")]
        //[MinLength(11, ErrorMessage = "Phone必须为11位")]
        //[StringLength(11, MinimumLength = 11, ErrorMessage = "Phone长度必须为11位
        [Required(ErrorMessage = "Phone不能为空")]
        [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "UserNumber不能为空")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "UserNumber必须为8位纯数字")]
        public int UserNumber { get; set; }

        [Required(ErrorMessage = "DeptId不能为空")]
        public string DeptId { get; set; } = null!;

        public DateTime UserExpiryDate { get; set; }

        public int Status { get; set; }

        [Range(1, 10, ErrorMessage = "Level需在0-10范围内")]
        public int Level { get; set; }

        public double? Money { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Age必须是正整数")]
        public int? Age { get; set; }

        public string? Notes { get; set; }
    }

    public class UserFilterModel
    {
        public string? Name { get; set; }
        public string? UserNumber { get; set; }
        public string? DeptId { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

    public class AssignRoleVModel
    {
        [Required(ErrorMessage = "UserId不能为空")]
        public string Id { get; set; } = null!;

        public List<string> RoleIds { get; set; } = new();
    }
}
