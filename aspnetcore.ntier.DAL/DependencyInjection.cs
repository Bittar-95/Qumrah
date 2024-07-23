using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace aspnetcore.ntier.DAL;

public static class DependencyInjection
{
    public static void RegisterDALDependencies(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddDbContext<AspNetCoreNTierDbContext>(options =>
        {
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")));
        });
        services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<AspNetCoreNTierDbContext>()
                .AddDefaultTokenProviders();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}