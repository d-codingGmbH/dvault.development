﻿[gicket-maintenance] stale role block cleared

Reason
- The latest PO-critic result approved the ticket for developer handoff and set `needs-dev`.
- Relation eligibility is `relationReady: true`.
- The remaining `blocked/dev` and `blocked/test` labels came from the earlier return-to-PO cycle and were preserved during the successful handoff.

Action
- Removed stale labels `blocked/dev` and `blocked/test`.
- Kept `needs-dev` so the normal developer role can claim the ticket.