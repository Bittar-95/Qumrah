using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.Extensions;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using AutoMapper;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;
        private readonly IAWSS3Service _aWSS3Service;
        private readonly IConfiguration _config;

        public UserService(IMapper mapper, IGenericRepository<ApplicationUser> applicationUserRepository, IAWSS3Service aWSS3Service, IConfiguration config)
        {
            _mapper = mapper;
            _applicationUserRepository = applicationUserRepository;
            _aWSS3Service = aWSS3Service;
            _config = config;
        }

        public async Task<ApplicationUserDto> GetAsync(string Email)
        {
            var result = await _applicationUserRepository.GetAsync(x => x.Email == Email);
            return _mapper.Map<ApplicationUserDto>(result);
        }
        public ApplicationUserDto GetWithImages(int Id)
        {
            var result = _applicationUserRepository.Get().Include(mm => mm.Multimedias).Where(u => u.Id == Id).AsNoTracking().FirstOrDefault();
            return _mapper.Map<ApplicationUserDto>(result);
        }
        public async Task EditAsync(EditUserDto model, IFormFile? imageProfile)
        {
            var user = await _applicationUserRepository.GetAsync(x => x.Id == model.Id);
            if (user is null)
            {
                throw new Exception("المستخدم غير موجود");
            }

            if (imageProfile is not null && imageProfile.Length > 0)
            {
                var allowExtensions = _config.GetSection("AllowImageProfileExtensions").Get<List<string>>();
                var imageExtension = System.IO.Path.GetExtension(imageProfile.FileName).ToLower();

                var key = $"{user.Id}/{user.Id}" + imageExtension;
                if (allowExtensions.Contains(imageExtension))
                {
                    await _aWSS3Service.UploadFileAsync(new DTO.DTOs.AWS.S3.AWSS3Dto
                    {
                        BucketName = _config["Buckets:Profile"],
                        Key = key,
                        File = imageProfile.ToBytes()
                    });
                    model.ImagePath = key;
                }
                else
                {
                    throw new Exception("صيغة الصورة الشخصية غير صحيحة");
                }
            }

            _mapper.Map<EditUserDto, ApplicationUser>(model, user);
            await _applicationUserRepository.UpdateAsync(user);
        }
    }
}
