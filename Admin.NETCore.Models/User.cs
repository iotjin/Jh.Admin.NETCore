namespace Admin.NETCore.Models
{
    public class User
    {
        public Guid? Id { get; set; }  // 使用 Guid 类型，表示 UUID
        public string Name { get; set; }  // 姓名
        public string LoginName { get; set; }  // 登录用户名
        public string Phone { get; set; }  // 手机号
        public int UserNumber { get; set; }  // 员工编号
        public string DeptId { get; set; }  // 部门ID
        public DateTime UserExpiryDate { get; set; }  // 用户有效期止
        public bool Status { get; set; }  // 用户状态，启用/停用
        public int Level { get; set; }  // 级别
        public double? Money { get; set; }  // 补助，可为空
        public int? Age { get; set; }  // 年龄，可为空
        public string Notes { get; set; }  // 备注，最多100个字符
        public bool IsDelete { get; set; } // 是否删除
    }
}