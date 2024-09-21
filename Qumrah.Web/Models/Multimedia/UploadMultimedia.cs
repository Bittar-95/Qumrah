using aspnetcore.ntier.DAL.Entities;

namespace Qumrah.Web.Models.Multimedia
{
    public class UploadMultimedia
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public List<string> Tags { get; set; }


    }
}
