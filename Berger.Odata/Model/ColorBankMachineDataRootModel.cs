namespace Berger.Odata.Model
{
    public class ColorBankMachineDataRootModel
    {
        public string bukrs { get; set; }
        public string aktiv { get; set; }
        public string installdate { get; set; }
        public string transferdate { get; set; }
        public string expirydate { get; set; }
        public string downpayment { get; set; }
        public string eligibility { get; set; }
        public string installamount { get; set; }
        public string installno { get; set; }
        public string ownername { get; set; }
        public string paymentmode { get; set; }
        public string anlkl { get; set; }
        public string remainingval { get; set; }
        public string period { get; set; }
        public string subdealer { get; set; }
        public string upstat { get; set; }
        public string gsber { get; set; }
        public string gtext { get; set; }
        public string mdname { get; set; }
        public string sdname { get; set; }
        public string anln1 { get; set; }
        public string anln2 { get; set; }
        public string ktokd { get; set; }
        public string maindealer { get; set; }
        public string machinestatus { get; set; }
        public string txt50 { get; set; }
        public string txa50 { get; set; }

        public ColorBankMachineDataModel ToModel()
        {
            var model = new ColorBankMachineDataModel
            {
                Date = period,
                CustomerNo = maindealer,
                CustomerName = mdname,
                Depot = gsber,
                DepotName = gtext,
                CompanyCode = bukrs,
                InstallDate = installdate,
                TransferDate = transferdate,
                ExpiryDate = expirydate,
                DownPayment = downpayment,
                AccountGroup = ktokd,
                InstallAmount = installamount,
                InstallNo = installno,
                Ownername = ownername,
                PaymentMode = paymentmode,
                anlkl = anlkl,
                RemainingVal = remainingval,
                SubDealer = subdealer,
                UpStat = upstat,
                sdname = sdname,
                anln1 = anln1,
                anln2 = anln2,
                MachineStatus = machinestatus,
                Txt50 = txt50,
                Txa50 = txa50,
               
            };

            return model;
        }
    }

    public class ColorBankMachineDataModel
    {
        public string Date { get; internal set; }
        public string AccountGroup { get; internal set; }
        public string CustomerName { get; internal set; }
        public string CustomerNo { get; internal set; }
        public string Depot { get; internal set; }
        public string DepotName { get; set; }
        public string CompanyCode { get; set; }
        public string InstallDate { get; set; }
        public string TransferDate { get; set; }
        public string ExpiryDate { get; set; }
        public string DownPayment { get; set; }
        public string InstallAmount { get; set; }
        public string InstallNo { get; set; }
        public string Ownername { get; set; }
        public string PaymentMode { get; set; }
        public string anlkl { get; set; }
        public string RemainingVal { get; set; }
        public string SubDealer { get; set; }
        public string UpStat { get; set; }
        public string sdname { get; set; }
        public string anln1 { get; set; }
        public string anln2 { get; set; }
        public string MachineStatus { get; set; }
        public string Txt50 { get; set; }
        public string Txa50 { get; set; }
    }




}
