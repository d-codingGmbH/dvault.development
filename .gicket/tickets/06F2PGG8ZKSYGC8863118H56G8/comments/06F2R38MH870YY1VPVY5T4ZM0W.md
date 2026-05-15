﻿Manual repair after PO/PO-critic routing loop.

The previous contract required implementation evidence before developer work, which made the pre-development PO-critic gate block on src/tests changes that cannot exist yet. The ticket contract has been corrected to `ready_for_dev` while preserving the provider-reader scope and acceptance criteria.

Routing now mirrors a PO-critic-approved developer handoff: `needs-dev` and `critic-approved` are present, while `needs-po`, `blocked/dev`, and `blocked/test` were removed.

[gicket-bot] runtime-escalation-resolved-v1
```json
{
  "operationToken": "workflow-stop-the-line",
  "role": "po",
  "outcome": "manual-resolved",
  "reason": "Cleared a pre-development implementation-evidence loop and restored developer handoff labels without dropping feature scope."
}
```