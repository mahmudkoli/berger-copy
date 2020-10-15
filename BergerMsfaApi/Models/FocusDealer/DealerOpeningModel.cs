using BergerMsfaApi.Models.PainterRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    public class DealerOpeningModel
    {
        public int Id { get; set; }
        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string TerritoryNo { get; set; }
        public string ZoneNo { get; set; }
        public string EmployeId { get; set; }
        public List<AttachmentModel> Attachments { get; set; }

    }
}
