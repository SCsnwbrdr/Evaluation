using System;
using System.Threading.Tasks;
using Octokit;
using RepositoryComplianceEnforcer.Config;

namespace RepositoryComplianceEnforcer.GitHub
{
    /// <summary>
    /// Dynamic credential switcher for using both Personal Access Tokens for local testing AND using BOTs in GitHub
    /// </summary>
    public class ConfigurableGithubCredentials
    {

        public CredentialType CredentialType { get; private set; }


        private Credentials _credentials;
        public Credentials Credentials
        {
            get
            {

                if (_credentials == null)
                    _credentials = new Credentials(_credentialString);
                return _credentials;
            }
        }

        private readonly string _credentialString;
        public ConfigurableGithubCredentials(CredentialType type, string credentialString)
        {
            CredentialType = type;
            _credentialString = credentialString;
        }

        public Credentials GetCredentials()
        {
            return new Credentials(_credentialString);

        }

        //Most of this code is from https://github.com/octokit/octokit.net/blob/356588288e2d07fb4844911f6e03ef129540b124/docs/github-apps.md
        internal static async Task<string> GetGithubApplicationToken(FunctionConfiguration functionConfiguration)
        {
            var generator = new GitHubJwt.GitHubJwtFactory(
                    new GitHubJwt.StringPrivateKeySource(functionConfiguration.GithubPrivateKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "")),
                    new GitHubJwt.GitHubJwtFactoryOptions
                    {
                        AppIntegrationId = functionConfiguration.GithubAppId, // The GitHub App Id
                        ExpirationSeconds = 600 // 10 minutes is the maximum time allowed
                    }
                );

            var jwtToken = generator.CreateEncodedJwtToken();

            var appClient = new GitHubClient(new ProductHeaderValue("scnwbrdr-demo-authentication"))
            {
                Credentials = new Credentials(jwtToken, AuthenticationType.Bearer)
            };

            var app = await appClient.GitHubApps.GetCurrent();

            // Get a list of installations for the authenticated GitHubApp
            var installations = await appClient.GitHubApps.GetAllInstallationsForCurrent();

            // This is only one installation.
            var installation = await appClient.GitHubApps.GetInstallationForCurrent(installations[0].Id);
            var installationTokenResponse = await appClient.GitHubApps.CreateInstallationToken(installation.Id);

            return installationTokenResponse.Token;
        }
    }
}
