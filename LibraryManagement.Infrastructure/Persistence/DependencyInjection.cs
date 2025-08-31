using library_management_api.Domain.Repositories;
using library_management_api.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace library_management_api.Infrastructure.Persistence;

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
        
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IPatronRepository, PatronRepository>();
        
        return services;
    }
}