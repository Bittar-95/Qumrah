using aspnetcore.ntier.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore.ntier.DAL.DataContext;

public class AspNetCoreNTierDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AspNetCoreNTierDbContext(DbContextOptions<AspNetCoreNTierDbContext> options) : base(options) { }
    public DbSet<Multimedia> Multimedias { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Color> Colors { get; set; }
}