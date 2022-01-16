using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.Somporko.Users
{
    public class SomporkoLoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
