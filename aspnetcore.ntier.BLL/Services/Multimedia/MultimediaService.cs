using aspnetcore.ntier.BLL.Services.AWS.S3;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.BLL.Utilities.ProcessingImages;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using aspnetcore.ntier.DTO.DTOs.AWS.S3;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaEntity = aspnetcore.ntier.DAL.Entities.Multimedia;
using TagEntity = aspnetcore.ntier.DAL.Entities.Tag;

namespace aspnetcore.ntier.BLL.Services.Multimedia
{
    public class MultimediaService : IMultimediaService
    {
        private readonly ImageProcess _imageProcess;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<MultimediaEntity> _multimediaRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly IGenericRepository<TagEntity> _tagRepository;
        private readonly IGenericRepository<MultimediaTag> _multimediaTagRepository;
        private readonly IAWSS3Service _AWSS3Service;
        private readonly IConfiguration _configuration;


        public MultimediaService(ImageProcess imageProcess,
            IMapper mapper,
            IGenericRepository<MultimediaEntity> multimediaRepository,
            IWebHostEnvironment webHostEnvironment,
            IGenericRepository<ApplicationUser> userRepository,
            IGenericRepository<TagEntity> tagRepository,
            IGenericRepository<MultimediaTag> multimediaTagRepository,
            IAWSS3Service aWSS3Service,
            IConfiguration configuration)
        {
            _imageProcess = imageProcess;
            _mapper = mapper;
            _multimediaRepository = multimediaRepository;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _multimediaTagRepository = multimediaTagRepository;
            _AWSS3Service = aWSS3Service;
            _configuration = configuration;
        }

        public async Task CreateMultimedia(MultimediaDto Model, string userEmail)
        {
            var user = await _userRepository.GetAsync(x => x.Email == userEmail);
            var buckets = _configuration.GetSection("Buckets");
            if (user is null)
            {
                throw new UserNotFoundException();
            }
            var imageExtension = Path.GetExtension(Model.Image.FileName);
            var entity = _mapper.Map<MultimediaEntity>(Model);

            var imageAspectRation = _imageProcess.GetImageAspectRation(Model.Image);

            //for thumbnail
            var compressedImage = _imageProcess.CompressImage(Model.Image);
            //originaliage
            var originalImage = ConvertStreamToBytes(Model.Image.OpenReadStream());

            var extractedColors = _imageProcess.ExtractColors(Model.Image);

            var thumbnailUrl = _configuration.GetSection("AWSUrls").GetValue<string>("ProfileThumbnail");
            var originUrl = _configuration.GetSection("AWSUrls").GetValue<string>("ProfileOrigin");


            entity.ThumbnailPath = string.Concat(thumbnailUrl, await UploadedFile(compressedImage, imageExtension, buckets.GetValue<string>("Thumbnail")));
            entity.FilePath = string.Concat(originUrl, await UploadedFile(originalImage, imageExtension, buckets.GetValue<string>("Origin")));

            entity.Title = Model.Title;
            entity.Colors = extractedColors.Select(x => new Color { Hexadecimal = x }).ToList();
            entity.Width = imageAspectRation.Heght;
            entity.Height = imageAspectRation.Width;
            entity.UserId = user.Id;
            var result = await _multimediaRepository.AddAsync(entity);

            if (Model.Tags is not null && Model.Tags.Count() > 0)
            {
                var tags = _mapper.Map<List<TagEntity>>(Model.Tags);
                var multimediaTags = (await AddNewTags(tags)).Select(mmt => new MultimediaTag
                {
                    MultimediaId = result.Id,
                    TagId = mmt.Id,
                }).ToList();

                await _multimediaTagRepository.AddRangeAsync(multimediaTags);
            }


        }

        public async Task<List<MultimediaDto>> GetAsync(FilterMultimediaDto filter)
        {
            var results = await _multimediaRepository.GetListAsync(x =>
            (string.IsNullOrEmpty(filter.Title) || x.Title.Contains(filter.Title)) &&
            (filter.TagId == null || x.MultimediaTags.Where(x => x.TagId == filter.TagId).Any())
            , default, x => x.MultimediaTags, x => x.User);
            return _mapper.Map<List<MultimediaDto>>(results).OrderByDescending(x => x.TotalDownloads).ToList();
        }
        public async Task<MultimediaDto> GetAsync(int Id)
        {
            var results = await _multimediaRepository.GetAsync(m => m.Id == Id);
            return _mapper.Map<MultimediaDto>(results);
        }


        private byte[] ConvertStreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private async Task<string> UploadedFile(byte[] file, string extension, string bucketName)
        {
            string uniqueFileName = null;
            uniqueFileName = string.Concat(Guid.NewGuid().ToString(), extension);

            await _AWSS3Service.UploadFileAsync(new AWSS3Dto
            {
                File = file,
                BucketName = bucketName,
                FileName = uniqueFileName
            });
            return uniqueFileName;
        }
        private async Task<List<TagEntity>> AddNewTags(List<TagEntity> Tags)
        {
            var results = new List<TagEntity>();

            foreach (var tag in Tags)
            {
                var entity = await _tagRepository.GetAsync(x => x.Name == tag.Name.ToLower().Trim());
                if (entity is null)
                {
                    entity = await _tagRepository.AddAsync(new TagEntity { Name = tag.Name.ToLower().Trim() });
                }
                results.Add(entity);
            }
            return results;
        }
    }
}

