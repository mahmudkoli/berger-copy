using Berger.Data.MsfaEntity.DemandGeneration;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Models.HappyWallet.Lead;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.HappyWallet.Lead.Interfaces
{
    public interface IHappyWalletLeadService
    {
        Task<IList<HappyWalletAppLeadStatusModel>> GetAllHappyWalletLeadsStatusByLeadIdsAsync(IList<string> ids);
        Task<IList<HappyWalletAppLeadDetailModel>> GetAllHappyWalletLeadsDetailByLeadIdsAsync(IList<string> ids);
    }
}
