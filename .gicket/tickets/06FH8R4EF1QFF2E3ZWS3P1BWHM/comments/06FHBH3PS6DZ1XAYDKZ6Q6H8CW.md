[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'model-execution' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-06-29T23:43:43.3935916Z",
  "operationToken": "model-execution",
  "reason": "Manual recovery completed; tester false-negative clarified that analyzers/dotnet/cs is a package archive path, not a tracked repository directory. Package verification and dual SDK package smokes pass.",
  "clearedBy": "codex"
}
```