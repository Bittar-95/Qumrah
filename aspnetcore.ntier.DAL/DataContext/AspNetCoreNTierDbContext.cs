using aspnetcore.ntier.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore.ntier.DAL.DataContext;

public class AspNetCoreNTierDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AspNetCoreNTierDbContext(DbContextOptions<AspNetCoreNTierDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<MultimediaTag>().HasIndex(x => new { x.TagId, x.MultimediaId }).IsUnique();
        base.OnModelCreating(builder);
    }
    public DbSet<Multimedia> Multimedias { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Color> Colors { get; set; }
}