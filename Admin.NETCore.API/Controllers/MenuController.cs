using Microsoft.AspNetCore.Mvc;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Common.Constant_Value_Types;

namespace Admin.NETCore.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")] // RPC风格, api/[控制器名称]/函数名称
    //[Route("api/[controller]")]  // RESTful 风格
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpPost]
        public async Task<ApiResult<MenuVModel>> SaveMenuAsync(MenuVModel model)
        {
            if (string.IsNullOrEmpty(model.Code))
            {
                return ApiResult<MenuVModel>.FailResult("Code不能为空");
            }
            if (model.MenuType != MenuType.Catalog && model.MenuType != MenuType.Menu && model.MenuType != MenuType.Button)
            {
                return ApiResult<MenuVModel>.FailResult($"MenuType必须是以下值之一: {string.Join(", ", MenuType.AllTypes)}");
            }

            return await _menuService.CreateOrUpdateMenuAsync(model);
        }

        //[HttpGet("{id}")] // 接口格式为 /api/user/GetMenuById/123
        [HttpGet] // 接口格式为 /api/user/GetMenuById?id=123
        public async Task<ApiResult<MenuListDTO>> GetMenuByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ApiResult<MenuListDTO>.FailResult("Id不能为空");
            }
            return await _menuService.GetMenuByIdAsync(id);
        }

        [HttpPost]
        public async Task<ApiResult<string>> DeleteMenuByIdsAsync([FromBody] IdsRequest request)
        {
            if (request.Ids.Any(id => string.IsNullOrWhiteSpace(id) || id.Length != 36))
            {
                return ApiResult<string>.FailResult("每个Id不能为空，并且必须是36位字符");
            }
            return await _menuService.DeleteMenuByIdsAsync(request.Ids);
        }

        [HttpGet]
        public async Task<ApiResult<List<MenuListDTO>>> GetAllMenuTreeListAsync([FromQuery] MenuFilterModel filter)
        {
            return await _menuService.GetAllMenuTreeListAsync(filter);
        }

    }
}
