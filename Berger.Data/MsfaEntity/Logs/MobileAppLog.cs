using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.Logs
{
    public class MobileAppLog
    {
        public int Id { get; set; }
        public string EmployeeID { get; set; }
        public string ErrorMessage { get; set; }
        public string BearerToken { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Url { get; set; }
        public string LastActivity { get; set; }
    }
}
