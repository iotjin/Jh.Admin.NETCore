using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.Version
{

    [ApiController]
    [Route("v1/api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "v1")] // 写在这里的话整个 Controller 属于 V1
    [Obsolete("🚨 此控制器已废弃，请使用 v2")]
    public class V1BaseController : ControllerBase
    {

    }

}


/*
  若希望所有继承自 V1BaseController 的子类自动标记为废弃，可通过自定义过滤器实现。InheritedObsoleteOperationFilter

  或者直接通过在子类添加Obsolete
 */