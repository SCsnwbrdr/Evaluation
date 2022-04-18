# ADR 5 - Audit and Detect Drift

## Context

Currently the solution only works for new repositories or ones that tried to have their branch protection changed to something different. This should cover a majority of new cases but we need to ensure that the following cases are also covered:

- Transient errors cause the webhook to not be delivered
- Existing Repos have to be manually updated
- Changes to policy cause the organization to manually update

## Decision

Create new event trigger that happens daily to review existing branch protection policies and apply the required reviews.

## Status

Proposed

## Consequences

Increases the amount of code required to maintain the solution and requires some optimization regarding the API calls to not hit any limits.

Ensures old repositories are covered by the new policies.

Automatically corrects issues every night. 