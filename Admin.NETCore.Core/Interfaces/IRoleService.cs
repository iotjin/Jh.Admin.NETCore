
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResult<RoleVModel>> CreateRoleAsync(RoleVModel role);
        Task<ApiResult<RoleVModel>> UpdateRoleAsync(RoleVModel model);
        Task<ApiResult<RoleVModel>> GetRoleByIdAsync(string id);
        Task<ApiResult<string>> DeleteRoleByIdAsync(string id);
        Task<PagedResult<RoleListDTO>> GetRoleListAsync(RoleFilterModel filter);
        Task<ApiResult<List<RoleListDTO>>> GetRoleListByUserIdAsync(UserRolesVModel filter);
    }
}
