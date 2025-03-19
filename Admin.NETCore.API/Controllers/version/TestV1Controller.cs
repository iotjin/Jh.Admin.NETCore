using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers
{
    // [Obsolete("🚨 此控制器已废弃，请使用 v2")] // 若希望所有继承自 V1BaseController 的子类自动标记为废弃，可通过自定义过滤器实现。InheritedObsoleteOperationFilter
    public class TestV1Controller : V1BaseController
    {

        [HttpGet]
        [ApiExplorerSettings(GroupName = "v2")] // 正常这里的 Action 属于 V2 文档 (注意：控制器加了 ApiExplorerSettings，这里的不生效)
        public IActionResult GetV2() => Ok("Version 2");


        [HttpGet("v1")]
        [ApiExplorerSettings(GroupName = "v1")] // 正常这里的 Action 属于 V1 文档 (注意：控制器加了 ApiExplorerSettings，这里的不生效)
        public IActionResult GetV1() => Ok("Version 1");

        public class TestModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [HttpGet]
        public object[] GetV1Test1()
        {
            var tempArr = new [] {
                new { Id = 1, Name = "Name1" },
                new { Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

        [HttpGet]
        public IEnumerable<TestModel> GetV1Test2()
        {
            var tempArr = new List<TestModel> {
                new TestModel { Id = 1, Name = "Name1" },
                new TestModel{ Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

    }
}
