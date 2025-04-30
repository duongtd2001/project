using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace project.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString;
        public RepositoryBase()
        {
            connectionString = "Data Source=192.168.100.100;Initial Catalog=OST; Persist Security Info=True;User ID=ost_pe;Password=ost_pe@spclt";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
