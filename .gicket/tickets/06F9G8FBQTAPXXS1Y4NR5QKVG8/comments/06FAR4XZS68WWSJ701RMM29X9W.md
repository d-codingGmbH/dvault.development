[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9G8FBQTAPXXS1Y4NR5QKVG8'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9G8FBQTAPXXS1Y4NR5QKVG8`.
- Optimistic claim succeeded (`expectedRevision=06F9GF2W5YQ6HS34VT5XHVH984`, `currentRevision=06FAR277QZ8F6YDJ12H1YP198R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9G8FBQTAPXXS1Y4NR5QKVG8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9G8FBQTAPXXS1Y4NR5QKVG8': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9G8FBQTAPXXS1Y4NR5QKVG8-task-update-package-verifier-and-ci-guidance-for' from source 'e64718c0c97617a9402451ae38d24b1fca5384d7'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F9G8FBQTAPXXS1Y4NR5QKVG8-task-update-package-verifier-and-ci-guidance-for` as `a276547dab38`.

Open questions / Risiken
- Current root and analyzer README examples are still single-line 0.32.0 guidance, so verifier and documentation updates can drift apart unless this ticket treats packaged README content as part of the same dual-line contract.
- Because tools/DCoding.Data.DVault.PackageVerification stays net10-only, guidance must be explicit that the .NET 10 SDK validation lane still proves both net8.0 and net10.0 package outputs; otherwise maintainers may incorrectly narrow validation to one package line.
- Broader consumer-facing compatibility documentation remains blocked by 06F9G8FJMZ3AY43YG06W2V4T8G, so stale public install examples can persist even after matrix tests and verifier checks are in place unless the downstream docs task is completed promptly.
- Split recommendation: No additional split is recommended: done story 06F9G8F4RQ0T7RV82M3H2H3FVG already carved out exact provider and version matrix proof, while this ticket remains the bounded home for reusable package verification, README and manual guidance, and CI document...
- Split recommendation: Keep broader release-note and compatibility-prose updates in blocked task 06F9G8FJMZ3AY43YG06W2V4T8G rather than expanding this ticket into full v0.33.0 documentation ownership.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8364`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `098a139a353446b7baeae971fbce0e44`
- completed-at-utc: `<redacted>-09T11:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9G8FBQTAPXXS1Y4NR5QKVG8/runs/20260609T111035971Z-098a139a353446b7baeae971fbce0e44.json`