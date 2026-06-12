[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'runtime-environment-precondition' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-06-12T12:55:14.4025159Z",
  "operationToken": "runtime-environment-precondition",
  "reason": "Manual review: dotnet restore DVault.slnx --nologo and dotnet build DVault.slnx --nologo now pass on the ticket branch after refreshing local package/cache state; the prior runtime-environment precondition is no longer active.",
  "clearedBy": "codex"
}
```