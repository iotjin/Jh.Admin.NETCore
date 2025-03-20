using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.version
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Route("api/[controller]")]  //restful 风格

    //[ApiExplorerSettings(GroupName = "v1")] // 写在这里的话整个 Controller 属于 V1，如果这个实现内部的会失效
    public class TestVersionController : ControllerBase
    {

        [HttpGet]
        [ApiExplorerSettings(GroupName = "v2")] // 该 Action 属于 V2 文档
        public IActionResult GetTestVersionV2() => Ok("Version 2");


        [HttpGet("v1")]
        [ApiExplorerSettings(GroupName = "v1")] // 该 Action 属于 V1 文档
        public IActionResult GetTestVersionV1() => Ok("Version 1");


        [HttpGet]
        public object[] GetTest1()
        {
            var tempArr = new[] {
                new { Id = 1, Name = "Name1" },
                new { Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

        [HttpGet]
        public IEnumerable<Dictionary<string, object>> GetTest2()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object> { { "Id", 1 }, { "Name", "Name1" } },
                new Dictionary<string, object> { { "Id", 2 }, { "Name", "Name2" } }
            };
        }

        public class TestModelA
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [HttpGet]
        public IEnumerable<TestModelA> GetTest3()
        {
            var tempArr = new List<TestModelA> {
                new TestModelA { Id = 1, Name = "Name1" },
                new TestModelA{ Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

        public record TestModelB(int Id, string Name);

        [HttpGet]
        [Obsolete("🚨 此接口已废弃")]
        public IEnumerable<TestModelB> GetTest4()
        {
            Response.Headers.Add("Deprecation", "version=\"1.0\"");
            Response.Headers.Add("Sunset", "Sat, 31 Dec 2023 23:59:59 GMT");
            Response.Headers.Add("version", "version=\"1.0\"");

            var tempArr = new List<TestModelB>
                {
                    new(1, "Name1"),
                    new(2, "Name2")
                };
            return tempArr;
        }
    }
}
