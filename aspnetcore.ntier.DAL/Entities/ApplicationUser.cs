﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore.ntier.DAL.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FBLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Location { get; set; }
        public List<Multimedia> Multimedias { get; set; }
    }
}
