using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class SalesDataRootModel
    {
        public string TotaledProperties { get; set; }
        public string pay_responsible { get; set; }
        public string bill_status { get; set; }
        public string arktx { get; set; }
        public string aubel { get; set; }
        public string aupos { get; set; }
        public string bstkd_e { get; set; }
        public string bstkd_e_T { get; set; }
        public string bukrs { get; set; }
        public string bzirk { get; set; }
        public string charg { get; set; }
        public string cname { get; set; }
        public string erdat { get; set; }
        public string erdat_T { get; set; }
        public string ernam { get; set; }
        public string erzet { get; set; }
        public string erzet_T { get; set; }
        public string fkart { get; set; }
        public string fkdat { get; set; }
        public string fkdat_T { get; set; }
        public string gewei { get; set; }
        public string gk { get; set; }
        public string groes { get; set; }
        public string gsber { get; set; }
        public string kdgrp { get; set; }
        public string knuma_ag { get; set; }
        public string knumv { get; set; }
        public string konda { get; set; }
        public string ktokd { get; set; }
        public string kukla { get; set; }
        public string kunag { get; set; }
        public string kunnr_sh { get; set; }
        public string kunrg { get; set; }
        public string lgort { get; set; }
        public string matkl { get; set; }
        public string matkl_mts { get; set; }
        public string matnr { get; set; }
        public string meins { get; set; }
        public string MobileNo { get; set; }
        public string mvgr4 { get; set; }
        public string payer { get; set; }
        public string payer_BP { get; set; }
        public string Payer_DL { get; set; }
        public string pernr { get; set; }
        public string posnr { get; set; }
        public string pstyv { get; set; }
        public string salesdiv { get; set; }
        public string sfakn { get; set; }
        public string spart { get; set; }
        public string spart_text { get; set; }
        public string stceg { get; set; }
        public string Szone { get; set; }
        public string Territory { get; set; }
        public string vbeln { get; set; }
        public string vbtyp { get; set; }
        public string vgbel { get; set; }
        public string vgpos { get; set; }
        public string vgtyp { get; set; }
        public string vkbur { get; set; }
        public string vkbur_c { get; set; }
        public string vkgrp { get; set; }
        public string vkgrp_c { get; set; }
        public string vkorg { get; set; }
        public string voleh { get; set; }
        public string vrkme { get; set; }
        public string vtweg { get; set; }
        public string waerk { get; set; }
        public string werks { get; set; }
        public string wgbez { get; set; }
        public string WGBEZ60 { get; set; }
        public string Revenue { get; set; }
        public string Revenue_F { get; set; }
        public string mwas { get; set; }
        public string mwas_F { get; set; }
        public string zsdx { get; set; }
        public string zsdx_F { get; set; }
        public string zcom { get; set; }
        public string zcom_F { get; set; }
        public string zmrp { get; set; }
        public string zmrp_F { get; set; }
        public string zdis { get; set; }
        public string zdis_F { get; set; }
        public string zdit { get; set; }
        public string zdit_F { get; set; }
        public string yfg3 { get; set; }
        public string yfg3_F { get; set; }
        public string zcol { get; set; }
        public string zcol_F { get; set; }
        public string mwsbp { get; set; }
        public string mwsbp_F { get; set; }
        public string fkimg { get; set; }
        public string fkimg_F { get; set; }
        public string fklmg { get; set; }
        public string fklmg_F { get; set; }
        public string lmeng { get; set; }
        public string lmeng_F { get; set; }
        public string ntgew { get; set; }
        public string ntgew_F { get; set; }
        public string brgew { get; set; }
        public string brgew_F { get; set; }
        public string volum { get; set; }
        public string volum_F { get; set; }
        public string amt_kzwi1 { get; set; }
        public string amt_kzwi1_F { get; set; }
        public string amt_kzwi2 { get; set; }
        public string amt_kzwi2_F { get; set; }
        public string amt_kzwi3 { get; set; }
        public string amt_kzwi3_F { get; set; }
        public string amt_kzwi4 { get; set; }
        public string amt_kzwi4_F { get; set; }
        public string amt_kzwi5 { get; set; }
        public string amt_kzwi5_F { get; set; }
        public string amt_kzwi6 { get; set; }
        public string amt_kzwi6_F { get; set; }
        public string bonba { get; set; }
        public string bonba_F { get; set; }

        public SalesDataModel ToModel()
        {
            var model = new SalesDataModel();
            model.CompanyCode = this.bukrs;
            model.Division = this.spart;
            model.MatarialGroupOrBrand = this.matkl;
            model.MatrialCode = this.matnr;
            model.UnitOfMeasurement = this.gk;
            model.SalesOrgranization = this.vkorg;
            model.CustomerNoOrSoldToParty = this.kunrg;
            model.CancelledInvoiceNumber = this.sfakn;
            model.CustomerNoOrShipToParty = this.kunnr_sh;
            model.CustomerNo = this.Payer_DL;
            model.EmployeeNumber = this.payer_BP;
            model.InvoiceNoOrBillNo = this.vbeln;
            model.DealerOrSubDealer = this.pay_responsible;
            model.PriceGroupCashOrCredit = this.konda;
            model.CustomerGroup = this.kdgrp;
            model.SalesDistrict = this.bzirk;
            model.SalesOffice = this.vkbur_c;
            model.SalesGroup = this.vkgrp_c;
            model.CustomerClassification = this.kukla;
            model.BillingType = this.fkart;
            model.CustomerNumber = this.payer;
            model.Date = this.fkdat;
            model.LineNumber = this.posnr;
            model.MatarialDescription = this.arktx;
            model.UnitOfMeasure = this.meins;
            model.VolumeUnit = this.voleh;
            //model.SalesOffice = this.vkbur;
            //model.SalesGroup = this.vkgrp;
            model.Territory = this.Territory;
            model.Zone = this.Szone;
            model.CustomerName = this.cname;
            model.MobileNo = this.MobileNo;
            model.DivisionName = this.spart_text;
            model.NetAmount = this.Revenue;
            model.VATAmount = this.mwas;
            model.Currency = this.zsdx;
            model.Discount = this.zcom;
            model.MRP = this.zmrp;
            model.DefaultDiscountOrInitialDiscount = this.zdis;
            model.AdditionalDiscount = this.zdit;
            model.SchemeBenefit = this.yfg3;
            model.PlantOrBusinessArea = this.gsber;
            model.CollectionTax = this.zcol;
            //model.VATAmount = this.mwsbp;
            model.Quantity = this.fkimg;
            //model.Quantity = this.fklmg;
            model.Volume = this.volum;
            model.CustomerAccountGroup = this.ktokd;
            model.PackSize = this.groes;
            model.InvoiceCreateBy = this.ernam;
            model.SDDocumentCategory = this.vbtyp;
            model.NumberOfDocumentCondition = this.knumv;
            model.OutboundDeliveryDocumentNumber = this.vgbel;
            model.SalesOrderNumber = this.aubel;
            model.ItemCategory = this.pstyv;
            model.DistributionChannel = this.vtweg;
            return model;
        }
    }
}
