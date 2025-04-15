namespace Qumrah.Web.Models.Multimedia
{
    public class FilterMultimediaViewModel
    {
        public string Title { get; set; }
        public int? TagId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
