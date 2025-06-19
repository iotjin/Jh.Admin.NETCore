using Admin.NETCore.Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.NETCore.Infrastructure.DB.configs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // 设置表名
            builder.ToTable("Role");

            builder.HasKey(e => e.Id); // 设置主键

            // 属性配置
            builder.Property(e => e.Id).HasMaxLength(36);
            builder.Property(e => e.Name).HasMaxLength(32).IsRequired().HasComment("角色名称");
            builder.Property(e => e.Code).HasMaxLength(32).HasColumnType("varchar(32)").IsRequired().HasComment("角色编码");
            builder.Property(e => e.Builtin).HasMaxLength(1).IsRequired();  // 数值类型（如 int）无法添加长度限制，此处HasMaxLength不生效
            builder.Property(e => e.Notes).HasMaxLength(100).IsRequired(false).HasComment("这是备注");



            // 公共属性配置
            builder.Property(e => e.CreateBy).IsRequired(false).HasMaxLength(10).HasColumnName("CreateBy");
            builder.Property(e => e.UpdateBy).IsRequired(false).HasMaxLength(10).HasColumnName("UpdateBy");

            // 创建时间（只设置一次）
            builder.Property(e => e.CreateTime)
                .HasColumnName("CreateTime")
                .HasColumnType("DATETIME(6)")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            // 更新时间（每次更新）
            builder.Property(e => e.UpdateDate)
                .HasColumnName("UpdateDate")
                .HasColumnType("DATETIME(6)")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)") // 
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            //builder.Ignore(e => e.CreateTime);// 设置某字段不映射到数据表
            //builder.Property(e => e.IsDelete).HasColumnName("delFlag"); // 某字段对应数据表的某字段

        }
    }
}