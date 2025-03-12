// 自定义异常处理类
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

                // 构造自定义响应对象
                var response = new
                {
                    code = 201,
                    msg = msgs,
                    success = false,
                    data = ""
                };

                context.Result = new BadRequestObjectResult(response);
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

            var response = new
            {
                code = 201, // 可根据需求调整
                msg = message,
                errors = errorMessages,
                success = false,
                data = ""
            };
            return new BadRequestObjectResult(response);
        }
    }
}
