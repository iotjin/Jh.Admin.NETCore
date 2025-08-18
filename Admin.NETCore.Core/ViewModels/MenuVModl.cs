using Admin.NETCore.Core.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Core.ViewModels
{

    public class MenuVModel : BaseModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "MenuType不能为空")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "MenuType长度必须在1-10个字符范围内")]
        [RegularExpression(@"^(catalog|menu|button)$", ErrorMessage = "MenuType必须是'catalog'、'menu'或'button'中的一个")]
        public string MenuType { get; set; } = null!;  // 菜单类型(catalog / menu / button)


        [Required(ErrorMessage = "Title不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "Title长度必须在1-32个字符范围内")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Code不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "Code长度必须在1-32个字符范围内")]
        public string Code { get; set; } = null!;

        public string? ParentId { get; set; }
        public string? ParentTitle { get; set; }


        [Range(0, 999, ErrorMessage = "Sort必须是0-999之间的整数")]
        public int Sort { get; set; }

        [StringLength(32, ErrorMessage = "Icon长度不能超过32个字符")]
        public string? Icon { get; set; }

        public bool Hidden { get; set; }

        [Required(ErrorMessage = "Component不能为空")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "Component长度必须在1-64个字符范围内")]
        public string Component { get; set; } = null!;  // 路由 "Layout" | "system/menu" (文件路径: src/views/) | ""

    }

    public class MenuFilterModel
    {
        public string? Title { get; set; }
        public string? Code { get; set; }
    }
}
