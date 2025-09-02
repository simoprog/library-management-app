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
        
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
            // services.AddValidatorsFromAssembly(assembly);
        
            return services;
        }
    }