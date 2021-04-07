using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.Users
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string FCMToken { get; set; }
    }
}
