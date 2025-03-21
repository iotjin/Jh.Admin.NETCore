using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

// 插件 Asp.Versioning.Mvc 实现
namespace Admin.NETCore.API.ServiceExtensions
{
    public static class SwaggerExtensions22
    {
        public static IServiceCollection AddSwaggerConfig22(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(22, 2); // 指定在请求中未指明版本时要使用的默认 API 版本 
                options.AssumeDefaultVersionWhenUnspecified = true; // 未指定版本时使用默认版本
                options.ReportApiVersions = true; // 响应头中返回支持的版本
                //options.ApiVersionReader = new UrlSegmentApiVersionReader(); // 从 URL 读取版本号

                // 读取版本号的方式
                options.ApiVersionReader = ApiVersionReader.Combine(
                      new UrlSegmentApiVersionReader(),  // 通过 URL 路径读取版本, 例如：http://xxxx/api/v1/getuser 和 http://xxxx/api/v2/getuser
                      new QueryStringApiVersionReader("version"), // 是指通过URL中的参数来确认。例如：http://xxxx/getuser?version=1
                      new HeaderApiVersionReader("x-api-version"), // 是指通过header头信息来确认。例如，设置header.add("X-API-Version","1")来设置
                      new MediaTypeApiVersionReader("version") // 是通过content_type信息来设置
                 );

            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // 格式化版本号
                options.SubstituteApiVersionInUrl = true; // 在 URL 中替换版本号, 让 {version} 在 URL 中生效
                                                          // options.IncludeDeprecatedApiVersions = true; // 让过期版本在 Swagger 中显示
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"MyAPI v{description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "API 文档： 版本v" + description.ApiVersion.ToString() + (description.IsDeprecated ? "( ⚠️ 此版本已弃用 )" : "")
                    });
                }

                // 手动添加额外的 Swagger 组
                // options.SwaggerDoc("v10", new OpenApiInfo { Title = "MyAPI API v10", Version = "v10" });

                // 自动获取所有 ApiExplorerSettings(GroupName = "xxx") 组
                var groupNames = GetGroupNames();
                foreach (var group in groupNames)
                {
                    options.SwaggerDoc(group, new OpenApiInfo
                    {
                        Title = $"My API - {group}",
                        Version = "1.0"  // 额外分组不依赖 ApiVersion
                    });
                }

            });

            return services;
        }

        public static WebApplication UseCustomSwaggerUI22(this WebApplication app)
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
                    //var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    var provider = app.DescribeApiVersions();
                    foreach (var description in provider)
                    {
                        var version = $"v{description.ApiVersion}";
                        var deprecated = description.IsDeprecated ? " (Deprecated)" : "";
                        c.SwaggerEndpoint(
                           url: $"/swagger/{description.GroupName}/swagger.json",
                           name: $"{description.GroupName.ToUpperInvariant()}{deprecated}");
                    }
                    //foreach (var description in app.DescribeApiVersions())
                    //{
                    //    var url = $"/swagger/{description.GroupName}/swagger.json";
                    //    var name = description.GroupName.ToUpperInvariant();
                    //    c.SwaggerEndpoint(url, name);
                    //}


                    // 手动添加额外的 Swagger 组
                    //   c.SwaggerEndpoint("/swagger/v10/swagger.json", "API - v10");

                    // 自动添加所有额外分组
                    var groupNames = GetGroupNames();
                    foreach (var group in groupNames)
                    {
                        c.SwaggerEndpoint($"/swagger/{group}/swagger.json", $"API - {group}");
                    }


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


        private static IEnumerable<string> GetGroupNames()
        {
            // 扫描控制器和方法上的 ApiExplorerSettings 特性，获取所有分组名称
            //var controllerGroupNames = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .Where(t => t.IsClass && typeof(ControllerBase).IsAssignableFrom(t))
            //    .SelectMany(t => t.GetCustomAttributes<ApiExplorerSettingsAttribute>())
            //    .Select(attr => attr.GroupName)
            //    .Where(name => !string.IsNullOrEmpty(name));

            //var methodGroupNames = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .SelectMany(t => t.GetMethods())
            //    .SelectMany(m => m.GetCustomAttributes<ApiExplorerSettingsAttribute>())
            //    .Select(attr => attr.GroupName)
            //    .Where(name => !string.IsNullOrEmpty(name));
            // 合并所有的分组名称，去重并返回
            //return controllerGroupNames.Concat(methodGroupNames).Distinct();

            return Assembly.GetExecutingAssembly()
                .GetTypes()
                // 扫描控制器级别的分组
                .Where(t => t.IsClass && !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t))
                .SelectMany(t => t.GetCustomAttributes<ApiExplorerSettingsAttribute>()
                    .Select(attr => attr.GroupName)
                    // 扫描方法级别的分组
                    .Concat(t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .SelectMany(m => m.GetCustomAttributes<ApiExplorerSettingsAttribute>()
                            .Select(attr => attr.GroupName)
                        )
                    )
                )
                // 明确处理空值（消除警告）
                .Select(name => name ?? string.Empty)
                // 过滤无效名称
                .Where(name => !string.IsNullOrWhiteSpace(name))
                // 去重（不区分大小写）
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }
}
