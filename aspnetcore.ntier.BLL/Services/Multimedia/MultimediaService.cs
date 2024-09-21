using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.ProcessingImages;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
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

        public MultimediaService(ImageProcess imageProcess, IMapper mapper, IGenericRepository<MultimediaEntity> multimediaRepository, IWebHostEnvironment webHostEnvironment)
        {
            _imageProcess = imageProcess;
            _mapper = mapper;
            _multimediaRepository = multimediaRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task CreateMultimedia(MultimediaDto Model)
        {
            var entity = _mapper.Map<MultimediaEntity>(Model);
            //for thumbnail
            var compressedImage = _imageProcess.CompressImage(Model.Image);
            //originaliage
            var originalImage = ConvertStreamToBytes(Model.Image.OpenReadStream());

            var extractedColors = _imageProcess.ExtractColors(Model.Image);

            entity.ThumbnailPath = UploadedFile(compressedImage, ".webp");
            entity.FilePath = UploadedFile(originalImage, Path.GetExtension(Model.Image.FileName));
            entity.Tags = Model.Tags.Select(x => new Tag { Name = x }).ToList();
            entity.Title = Model.Title;
            entity.Colors = extractedColors.Select(x => new Color { Hexadecimal = x }).ToList();
            entity.Width = 100;
            entity.Height = 100;
            entity.UserId = Model.UserId;
            await _multimediaRepository.AddAsync(entity);
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
    }
}

