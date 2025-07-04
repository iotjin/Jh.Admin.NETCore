using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Admin.NETCore.Infrastructure.DB.Entities;

namespace Admin.NETCore.Infrastructure.DB
{
    /// <summary>
    /// 应用数据库上下文，用于管理 EF Core 与数据库之间的交互
    /// </summary>
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /*
             用户表（User 实体）对应的数据集合，AppDbContext 可以对 User 实体进行增删查改操作

             DbSet<T> 是 EF Core 中用于操作某个实体类型的集合
             放在 AppDbContext 中，EF Core 会自动识别这些属性，对应到数据库的表，进行 Code First 映射
         */
        public DbSet<User> User { get; set; } //  DbSet<模型> 表名
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<DictType> DictType { get; set; }
        public DbSet<DictItem> DictItem { get; set; }




        /*
         * 数据库配置建议在 Program.cs 中完成（使用依赖注入方式）
         * 如果在这里同时使用 OnConfiguring，会导致配置冲突或冗余。
         * 所以此处注释掉 OnConfiguring 方法
         */
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // 数据库连接字符串
        //    var connectionString = "server=localhost;Database=TestDB;Uid=root;Pwd=root;";
        //    // 使用 MySQL 数据库，并指定服务器版本
        //    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        //     base.OnConfiguring(optionsBuilder);
        //}

        /// <summary>
        /// 模型构建方法，用于配置实体与数据库的映射关系
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ❎ 下面是手动配置实体属性的示例，仅供参考，实际建议写在单独的配置类中
            //modelBuilder.Entity<User>().Property(e => e.Name)
            //    .IsRequired()
            //    .HasComment("备注");

            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.ToTable("User");

            //    // 属性配置
            //    entity.HasKey(e => e.Id); // 设置主键
            //    entity.Property(e => e.Id).HasMaxLength(36);
            //    entity.Property(e => e.Name).HasMaxLength(10).IsRequired().HasComment("姓名");
            //    entity.Property(e => e.LoginName).HasMaxLength(10).IsRequired();
            //    entity.Property(e => e.Phone).HasMaxLength(11).IsRequired().IsFixedLength().HasAnnotation("PhoneNumber", "^[0-9]{11}$"); // 正则校验（可以自定义更复杂的格式）
            //    entity.Property(e => e.UserNumber).HasMaxLength(8).IsRequired();
            //    entity.Property(e => e.DeptId).IsRequired();
            //    entity.Property(e => e.UserExpiryDate).HasMaxLength(10).IsRequired();
            //    entity.Property(e => e.Status).IsRequired();
            //    entity.Property(e => e.Level).IsRequired();
            //    entity.Property(e => e.Money).IsRequired(false);
            //    entity.Property(e => e.Age).IsRequired(false).HasAnnotation("Age", "^[1-9][0-9]*$");
            //    entity.Property(e => e.Notes).HasMaxLength(100).IsRequired(false).HasComment("这是备注");
            //});


            base.OnModelCreating(modelBuilder);

            //方式一. 分开注册, 逐个添加实体的配置类
            //modelBuilder.Configurations.Add(new UserConfig());
            //modelBuilder.Configurations.Add(new RoleConfig());

            //方式二. 一次性加载所有Fluent API的配置
            // ✅ 推荐方式：统一加载所有 IEntityTypeConfiguration 的配置类（如 UserConfig、RoleConfig 等）
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}

/* 
 
 Migration的一些命令(因为在Program中配置了 MigrationsAssembly对应的项目，这里放在配置的项目路径下) 


 工具>NuGet 包管理器 > 程序包管理器控制台

 Add-Migration Init 
 Update-Database 

指定 Migration 路径

Add-Migration updateUserTable6 -OutputDir "DB/Migrations"
 
 */


