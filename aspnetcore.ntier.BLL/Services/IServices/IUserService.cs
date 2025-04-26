using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Services.IServices
{
    public interface IUserService
    {
        Task EditAsync(EditUserDto model, IFormFile? imageProfile);
        Task<ApplicationUserDto> GetAsync(string Email);
        ApplicationUserDto GetWithImages(int Id);
    }
}
