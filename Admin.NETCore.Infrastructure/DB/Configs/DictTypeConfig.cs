using Admin.NETCore.Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.NETCore.Infrastructure.DB.configs
{
    public class DictTypeConfig : IEntityTypeConfiguration<DictType>
    {
        public void Configure(EntityTypeBuilder<DictType> builder)
        {
            // 设置表名
            builder.ToTable("DictType");

            builder.HasKey(e => e.Id); // 设置主键

            // 属性配置
            builder.Property(e => e.Id).HasMaxLength(36);
            builder.Property(e => e.Name).HasMaxLength(32).IsRequired().HasComment("字典名称");
            builder.Property(e => e.Code).HasMaxLength(32).HasColumnType("varchar(32)").IsRequired().HasComment("字典编码");
            builder.Property(e => e.Sort).IsRequired();
            builder.Property(e => e.Builtin).IsRequired();
            builder.Property(e => e.Status).HasMaxLength(1).IsRequired();
            builder.Property(e => e.Notes).HasMaxLength(100).IsRequired(false).HasComment("这是备注");


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

        }
    }
}