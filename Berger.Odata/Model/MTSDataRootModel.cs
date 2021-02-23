using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class MTSDataRootModel
    {
        public string period { get; set; }
        public string pernr { get; set; }
        public string RPMKR { get; set; }
        public string regiogroup { get; set; }
        public string kdgrp { get; set; }
        public string meins { get; set; }
        public string cname { get; set; }
        public string asp { get; set; }
        public string tarvol { get; set; }
        public string tarval { get; set; }
        public string ktokd { get; set; }
        public string gsber { get; set; }
        public string vkbur { get; set; }
        public string vkgrp { get; set; }
        public string kunnr { get; set; }
        public string vtweg { get; set; }
        public string spart { get; set; }
        public string matkl { get; set; }

        public MTSDataModel ToModel()
        {
            var model = new MTSDataModel();
            model.Date = this.period;
            model.EmployeeNo = this.pernr;
            model.Territory = this.RPMKR;
            model.Zone = this.regiogroup;
            model.CustomerGroup = this.kdgrp;
            model.UnitOfMeasure = this.meins;
            model.CustomerName = this.cname;
            model.AverageSalesPrice = this.asp;
            model.TargetVolume = this.tarvol;
            model.TargetValue = this.tarval;
            model.CustomerAccountGroup = this.ktokd;
            model.PlantOrBusinessArea = this.gsber;
            model.SalesOffice = this.vkbur;
            model.SalesGroup = this.vkgrp;
            model.CustomerNo = this.kunnr;
            model.DistributionChannel = this.vtweg;
            model.Division = this.spart;
            model.MatarialGroupOrBrand = this.matkl;
            return model;
        }
    }
}
