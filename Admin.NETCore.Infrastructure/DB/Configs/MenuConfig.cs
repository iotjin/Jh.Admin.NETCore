using Admin.NETCore.Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.NETCore.Infrastructure.DB.configs
{
    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            // 设置表名
            builder.ToTable("Menu");

            builder.HasKey(e => e.Id); // 设置主键

            // 属性配置
            builder.Property(e => e.Id).HasMaxLength(36);
            builder.Property(e => e.MenuType).HasMaxLength(10).IsRequired().HasComment("菜单类型(catalog / menu / button)");
            builder.Property(e => e.Title).HasMaxLength(32).IsRequired().HasComment("菜单名称");
            builder.Property(e => e.Code).HasMaxLength(32).HasColumnType("varchar(32)").IsRequired().HasComment("菜单编码");
            builder.Property(e => e.ParentId).HasMaxLength(36).HasComment("父类菜单id");
            builder.Property(e => e.ParentTitle).HasMaxLength(32).HasComment("父类菜单名称");
            builder.Property(e => e.Icon).HasMaxLength(32).HasComment("菜单图标");
            builder.Property(e => e.Component).HasMaxLength(64).IsRequired().HasComment("路由");


            // 调用扩展方法，配置公共属性
            builder.ConfigureBaseProperties();



            //// 公共属性配置
            //builder.Property(e => e.CreateBy).IsRequired(false).HasMaxLength(10).HasColumnName("CreateBy");
            //builder.Property(e => e.UpdateBy).IsRequired(false).HasMaxLength(10).HasColumnName("UpdateBy");

            //// 创建时间（只设置一次）
            //builder.Property(e => e.CreateTime)
            //    .HasColumnName("CreateTime")
            //    .HasColumnType("DATETIME(6)")
            //    .ValueGeneratedOnAdd()
            //    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            //    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            //// 更新时间（每次更新）
            //builder.Property(e => e.UpdateDate)
            //    .HasColumnName("UpdateDate")
            //    .HasColumnType("DATETIME(6)")
            //    .ValueGeneratedOnAddOrUpdate()
            //    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            //    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            //builder.Ignore(e => e.CreateTime);// 设置某字段不映射到数据表
            //builder.Property(e => e.IsDelete).HasColumnName("delFlag"); // 某字段对应数据表的某字段

        }
    }
}