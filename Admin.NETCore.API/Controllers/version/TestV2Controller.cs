using Microsoft.AspNetCore.Mvc;

namespace Admin.NETCore.API.Controllers.version
{
    public class TestV2Controller : V2BaseController
    {
        public class TestModel
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
        public IEnumerable<TestModel> GetV2Test2()
        {
            var tempArr = new List<TestModel> {
                new TestModel { Id = 1, Name = "Name1" },
                new TestModel{ Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

    }
}
