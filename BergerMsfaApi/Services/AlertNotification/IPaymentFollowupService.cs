﻿using Berger.Data.MsfaEntity.AlertNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Service.AlertNotification
{
    public interface IPaymentFollowupService
    {
        Task<bool> SavePaymentFollowup(IList<PaymentFollowup> paymentFollowups);
    }
}