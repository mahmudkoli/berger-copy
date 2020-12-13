using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Models.Painter
{
    public class PainterAttachmentModel
    {
        public string Name { get; set; }
        [RegularExpression(
           "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", 
            ErrorMessage = "path must be properly base64 formatted.")]
        public string Path { get; set; }
       
    }
}
