using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.Users
{
    public class UserInfoViewModel
    {

        public int Id { get; set; }
        [Required]

        public string EmployeeId { get; set; }

    
        [Required]
        [StringLength(128,MinimumLength =3)]
        public string Code { get; set; }

        [Required]
      
        public string Name { get; set; }

       
        public string PhoneNumber { get; set; }


        
        public string Designation { get; set; }

        public int SalesPointId { get; set; }
    }
}
