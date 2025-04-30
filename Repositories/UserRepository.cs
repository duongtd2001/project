using project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace project.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            string query = @"INSERT INTO dbo.ASS_LX (Machine, ID, Name, PO, LotNo, Time) 
                         VALUES (@Machine, @ID, @Name, @PO, @LotNo, @Time)";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Machine", userModel.Machine);
                cmd.Parameters.AddWithValue("@ID", userModel.ID);
                cmd.Parameters.AddWithValue("@Name", userModel.Name);
                cmd.Parameters.AddWithValue("@PO", userModel.ProducName);
                cmd.Parameters.AddWithValue("@LotNo", userModel.LotNo);
                cmd.Parameters.AddWithValue("@Time", userModel.Time);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                //connection.Open();
                //command.Connection = connection;
                command.CommandText = "select *from [User] where username=@username and [password]=@password";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            string query = @"UPDATE dbo.ASS_LX 
                         SET Machine = @Machine, Name = @Name, PO = @PO, LotNo = @LotNo, Time = @Time 
                         WHERE ID = @ID";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Machine", userModel.Machine);
                cmd.Parameters.AddWithValue("@Name", userModel.Name);
                cmd.Parameters.AddWithValue("@PO", userModel.ProducName);
                cmd.Parameters.AddWithValue("@LotNo", userModel.LotNo);
                cmd.Parameters.AddWithValue("@Time", userModel.Time);
                cmd.Parameters.AddWithValue("@ID", userModel.ID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            //using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            //{
            //    connection.Open();
            //    command.Connection = connection;
            //    command.CommandText = "select *from [User] where username=@username";
            //    command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
            //    using (var reader = command.ExecuteReader())
            //    {
            //        if (reader.Read())
            //        {
            //            user = new UserModel()
            //            {
            //                Id = reader[0].ToString(),
            //                Username = reader[1].ToString(),
            //                Password = string.Empty,
            //                Name = reader[3].ToString(),
            //                LastName = reader[4].ToString(),
            //                Email = reader[5].ToString(),
            //            };
            //        }
            //    }
            //}
            return user;
        }

        public void Remove(string Id)
        {
            string query = @"DELETE FROM dbo.ASS_LX 
                         WHERE ID = @ID AND Time = (
                             SELECT MAX(Time) FROM dbo.ASS_LX WHERE ID = @ID)";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", Id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
