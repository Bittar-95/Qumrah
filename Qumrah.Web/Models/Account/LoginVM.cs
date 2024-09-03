using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Qumrah.Web.Models.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "يرجى إدخال البريد الألكتروني")]
        [DisplayName("البريد الألكتروني")]
        //[EmailAddress(ErrorMessage =)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "صيغة البريد الالكتروني غير صحيحة")]
        public string Email { get; set; }
        [DisplayName("كلمة المرور")]
        [Required(ErrorMessage = "يرجى ادخال كلمة المرور")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool isPersistent { get; set; }
    }
}
