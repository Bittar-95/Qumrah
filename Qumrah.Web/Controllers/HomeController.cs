using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Services.Multimedia;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qumrah.Web.Models;
using Qumrah.Web.Models.Multimedia;
using Qumrah.Web.Models.UserProfile;
using System.Diagnostics;

namespace Qumrah.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMultimediaService _multimediaService;
        private readonly ITagService _tagService;

        public HomeController(ILogger<HomeController> logger, IMultimediaService multimediaService, ITagService tagService)
        {
            _logger = logger;
            _multimediaService = multimediaService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(FilterMultimediaViewModel filter)
        {

            var filterDto = new FilterMultimediaDto
            {
                Title = filter.Title,
                TagId = filter.TagId
            };
            var multimedia = await _multimediaService.GetAsync(filterDto);

            var multimediaVM = new List<MultimediaVM>();

            var Tags = await _tagService.GetAll(true);


            foreach (var item in multimedia)
            {
                var MM = new MultimediaVM()
                {
                    Id = item.Id,
                    FilePath = item.FilePath,
                    ThumbnailPath = item.ThumbnailPath,
                    Tags = item.Tags?.Select(tn => tn.Name).ToList(),
                    Height = item.Height,
                    Title = item.Title,
                    Location = item.Location,
                    TotalDownloads = item.TotalDownloads,
                    TotalViews = item.TotalViews,
                    Width = item.Width,
                    User = item.User
                };
                multimediaVM.Add(MM);
            }

            var model = new IndexViewModel
            {
                Multimedia = multimediaVM
            };

            model.Tags = Tags;
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
