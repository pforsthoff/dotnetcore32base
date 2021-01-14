using Steeltoe.Management.Info;

namespace Cheetas3.EU.Actuators
{
    public class ArbitraryInfoContributor : IInfoContributor
    {
        public void Contribute(IInfoBuilder builder)
        {
            // pass in the info
            builder.WithInfo("arbitraryInfo", new { someProperty = "someValue" });
        }
    }
}
