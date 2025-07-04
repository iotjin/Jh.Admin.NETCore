using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Admin.NETCore.Infrastructure.DB.Entities;

namespace Admin.NETCore.Infrastructure.DB.configs
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureBaseProperties<T>(this EntityTypeBuilder<T> builder) where T : BaseEntity
        {

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
        }
    }
}