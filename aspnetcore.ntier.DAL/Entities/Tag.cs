using aspnetcore.ntier.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DAL.Entities
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }

        public List<MultimediaTag> MultimediaTags { get; set; }
    }
}
