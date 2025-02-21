using MySql.Data.MySqlClient;
using System.Data;

namespace Admin.NETCore.Common
{
    public class SqlHelper
    {

        //public static string ConnectionString2 { get; set; } = "server=localhost;Database=testdb;Uid=root;Pwd=root;";

        //public static DataTable ExecuteTable2(string cmdText, params SqlParameter[] sqlParameters)
        //{
        //    using SqlConnection conn = new SqlConnection(ConnectionString2);
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand(cmdText, conn);
        //    cmd.Parameters.AddRange(sqlParameters);
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    sda.Fill(ds);
        //    return ds.Tables[0];
        //}


        public static string ConnectionString { get; set; } = "server=localhost;Database=testdb;Uid=root;Pwd=root;";

        public static DataTable ExecuteTable(string cmdText, params MySqlParameter[] sqlParameters)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.Parameters.AddRange(sqlParameters);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds.Tables[0];
        }

        public static int ExecuteNonQuery(string cmdText, params MySqlParameter[] sqlParameters)
        {

            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            cmd.Parameters.AddRange(sqlParameters);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 将值传入到数据库时，如果值为null则转化为数据库的DBNull
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 从数据库取值时，如果取出的值为DBNull则转化为null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object? FromDbValue(object value)
        {
            return value == DBNull.Value ? null : value;
        }

    }
}