﻿using Amazon;
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
using System.Security.AccessControl;
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
                    new FileExtensionContentTypeProvider().TryGetContentType(aWSS3Dto.Key, out contentType);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = aWSS3Dto.Key,
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

        public async Task<byte[]> GetObjectAsync(string bucketName, string Key)
        {
            {
                // Create a GetObject request
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = Key,
                };

                // Issue request and remember to dispose of the response
                using GetObjectResponse response = await _awsS3Client.GetObjectAsync(request);
                return ConvertStreamToArrayOfByte(response.ResponseStream);

            }
        }

        private byte[] ConvertStreamToArrayOfByte(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
