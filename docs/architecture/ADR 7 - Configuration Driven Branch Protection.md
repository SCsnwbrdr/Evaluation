# ADR 7 - Configuration Driven Branch Protection

## Context

The POC only checks for one branch protection condition, that two reviewers are required on the pull request. Any changes to this requires a full redeployment of the solution which is less than ideal. We also only enforce on the default branch and may need to enforce different branching strategies/policies at a later date. 

## Decision

Use blob storage to store unstructured data/blobs that represent policies that need to be enforced on GitHub repositories. Use HTTP function triggers to manage the policies in blob storage and ensure that the blobs stay in a readable format for the Functions. 

Use these blobs to give the team additional flexibility in managing policies and the ability to change without spending dev cycles to deploy.

## Status

Proposed

## Consequences

Increases overall complexity of the solution by adding a repository of policies.

Decreases risk long term by granting the team the ability to change policies without a deployment. 

Teams eventually could build in exceptions or additional rules/workflows around classification/topic of the repository.