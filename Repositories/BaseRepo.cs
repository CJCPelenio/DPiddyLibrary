using System.Data.SqlClient;

namespace DPiddyLibrary.Repositories
{
    public abstract class BaseRepo
    {
        private readonly string _connectionString;
        public BaseRepo()
        {
            _connectionString = "Data Source=127.0.0.1;Initial Catalog=DPiddyLibrary;Persist Security Info=True;User ID=sqlserver;Password=dpiddylibrary;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
