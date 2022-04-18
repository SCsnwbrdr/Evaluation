# ADR 1 - Pulumi for IAC

## Context
From the [requirements email](../requirements/2022_04_12_Email.md):
"Our security team is asking for help ensuring proper reviews are being done to code being added into our repositories. We have hundreds of repositories in our organization. What is the best way we can achieve at scale? We are new to some of the out-of-the-box settings and the GitHub API. Can you please help us create a solution that will accomplish this for our security team?"

This requirement means that we need to deploy out infrastructure in a cloud provider to react to events from Github. We will need to manage the deployed resources efficiently. Powershell could be used here but I already have written some Github action code that deploys out and manages Azure Functions with actions already created. 

Additionally, I am preparing for my wedding and a month long honeymoon. Time is a significant constraint. 

## Decision

Use Pulumi as an Infastructure as Code and reuse what I can for the interview. 

I will need to provision my own account in Pulumi to connect this Github Repo to as well. 

## Status

Accepted

## Consequences

I get to save some time using patterns and code I have experience with. Also displays efficient re-use of code and previous patterns for time savings. 

Won't really reflect a "from scratch project" for the interview, however, this is code I previously wrote on my own so it is an accurate representation of prior work. 