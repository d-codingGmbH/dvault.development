[gicket-bot] runtime escalation resolved

Manual override: clear runtime escalation 'workflow-stop-the-line' for role 'po'.

PO repair note:
- Rebased this ticket branch onto current develop so the completed upstream encrypted-attribute conversion work is visible in the branch.
- Confirmed 06FE4RASEQZN7XEYH1XR4H06PR is done and the Gicket relation 06FE4RASEQZN7XEYH1XR4H06PR --blocks--> 06FE4RB219AXVF2535MFF36PN4 is materialized.
- Removed stale blocked/dev, blocked/test, and needs-po labels, then routed this ticket back to po-critic with critic-needed.

[gicket-bot] runtime-escalation-resolved-v1

```json
{
  "role": "po",
  "clearedAtUtc": "2026-06-22T09:16:09.0673463Z",
  "operationToken": "workflow-stop-the-line",
  "reason": "PO-critic blocker resolved: upstream 06FE4RASEQZN7XEYH1XR4H06PR is done, the blocking relation to this ticket is materialized, this branch was rebased onto develop, and stale blocked/needs-po labels were replaced with critic-needed.",
  "clearedBy": "codex"
}
```
