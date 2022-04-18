# GitHub Folder

This folder manages the various github artifacts deployed out as part of this solution. Here is how to use this folder:

## Deployment File : 

*LocalDeploy.sh*

Purpose: Local Deployment commands to spin up your personal development environment for testing GitHub webhooks. Assumes it ran as part of the *./LocalDeploy.sh* script at the root folder of the repo.

## Payloads File

*WebhookPayloadTemplate.json*

Purpose: Contains json template that have simple string replacement tokens in it to quickly create .json templates for calling the API endpoints. This is needed as the github CLI does not have a way to add/manage webhooks

## Mapping File

*Mappings.tsv*

Purpose: Tab Delimited File for mapping AzureFunction paths to a given event. The space delimited file is laid out as follows:

{Function Path} {Comma Delimited Events surrounded by quotes}

**Sample:**

/api/branchprotectionevents "branch_protection_rule" <br />
/api/repositoryevents   "repository","push"