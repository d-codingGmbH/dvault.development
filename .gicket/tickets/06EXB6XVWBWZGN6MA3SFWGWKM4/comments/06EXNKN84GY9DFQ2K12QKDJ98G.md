[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'model-execution' for role 'dev'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "dev",
  "clearedAtUtc": "2026-04-29T19:54:28.8091446Z",
  "operationToken": "model-execution",
  "reason": "The model-execution failure was caused by duplicate Codex response metadata key handling in the bot runtime. The bot binary has been updated to normalize duplicate keys, and the failure snapshot commit remains preserved on this ticket branch.",
  "clearedBy": "codex"
}
```