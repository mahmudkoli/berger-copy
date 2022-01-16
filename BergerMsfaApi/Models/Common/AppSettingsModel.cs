using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Common
{
    public class ActiveDirectorySettingsModel
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsIgnoreADLogin { get; set; }
    }

    public class TokensSettingsModel
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresHours { get; set; }
        public int ExternalAppExpiresHours { get; set; }
    }

    public class FCMSettingsModel
    {
        public string FileName { get; set; }
    }

    public class AppSettingsModel
    {
        public string ExternalAppSecurityKey { get; set; }
    }
}
