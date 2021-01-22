using Steeltoe.Common.HealthChecks;

namespace Cheetas3.EU.Converter.Actuators
{
    public class CustomHealthContributor : IHealthContributor
    {
        public string Id => "CustomHealthContributor";

        public HealthCheckResult Health()
        {
            var result = new HealthCheckResult
            {
                // this is used as part of the aggregate, it is not directly part of the middleware response
                Status = HealthStatus.UP,
                Description = "This health check does not check anything"
            };
            result.Details.Add("status", HealthStatus.UP.ToString());
            result.Details.Add("description", "Something Descriptive.");
            return result;
        }
    }
}
