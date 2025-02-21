using Admin.NETCore.Models;

namespace Admin.NETCore.BLL.Interfaces
{
    public interface IUserBLL
    {
        List<User> GetUserList();

        User GetUserById(string id);

        string SaveUser(User user);

        string DeleteUserById(string id);
    }
}
