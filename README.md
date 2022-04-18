# Solution
Technical Interview

## Business Problem

From email:

*We are most interested in your approach, the mindset you apply, and how the solution is presented to your customer. The technical solution to accomplish this is to listen for organization events to know when a repository has been created. When the repository is created, please automate the protection of the default branch. Notify yourself with an @mention in an issue within the repository that outlines the protections that were added.*

[Full Original Email is Here](/docs/requirements/2022_04_12_Email.md)

## Services & Technologies Used
This solution uses the following services and features to accomplish the business criteria:

- Github
    - Organization
    - GitHub API
    - Webhooks
    - GitHub Application
    - GitHub CLI
    - OctoKit for .Net 
- Azure
    - Azure Subscription
    - Azure Function (C# .NET 6)
    - Azure Key Vault
    - Azure Storage Account

## Testing The Solution

Below are instructions on how to setup the test for local review. 

### Prerequisite Services
1. GitHub Org that you have permisions to install webhooks into and create/modify repositories on. [Join GitHub](https://github.com/join)
2. An Azue Subscription you have owners rights to. Check out this link for a free subscription [Link to Free Azure Account](https://azure.microsoft.com/en-us/free/)

### Prerequisite Tools
1. Install Code Editor of your Choice (I used [Visual Studio Code](https://code.visualstudio.com/) for this one)
2. A way to run bash scripts. I have WSL (Windows Subsystem for Linux) activated as well as Git Bash. 
3. [Install Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
4. [Install Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-local)
5. [Install GitHub CLI](https://github.com/cli/cli#installation)

You can also run the "./PreflightToolCheck.sh" to quickly check if your tools are configured and installed.

### Before you Start
1. Go to GitHub and get a personal access token with *repo* (all) permissions assigned to it. [How to](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token). This PAT will be used to authenticate your own function GitHub so you can test live webhooks out of GitHub.
2. Open your bash shell and set the environment variable. Hint: 'export GITHUB_PERSONAL_ACCESS_TOKEN={YOUR ACCESS TOKEN HERE}'

### Initial Setup
1. Open your bash shell on the root folder of the solution.
2. Run *gh auth login --web -s admin:org_hook* to authenticate to GitHub and grant access to manage webhooks
3. Run *az login* to authenticate to Azure. (Ensure you switch to the subscription you have owner rights on by using *az account set --subscription {Name or ID}*)
4. Run *./LocalDeploy.sh*
5. You will be prompted for your unique identifier (random string to prevent global collisions in Azure). Hint, use 'export UNIQUEID={5 random characters}' to persist across runs.
6. You will be prompted for the GitHub Org you would like to deploy your webhooks into. Hint, 'export GIT_ORGANIZATION={Your Git Organization Here}' to persist across runs.
7. Get some coffee while the run completes

### Validating Infrastructure
Once the run completes you can validate the run by checking the following services:

- GitHub
  - 2 Webhooks should have been deployed into Azure (this can be modified later)
- Azure
  - 1 Resource Group
  - 1 Application Insights Resource
  - 1 Azure Key Vault
  - 1 Application Service Plan
  - 1 Azure Function
  - 1 Storage Account

### Validating the Solution
Once you have validated all the different services and artifacts that should be provisioned. You can start to test the solution.

Primary Use Case:
1. Create a new Repository in your organization with the webhooks (any settings should work)
2. Once created, click on the branch icon and branch count to the right of your branch dropdown
3. You should be able to see that the main branch has a protected icon on it now

### Cleanup
To cleanup, run the *./LocalDestroy.sh* script to automatically cleanup your solution. You will be prompted if you are sure you want to destroy the Azure Resources.

**CAUTION** If you change your Unique ID between runs, you will have orphaned resources you will have to delete manually in both GitHub AND Azure. 

## Folder Reference

### [Documents Folder](./docs/)

Contains architectural decision notes, images, and requirements for now. Could be expanded later. 

### [Infra Folder](./infra/)

Contains ARM Templates and local deployment scripts. 

### [Src Folder](./src/)

Contains Azure Function code that reacts to webhooks.

### [GitHub-Artifacts Folder](./github-artifacts/)

Contains webhook deployment code. Moved away from the special .github folder to allow for easier documentation navigation.

## Deployment Setup

Deployment via GitHub takes a bit more effort but it is possible. This is out of Scope for the POC, these instructions are use at your own risk You will need the following setup:

1. GitHub App for Deployment (actions can't create webhooks on the org) [Creating an App](hhttps://docs.github.com/en/developers/apps/building-github-apps/creating-a-github-app), [App Concept](https://docs.github.com/en/developers/apps/getting-started-with-apps/about-apps)
  - DEPLOYMENT_APP_ID
  - DEPLOYMENT_APP_PRIVATE_KEY

2. GitHub App for Running the App (principle of least privilege, shouldn't use the same bot to do both) 
  - SOLUTION_GITHUB_APP_ID
  - SOLUTION_GITHUB_APP_PRIVATE_KEY

3. Azure Service Principal that has owner rights on the subscription you are deploying to:
  - AZURE_CREDENTIALS => Result from *az ad sp create-for-rbac --sdk-auth --role Owner  --scopes /subscriptions/{subscriptionId}* (not ideal but it works for a poc)
  - AZURE_OBJECT_ID => There was a bug in the Azure CLI with permissions on Azure AD. Given the deprecation of the old Azure AD endpoints, I deprioritized fixing this.
  - AZURE_SUBSCRIPTION => That you are deploying into

The CICD action file is mostly ready to go but was tested on a private repository. We will view the repository for the demo. 

## Attributions

If code was explicitly copied and had more than one line to it, I pasted the attribution to the page right inline with the code or in a comment near the code. Otherwise, here were my main sources to create and reference for this solution:

- [Stack Overflow](https://stackoverflow.com/) => General Questions around Bash
- [Webhook and Event Payloads for GitHub](https://docs.github.com/en/developers/webhooks-and-events/webhooks/webhook-events-and-payloads) => Webhook and Events Payloads
- [GitHub API Reference](https://docs.github.com/en/rest/reference) => Figuring out API Calls to use in the application and the GitHub Cli
- [GitHub CLI Reference](https://cli.github.com/manual/index) => Used to Register Webhooks
- [OctoKit Project for .Net](https://github.com/octokit/octokit.net) => Used to Make API Calls into GitHub with strongly typed objects
- [GitHub documents for general questions](https://docs.github.com/en) => Used for general reference to GitHub
