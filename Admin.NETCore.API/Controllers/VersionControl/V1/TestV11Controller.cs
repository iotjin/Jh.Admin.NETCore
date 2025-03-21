using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.VersionControl.V1
{

    [ApiVersion(11.1, Deprecated = true)] // Deprecated=true 表示 v1 即将弃用
    public class TestV11FController : V11BaseController
    {

        [HttpGet]
        [ApiExplorerSettings(GroupName = "v20")]
        public IActionResult GetV2() => Ok("Version 20");


        [HttpGet("v10")]
        [ApiExplorerSettings(GroupName = "v10")]
        public IActionResult GetV1() => Ok("Version 10");

        public class TestModelC
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [HttpGet]
        public object[] GetV1Test1()
        {
            var tempArr = new[] {
                new { Id = 1, Name = "Name1" },
                new { Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

        [HttpGet]
        public IEnumerable<TestModelC> GetV1Test2()
        {
            var tempArr = new List<TestModelC> {
                new TestModelC { Id = 1, Name = "Name1" },
                new TestModelC{ Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

    }
}
