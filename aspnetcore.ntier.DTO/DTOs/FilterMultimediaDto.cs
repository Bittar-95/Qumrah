﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DTO.DTOs
{
    public class FilterMultimediaDto
    {
        public string Title { get; set; }
        public int? TagId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
