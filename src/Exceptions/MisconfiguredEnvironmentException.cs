using System.Configuration;

namespace RepositoryComplianceEnforcer.Config
{
    public class MisconfiguredEnvironmentException : ConfigurationErrorsException
    {
        public MisconfiguredEnvironmentException(string message) : base(message)
        {
        }
    }
}
