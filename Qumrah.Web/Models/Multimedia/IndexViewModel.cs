using aspnetcore.ntier.DTO.DTOs;

namespace Qumrah.Web.Models.Multimedia
{
    public class IndexViewModel
    {
        public FilterMultimediaViewModel Filter { get; set; }
        public List<MultimediaVM> Multimedia { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}
