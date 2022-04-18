using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Octokit;
using RepositoryComplianceEnforcer.Config;
using RepositoryComplianceEnforcer.Notifications;
using RepositoryComplianceEnforcer.GitHub;

namespace RepositoryComplianceEnforcer
{
    public class BranchProtectionEventsFunction
    {
        private readonly FunctionConfiguration _config;
        private readonly IGitHubClient _githubClient;
        private readonly INotificationClient _notificationClient;
        private readonly ILogger<BranchProtectionEventsFunction> _logger;

        public BranchProtectionEventsFunction(ILogger<BranchProtectionEventsFunction> logger, FunctionConfiguration config, IGitHubClient githubClient, INotificationClient notificationClient)
        {
            _config = config;
            _githubClient = githubClient;
            _notificationClient = notificationClient;
            _logger = logger;
        }

        [FunctionName("BranchProtectionEvents")]
        public async Task<IActionResult> RunBranchProtectionEvents(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            //Get the Body to parse the webhook
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if(data.zen != null)
            {
                //This means its the ping webhook, skip it
                return new OkResult();
            }

            //Always Enforce
            var repoId = (long)data.repository.id;
            var repoEnforcer = RepositoryComplianceEnforcer.Create(_githubClient, repoId, _notificationClient, _config.NotificationUser);
            await repoEnforcer.ValidateExistingRepoCompliance();

            return new OkResult();
        }


    }
}

