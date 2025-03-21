using Admin.NETCore.API.Identity.filters;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Infrastructure.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.NETCore.API.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        private const string MigrationAssembly = "Admin.NETCore.Infrastructure";

        public static IServiceCollection AddBasicServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllers();

            services.AddHealthChecks();

            // 日志配置
            services.AddLogging(logging =>
            {
                logging.AddConfiguration(configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
            });

            services.AddCorsPolicy();

            services.AddSwaggerConfig();
            // services.AddSwaggerConfig22();

            return services;
        }

        public static IServiceCollection AddDataBaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("缺少数据库连接字符串配置");

            // services.AddDbContext<AppDbContext>(options =>
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    x => x.MigrationsAssembly(MigrationAssembly) // 指定迁移程序集
                   ));

            services.AddScoped<IUserService, UserService>();
            return services;
        }

        public static IServiceCollection AddControllerWithValidation(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilterAttribute>();
                options.Filters.Add<CustomValidationFilter>();
            });

            // 禁用默认的模型验证响应
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}




/* 
  数据库配置
 
 
 // 添加数据库上下文
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
//    ));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//        new MySqlServerVersion(new Version(8, 0, 21))));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  指定迁移程序集

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
//        x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
//    ));
 
 */
