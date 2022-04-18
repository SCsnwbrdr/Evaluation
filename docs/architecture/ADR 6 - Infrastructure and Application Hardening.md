# ADR 6 - Infrastructure and Application Hardening

## Context

The solution was built quickly as a POC, it is not completely hardened for a production scenario and has the following issues:

- Authorization of GitHub to the Azure Function to prevent someone from hitting the endpoints of the function randomly by just having the function key
- Function key is "leaky" in the deployments at the moment. GitHub secrets would be more secure.
- The infrastructure of the Azure Function is open to the internet at the moment

## Decision

Perform the following hardening tasks:

- Use of GitHub Secrets on the webhooks to prevent accidental disclosure of secrets intended for machine-to-machine communication.
- Use VNETs for the backend resources for the Azure Function to ensure only authorized networks can access it. 

## Status

Proposed

## Consequences

Increases complexity of the deployment to ensure that both solutions have the proper secrets.

Improves security posture by ensuring not just anyone could hit the endpoints in question.

Allows the solution to move away from using the FunctionKeys on the urls to secure them. 