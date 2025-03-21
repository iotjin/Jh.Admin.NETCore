using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

// 通过[ApiExplorerSettings(GroupName = "v1")] 实现
namespace Admin.NETCore.API.ServiceExtensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API (V1)",
                    Version = "v1",
                    //Description = "API 文档1",
                    Description = "🚨 此版本已废弃，请使用 v2",
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "API (V2)",
                    Version = "v2",
                    Description = "API 文档2"
                });


                // 核心逻辑：根据 GroupName 过滤接口到对应文档
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    // 获取 Controller/Action 上的 GroupName
                    var groupName = apiDesc.ActionDescriptor.EndpointMetadata
                        .OfType<ApiExplorerSettingsAttribute>()
                        .FirstOrDefault()?.GroupName;

                    // 未标记 GroupName 的接口默认显示在所有文档中
                    if (string.IsNullOrEmpty(groupName)) return true;

                    // 匹配当前 Swagger 文档名，只有标记了 GroupName 的接口才显示（如 v1/v2）
                    return groupName == docName;
                });

                options.OperationFilter<InheritedObsoleteOperationFilter>(); // 注册自定义过滤器


                //// 解决 Schema ID 冲突问题
                //options.CustomSchemaIds(type => type.FullName); // 使用完整命名空间

            });

            return services;
        }

        public static WebApplication UseCustomSwaggerUI(this WebApplication app)
        {
            // 获取 IConfiguration
            var configuration = app.Services.GetRequiredService<IConfiguration>();

            // 获取配置EnableSwagger
            bool enableSwagger = configuration.GetValue<bool>("EnableSwagger", false);

            if (app.Environment.IsDevelopment() && enableSwagger)
            {
                app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "versionV2"); // 这个顺序影响swagger select a definition 默认选中
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "versionV1 (Deprecated)");
                    // API 文档的默认展开方式
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List); // 展开分组
                    //c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // 默认折叠
                    //c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.Full); // 完全展开所有 API 的详细描述（包括请求参数、响应模型等）
                    //c.DefaultModelsExpandDepth(-1); // -1：完全折叠，不显示任何模型详情。0：展开模型的顶层属性（单层展开）。1或更高：展开到指定层级。
                });
            }

            // 动态处理根路径重定向
            app.MapGet("/", context =>
            {
                if (enableSwagger)
                {
                    // 重定向到 Swagger 页面
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                }
                else
                {
                    // 返回其他信息（例如 API 状态）
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    return context.Response.WriteAsync($"API 运行中（{(app.Environment.IsDevelopment() ? "Development" : "Production")}），Swagger 已禁用。");
                }
            });

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/")
            //    {
            //        // 返回自定义响应
            //        context.Response.ContentType = "text/plain; charset=utf-8";
            //        await context.Response.WriteAsync("API 运行中，Swagger 已禁用");
            //        return;
            //    }
            //    await next();
            //});

            return app;
        }
    }

    // 若希望所有继承自 V1BaseController 的子类自动标记为废弃，可通过自定义过滤器实现。
    public class InheritedObsoleteOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 检查控制器是否继承自带有 [Obsolete] 的基类
            var controllerType = context.MethodInfo.DeclaringType;
            var baseType = controllerType?.BaseType;

            if (baseType != null && baseType.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any())
            {
                operation.Deprecated = true; // 标记操作已废弃
                operation.Description += " (已废弃：此接口所属控制器基类已废弃)";
            }
        }
    }
}
