[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'workflow-stop-the-line' for role 'po'.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "po",
  "clearedAtUtc": "2026-05-31T14:21:55.8837038Z",
  "operationToken": "workflow-stop-the-line",
  "reason": "Manual resolution after investigation: the stale incoming blocks relation from 06F5Q93H60W6X8FJ88PWTR6NG4 has been removed, blocked/dev and blocked/test labels have been cleared, current relation eligibility is true, incoming relation count is zero, all five v0.23.0 child tickets are done, and the durable epic contract now records no open questions. Clear the PO workflow-stop-the-line escalation so the bot can rerun PO normally.",
  "clearedBy": "codex"
}
```