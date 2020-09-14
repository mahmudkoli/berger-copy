using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BergerMsfaApi.Core;

namespace BergerMsfaApi.Domain.Users
{
    public class Delegation : AuditableEntity<int>
    {
        public int DeligatedFromUserId { get; set; }

        public int DeligatedToUserId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("DeligatedFromUserId")]
        public UserInfo DeligatedFromUserInfo { get; set; }

        [ForeignKey("DeligatedToUserId")]
        public UserInfo DeligatedToUserInfo { get; set; }


    }
}
