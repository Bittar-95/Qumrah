using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DTO.DTOs.AWS.S3
{
    public class AWSS3Dto
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
