using System.ComponentModel.DataAnnotations;
using Berger.Data.MsfaEntity.Examples;
using GenericServices;

namespace BergerMsfaApi.Models.Examples
{
    public class QuickCrudModel: ILinkToEntity<Example>
    {
        public int Id { get; set; }

        [StringLength(128, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

    }
}
