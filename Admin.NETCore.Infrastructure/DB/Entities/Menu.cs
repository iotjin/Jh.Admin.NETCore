namespace Admin.NETCore.Infrastructure.DB.Entities
{
    public class Menu : BaseEntity
    {
        //public Guid Id { get; set; }  // 使用 Guid 类型，表示 UUID

        // Guid.NewGuid().ToString().ToLower() 得到id
        public string Id { get; set; } = null!;  // 存36位字符串 char

        public string MenuType { get; set; } = null!;  // 菜单类型（catalog / menu / button）

        public string Title { get; set; } = null!;  // 菜单名称
        public string? ParentId { get; set; }  // 父类菜单id 
        public string? ParentTitle { get; set; }  // 父类菜单名称

        public int Sort { get; set; } = 0; // 排序优先级（0-999之间的正整数）
        public string Code { get; set; } = null!;  // 菜单编码
        public string? Icon { get; set; }  // 菜单图标
        public bool Hidden { get; set; } // 是否隐藏菜单（true为隐藏 false为展示）
        public string Component { get; set; } = null!;  // 路由 "Layout" | "system/menu" (文件路径: src/views/) | ""

    }
}
