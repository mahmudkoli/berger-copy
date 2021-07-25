using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Common
{
    public static class CustomerDeliveryColDef
    {
        public static string InvoiceDate = "FKDAT";
        public static string InvoiceCreateTime = "ERZET";
        public static string InvoiceNumber = "VBELN";
        public static string Volume = "VOLUM";
        public static string DeliveryDate = "ERSDA";
        public static string DeliveryTime = "ERZET"; //TODO: must be changed
        public static string DriverName = "DRIVERNAME";
        public static string DriverMobileNo = "TELF1";
        public static string CustomerNo = "KUNAG";
    }
}