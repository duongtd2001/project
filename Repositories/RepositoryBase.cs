using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using project.Models;

namespace project.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString;
        public RepositoryBase()
        {
            //User ID = ost_pe; Password = ost_pe@spclt
            connectionString = $"{DataConfigModel.DataSource};{DataConfigModel.InitialCatalog};{DataConfigModel.PersistSecurityInfo};" +
                $"{DataConfigModel.UserID};{DataConfigModel.Password};";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        } 
    }
}
