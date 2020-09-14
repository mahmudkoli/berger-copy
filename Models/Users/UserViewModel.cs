using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Enumerations;

namespace BergerMsfaApi.Models.Users
{
    public class UserViewModel
    {

        public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Name must be greater than 2 letters")]
        public string Name { get; set; }
        public string Code { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        //[Required]
        //[EmailAddress(ErrorMessage ="Please provide a valid email address")]
        public string Email { get; set; }
        [Required]
        //[MinLength(6,ErrorMessage ="Password Must be greated than 6 letters")]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }
        //[Required]
        public string FamilyContactNo { get; set; }

        //public bool IsActive { get; set; }
        public Status Status { get; set; }
        public int? FMUserId{ get; set; }



    }
}
