using Admin.NETCore.Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.NETCore.Infrastructure.DB.configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 设置表名
            builder.ToTable("User");

            builder.HasKey(e => e.Id); // 设置主键

            // 属性配置
            builder.Property(e => e.Id).HasMaxLength(36);
            builder.Property(e => e.Name).HasMaxLength(10).IsRequired().HasComment("姓名");
            builder.Property(e => e.LoginName).HasMaxLength(10).HasColumnType("varchar(10)").IsRequired();
            builder.Property(e => e.Phone).HasMaxLength(11).IsRequired().IsFixedLength().HasAnnotation("PhoneNumber", "^[0-9]{11}$"); // 正则校验（可以自定义更复杂的格式）
            builder.Property(e => e.UserNumber).HasMaxLength(8).IsRequired();  // 数值类型（如 int）无法添加长度限制，此处HasMaxLength不生效
            builder.Property(e => e.DeptId).IsRequired(); // 不设置长度，类型为longtext
            builder.Property(e => e.UserExpiryDate).HasMaxLength(10).IsRequired(); // DateTime类型，无法添加长度限制，此处HasMaxLength不生效
            builder.Property(e => e.Status).HasMaxLength(1).IsRequired();
            builder.Property(e => e.Level).IsRequired();
            builder.Property(e => e.Money).IsRequired(false);
            builder.Property(e => e.Age).IsRequired(false).HasAnnotation("Age", "^[1-9][0-9]*$");
            builder.Property(e => e.Notes).HasMaxLength(100).IsRequired(false).HasComment("这是备注");



            // 公共属性配置
            builder.Property(e => e.CreateBy).IsRequired(false).HasMaxLength(10).HasColumnName("CreateBy");
            builder.Property(e => e.UpdateBy).IsRequired(false).HasMaxLength(10).HasColumnName("UpdateBy");

            // 创建时间（只设置一次）
            builder.Property(e => e.CreateTime)
                .HasColumnName("CreateTime")
                .HasColumnType("DATETIME(6)")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)") // GETDATE() SQL Server  / NOW() mysql  GETUTCDATE()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            // 更新时间（每次更新）
            builder.Property(e => e.UpdateDate)
                .HasColumnName("UpdateDate")
                .HasColumnType("DATETIME(6)")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            //builder.Ignore(e => e.CreateTime);// 设置某字段不映射到数据表
            //builder.Property(e => e.IsDelete).HasColumnName("delFlag"); // 某字段对应数据表的某字段

        }
    }
}



/*

EF默认规则是“主键属性不允许为空，引用类型允许为空，可空的值类型long?等允许为空，值类型不允许为空。”
基于“尽量少配置”的原则：如果属性是值类型并且允许为null，就声明成long?等，否则声明成long等；
如果属性属性值是引用类型，只有不允许为空的时候设置IsRequired()。

其他一般不用设置的
主键：this.HasKey(p => p.pId);
某个字段不参与映射数据库：this.Ignore(p => p.Name1);
this.Property(p => p.Name).IsFixedLength(); 是否对应固定长度
this.Property(p => p.Name).IsUnicode(false) 对应的数据库类型是varchar类型，而不是nvarchar
this.Property(p => p.Id).HasColumnName(“Id”); Id列对应数据库中名字为Id的字段
this.Property(p=>p.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity) 指定字段是自动增长类型。

 
 */