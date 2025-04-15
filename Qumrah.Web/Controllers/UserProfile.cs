using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qumrah.Web.Models.UserProfile;
using Microsoft.AspNet.Identity;
using aspnetcore.ntier.DAL.Entities;
using Qumrah.Web.Models.Multimedia;
using aspnetcore.ntier.BLL.Services.Multimedia;
using X.PagedList.Extensions;
using aspnetcore.ntier.BLL.Utilities.Extensions;
namespace Qumrah.Web.Controllers
{
    [Authorize]
    public class UserProfile : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IMultimediaService _multimediaService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UserProfile(IUserService userService, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IMultimediaService multimediaService)
        {
            _userService = userService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _multimediaService = multimediaService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> index(int? id, int pageNumber = 1)
        {
            int pageSize = 10;
            int userId;
            if (id == null && User.Identity.IsAuthenticated)
            {
                userId = Convert.ToInt32(User.Identity.GetUserId());
            }
            else
            {
                userId = id is null ? 0 : id.Value;
            }

            var user = _userService.GetWithImages(userId);
            if (user is null)
            {
                return NotFound();
            }
            var editProfileVM = new IndexVM
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
                Multimedias = new List<MultimediaVM>()
            };

            foreach (var multimedia in user.Multimedias.ToPagedList(pageNumber, pageSize))
            {
                var MM = new MultimediaVM()
                {
                    Id = multimedia.Id,
                    FilePath = multimedia.FilePath,
                    ThumbnailPath = multimedia.ThumbnailPath,
                    Tags = multimedia.Tags?.Select(tn => tn.Name).ToList(),
                    Height = multimedia.Height,
                    Title = multimedia.Title,
                    Location = multimedia.Location,
                    TotalDownloads = multimedia.TotalDownloads,
                    TotalViews = multimedia.TotalViews,
                    Width = multimedia.Width,
                    User = multimedia.User
                };
                editProfileVM.Multimedias.Add(MM);
            }
            return View(editProfileVM);
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
                TempData["Success"] = true;
                return RedirectToAction(nameof(Edit));
            }
            return View(model);
        }

        public IActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(UploadMultimediaVM model)
        {
            if (ModelState.IsValid)
            {
                await _multimediaService.CreateMultimedia(new aspnetcore.ntier.DTO.DTOs.MultimediaDto
                {
                    Title = model.Title,
                    Image = model.File,
                    Tags = model.Tags?.Split(",").Distinct().Where(s => !string.IsNullOrEmpty(s.Trim())).Select(x => new TagDto { Name = x.Trim() }).ToList(),
                    Location = model.Location
                }, User.Identity.Name);
            }
            return View(model);
        }

    }
}
