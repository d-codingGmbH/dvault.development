[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'runtime-environment-precondition' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-06-15T21:59:38.5708864Z",
  "operationToken": "runtime-environment-precondition",
  "reason": "Manual review: the escalation was caused by a transient git status call hang in the isolated Dev tool session; repository state is clean and no product/runtime precondition remains.",
  "clearedBy": "codex"
}
```