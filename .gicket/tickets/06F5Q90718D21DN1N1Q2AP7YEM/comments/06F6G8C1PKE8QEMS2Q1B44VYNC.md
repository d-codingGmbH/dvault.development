﻿[gicket-maintenance] contract wording clarified for dev retry

Reason
- The dev workflow failed three times before implementation because the bot interpreted historical PO audit wording (`description update` / `ticket-description update`) as a current requirement for a developer-side ticket description artifact.
- The ticket's actual delivery scope is repository documentation: `README.md`, `docs/production-adoption-checklist.md`, `benchmarks/DCoding.Data.DVault.Benchmarks/README.md`, and `docs/releases/v0.20.0.md`.

Action
- Reworded the historical PO audit phrases to `delivery-contract refresh` / `contract refresh` without changing scope, acceptance criteria, labels, status, or route.
- Kept `needs-dev` so the normal developer role can retry the ticket.