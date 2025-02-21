using Admin.NETCore.BLL.Interfaces;
using Admin.NETCore.DAL;
using Admin.NETCore.Models;

namespace Admin.NETCore.BLL
{
    public class UserBLL : IUserBLL
    {
        UserDAL userDal = new UserDAL();
        /// <summary>
        /// 调用 userDal.GetAll()得到全部数据 
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            // IsDelete 数据库中的默认的值为0 表示该用户 已删除（软删除）
            return userDal.GetAll().FindAll(m => !m.IsDelete);
        }

        public User GetUserById(string id)
        {
            return userDal.GetUserById(id);
        }

        public string SaveUser(User user)
        {
            //if (user.Id == null || user.Id.ToString() == "" || user.Id == Guid.Empty)
            if (!user.Id.HasValue || user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
                user.IsDelete = false;
                int rows = userDal.AddUser(user);
                if (rows > 0)
                {
                    return "数据新增成功";
                }
                else
                {
                    return "数据新增失败";
                }
            }
            else
            {
                int rows = userDal.UpdateUser(user);
                if (rows > 0)
                {
                    return "数据更新成功";
                }
                else
                {
                    return "数据更新失败";
                }
            }
        }

        public string DeleteUserById(string id)
        {
            UserDAL userDal = new UserDAL();
            int rows = userDal.DeleteUserById(id);
            if (rows > 0)
            {
                return "数据删除成功";
            }
            else
            {
                return "数据删除失败";
            }
        }
    }
}