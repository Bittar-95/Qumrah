using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.DTO.DTOs.AWS.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.BLL.Services.AWS.S3
{
    public class AWSS3Service : IAWSS3Service
    {
        private IAmazonS3 _awsS3Client;
        private readonly IConfiguration _configuration;
        public AWSS3Service(IConfiguration configuration)
        {
            _configuration = configuration;
            _awsS3Client = new AmazonS3Client(_configuration.GetSection("AWSCredentials").GetValue<string>("Accesskey"), _configuration.GetSection("AWSCredentials").GetValue<string>("Secretkey"), RegionEndpoint.EUWest1);
        }

        public async Task<bool> UploadFileAsync(AWSS3Dto aWSS3Dto)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    newMemoryStream.Write(aWSS3Dto.File, 0, aWSS3Dto.File.Length);
                    string contentType = string.Empty;
                    new FileExtensionContentTypeProvider().TryGetContentType(aWSS3Dto.FileName, out contentType);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = aWSS3Dto.FileName,
                        BucketName = aWSS3Dto.BucketName,
                        ContentType = contentType
                    };

                    var fileTransferUtility = new TransferUtility(_awsS3Client);

                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
