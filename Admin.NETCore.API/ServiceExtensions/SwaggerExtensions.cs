using Microsoft.OpenApi.Models;

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
                    Description = "API 文档1"
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "API (V2)",
                    Version = "v2",
                    Description = "API 文档2"
                });
            });

            return services;
        }

        public static WebApplication UseCustomSwaggerUI(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "versionV2");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "versionV1");
                    // API 文档的默认展开方式
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List); // 展开分组
                    //c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // 默认折叠
                    //c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.Full); // 完全展开所有 API 的详细描述（包括请求参数、响应模型等）
                    //c.DefaultModelsExpandDepth(-1); // -1：完全折叠，不显示任何模型详情。0：展开模型的顶层属性（单层展开）。1或更高：展开到指定层级。
                });
            }
            return app;
        }
    }
}
