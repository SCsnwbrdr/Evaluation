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
    public class RepositoryEventsFunction
    {
        private readonly FunctionConfiguration _config;
        private readonly IGitHubClient _githubClient;
        private readonly INotificationClient _notificationClient;
        private readonly ILogger<RepositoryEventsFunction> _logger;

        public RepositoryEventsFunction(ILogger<RepositoryEventsFunction> logger, FunctionConfiguration config, IGitHubClient githubClient, INotificationClient notificationClient)
        {
            _config = config;
            _githubClient = githubClient;
            _notificationClient = notificationClient;
            _logger = logger;
        }


        [FunctionName("RepositoryEvents")]
        public async Task<IActionResult> RunRepositoryEvents(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            //Get the Body to parse the webhook
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data.zen != null)
            {
                //This means its the ping webhook, skip it
                return new OkResult();
            }

            //We only care about created events
            if (data.action == "created")
            {
                var repoId = (long)data.repository.id;
                
                //Setup the default branch proection on the repository
                //This could be done more elegantly in the future adhering more to DDD principles
                var repoEnforcer = RepositoryComplianceEnforcer.Create(_githubClient, repoId, _notificationClient, _config.NotificationUser);
                await repoEnforcer.SetupNewRepository();
            }

            return new OkResult();
        }
    }
}

