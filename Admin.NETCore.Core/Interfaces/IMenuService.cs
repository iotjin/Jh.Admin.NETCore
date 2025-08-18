
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.Interfaces
{
    public interface IMenuService
    {
        Task<ApiResult<MenuVModel>> CreateOrUpdateMenuAsync(MenuVModel model);

        Task<ApiResult<MenuListDTO>> GetMenuByIdAsync(string id);

        Task<ApiResult<string>> DeleteMenuByIdsAsync(List<string> ids);

        Task<ApiResult<List<MenuListDTO>>> GetAllMenuTreeListAsync(MenuFilterModel filter);

        Task<ApiResult<List<MenuListDTO>>> GetMenuInfoByRoleIdAsync(string roleId);

        Task<ApiResult<List<MenuListDTO>>> GetMenuInfoByUserIdAsync(string userId);
    }
}
