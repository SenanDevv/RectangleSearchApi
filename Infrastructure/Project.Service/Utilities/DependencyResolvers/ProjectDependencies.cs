using Microsoft.Extensions.DependencyInjection;
using Project.Infrastructure.Repositories.Abstraction;
using Project.Infrastructure.Repositories.Implementation;
using Project.Service.Services.Abstraction;
using Project.Service.Services.Implementation;

namespace Project.Service.Utilities.DependencyResolvers
{
    public static class ProjectDependencies
    {
        public static void AddProjectDependencies(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IRectangleRepository, RectangleRepository>();


            // Services
            services.AddScoped<IRectangleService, RectangleService>();
        }
    }
}
