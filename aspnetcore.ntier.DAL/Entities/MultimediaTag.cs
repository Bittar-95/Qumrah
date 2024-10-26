using aspnetcore.ntier.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DAL.Entities
{
    public class MultimediaTag : BaseEntity<int>
    {
        public int MultimediaId { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public Multimedia Multimedia { get; set; }
    }
}
