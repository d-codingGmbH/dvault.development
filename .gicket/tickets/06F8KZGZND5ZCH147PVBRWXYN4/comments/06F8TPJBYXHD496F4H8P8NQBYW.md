[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZGZND5ZCH147PVBRWXYN4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZGZND5ZCH147PVBRWXYN4`.
- Optimistic claim succeeded (`expectedRevision=06F8M00M2F80AN0682K4RTMMB8`, `currentRevision=06F8TKGDPTZ79X3J95DWS70CY0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZGZND5ZCH147PVBRWXYN4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZGZND5ZCH147PVBRWXYN4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg' from source '762b610ef6a278348cf9238e6227a455abb26650'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZGZND5ZCH147PVBRWXYN4-story-add-ef-lifecycle-analyzer-fixtures-and-reg` as `f9e0791ffb4d`.

Open questions / Risiken
- The analyzer currently keys off direct ApplyDataVaultMetadata(...), UseModel(...), and AddDbContextPool<TContext>(...) source evidence, so overly ambitious fixtures could accidentally require unsupported inference instead of validating the documented high-confidence boundary.
- Metadata-first and model-first baselines are safe because DVault-owned UseDataVaultMetadata(...) isolation is already proven elsewhere; fixtures must preserve that distinction so they do not imply raw model or metadata parsing by the analyzer.
- The stale blocks relation removal is queued for replay on another ticket's owner branch and may remain visibly present until that replay completes, even though the intended contract has already been cleaned up.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8742`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `599cf2bf292147a59dc12525f683cb1f`
- completed-at-utc: `<redacted>-03T11:59:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZGZND5ZCH147PVBRWXYN4/runs/20260603T115925681Z-599cf2bf292147a59dc12525f683cb1f.json`