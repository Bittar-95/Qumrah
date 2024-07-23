using aspnetcore.ntier.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Services.IServices
{
    public interface IAccountService
    {
        Task<bool> LoginUser(LoginUserDto model);
        Task LogoutUser();
        Task<IdentityResult> RegisterNewUser(RegisterUserDto model);
    }
}
