using aspnetcore.ntier.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Qumrah.Web.Controllers
{
    public class MultimediaController : Controller
    {
        private readonly IMultimediaService _multimediaService;
        private readonly IAWSS3Service _AWSS3Service;
        private readonly IConfiguration _configuration;
        public MultimediaController(IMultimediaService multimediaService, IAWSS3Service aWSS3Service, IConfiguration configuration)
        {
            _multimediaService = multimediaService;
            _AWSS3Service = aWSS3Service;
            _configuration = configuration;
        }

        public async Task<IActionResult> DownloadImage(int Id)
        {
            var buckets = _configuration.GetSection("Buckets");
            var originBucket = buckets.GetValue<string>("Origin");
            var multimedia = await _multimediaService.GetAsync(Id);
            var key = string.Join("/", multimedia.FilePath.Replace("//","").Split("/").Skip(1));
            var stream = await _AWSS3Service.GetObjectAsync(originBucket, key);
            return File(stream, "application/octet-stream", Guid.NewGuid().ToString() + Path.GetExtension(key)); // returns a FileStreamResult
        }
    }
}
