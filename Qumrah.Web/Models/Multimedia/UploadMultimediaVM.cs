using aspnetcore.ntier.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Qumrah.Web.Models.Multimedia
{
    public class UploadMultimediaVM
    {
        [Required]
        public IFormFile File { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Tags { get; set; }


    }
}
