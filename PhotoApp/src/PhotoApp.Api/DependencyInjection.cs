using Microsoft.EntityFrameworkCore;
using PhotoApp.Core.Interfaces.Repositories;
using PhotoApp.Core.Interfaces.Services;
using PhotoApp.Core.Services;
using PhotoApp.Infrastructure.Context;
using PhotoApp.Infrastructure.Repositories;

namespace PhotoApp.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }   

            if (configuration == null)
            {
                throw new ArgumentException(nameof(configuration));
            }

            services.AddDbContext<PhotoAppDBContext>(option => {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                     {
                         sqlOptions.EnableRetryOnFailure(
                             maxRetryCount: 10,
                             maxRetryDelay: TimeSpan.FromSeconds(5),
                             errorNumbersToAdd: null
                         );
                     });
            });

            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();

            services.AddHttpClient();

            return services;
        }
    }
}
