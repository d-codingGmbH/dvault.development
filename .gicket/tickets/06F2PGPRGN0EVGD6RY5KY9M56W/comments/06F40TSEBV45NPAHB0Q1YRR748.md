[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGPRGN0EVGD6RY5KY9M56W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPRGN0EVGD6RY5KY9M56W`.
- Optimistic claim succeeded (`expectedRevision=06F2PNMTSQ8RVW82K84XG4WKK4`, `currentRevision=06F40QMQRK4BXRV10NCX01HJQW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGPRGN0EVGD6RY5KY9M56W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGPRGN0EVGD6RY5KY9M56W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt' from source '49ba94dcf686da0e9b6fd8bb205809eabffd24d2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt` as `2dc40c28a29b`.

Open questions / Risiken
- Because current provider-read strategy and diagnostic surfaces are latest-satellite-specific, PIT/bridge optimization may require cross-cutting internal refactoring before provider packages can plug in cleanly.
- PIT and bridge reads consume maintained tables with correctness-sensitive ordering and snapshot semantics; provider-specific SQL that changes tie-breaking or filtering would create hard-to-detect read regressions.
- Repository benchmark guidance currently supports provider-specific read evidence only for latest-satellite reads, so documentation must not over-claim PIT/bridge optimization until artifact-backed evidence exists.
- External-provider proof beyond SQLite is opt-in and consumer-managed, which can slow validation of non-SQLite optimized implementations.
- Split recommendation: If this story becomes too large during implementation, split first into a common provider-aware PIT/bridge dispatch-plus-diagnostics slice and two execution slices: PIT optimized reads and bridge optimized reads.
- Split recommendation: If non-SQLite provider-specific SQL is desired in the same release, track each external provider package in its own child ticket so fallback-safe SQLite and local proof are not blocked by external database setup.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8725`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `8c92b2e080b64c7aa165ad8f5494ed2d`
- completed-at-utc: `<redacted>-19T13:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/runs/20260519T132706457Z-8c92b2e080b64c7aa165ad8f5494ed2d.json`