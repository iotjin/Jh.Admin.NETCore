using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.ViewModels
{
    public class RoleListDTO : BaseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public int Builtin { get; set; }

        public string? Notes { get; set; }
    }
}
