[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'runtime-environment-precondition' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-06-30T19:02:20.4372762Z",
  "operationToken": "runtime-environment-precondition",
  "reason": "Manual dev rework completed in the ticket branch. The previous runtime precondition was caused by the bot\u0027s temporary patcher path not being visible to its declared shell transport; the replacement mutation was applied directly in the branch, verified, and routed to test.",
  "clearedBy": "codex"
}
```