using System;
using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.Users
{
    public class DelegationModel
    {
        public int Id { get; set; }
        public int DeligatedFromUserId { get; set; }

        public int DeligatedToUserId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string DeligatedFromUserName { get; set; }

        public string DeligatedToUserName { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
