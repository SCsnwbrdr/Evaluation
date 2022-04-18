# ADR 4 - Bash For Scripts

## Context
In order to have the solution deploy locally, I need to use a scripting language that is easy to use no matter the platform the evaluator will be working on. Sure, I could leave instructions on what to install but trying to lower the barrier to test the solution.

I know Bash enough to be "dangerous" but I'm not as proficient in it as PowerShell.

## Decision

Use Bash for the primary scripting language for setup and tear down. 

## Status

Accepted

## Consequences

Bash is not my primary scripting language of choice. I'll be a little slower and probably not write as clean of code. 

Evaluator should have an easier time executing the scripts.