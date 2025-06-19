using Admin.NETCore.Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.NETCore.Infrastructure.DB.configs
{
    public class UserRoleConfig: IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // 设置表名
            builder.ToTable("UserRole");

            //builder.HasKey(e => e.Id); // 设置主键

            // 复合主键
            builder.HasKey(e => new { e.UserId, e.RoleId });

            // 配置外键关系

            // User <=> UserRole
            builder.HasOne(e => e.User)
                   .WithMany(e => e.UserRoles)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // 当 User 被删除时，相关的 UserRole 自动删除

            // Role <=> UserRole
            builder.HasOne(e => e.Role)
                   .WithMany(e => e.UserRoles)
                   .HasForeignKey(e => e.RoleId)
                   .OnDelete(DeleteBehavior.Cascade); // 当 Role 被删除时，相关的 UserRole 自动删除

            /*  DeleteBehavior 枚举值
             
                Cascade	    删除主实体时，自动删除相关子实体
                Restrict	禁止删除主实体（如果有子记录）
                SetNull	    删除主实体时，把子实体的外键设置为 NULL（需要外键允许为 null）
                NoAction	由数据库自行决定行为（一般是报错）

             */


            // 属性配置
            builder.Property(e => e.UserId).HasMaxLength(36).IsRequired();
            builder.Property(e => e.RoleId).HasMaxLength(36).IsRequired();
            builder.Property(e => e.Status).IsRequired().HasComment("分配状态(1：已分配，0：未分配)");



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
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)") // 
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            //builder.Ignore(e => e.CreateTime);// 设置某字段不映射到数据表
            //builder.Property(e => e.IsDelete).HasColumnName("delFlag"); // 某字段对应数据表的某字段

        }
    }
}