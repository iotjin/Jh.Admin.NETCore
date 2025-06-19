using Microsoft.AspNetCore.Mvc;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")] // RPC风格, api/[控制器名称]/函数名称
    //[Route("api/[controller]")]  // RESTful 风格
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ApiResult<RoleVModel>> CreateRoleAsync(RoleVModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return ApiResult<RoleVModel>.FailResult("角色名不能为空");
            }
            return await _roleService.CreateRoleAsync(model);
        }


        [HttpPost]
        public async Task<ApiResult<RoleVModel>> UpdateRoleAsync(RoleVModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return ApiResult<RoleVModel>.FailResult("角色名不能为空");
            }
            return await _roleService.UpdateRoleAsync(model);
        }

        //[HttpGet("{id}")] // 接口格式为 /api/user/GetRoleById/123
        [HttpGet] // 接口格式为 /api/user/GetRoleById?id=123
        public async Task<ApiResult<RoleVModel>> GetRoleByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ApiResult<RoleVModel>.FailResult("id不能为空");
            }
            return await _roleService.GetRoleByIdAsync(id);
        }

        [HttpPost]
        public async Task<ApiResult<string>> DeleteRoleByIdAsync([FromBody] IDRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return ApiResult<string>.FailResult("id不能为空");
            }
            return await _roleService.DeleteRoleByIdAsync(request.Id);
        }

        [HttpGet]
        public async Task<PagedResult<RoleListDTO>> GetRoleListAsync([FromQuery] RoleFilterModel filter)
        {
            return await _roleService.GetRoleListAsync(filter);
        }

        [HttpGet]
        public async Task<ApiResult<List<RoleListDTO>>> GetRoleListByUserIdAsync([FromQuery] UserRolesVModel filter)
        {
            if (string.IsNullOrEmpty(filter.UserId))
            {
                return ApiResult<List<RoleListDTO>>.FailResult("用户ID不能为空");
            }
            return await _roleService.GetRoleListByUserIdAsync(filter);
        }

    }
}
