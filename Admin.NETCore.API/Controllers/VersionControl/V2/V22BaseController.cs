using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.VersionControl.V2
{

    [ApiController]
    [ApiVersion(22.2)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public abstract class V22BaseController : ControllerBase
    {

    }
}
