using aspnetcore.ntier.DTO.DTOs;

namespace Qumrah.Web.Models.Multimedia
{
    public class IndexViewModel
    {
        public List<MultimediaVM> Multimedia { get; set; }
        public List<string> Locations { get { return Multimedia?.Select(l => l.Location).Where(t => !string.IsNullOrEmpty(t)).ToList(); } }
    }
}
