using aspnetcore.ntier.DTO.DTOs;
using Qumrah.Web.Models.Multimedia;

namespace Qumrah.Web.Models.UserProfile
{
    public class IndexVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FBLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? Location { get; set; }
        public string? WebsiteUrl { get; set; }
        public List<MultimediaVM> Multimedias { get; set; }
    }
}
