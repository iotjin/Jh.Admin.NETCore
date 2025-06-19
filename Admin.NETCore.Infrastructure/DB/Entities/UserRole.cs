namespace Admin.NETCore.Infrastructure.DB.Entities
{
    public class UserRole : BaseEntity
    {
        public string UserId { get; set; } = null!; // default! 也可以
        public User User { get; set; } = null!;

        public string RoleId { get; set; } = null!;
        public Role Role { get; set; } = null!;

        public int Status { get; set; }  // 分配状态(1：已分配，0：未分配)

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow; // 分配时间

    }
}