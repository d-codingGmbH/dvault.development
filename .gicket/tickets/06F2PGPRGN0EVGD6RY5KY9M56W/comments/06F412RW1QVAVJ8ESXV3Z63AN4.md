[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGPRGN0EVGD6RY5KY9M56W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPRGN0EVGD6RY5KY9M56W`.
- Optimistic claim succeeded (`expectedRevision=06F410720CTDTJQYZT7YWPW10G`, `currentRevision=06F41091YKQYZ33Q0YTAQ5G2NW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGPRGN0EVGD6RY5KY9M56W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGPRGN0EVGD6RY5KY9M56W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt' from source 'eb4406e5dd6ec4485e4776c3ec063f75a4156340'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt` as `f2efd9beb279`.

Open questions / Risiken
- Because `IDataVaultProviderReadStrategy` is currently latest-satellite-specific and bridge reads bypass `DefaultDataVaultReadService`, extending provider-aware optimization may require additive dispatch abstractions or public API snapshot churn.
- PIT snapshot selection and bridge depth and ordering rules are correctness-sensitive; provider-specific SQL can introduce subtle regressions even when happy-path results look similar.
- Repository docs and benchmark guidance currently describe PIT and bridge reads as provider-neutral baselines; shipping optimization without matching evidence would create documentation and release-note drift.
- SQLite is the only required local proof lane for this ticket, so non-SQLite expansion may surface new dispatch or SQL-shape constraints later.
- Split recommendation: No split is required from the current evidence; the story can stay whole if the work remains bounded to SQLite proof plus provider-neutral fallback safety.
- Split recommendation: If implementation grows materially, split first into a shared provider-aware read-dispatch slice and two execution slices: PIT optimization and bridge optimization.
- Split recommendation: If same-release proof is needed for non-SQLite providers, track each external provider package in its own child ticket instead of expanding this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8388`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `45653b931e9f405dbb09374e6c08cfab`
- completed-at-utc: `<redacted>-19T14:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/runs/20260519T140158832Z-45653b931e9f405dbb09374e6c08cfab.json`