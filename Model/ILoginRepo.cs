using System.Collections.Generic;
using System.Net;

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
