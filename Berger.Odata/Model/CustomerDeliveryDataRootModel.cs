using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Odata.Model
{
    public class CustomerDeliveryDataRootModel
    {
        public string BILLSL { get; set; }
        public string TELF1 { get; set; }
        public string VEHICLE { get; set; }
        public string WERKS { get; set; }
        public string VBELN { get; set; }
        public string KUNAG { get; set; }
        public string ERSDA { get; set; }
        public string FKDAT { get; set; }
        public string ERZET { get; set; }
        public string VOLUM { get; set; }
        public string DRIVERNAME { get; set; }

        public CustomerDeliveryDataModel ToModel()
        {
            var model = new CustomerDeliveryDataModel();
            model.InvoiceDate = this.FKDAT;
            model.InvoiceCreateTime = this.ERZET;
            model.InvoiceNumber = this.VBELN;
            model.Volume = this.VOLUM;
            model.DeliveryDate = this.ERSDA;
            model.DeliveryTime = this.ERZET; //TODO: must be changed
            model.DriverName = this.DRIVERNAME;
            model.DriverMobileNo = this.TELF1;
            model.CustomerNo = this.KUNAG;
            return model;
        }
    }

}
