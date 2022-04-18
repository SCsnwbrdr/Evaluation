# ADR 10 - Logging, Monitoring, And Alerting

## Context

While the POC gives basic logging and alerting, a number of scenarios may happen that could cause the solution to fail silently. If communication to GitHub is lost, currently, you would have to check the status of the solution every day to ensure it was still running.

## Decision

Implement alerts in Azure that trigger on failures.

Integrate into central logging system to have a single pane of glass into the environment. 

## Status

Proposed

## Consequences

The team will be able to reduce their Mean Time to Detect (MTTD) problems and correct them before they become a much larger issue.