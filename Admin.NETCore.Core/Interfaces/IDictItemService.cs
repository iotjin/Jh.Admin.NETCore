
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.Interfaces
{
    public interface IDictItemService
    {
        Task<ApiResult<DictItemVModel>> CreateOrUpdateDictItemAsync(DictItemVModel model);
        Task<ApiResult<DictItemVModel>> GetDictItemByIdAsync(string id);
        Task<ApiResult<string>> DeleteDictItemByIdsAsync(List<string> ids);
        Task<PagedResult<DictItemListDTO>> GetDictItemListAsync(DictItemFilterModel filter);

        Task<ApiResult<List<DictItemListDTO>>> GetDictItemListByCodeAsync(string code);
    }
}
