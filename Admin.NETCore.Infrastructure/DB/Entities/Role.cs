namespace Admin.NETCore.Infrastructure.DB.Entities
{
    public class Role : BaseEntity
    {
        //public Guid Id { get; set; }  // 使用 Guid 类型，表示 UUID

        // Guid.NewGuid().ToString().ToLower() 得到id
        public string Id { get; set; } = null!;  // 存36位字符串 char

        public string Name { get; set; } = null!;  // 角色名称

        public string Code { get; set; } = null!;  // 角色编码

        public int Builtin { get; set; }  // 是否内置角色（1：是，0：否）

        public string? Notes { get; set; }  // 备注，最多100个字符


        // 多对多导航属性
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
