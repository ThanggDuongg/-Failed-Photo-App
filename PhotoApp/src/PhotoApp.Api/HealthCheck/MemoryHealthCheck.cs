using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace PhotoApp.Api.HealthCheck
{
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<MemoryCheckOptions> _optionsMonitor;

        public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        public string Name => "memory_check";

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellation = default(CancellationToken))
        {
            var options = this._optionsMonitor.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
            {
                { "AllocatedBytes", allocated },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) },
            };

            var status = (allocated < options.Threshold ) ? HealthStatus.Healthy : HealthStatus.Unhealthy;

            return Task.FromResult(new HealthCheckResult(
                status,
                description: "Reports degraded status if allocated bytes " + 
                    $">= {options.Threshold} bytes.",
                exception: null,
                data: data
            ));
        }
    } 

    public class MemoryCheckOptions
    {
        public string? Memorystatus { get; set; }

        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }
}
