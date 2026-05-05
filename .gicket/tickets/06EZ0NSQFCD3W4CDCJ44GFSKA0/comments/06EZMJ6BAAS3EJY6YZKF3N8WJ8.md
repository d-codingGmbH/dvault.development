﻿[gicket-bot] manual PO cleanup after relation-loop diagnosis

Summary
- Downgraded stale workflow-blocking `blocks` relations around this closure-only snapshot guardrail ticket to `relates` so they remain traceability-only.
- Updated the delivery contract to name the advanced-hook owner story and to remove the obsolete "relation cleanup unavailable" note.
- Routed the ticket back to PO-critic with `critic-needed`; no developer or tester execution is intended for this ticket.

Relation cleanup
- `06EZ0NSQFCD3W4CDCJ44GFSKA0 -> 06EZ0NSXY2Y1JZ8SSCX177C770`: `blocks` -> `relates`
- `06EZ0NSQFCD3W4CDCJ44GFSKA0 -> 06EZ0NTV4SVAKV98C418T8A3CC`: `blocks` -> `relates`
- `06EZ0NSQFCD3W4CDCJ44GFSKA0 -> 06EZ0NVN71BN0QWJDCWGVZ2PYG`: `blocks` -> `relates`
- `06EZ0NSHJVC9SD2KS6PWWNHPJM -> 06EZ0NSQFCD3W4CDCJ44GFSKA0`: `blocks` -> `relates`