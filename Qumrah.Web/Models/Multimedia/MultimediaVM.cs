using aspnetcore.ntier.DTO.DTOs;

namespace Qumrah.Web.Models.Multimedia
{
    public class MultimediaVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int TotalDownloads { get; set; }
        public int TotalViews { get; set; }
        public int UserId { get; set; }
        public IFormFile Image { get; set; }
        public List<string> Tags { get; set; }
        public ApplicationUserDto User { get; set; }

    }
}
