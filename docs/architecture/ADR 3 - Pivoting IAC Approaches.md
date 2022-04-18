# ADR 1 - Pulumi for IAC

## Context
Had a talk with the recruiter for Github. The interviewer will need to replicate my solution to test the project. Pulumi does require more setup than just Azure ARM Templates.

## Decision

Pivot from Pulumi to Azure ARM Templates.

## Status

Accepted

## Consequences

I lose some work around very specific Pulumi implementation pieces but this should lessen complications from adding Pulumi to the mix. 