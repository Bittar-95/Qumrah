using aspnetcore.ntier.DTO.DTOs.AWS.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Services.IServices
{
    public interface IAWSS3Service
    {
        Task<byte[]> GetObjectAsync(string bucketName, string Key);
        Task<bool> UploadFileAsync(AWSS3Dto aWSS3Dto);
    }
}
