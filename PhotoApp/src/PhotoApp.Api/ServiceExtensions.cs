using Microsoft.Extensions.Diagnostics.HealthChecks;
using PhotoApp.Api.HealthCheck;

namespace PhotoApp.Api
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration["ConnectionStrings:DefaultConnection"], healthQuery: "select 1", name: "SQLServer", failureStatus: HealthStatus.Unhealthy, tags: new[] { "PhotoApp", "Database" })
                .AddCheck<RemoteHealthCheck>("Remote endpoints Health Check", failureStatus: HealthStatus.Unhealthy)
                .AddCheck<MemoryHealthCheck>($"Photo App Memory Check", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Photo App Service" })
                .AddUrlGroup(new Uri("https://localhost:7016/api/photoappservice/v1/heartbeat/ping"), name: "base URL", failureStatus: HealthStatus.Unhealthy);

            services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(10); //time in seconds between check
                options.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                options.SetApiMaxActiveRequests(1); // api requests concurrency
                options.AddHealthCheckEndpoint("ChatApp api", "/api/health"); // map health check api
            })
                .AddInMemoryStorage();
        }
    }
}
