namespace Admin.NETCore.Infrastructure.DB.Entities
{
    public class DictType : BaseEntity
    {
        public string Id { get; set; } = null!;  // 存36位字符串 char
        public string Name { get; set; } = null!;  // 字典名称（32字符以内）
        public string Code { get; set; } = null!;  // 字典编码（32字符以内）
        public int Sort { get; set; } = 0; // 排序优先级（0-999之间的正整数）
        public int Builtin { get; set; }  // 是否内置（1：是，0：否）

        public int Status { get; set; }  // 状态（1：启用，0：停用）

        public string? Notes { get; set; }  // 备注（100字符以内）


        // 多对多导航属性
        //public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
