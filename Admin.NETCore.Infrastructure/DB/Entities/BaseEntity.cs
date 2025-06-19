using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NETCore.Infrastructure.DB.Entities
{
    public abstract class BaseEntity
    {
        //public Guid Id { get; set; }  // 使用 Guid 类型，表示 UUID

        // Guid.NewGuid().ToString().ToLower() 得到id
        //public string Id { get; set; } = null!;  // 存36位字符串 char    主键要定义基类中

        public DateTime CreateTime { get; set; }
        public DateTime UpdateDate { get; set; }

        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public bool IsDelete { get; set; } // 是否删除




        //[Column(TypeName = "DATETIME(6)")] // MySQL 支持高精度时间
        //public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        //[Column(TypeName = "DATETIME(6)")]
        //public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        //[Column(TypeName = "VARCHAR(10)")] // 明确指定 VARCHAR 类型
        //public string CreateBy { get; set; } = "system";

        //[Column(TypeName = "VARCHAR(10)")]
        //public string UpdateBy { get; set; } = "system";

        //[Column(TypeName = "TINYINT(1)")] // MySQL 通常用 TINYINT 表示 bool
        //public bool IsDelete { get; set; }
    }
}
