using FluentValidation;
using IdeaTracker.Application.Repository;
using IdeaTracker.Application.Services;
using IdeaTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdeaTracker.Api.Extensions
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<IdeaService>();
            services.AddScoped<IIdeaService, IdeaService>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                 ?? Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection")
                ?? Environment.GetEnvironmentVariable("CUSTOMCONNSTR_DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No database connection string configured. Set 'ConnectionStrings__DefaultConnection' " +
                    "as an environment variable / Azure App Setting, or add it to appsettings.json for local dev.");

            services.AddDbContext<IdeaTrackerDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IIdeaRepository, IdeaRepository>();

            return services;
        }
    }
}
