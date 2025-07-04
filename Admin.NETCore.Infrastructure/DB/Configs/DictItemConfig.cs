using Admin.NETCore.Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.NETCore.Infrastructure.DB.configs
{
    public class DictItemConfig : BaseEntityConfig<DictItem> // 继承 BaseEntityConfig，不使用 IEntityTypeConfiguration
    {
        public override void Configure(EntityTypeBuilder<DictItem> builder)
        {

            base.Configure(builder); // 调用基类公共配置

            // 设置表名
            builder.ToTable("DictItem");

            builder.HasKey(e => e.Id); // 设置主键

            // 属性配置
            builder.Property(e => e.Id).HasMaxLength(36);
            builder.Property(e => e.Label).HasMaxLength(32).IsRequired().HasComment("字典项名称");
            builder.Property(e => e.Value).HasMaxLength(32).HasColumnType("varchar(32)").IsRequired().HasComment("字典项值");
            builder.Property(e => e.DictTypeCode).HasMaxLength(32).IsRequired().HasComment("所属的字典类型");
            builder.Property(e => e.Sort).IsRequired();
            builder.Property(e => e.Builtin).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.Notes).HasMaxLength(100).IsRequired(false).HasComment("这是备注");



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