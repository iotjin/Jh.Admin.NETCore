
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<UserVModel>> CreateUserAsync(UserVModel model);
        Task<ApiResult<UserVModel>> UpdateUserAsync(UserVModel model);
        Task<ApiResult<UserVModel>> GetUserByIdAsync(string id);
        Task<ApiResult<string>> DeleteUserByIdAsync(string id);
        Task<PagedResult<UserDTO>> GetUserListAsync(UserFilterModel filter);
        Task<ApiResult<string>> AssignRoleAsync(string userId, List<string> roleIds);
    }
}
