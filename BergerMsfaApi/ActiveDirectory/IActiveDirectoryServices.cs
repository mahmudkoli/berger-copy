using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.ActiveDirectory
{
    public interface IActiveDirectoryServices
    {
        bool AuthenticateUser(string username, string password);
        AdModel GetUserByUserName(String userName);
    }
}
