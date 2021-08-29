using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Common
{
    public static class DataColumnDef
    {
        // Sales Data
        public static string CompanyCode = "bukrs"; //................................
        public static string Division = "spart";
        public static string MatarialGroupOrBrand = "matkl";
        public static string MatarialGroupOrBrandName = "wgbez";
        public static string MatrialCode = "matnr";
        public static string SalesOrgranization = "vkorg"; //............
        public static string CustomerNoOrSoldToParty = "kunrg";
        public static string CustomerNoOrShipToParty = "kunnr_sh"; //..............................
        public static string CustomerNo = "Payer_DL";
        public static string InvoiceNoOrBillNo = "vbeln";
        public static string SalesOffice = "vkbur_c";
        public static string SalesGroup = "vkgrp_c";
        public static string CustomerClassification = "kukla";
        public static string Date = "fkdat";
        public static string LineNumber = "posnr";
        public static string MatarialDescription = "arktx";
        public static string UnitOfMeasure = "meins";
        public static string VolumeUnit = "voleh"; //....................................
        //public static string SalesOffice = "vkbur";
        //public static string SalesGroup = "vkgrp";
        public static string Territory = "Territory";
        public static string Zone = "Szone";
        public static string CustomerName = "cname";
        public static string DivisionName = "spart_text";
        public static string NetAmount = "Revenue";
        public static string PlantOrBusinessArea = "gsber";
        //public static string VATAmount = "mwsbp";
        public static string Quantity = "fkimg";
        //public static string Quantity = "fklmg";
        public static string Volume = "volum";
        public static string CustomerAccountGroup = "ktokd";
        public static string DistributionChannel = "vtweg";
        //public static string Time = "erzet";
        public static string Time = "erzet_T";
        public static string CreditControlArea = "kkber";

        // MTS Data
        public static string MTS_Date = "period";
        public static string MTS_EmployeeNo = "pernr";
        public static string MTS_Territory = "RPMKR";
        public static string MTS_Zone = "regiogroup";
        public static string MTS_CustomerGroup = "kdgrp";
        public static string MTS_UnitOfMeasure = "meins";
        public static string MTS_CustomerName = "cname";
        public static string MTS_AverageSalesPrice = "asp";
        public static string MTS_TargetVolume = "tarvol";
        public static string MTS_TargetValue = "tarval";
        public static string MTS_CustomerAccountGroup = "ktokd";
        public static string MTS_PlantOrBusinessArea = "gsber";
        public static string MTS_SalesOffice = "vkbur";
        public static string MTS_SalesGroup = "vkgrp";
        public static string MTS_CustomerNo = "kunnr";
        public static string MTS_DistributionChannel = "vtweg";
        public static string MTS_Division = "spart";
        public static string MTS_MatarialGroupOrBrand = "matkl";

        // Driver Data
        public static string Driver_BillSl = "BILLSL";
        public static string Driver_PlantOrBusinessArea = "WERKS";
        public static string Driver_InvoiceNoOrBillNo = "VBELN";
        public static string Driver_Date = "fkdat";
        public static string Driver_DriverName = "DRIVERNAME";
        public static string Driver_DriverMobileNo = "TELF1";
        public static string Driver_Vehicle = "VEHICLE";

        // Brand Family Data
        public static string BrandFamily_MatarialGroupOrBrandFamily = "MATKL_MTS";
        public static string BrandFamily_MatarialGroupOrBrandFamilyName = "WGBEZMTS";
        public static string BrandFamily_MatarialGroupOrBrand = "MATKL";
        public static string BrandFamily_MatarialGroupOrBrandName = "WGBEZ";

        //Collection Data

        public static string Collection_Amount = "Amount";
        public static string Collection_Customer = "Customer";
    }
}
