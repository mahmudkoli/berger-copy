using BergerMsfaApi.Models.PainterRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    public class DealerOpeningModel
    {
        public DealerOpeningModel()
        {
            DealerOpeningAttachments = new List<DealerOpeningAttachmentModel>();
        }
        public int Id { get; set; }
        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string TerritoryNo { get; set; }
        public string ZoneNo { get; set; }
        public string EmployeeId { get; set; }
        public List<DealerOpeningAttachmentModel> DealerOpeningAttachments { get; set; }

    }
}
