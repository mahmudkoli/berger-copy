using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Common
{
    public class AppActiveDirectorySettingsModel
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AppTokensSettingsModel
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresHours { get; set; }
    }
}
