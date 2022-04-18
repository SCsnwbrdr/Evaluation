using System.Threading.Tasks;
using Octokit;
using RepositoryComplianceEnforcer.Exceptions;

namespace RepositoryComplianceEnforcer.Notifications
{
    public class GithubIssueNotificationClient : INotificationClient
    {
        private readonly IGitHubClient _client;

        /// <summary>
        /// Need the client and repo id to post the message as an issue
        /// </summary>
        public GithubIssueNotificationClient(IGitHubClient client)
        {
            _client = client;
        }

        public async Task Notify(string title, string body,dynamic repoId)
        {
            var internalRepo = (long)repoId;
            var newIssue = new NewIssue(title)
            {
                Body =  body
            };
            await _client.Issue.Create(internalRepo, newIssue);
        }
    }
}

