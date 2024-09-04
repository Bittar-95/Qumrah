namespace Qumrah.Web.Models.UserProfile
{
    public class EditProfileVM
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FBLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageProfile { get; set; }
    }
}
