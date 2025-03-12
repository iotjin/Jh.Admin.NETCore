using Microsoft.AspNetCore.Mvc;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB.Entities;

namespace Admin.NETCore.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Route("api/[controller]")]  //restful 风格
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

        [HttpGet("{id}")]
        public async Task<ApiResult<UserVModel>> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ApiResult<UserVModel>.FailResult("id不能为空");
            }
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPost]
        public async Task<ApiResult<string>> DeleteUserByIdAsync([FromBody] BaseRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return ApiResult<string>.FailResult("id不能为空");
            }
            return await _userService.DeleteUserByIdAsync(request.Id);
        }

        [HttpGet]
        public async Task<PagedResult<User>> GetUserListAsync([FromQuery] UserFilterModel filter)
        {
            return await _userService.GetUserListAsync(filter);
        }

    }
}
