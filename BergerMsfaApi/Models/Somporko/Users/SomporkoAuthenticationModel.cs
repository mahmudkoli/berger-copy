using BergerMsfaApi.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Somporko.Users
{
    public class SomporkoAuthenticateUserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public SomporkoAuthenticateUserModel()
        {

        }
    }
}
