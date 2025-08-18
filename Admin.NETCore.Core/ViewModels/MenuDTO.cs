using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.ViewModels
{
    public class MenuListDTO : BaseModel
    {
        public string? Id { get; set; }
        public string MenuType { get; set; } = null!;  // 菜单类型(catalog / menu / button)

        public string Title { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? ParentId { get; set; }
        public string? ParentTitle { get; set; }

        public int Sort { get; set; }

        public string? Icon { get; set; }

        public bool Hidden { get; set; }

        public string Component { get; set; } = null!;

        public List<MenuListDTO>? Children { get; set; } = new List<MenuListDTO>();
    }
}
