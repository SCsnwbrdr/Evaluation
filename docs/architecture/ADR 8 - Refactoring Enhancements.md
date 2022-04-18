# ADR 8 - Refactoring Enhancements

## Context

The POC has dependency injection placed into it but it could be further improved by implementing basic domain driven design to make the business logic more easily testable as well as centralize the logic for easy maintenance.

Unit testing also needs to be added to make the app ready for production.

An additional library like Polly should be implemented to perform automatic retries on transient failures in order to minimize the chance of a webhook event being missed.

## Decision

Refactor to use domain driven design in the solution.

Add unit Testing.

Wrap calls to GitHub in polly calls/library for resiliency.

## Status

Proposed

## Consequences

Improves overally quality and maintainability of the application.

Reduces the impact of transient errors on the solution and minimizes the need for "corrective" actions.

