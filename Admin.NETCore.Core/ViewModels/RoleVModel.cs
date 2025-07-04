﻿using Admin.NETCore.Core.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels
{
    public class RoleVModel : BaseModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Name不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "Name长度必须在1-32个字符范围内")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Code不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "Code长度必须在1-32个字符范围内")]
        public string Code { get; set; } = null!;

        [Range(0, 1, ErrorMessage = "Status值只能是 0 或 1")]
        public int Status { get; set; }

        [Range(0, 1, ErrorMessage = "Builtin值只能是 0 或 1")]
        public int Builtin { get; set; }

        public string? Notes { get; set; }
    }

    public class RoleFilterModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

    public class UserRolesVModel
    {
        [Required(ErrorMessage = "UserId不能为空")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "AssignStatus不能为空")]
        [Range(0, 1, ErrorMessage = "AssignStatus值只能是 0 或 1")]
        public string AssignStatus { get; set; } = null!;

        public string? Name { get; set; }
    }
}
