[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZQAWZ7QRGB68KB21C9B0R'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZQAWZ7QRGB68KB21C9B0R`.
- Optimistic claim succeeded (`expectedRevision=06F8M03JGBE4AWR46DP1C9E9J0`, `currentRevision=06F9BCZ86FAYB2WXTR3B22H5P8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZQAWZ7QRGB68KB21C9B0R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZQAWZ7QRGB68KB21C9B0R': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZQAWZ7QRGB68KB21C9B0R-task-update-v0-30-0-typed-helper-freshness-docum' from source '155262b89b8f21636cbe094bbd5dd21af7211fb5'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Because the authoritative boundary is spread across README, analyzer README, design-time workflow guidance, and architecture docs, partial documentation updates can reintroduce ambiguity about freshness versus PIT/bridge ReadShape compatibility.
- No `docs/releases/v0.30.0.md` file is present in the current branch evidence, so the current coordinated release narrative will stay incomplete unless this ticket explicitly adds that release-note record.
- Split recommendation: No split recommended; the epic already separated contract definition, diagnostics implementation, transition-test coverage, and this documentation follow-through into bounded sibling tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9282`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `161032df79e24d2588b28b35f4674437`
- completed-at-utc: `<redacted>-05T03:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/runs/20260605T031009662Z-161032df79e24d2588b28b35f4674437.json`