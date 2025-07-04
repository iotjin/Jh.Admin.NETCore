using Microsoft.AspNetCore.Mvc;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.API.Controllers
{

    [ApiController]
    [Route("api/Dict/[action]")] // 方式一(两个控制器合一)
    //[Route("api/[controller]/[action]")] // RPC风格, api/[控制器名称]/函数名称
    ////[Route("api/[controller]")]  // RESTful 风格
    public class DictItemController : ControllerBase
    {
        //private readonly IDictItemService _DictItemService;

        private readonly IDictItemService _dictItemService;

        public DictItemController(IDictItemService dictItemService)
        {
            _dictItemService = dictItemService;
        }

        // 方式二，代码写一个文件内
        //public DictController(IDictItemService DictItemService, IDictItemService dictItemService)
        //{
        //    _DictItemService = DictItemService;
        //    _dictItemService = dictItemService;
        //}

        // 字典项相关的接口

        [HttpPost]
        public async Task<ApiResult<DictItemVModel>> SaveDictItemAsync(DictItemVModel model)
        {
            if (string.IsNullOrEmpty(model.Value))
            {
                return ApiResult<DictItemVModel>.FailResult("Value不能为空");
            }
            return await _dictItemService.CreateOrUpdateDictItemAsync(model);
        }

        //[HttpGet("{id}")] // 接口格式为 /api/user/GetDictItemById/123
        [HttpGet] // 接口格式为 /api/user/GetDictItemById?id=123
        public async Task<ApiResult<DictItemVModel>> GetDictItemByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ApiResult<DictItemVModel>.FailResult("Id不能为空");
            }
            return await _dictItemService.GetDictItemByIdAsync(id);
        }

        [HttpPost]
        public async Task<ApiResult<string>> DeleteDictItemByIdsAsync([FromBody] IdsRequest request)
        {
            if (request.Ids.Any(id => string.IsNullOrWhiteSpace(id) || id.Length != 36))
            {
                return ApiResult<string>.FailResult("每个Id不能为空，并且必须是36位字符");
            }
            return await _dictItemService.DeleteDictItemByIdsAsync(request.Ids);
        }

        [HttpGet]
        public async Task<PagedResult<DictItemListDTO>> GetDictItemListAsync([FromQuery] DictItemFilterModel filter)
        {
            return await _dictItemService.GetDictItemListAsync(filter);
        }

        [HttpGet]
        public async Task<ApiResult<List<DictItemListDTO>>> GetDictItemListByCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return ApiResult<List<DictItemListDTO>>.FailResult("Code不能为空");
            }
            return await _dictItemService.GetDictItemListByCodeAsync(code);
        }

    }
}
