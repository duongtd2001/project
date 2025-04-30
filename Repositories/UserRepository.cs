using ExcelDataReader;
using project.Class;
using project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace project.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private ReadExcelData readExcelData;
        private UserModel _user;
        //
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
            // SQL
            //bool validUser;
            //using (var connection = GetConnection())
            //using (var command = new SqlCommand())
            //{
            //    //connection.Open();
            //    //command.Connection = connection;
            //    command.CommandText = "select *from [User] where username=@username and [password]=@password";
            //    command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
            //    command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
            //    validUser = command.ExecuteScalar() == null ? false : true;
            //}

            //return validUser;

            readExcelData = new ReadExcelData();
            //Excel
            bool validUser = false;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(readExcelData.pathExcel, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                });

                var table = dataSet.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    if (row[2].ToString() == credential.UserName && row[1].ToString() == credential.Password)
                    {
                        validUser = true;
                        _user = new UserModel
                        {
                            Name = row[2].ToString()
                        };
                        break;
                    }
                }
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
            if (_user != null && _user.Name == username)
            {
                return _user;
            }

            return null;
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
