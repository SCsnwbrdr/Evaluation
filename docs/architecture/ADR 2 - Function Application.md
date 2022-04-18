# ADR 2 - Function Application

## Context
From the [requirements email](../requirements/2022_04_12_Email.md):
"Our security team is asking for help ensuring proper reviews are being done to code being added into our repositories. We have hundreds of repositories in our organization. What is the best way we can achieve at scale? We are new to some of the out-of-the-box settings and the GitHub API. Can you please help us create a solution that will accomplish this for our security team?"

As part of this solution, we need a way to react to webhook events coming from Github. The compute required to enable this event should be lightweight and ideally consumption driven to minimize costs. 

Additionally, I am preparing for my wedding and a month long honeymoon. Time is a significant constraint. 

## Decision

Use Azure Functions in consumption mode to react to webhooks and minimize costs. C# wil be utilized for speed to develop based on my experience.

## Status

Accepted

## Consequences

Allows me to quickly use Azure Functions to respond to the webhooks and start to experiment with the webhook responses. Adding functionality should be fairly quick given how lightweight Azure Function code tends to be. 