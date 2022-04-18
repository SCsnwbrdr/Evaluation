using System;
using Microsoft.Extensions.Logging;

namespace RepositoryComplianceEnforcer.Config
{
    /// <summary>
    /// Configuration validation on the function to ensure that
    /// </summary>
    public class FunctionConfiguration
    {
        private readonly ILogger _logger;

        public string ProductHeader
        {
            get { return "repository-compliance-helper"; }
        } 
        public string GithubPersonalAccessToken { get; private set; }
        public string GithubPrivateKey { get; private set; }

        public string NotificationUser { get; private set; }
        public int GithubAppId { get; private set; }
        public CredentialType CredentialType { get; private set; }

        public FunctionConfiguration(ILogger logger)
        {
            var personalAccessToken = Environment.GetEnvironmentVariable("GITHUB_PERSONAL_ACCESS_TOKEN");
            var privatekey = Environment.GetEnvironmentVariable("GITHUB_PRIVATEKEY");
            var notificationUser = Environment.GetEnvironmentVariable("GITHUB_NOTIFICATION_USER");
            var appId = Environment.GetEnvironmentVariable("GITHUB_APP_ID");

            _logger = logger;

            //Lets validate the configuration is accurate
            ValidateAuthenticationForEnvironment(personalAccessToken, privatekey);
            ValidateNotificationUser(notificationUser);

            //Set the private properties
            GithubPersonalAccessToken = personalAccessToken;
            GithubPrivateKey = privatekey;
            NotificationUser = notificationUser;
            GithubAppId = int.Parse(appId);

            //Set the Credential Type works for now since we have only 2 types
            CredentialType = string.IsNullOrEmpty(personalAccessToken) ? CredentialType.GithubApplication : CredentialType.PersonalAccessToken;
        }

        private void ValidateNotificationUser(string notificationUser)
        {
            if (string.IsNullOrEmpty(notificationUser))
            {
                throw new MisconfiguredEnvironmentException("The Environment Variable 'GITHUB_NOTIFICATION_USER' is not set. This should be a github user or team.");
            }
        }

        private void ValidateAuthenticationForEnvironment(string personalAccessToken, string privatekey)
        {
            var patIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(personalAccessToken);
            var privatekeyIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(privatekey);
            if (!(patIsNullOrWhiteSpace ^ privatekeyIsNullOrWhiteSpace))
            {
                var advice = !patIsNullOrWhiteSpace && !privatekeyIsNullOrWhiteSpace ? "You cannot set both." : "Must set one or the other, but not both.";
                _logger.LogError("Is GITHUB_PERSONAL_ACCESS_TOKEN Environment Variable Empty? {0}", patIsNullOrWhiteSpace);
                _logger.LogError("Is GITHUB_PRIVATEKEY Environment Variable Empty? {0}", privatekeyIsNullOrWhiteSpace);
                throw new MisconfiguredEnvironmentException($"You must set either 'GITHUB_PERSONAL_ACCESS_TOKEN' OR 'GITHUB_PRIVATEKEY'. {advice} ");
            }
        }
    }
}
