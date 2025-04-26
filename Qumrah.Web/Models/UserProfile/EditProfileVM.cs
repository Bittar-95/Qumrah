using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Qumrah.Web.Models.UserProfile
{
    public class EditProfileVM
    {
        [Display(Name = "الاسم الأول")]
        public string? FirstName { get; set; }
        [Display(Name = "الاسم الأخير")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "يرجى إدخال البريد الألكتروني")]
        [DisplayName("البريد الألكتروني")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "صيغة البريد الالكتروني غير صحيحة")]
        public string Email { get; set; }
        [Display(Name = "فيس بوك")]
        [RegularExpression(@"^https:\/\/web\.facebook\.com\/[a-zA-Z0-9\.]+$", ErrorMessage = "يرجى إدخال رابط فيس بوك")]
        public string? FBLink { get; set; }
        [Display(Name = "اكس")]
        [RegularExpression(@"^https:\/\/x\.com\/[A-Za-z0-9]+$", ErrorMessage = "يرجى إدخال رابط اكس")]
        public string? TwitterLink { get; set; }
        [Display(Name = "إنستغرام")]
        [RegularExpression(@"^https:\/\/(www\.)?instagram\.com\/[A-Za-z0-9_]+\/?$", ErrorMessage = "يرجى إدخال رابط انستغرام")]
        public string? InstagramLink { get; set; }
        [Display(Name = "نبذة شخصية")]
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        [Display(Name = "الموقع")]
        public string? Location { get; set; }
        [Display(Name = "الموقع الإلكتروني")]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([/\w \.-]*)*\/?$", ErrorMessage = "يرجى إدخال رابط الموقع الإلكتروني")]
        public string? WebsiteUrl { get; set; }
        public IFormFile? ImageProfile { get; set; }
    }
}
