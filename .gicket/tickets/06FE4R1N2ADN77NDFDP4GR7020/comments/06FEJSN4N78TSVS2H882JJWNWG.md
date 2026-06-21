[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'runtime-environment-precondition' for role 'dev'.

The developer workflow was blocked because the unattended environment lacked `Microsoft.EntityFrameworkCore.Analyzers` 10.0.9 and could not perform a network-dependent restore. I ran `dotnet restore DVault.slnx --nologo` successfully on this ticket branch; all projects are now up-to-date for restore in this environment.

No product or benchmark artifact changes were present from the interrupted developer attempt, so the ticket intentionally remains `needs-dev` for a normal developer retry.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-06-21T08:57:58.5579140Z",
  "operationToken": "runtime-environment-precondition",
  "reason": "NuGet restore now succeeds on the ticket branch; the missing Microsoft.EntityFrameworkCore.Analyzers package precondition is resolved and dev can retry normally.",
  "clearedBy": "Codex"
}
```