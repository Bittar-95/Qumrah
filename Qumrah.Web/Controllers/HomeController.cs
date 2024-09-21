using aspnetcore.ntier.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qumrah.Web.Models;
using Qumrah.Web.Models.Multimedia;
using System.Diagnostics;

namespace Qumrah.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMultimediaService _multimediaService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IMultimediaService multimediaService, IUserService userService)
        {
            _logger = logger;
            _multimediaService = multimediaService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        public IActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadImage(UploadMultimedia model)
        {
            var user = await _userService.GetAsync(User.Identity.Name);
            await _multimediaService.CreateMultimedia(new aspnetcore.ntier.DTO.DTOs.MultimediaDto
            {
                Title = model.Name,
                Image = model.File,
                UserId = user.Id,
                Tags = model.Tags,
            });
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
