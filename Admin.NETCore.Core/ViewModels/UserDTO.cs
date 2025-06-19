using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.ViewModels
{
    public class UserDTO : BaseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string LoginName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public int UserNumber { get; set; }

        public string DeptId { get; set; } = null!;

        public DateTime UserExpiryDate { get; set; }

        public int Status { get; set; }

        public int Level { get; set; }

        public double? Money { get; set; }

        public int? Age { get; set; }

        public string? Notes { get; set; }
    }
}
