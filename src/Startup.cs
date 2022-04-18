using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using RepositoryComplianceEnforcer.Config;
using RepositoryComplianceEnforcer.GitHub;
using RepositoryComplianceEnforcer.Notifications;

[assembly: FunctionsStartup(typeof(RepositoryComplianceEnforcer.Startup))]


namespace RepositoryComplianceEnforcer
{
    /// <summary>
    /// Startup class for the Azure Function
    /// Dependency injection registrations happen here
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddSingleton<FunctionConfiguration>();

            builder.Services.AddScoped(svc =>
            {
                var functionConfiguration = svc.GetService<FunctionConfiguration>();
                string credentialString;
                if (functionConfiguration.CredentialType == CredentialType.PersonalAccessToken)
                {
                    credentialString = functionConfiguration.GithubPersonalAccessToken;
                }
                else if (functionConfiguration.CredentialType == CredentialType.GithubApplication)
                {
                    credentialString = (ConfigurableGithubCredentials.GetGithubApplicationToken(functionConfiguration)).Result;
                }
                else
                {
                    //Just in case someone adds a new one credential type without setting up the right methods and logic
                    throw new ArgumentException("Credential Type not supported");
                }

                return new ConfigurableGithubCredentials(functionConfiguration.CredentialType, credentialString);
            });

            builder.Services.AddScoped<IGitHubClient>(svc =>
            {
                var credentials = svc.GetService<ConfigurableGithubCredentials>();
                var configuration = svc.GetService<FunctionConfiguration>();

                return new GitHubClient(new ProductHeaderValue(configuration.ProductHeader))
                {
                    Credentials = credentials.Credentials
                };
            });

            builder.Services.AddScoped<INotificationClient, GithubIssueNotificationClient>();
        }
    }
}
