[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGP7HM8F39K3J0H5JHB3B4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGP7HM8F39K3J0H5JHB3B4`.
- Optimistic claim succeeded (`expectedRevision=06F2PNMG80BBW5MGC2AQ5NCG2G`, `currentRevision=06F423K2K056EKS02HJQ370N2M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGP7HM8F39K3J0H5JHB3B4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGP7HM8F39K3J0H5JHB3B4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGP7HM8F39K3J0H5JHB3B4-epic-maintenance-and-query-operations' from source '00350c1c97c3dae3aada4a329527331e34524d45'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGP7HM8F39K3J0H5JHB3B4-epic-maintenance-and-query-operations` as `7f9eeb5a8c6a`.

Open questions / Risiken
- Live `gicket-read-ticket`, relation, comment, and attachment verification remained blocked in this run by `BOT-LOCAL-TOOL-TRUST-BLOCKED`, so persisted Gicket relation state could not be re-confirmed beyond the prompt snapshot and repository planning documents.
- Incremental hierarchy bridge maintenance is intentionally not delete-aware; teams that use it after topology shrinkage without a rebuild can retain stale rows or stale shorter-depth assumptions.
- SQLite is the only repository-proven optimized PIT/bridge read path today; release or adoption messaging that implies broader provider optimization would overstate current evidence.
- Split recommendation: No new split is recommended from current evidence; keep the epic aligned to the already-documented child-ticket tree in `docs/plans/pit-maintenance-service-v1-contract.md` rather than reopening scope.
- Split recommendation: If runtime relation cleanup later shows the epic is missing child links, restore links to the existing PIT maintenance, dependent PIT-read, PIT-read optimization, and documentation follow-through tickets already named in the repository planning contract i...

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9377`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c4c2e3af261e4701b1bfc1f989b5578f`
- completed-at-utc: `<redacted>-19T16:31:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGP7HM8F39K3J0H5JHB3B4/runs/20260519T163119322Z-c4c2e3af261e4701b1bfc1f989b5578f.json`