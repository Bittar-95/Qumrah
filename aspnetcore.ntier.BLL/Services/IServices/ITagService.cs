using aspnetcore.ntier.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Services.IServices
{
    public interface ITagService
    {
        Task<List<TagDto>> GetAll(bool Top10 = false);
    }
}
