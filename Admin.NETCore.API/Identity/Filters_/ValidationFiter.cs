// 自定义异常处理类
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace Admin.NETCore.API.Identity.filters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // 提取所有错误信息
                var errorMessages = context.ModelState.Values
                     .Where(x => x.Errors.Count > 0)
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                // 拼接错误信息为字符串
                var msgs = string.Join("; ", errorMessages);

                int statusCode = 201;

                // 构造自定义响应对象
                var response = new
                {
                    code = statusCode,
                    msg = msgs,
                    success = false,
                    data = ""
                };

                context.Result = new BadRequestObjectResult(response);

                // 自定义状态码
                //context.Result = new ObjectResult(response)
                //{
                //    StatusCode = statusCode 
                //};
            }
        }
    }

    public class CustomValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                //var errorMessages = context.ModelState.Values
                //                    .Where(x => x.Errors.Count > 0)
                //                   .SelectMany(x => x.Errors)
                //                   .Select(x => x.ErrorMessage)
                //                   .ToList();
                //var msgs = string.Join("; ", errorMessages);
                //var response = new
                //{
                //    code = 201, // 可根据需求调整
                //    msg = msgs,
                //    success = false,
                //    data = ""
                //};
                //context.Result = new BadRequestObjectResult(response);

                context.Result = CreateValidationErrorResult(context.ModelState);
                return;
            }

            await next();
        }

        private static IActionResult CreateValidationErrorResult(ModelStateDictionary modelState)
        {
            var errorMessages = modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value?.Errors
                        .Select(error =>
                            !string.IsNullOrEmpty(error.ErrorMessage)
                                ? error.ErrorMessage
                                : error.Exception?.Message ?? "Unknown error")
                        .ToArray()
                );

            var message = string.Join("; ", errorMessages.SelectMany(e => e.Value ?? Array.Empty<string>()));

            int statusCode = 201;

            var response = new
            {
                code = statusCode, // 可根据需求调整
                msg = message,
                errors = errorMessages,
                success = false,
                data = ""
            };

            return new BadRequestObjectResult(response);

            //// 自定义状态码
            //return new ObjectResult(response)
            //{
            //    StatusCode = statusCode
            //};
        }
    }
}
