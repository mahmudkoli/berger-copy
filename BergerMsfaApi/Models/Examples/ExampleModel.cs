using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.Examples
{
    public class ExampleModel
    {
        public int Id { get; set; }

        [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

    }
}
