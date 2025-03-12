using Admin.NETCore.Core.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels
{
    public class UserVModel : BaseModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }

        [Required(ErrorMessage = "LoginName不能为空")]
        public string LoginName { get; set; }

        [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "用户编号不能为空")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "用户编号必须为8位纯数字")]
        public int UserNumber { get; set; }

        public string DeptId { get; set; }

        public DateTime UserExpiryDate { get; set; }

        public int Status { get; set; }

        [Range(1, 10, ErrorMessage = "Level需在0-10范围内")]
        public int Level { get; set; }

        public double? Money { get; set; }

        public int? Age { get; set; }

        public string Notes { get; set; }
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
}
