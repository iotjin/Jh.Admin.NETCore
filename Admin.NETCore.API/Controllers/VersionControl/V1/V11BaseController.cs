using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.VersionControl.V1
{

    [ApiController]
    //[ApiVersion(11.1, Deprecated = true)] // Deprecated=true 表示 v1 即将弃用
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public abstract class V11BaseController : ControllerBase
    {

    }
}