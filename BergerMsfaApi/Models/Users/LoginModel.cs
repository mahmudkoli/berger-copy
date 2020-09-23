using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.Users
{
    public class LoginModel
    {
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
