[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'runtime-environment-precondition' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-06-07T21:38:06.0399774Z",
  "operationToken": "runtime-environment-precondition",
  "reason": "Manual recovery after bot fix: runtime/environment preconditions are no longer durable ticket stops. Clear the stale durable marker so the rebuilt bot can retry or defer based on available PostgreSQL/MySQL evidence hosts.",
  "clearedBy": "codex"
}
```