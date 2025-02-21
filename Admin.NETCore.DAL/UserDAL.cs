using Admin.NETCore.Common;
using Admin.NETCore.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Admin.NETCore.DAL
{
    public class UserDAL
    {

        /// <summary>
        /// 查询全部  Users
        /// </summary>
        /// <returns></returns>
        /// 
        public List<User> GetAll()
        {
            DataTable res = SqlHelper.ExecuteTable("SELECT * FROM User");
            List<User> userList = ToModelList(res);
            return userList;
        }

        /// <summary>
        /// 通过 Id 获取 user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(string id)
        {
            DataRow row = null;
            DataTable res = SqlHelper.ExecuteTable("SELECT * FROM User WHERE Id=@Id",
                new MySqlParameter("@Id", id));
            if (res.Rows.Count > 0)
            {
                row = res.Rows[0];
            }
            User user = ToModel(row);
            return user;
        }


        public int AddUser(User user)
        {
            return SqlHelper.ExecuteNonQuery(
                  "INSERT INTO User (Id, Name, LoginName, Phone, UserNumber, DeptId, UserExpiryDate, Status, Level, Money, Age, Notes, IsDelete) " +
                  "VALUES (@Id, @Name, @LoginName, @Phone, @UserNumber, @DeptId, @UserExpiryDate, @Status, @Level, @Money, @Age, @Notes, @IsDelete)",
                  new MySqlParameter("@Id", user.Id),
                  new MySqlParameter("@Name", user.Name),
                  new MySqlParameter("@LoginName", user.LoginName),
                  new MySqlParameter("@Phone", user.Phone),
                  new MySqlParameter("@UserNumber", user.UserNumber),
                  new MySqlParameter("@DeptId", user.DeptId),
                  new MySqlParameter("@UserExpiryDate", user.UserExpiryDate),
                  new MySqlParameter("@Status", user.Status),
                  new MySqlParameter("@Level", user.Level),
                  new MySqlParameter("@Money", user.Money),
                  new MySqlParameter("@Age", user.Age),
                  new MySqlParameter("@Notes", user.Notes),
                  new MySqlParameter("@IsDelete", user.IsDelete)
           );
        }


        public int UpdateUser(User user)
        {
            return SqlHelper.ExecuteNonQuery(
                  "UPDATE User SET Name = @Name, LoginName = @LoginName, Phone = @Phone, UserNumber = @UserNumber, DeptId = @DeptId, UserExpiryDate = @UserExpiryDate, Status = @Status, Level = @Level, Money = @Money, Age = @Age, Notes = @Notes, IsDelete = @IsDelete WHERE Id = @Id",

                  new MySqlParameter("@Name", user.Name),
                  new MySqlParameter("@LoginName", user.LoginName),
                  new MySqlParameter("@Phone", user.Phone),
                  new MySqlParameter("@UserNumber", user.UserNumber),
                  new MySqlParameter("@DeptId", user.DeptId),
                  new MySqlParameter("@UserExpiryDate", user.UserExpiryDate),
                  new MySqlParameter("@Status", user.Status),
                  new MySqlParameter("@Level", user.Level),
                  new MySqlParameter("@Money", user.Money),
                  new MySqlParameter("@Age", user.Age),
                  new MySqlParameter("@Notes", user.Notes),
                  new MySqlParameter("@IsDelete", user.IsDelete),
                  new MySqlParameter("@Id", user.Id)
           );
        }

        /// <summary>
        /// 通过id 删除一个User 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteUserById(string id)
        {
            return SqlHelper.ExecuteNonQuery(
                  "DELETE FROM User WHERE Id = @Id",
                  new MySqlParameter("@Id", id));
        }


        private List<User> ToModelList(DataTable table)
        {
            List<User> userList = new List<User>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                User user = ToModel(row);
                userList.Add(user);
            }
            return userList;
        }

        /// <summary>
        /// 必须使用  FromDbValue ，否则使用sql 语句报错
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private User ToModel(DataRow row)
        {
            User user = new User();
            user.Id = (Guid)SqlHelper.FromDbValue(row["Id"]);
            user.UserNumber = (int)SqlHelper.FromDbValue(row["UserNumber"]);
            user.Name = SqlHelper.FromDbValue(row["Name"]).ToString();
            user.LoginName = SqlHelper.FromDbValue(row["LoginName"]).ToString();
            user.Phone = SqlHelper.FromDbValue(row["Phone"]).ToString();
            user.DeptId = SqlHelper.FromDbValue(row["DeptId"]).ToString();
            user.UserExpiryDate = (DateTime)SqlHelper.FromDbValue(row["UserExpiryDate"]);
            user.Status = (bool)SqlHelper.FromDbValue(row["Status"]);
            user.Level = (bool)SqlHelper.FromDbValue(row["Level"]) ? 1 : 0;
            user.Money = SqlHelper.FromDbValue(row["Money"]) as double?;
            user.Age = SqlHelper.FromDbValue(row["Age"]) as int?;
            user.Notes = SqlHelper.FromDbValue(row["Notes"]).ToString();
            user.IsDelete = (bool)SqlHelper.FromDbValue(row["IsDelete"]);
            return user;
        }
    }
}