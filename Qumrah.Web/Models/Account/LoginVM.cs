namespace Qumrah.Web.Models.Account
{
    public class LoginVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool isPersistent { get; set; }
    }
}
