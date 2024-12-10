using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DPiddyLibrary.Model;

namespace DPiddyLibrary.Repositories
{
    public class LoginRepo : BaseRepo, ILoginRepo
    {
        public void Add(Login login)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var con = GetConnection())
            using (var command = new SqlCommand())
            {
                con.Open();
                command.Connection = con;
                command.CommandText = "SELECT Username, AccPassword FROM [StaffAccount] WHERE Username = @Username AND [AccPassword] = @Password";
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() != null;
            }
            return validUser;
        }

        public void Edit(Login login)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Login> GetByAll()
        {
            throw new NotImplementedException();
        }

        public Login GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Login GetByUsername(string username)
        {
            Login validUser = null;
            using (var con = GetConnection())
            using (var command = new SqlCommand())
            {
                con.Open();
                command.Connection = con;
                command.CommandText = "SELECT Username, AccPassword FROM [StaffAccount] WHERE Username = @Username";
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        validUser = new Login()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                        };
                    }
                }
            }
            return validUser;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
