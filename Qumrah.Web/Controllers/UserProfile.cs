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
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UserProfile(IUserService userService, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Edit()
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
                Email = user.Email,
                ImagePath = user.ImagePath,
                Location = user.Location,
                WebsiteUrl = user.WebsiteUrl,
            };
            return View(editProfileVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetAsync(User.Identity.Name);
                var imagePath = user.ImagePath;
                if (model.ImageProfile is not null && model.ImageProfile.Length > 0)
                {
                    var allowExtensions = _configuration.GetSection("AllowImageProfileExtensions").Get<List<string>>();
                    var imageExtension = System.IO.Path.GetExtension(model.ImageProfile.FileName).ToLower();
                    if (allowExtensions.Contains(imageExtension))
                    {
                        string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images", "Profiles");
                        var fileName = user.Id.ToString() + imageExtension;
                        var filePath = Path.Combine(uploads, fileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageProfile.CopyToAsync(fileStream);
                        }
                        imagePath = Path.Combine(@"\images", "Profiles", fileName);
                    }
                    else
                    {
                        ViewBag.Error = "صيغة الصورة الشخصية غير صحيحة";
                        return View(model);
                    }


                }


                await _userService.EditAsync(new EditUserDto
                {
                    Id = user.Id,
                    Description = model.Description,
                    TwitterLink = model.TwitterLink,
                    FBLink = model.FBLink,
                    FirstName = model.FirstName,
                    InstagramLink = model.InstagramLink,
                    LastName = model.LastName,
                    Email = model.Email,
                    ImagePath = imagePath,
                    Location = model.Location,
                    WebsiteUrl = model.WebsiteUrl,
                });
                return RedirectToAction(nameof(Edit));
            }
            return View(model);
        }

    }
}
