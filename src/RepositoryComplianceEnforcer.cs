using System.Threading.Tasks;
using Newtonsoft.Json;
using Octokit;
using System;
using RepositoryComplianceEnforcer.Notifications;

namespace RepositoryComplianceEnforcer
{
    public class RepositoryComplianceEnforcer
    {

        private readonly IGitHubClient _githubClient;
        private readonly Repository _repository;
        private readonly INotificationClient _notificationClient;
        private readonly string _notificationUser;

        private RepositoryComplianceEnforcer(IGitHubClient client, long repoId, INotificationClient notificationClient, string notificationUser)
        {
            _notificationUser = notificationUser;
            _notificationClient = notificationClient;
            _githubClient = client;
            _repository = _githubClient.Repository.Get(repoId).Result;
        }

        public static RepositoryComplianceEnforcer Create(IGitHubClient client, long repoId, INotificationClient notificationClient, string notificationUser)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));
            ArgumentNullException.ThrowIfNull(repoId, nameof(repoId));
            ArgumentNullException.ThrowIfNull(notificationClient, nameof(notificationClient));
            return new RepositoryComplianceEnforcer(client, repoId, notificationClient, notificationUser);
        }

        ///<summary>
        ///Sets up the default branch protection on the repository
        ///</summary>
        public async Task SetupNewRepository()
        {
            BranchProtectionSettings branch = await ApplyChanges();

            var title = $"New Repository Updated {_repository.Name} with Default Protection Policies";
            var body = $"@{_notificationUser} \n\n" +
                       $"The Default Branch of {_repository.Name} was protected with the following settings: \n\n" +
                       $"{JsonConvert.SerializeObject(branch, Formatting.Indented)}";

            await _notificationClient.Notify(title, body, _repository.Id);
        }

        ///<summary>
        ///Validates that the branch is protected as expected and if not, enforces it.
        ///</summary>
        public async Task ValidateExistingRepoCompliance()
        {
            //Get the expected protections which is in this case, at least one person has to review the branch
            BranchProtectionRequiredReviewsUpdate protection = new BranchProtectionRequiredReviewsUpdate(false, false, 2);
            BranchProtectionSettings branchProtection = null;
            try
            {
                branchProtection = await _githubClient.Repository.Branch.GetBranchProtection(_repository.Id, _repository.DefaultBranch);
            }
            catch (Octokit.NotFoundException)
            {
                //Do nothing, it means the branch is not protected and we should protect it
            }
            //We only check for number of reviewers, nothing else
            if (branchProtection == null || branchProtection.RequiredPullRequestReviews.RequiredApprovingReviewCount != protection.RequiredApprovingReviewCount)
            {
                BranchProtectionSettings branch = await ApplyChanges();


                var title = $"Configuration Drift Detected! Updated {_repository.Name} with Default Protection Policies";
                var body = $"@{_notificationUser} \n\n" +
                           $"The Default Branch of {_repository.Name} was protected with the following settings: \n\n" +
                           $"{JsonConvert.SerializeObject(branch, Formatting.Indented)}";

                await _notificationClient.Notify(title, body, _repository.Id);
            }
        }

        private async Task<BranchProtectionSettings> ApplyChanges()
        {
            await EmptyDefaultBranchCheck();

            //Protect the Default Branch, this should be 
            BranchProtectionRequiredReviewsUpdate protection = new BranchProtectionRequiredReviewsUpdate(false, false, 2);
            BranchProtectionSettingsUpdate settings = new BranchProtectionSettingsUpdate(protection);
            return await _githubClient.Repository.Branch.UpdateBranchProtection(_repository.Id, _repository.DefaultBranch, settings);
        }

        /// <summary>
        /// Checks to see if the default branch in a repo is empty and then pushes a commit to it.
        /// </summary>
        private async Task EmptyDefaultBranchCheck()
        {
            var defaultBranchDoesNotExist = true;
            try
            {
                var defaultBranch = await _githubClient.Repository.Branch.Get(_repository.Id, _repository.DefaultBranch);
                defaultBranchDoesNotExist = false;
            }
            catch (NotFoundException)
            {
                //That is ok, this means that the branch is not created yet and we need to push a single dummy commit
            }

            //Default branch does not exist until the first commit is made, if the count is 0, then we have to put a dummy file into the repo to function properly
            //Future Enhancement could be making the content flexible
            if (defaultBranchDoesNotExist)
            {
                var content = "Hello World";
                var commit = await _githubClient.Repository.Content.CreateFile(_repository.Id, "README.md", new CreateFileRequest("Initializing the Repo", content, _repository.DefaultBranch));
            }
        }
    }
}

