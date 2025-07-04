
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.Interfaces
{
    public interface IDictTypeService
    {
        Task<ApiResult<DictTypeVModel>> CreateOrUpdateDictTypeAsync(DictTypeVModel model);
        Task<ApiResult<DictTypeVModel>> GetDictTypeByIdAsync(string id);
        Task<ApiResult<string>> DeleteDictTypeByIdsAsync(List<string> ids);
        Task<PagedResult<DictTypeListDTO>> GetDictTypeListAsync(DictTypeFilterModel filter);

        Task<ApiResult<DictTypesAndItemsDTO>> GetDictTypesAndItemsAsync(string codes);

        Task<ApiResult<Dictionary<string, List<DictItemSimpleDto>>>> GetDictTypesAndItems2Async(string codes);
    }
}
