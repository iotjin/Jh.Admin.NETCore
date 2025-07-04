using Microsoft.AspNetCore.Mvc;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")] // RPC风格, api/[控制器名称]/函数名称
    //[Route("api/[controller]")]  // RESTful 风格
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ApiResult<UserVModel>> CreateUserAsync(UserVModel model)
        {
            if (string.IsNullOrEmpty(model.LoginName))
            {
                return ApiResult<UserVModel>.FailResult("用户名不能为空");
            }
            return await _userService.CreateUserAsync(model);
        }


        [HttpPost]
        public async Task<ApiResult<UserVModel>> UpdateUserAsync(UserVModel model)
        {
            if (string.IsNullOrEmpty(model.LoginName))
            {
                return ApiResult<UserVModel>.FailResult("用户名不能为空");
            }
            return await _userService.UpdateUserAsync(model);
        }

        //[HttpGet("{id}")] // 接口格式为 /api/user/GetUserById/123
        [HttpGet] // 接口格式为 /api/user/GetUserById?id=123
        public async Task<ApiResult<UserVModel>> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ApiResult<UserVModel>.FailResult("Id不能为空");
            }
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPost]
        public async Task<ApiResult<string>> DeleteUserByIdAsync([FromBody] IdRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return ApiResult<string>.FailResult("Id不能为空");
            }
            return await _userService.DeleteUserByIdAsync(request.Id);
        }

        [HttpGet]
        public async Task<PagedResult<UserDTO>> GetUserListAsync([FromQuery] UserFilterModel filter)
        {
            return await _userService.GetUserListAsync(filter);
        }


        [HttpPost]
        public async Task<ApiResult<string>> AssignRoleAsync([FromBody] AssignRoleVModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return ApiResult<string>.FailResult("Id不能为空");
            }
            return await _userService.AssignRoleAsync(model.Id, model.RoleIds);
        }

    }
}
