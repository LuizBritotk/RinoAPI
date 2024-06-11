using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Rino.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
                .Select(t => new { Service = t.GetInterface($"I{t.Name}"), Implementation = t });

            foreach (var type in serviceTypes)
            {
                if (type.Service != null)
                {
                    services.AddScoped(type.Service, type.Implementation);
                }
            }
        }

        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .Select(t => new { Repository = t.GetInterface($"I{t.Name}"), Implementation = t });

            foreach (var type in repositoryTypes)
            {
                if (type.Repository != null)
                {
                    services.AddScoped(type.Repository, type.Implementation);
                }
            }
        }
    }
}
