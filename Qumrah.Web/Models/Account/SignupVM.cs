using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Qumrah.Web.Models.Account
{
    public class SignupVM
    {

        [Required(ErrorMessage = "يرجى إدخال البريد الألكتروني")]
        [DisplayName("البريد الألكتروني")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "صيغة البريد الالكتروني غير صحيحة")]
        public string Email { get; set; }
        [DisplayName("كلمة المرور")]
        [Required(ErrorMessage = "يرجى ادخال كلمة المرور")]
        public string Password { get; set; }
        [Required(ErrorMessage = "يرجى ادخال تأكيد كلمة المرور")]
        [DisplayName("تأكيد كلمة المرور")]
        [Compare(nameof(Password), ErrorMessage = "كلمة المرور و تأكيد كلمة المرور غير متطابقين")]
        public string ReTypePassword { get; set; }
    }
}
