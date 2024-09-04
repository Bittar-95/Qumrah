using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Services.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;

        public UserService(IMapper mapper, IGenericRepository<ApplicationUser> applicationUserRepository)
        {
            _mapper = mapper;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<ApplicationUserDto> GetAsync(string Email)
        {
            var result = await _applicationUserRepository.GetAsync(x => x.Email == Email);
            return _mapper.Map<ApplicationUserDto>(result);
        }
        public async Task EditAsync(EditUserDto model)
        {
            var user = await _applicationUserRepository.GetAsync(x => x.Id == model.Id);
            if (user is null)
            {
                throw new Exception("the user not exist");
            }
            _mapper.Map<EditUserDto,ApplicationUser>(model,user);
            await _applicationUserRepository.UpdateAsync(user);
        }
    }
}
