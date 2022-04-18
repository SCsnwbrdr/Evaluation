# ADR 9 - CICD Enhancements

## Context

The POC has a basic pipeline checked in to act on the "main" branch. This works for the POC but we would need to speed up deployments as it takes a full redeploy of all resources which does take up a significant amount of time in the pipelines. Teams will also need ways to perform End-To-End testing on the solution and minimize outages using Blue/Green deployments. 

## Decision

The team should implement ephemeral deployments to create an environment for each developer on a PR or on demand.

The team should optmize and skip build steps if nothing has changed in the given folders for those solutions.

The team should implement/adopt an end-to-end testing approach with a tool that can run locally as well as in CI/CD Pipelines to enhance the overall stability of the pipelines.

## Status

Proposed

## Consequences

The team will need to manage deployments more carefully and ensure there are not orphaned resources out in the environment. 

Each branch will receive a dedicated cloud environment that ensures isolation from other developers and can increase velocity.

End to end testing of these ephemeral environments will enhance the overall stability of the solution by running consistent and through tests on every deployment. 
