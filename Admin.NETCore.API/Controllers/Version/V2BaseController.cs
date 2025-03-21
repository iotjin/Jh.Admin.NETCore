using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.Version
{

    [ApiController]
    [Route("v2/api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "v2")] // 写在这里的话整个 Controller 属于 V2
    public class V2BaseController : ControllerBase
    {

    }
}
