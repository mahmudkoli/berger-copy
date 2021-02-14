using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class DriverDataRootModel
    {
        public string BILLSL { get; set; }
        public string WERKS { get; set; } // Business Area
        public string VBELN { get; set; }
        public string fkdat { get; set; }
        public string DRIVERNAME { get; set; }
        public string VEHICLE { get; set; }

        public DriverDataModel ToModel()
        {
            var model = new DriverDataModel();
            model.BILLSL = this.BILLSL;
            model.WERKS = this.WERKS;
            model.InvoiceNoOrBillNo = this.VBELN;
            model.Date = this.fkdat;
            model.DRIVERNAME = this.DRIVERNAME;
            model.VEHICLE = this.VEHICLE;
            return model;
        }
    }
}
