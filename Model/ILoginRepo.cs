using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DPiddyLibrary.Model
{
    public interface ILoginRepo
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(Login login);
        void Edit(Login login);
        void Remove(int id);
        Login GetById(int id);
        Login GetByUsername(string username);
        IEnumerable<Login> GetByAll();
    }
}
