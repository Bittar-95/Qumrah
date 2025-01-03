using aspnetcore.ntier.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Qumrah.Web.Controllers
{
    public class MultimediaController : Controller
    {
        private readonly IMultimediaService _multimediaService;

        public MultimediaController(IMultimediaService multimediaService)
        {
            _multimediaService = multimediaService;
        }

        public IActionResult DownloadImage(int Id)
        {

            return View();
        }
    }
}
