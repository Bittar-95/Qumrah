using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DTO.DTOs
{
    public class MultimediaDto
    {
        public MultimediaDto()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int TotalDownloads { get; set; }
        public int TotalViews { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public IFormFile Image { get; set; }
        public List<TagDto> Tags { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}
