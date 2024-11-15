using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.CustomExceptions;
using aspnetcore.ntier.BLL.Utilities.ProcessingImages;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaEntity = aspnetcore.ntier.DAL.Entities.Multimedia;

namespace aspnetcore.ntier.BLL.Services.Multimedia
{
    public class MultimediaService : IMultimediaService
    {
        private readonly ImageProcess _imageProcess;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<MultimediaEntity> _multimediaRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly IGenericRepository<Tag> _tagRepository;
        private readonly IGenericRepository<MultimediaTag> _multimediaTagRepository;

        public MultimediaService(ImageProcess imageProcess,
            IMapper mapper,
            IGenericRepository<MultimediaEntity> multimediaRepository,
            IWebHostEnvironment webHostEnvironment,
            IGenericRepository<ApplicationUser> userRepository,
            IGenericRepository<Tag> tagRepository,
            IGenericRepository<MultimediaTag> multimediaTagRepository)
        {
            _imageProcess = imageProcess;
            _mapper = mapper;
            _multimediaRepository = multimediaRepository;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _multimediaTagRepository = multimediaTagRepository;
        }

        public async Task CreateMultimedia(MultimediaDto Model, string userEmail)
        {
            var user = await _userRepository.GetAsync(x => x.Email == userEmail);
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

            entity.ThumbnailPath = UploadedFile(compressedImage, imageExtension);
            entity.FilePath = UploadedFile(originalImage, imageExtension);
            entity.Title = Model.Title;
            entity.Colors = extractedColors.Select(x => new Color { Hexadecimal = x }).ToList();
            entity.Width = imageAspectRation.Heght;
            entity.Height = imageAspectRation.Width;
            entity.UserId = user.Id;
            var result = await _multimediaRepository.AddAsync(entity);

            if (Model.Tags is not null && Model.Tags.Count() > 0)
            {
                var tags = _mapper.Map<List<Tag>>(Model.Tags);
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
            (string.IsNullOrEmpty(filter.Location) || x.Location.Contains(filter.Location))

            );
            return _mapper.Map<List<MultimediaDto>>(results).OrderByDescending(x => x.TotalDownloads).ToList();
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
        private string UploadedFile(byte[] file, string extension)
        {
            string uniqueFileName = null;
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = string.Concat(Guid.NewGuid().ToString(), extension);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            File.WriteAllBytes(filePath, file);
            return uniqueFileName;
        }
        private async Task<List<Tag>> AddNewTags(List<Tag> Tags)
        {
            var results = new List<Tag>();

            foreach (var tag in Tags)
            {
                var entity = await _tagRepository.GetAsync(x => x.Name == tag.Name.ToLower().Trim());
                if (entity is null)
                {
                    entity = await _tagRepository.AddAsync(new Tag { Name = tag.Name.ToLower().Trim() });
                }
                results.Add(entity);
            }
            return results;
        }
    }
}

