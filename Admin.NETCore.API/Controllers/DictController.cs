using Microsoft.AspNetCore.Mvc;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")] // RPC风格, api/[控制器名称]/函数名称
    //[Route("api/[controller]")]  // RESTful 风格
    public class DictController : ControllerBase
    {
        private readonly IDictTypeService _dictTypeService;

        public DictController(IDictTypeService dictTypeService)
        {
            _dictTypeService = dictTypeService;
        }

        // 字典类型相关的接口

        [HttpPost]
        public async Task<ApiResult<DictTypeVModel>> SaveDictTypeAsync(DictTypeVModel model)
        {
            if (string.IsNullOrEmpty(model.Code))
            {
                return ApiResult<DictTypeVModel>.FailResult("Code不能为空");
            }
            return await _dictTypeService.CreateOrUpdateDictTypeAsync(model);
        }

        //[HttpGet("{id}")] // 接口格式为 /api/user/GetDictTypeById/123
        [HttpGet] // 接口格式为 /api/user/GetDictTypeById?id=123
        public async Task<ApiResult<DictTypeVModel>> GetDictTypeByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ApiResult<DictTypeVModel>.FailResult("Id不能为空");
            }
            return await _dictTypeService.GetDictTypeByIdAsync(id);
        }

        [HttpPost]
        public async Task<ApiResult<string>> DeleteDictTypeByIdsAsync([FromBody] IdsRequest request)
        {
            if (request.Ids.Any(id => string.IsNullOrWhiteSpace(id) || id.Length != 36))
            {
                return ApiResult<string>.FailResult("每个Id不能为空，并且必须是36位字符");
            }
            return await _dictTypeService.DeleteDictTypeByIdsAsync(request.Ids);
        }

        [HttpGet]
        public async Task<PagedResult<DictTypeListDTO>> GetDictTypeListAsync([FromQuery] DictTypeFilterModel filter)
        {
            return await _dictTypeService.GetDictTypeListAsync(filter);
        }

        [HttpGet]
        public async Task<ApiResult<DictTypesAndItemsDTO>> GetDictTypesAndItemsAsync(string codes)
        {
            return await _dictTypeService.GetDictTypesAndItemsAsync(codes);
        }

        [HttpGet]
        public async Task<ApiResult<Dictionary<string, List<DictItemSimpleDto>>>> GetDictTypesAndItems2Async(string codes)
        {
            return await _dictTypeService.GetDictTypesAndItems2Async(codes);
        }

    }
}
