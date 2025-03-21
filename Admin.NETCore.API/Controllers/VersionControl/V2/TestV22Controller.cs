using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.VersionControl.V2
{
    public class TestV22DController : V22BaseController
    {

        [HttpGet]
        [ApiExplorerSettings(GroupName = "v20")]
        public IActionResult GetV26() => Ok("Version 26");
        public class TestModelD
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [HttpGet]
        public object[] GetV2Test1()
        {
            var tempArr = new[] {
                new { Id = 1, Name = "Name1" },
                new { Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

        [HttpGet]
        public IEnumerable<TestModelD> GetV2Test2()
        {
            var tempArr = new List<TestModelD> {
                new TestModelD { Id = 1, Name = "Name1" },
                new TestModelD{ Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

    }
}
