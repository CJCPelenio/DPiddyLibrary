using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DPiddyLibrary.Repositories
{
    public abstract class BaseRepo
    {
        private readonly string _connectionString;
        public BaseRepo()
        {
            _connectionString = "Data Source=.;Initial Catalog=DPiddyLibrary;Integrated Security=True;TrustServerCertificate=True";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
