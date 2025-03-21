using Admin.NETCore.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Admin.NETCore.API.ServiceExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
        {
            // 异常处理应放在最前面
            app.UseExceptionHandler("/error");

            // Swagger
            app.UseCustomSwaggerUI();
            //app.UseCustomSwaggerUI22();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            // 健康检查端点
            app.MapHealthChecks("/health");

            // 全局日志中间件
            app.UseRequestLogging();

            app.UseCorsPolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // 可选：数据库自动迁移
            //app.ApplyMigrations<AppDbContext>();

            return app;
        }

        private static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Request {Method} {Path}", context.Request.Method, context.Request.Path);
                await next();
                logger.LogInformation("Response {StatusCode}", context.Response.StatusCode);
            });
        }

        public static void ApplyMigrations<TContext>(this WebApplication app) where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TContext>();
            db.Database.Migrate();
        }
    }
}
