using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qumrah.Web.Models.UserProfile;

namespace Qumrah.Web.Controllers
{
    [Authorize]
    public class UserProfile : Controller
    {
        private readonly IUserService _userService;

        public UserProfile(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> EditProfile()
        {
            var user = await _userService.GetAsync(User.Identity.Name);
            var editProfileVM = new EditProfileVM
            {
                Description = user.Description,
                TwitterLink = user.TwitterLink,
                FBLink = user.FBLink,
                FirstName = user.FirstName,
                InstagramLink = user.InstagramLink,
                LastName = user.LastName,
            };
            return View(editProfileVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileVM model)
        {
            var user = await _userService.GetAsync(User.Identity.Name);
            var imagePath = "test";
            await _userService.EditAsync(new EditUserDto
            {
                Id = user.Id,
                Description = model.Description,
                FBLink = model.FBLink,
                ImagePath = imagePath,
                InstagramLink = model.InstagramLink,
                TwitterLink = model.TwitterLink,
                LastName = model.LastName,
                FirstName = model.FirstName,
            });
            return View();
        }

    }
}
