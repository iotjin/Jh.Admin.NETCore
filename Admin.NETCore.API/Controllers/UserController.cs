using Admin.NETCore.BLL.Interfaces;
using Admin.NETCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Route("api/[controller]")]  //restful 风格
    public class UserController : ControllerBase
    {
        private readonly IUserBLL userBLL;

        public UserController(IUserBLL usersBLL)
        {
            userBLL = usersBLL;
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

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
        public IEnumerable<TestModel> GetTest2()
        {
            var tempArr = new List<TestModel> {
                new TestModel { Id = 1, Name = "Name1" },
                new TestModel{ Id = 2, Name = "Name2" }
            };
            return tempArr;
        }

        [HttpGet]
        public List<User> GetUserList()
        {
            var tempArr = userBLL.GetUserList();
            return tempArr;
        }

        [HttpGet]
        public User? GetUserById([Required(ErrorMessage = "User ID is required.")] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            var user = userBLL.GetUserById(id);
            return user;
        }

        [HttpPost]
        public string SaveUser(User user)
        {
            return userBLL.SaveUser(user);
        }

        [HttpDelete]
        public string DeleteUserById([Required(ErrorMessage = "User ID is required.")] string id)
        {
            return userBLL.DeleteUserById(id);
        }
    }
}
