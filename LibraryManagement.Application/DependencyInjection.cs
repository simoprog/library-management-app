using System.Reflection;
using FluentValidation;
using LibraryManagement.Application.Common.Behaviors;
using LibraryManagement.Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);
        
        // Pipeline Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Domain Services
        services.AddScoped<PlacingOnHoldPolicy>();
        
        return services;
    }
}