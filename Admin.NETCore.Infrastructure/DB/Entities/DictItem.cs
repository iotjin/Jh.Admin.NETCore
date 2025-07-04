namespace Admin.NETCore.Infrastructure.DB.Entities
{
    public class DictItem : BaseEntity
    {
        public string Id { get; set; } = null!;  // 存36位字符串 char
        public string Label { get; set; } = null!; // 字典项名称/字典项标签（32字符以内）
        public string Value { get; set; } = null!; // 字典项值（32字符以内）
        public string DictTypeCode { get; set; } = null!;  // 所属字典类型（32字符以内）
        public int Sort { get; set; } = 0; // 排序优先级（0-999之间的正整数）
        public int Builtin { get; set; }  // 是否内置（1：是，0：否）

        public int Status { get; set; }  // 状态（1：启用，0：停用）

        public string? Notes { get; set; }  // 备注（100字符以内）

        public DictType DictType { get; set; } = null!;
    }
}
