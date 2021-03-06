using Berger.Common.Enumerations;
using BergerMsfaApi.Controllers.HappyWallet.Lead;
using BergerMsfaApi.Controllers.Somporko.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Common
{
    public static class ApplicationAccess
    {
        public static Dictionary<string, List<string>> AccessControllers => new Dictionary<string, List<string>>()
        {
            {
                nameof(EnumApplicationCategory.SomporkoApp),
                new List<string>()
                {
                    nameof(ExternalAppUserInfoController).RemoveControllerEndName()
                }
            },
            {
                nameof(EnumApplicationCategory.HappyWalletApp),
                new List<string>()
                {
                    nameof(ExternalAppLeadController).RemoveControllerEndName()
                }
            }
        };

        public static string RemoveControllerEndName(this string value)
        {
            return value.Replace("Controller", "");
        }
    }
}
