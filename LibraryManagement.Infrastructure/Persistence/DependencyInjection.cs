using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.Services;
using LibraryManagement.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        // Unit of Work - Register the Application.Common.IUnitOfWork interface
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Domain Services
        services.AddScoped<PlacingOnHoldPolicy>();
        
        // Repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IPatronRepository, PatronRepository>();
        
        return services;
    }
}