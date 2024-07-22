using aspnetcore.ntier.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DAL.Entities
{
    public class Color : BaseEntity<int>
    {
        public string Hexadecimal { get; set; }
        public int MultimediaId { get; set; }
        public Multimedia Multimedia { get; set; }
    }
}
