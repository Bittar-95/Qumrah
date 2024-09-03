using aspnetcore.ntier.DAL.Entities.Base;
using aspnetcore.ntier.DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DAL.Entities
{
    public class Multimedia : BaseEntity<int>
    {
        public Multimedia()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public string? Title { get; set; }
        public string? Location { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int TotalDownloads { get; set; }
        public int TotalViews { get; set; }
        public DateTime CreatedAt { get; set; }
        public MultimediaType Type { get; set; } = MultimediaType.image;

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Color> Colors { get; set; }

    }
}
