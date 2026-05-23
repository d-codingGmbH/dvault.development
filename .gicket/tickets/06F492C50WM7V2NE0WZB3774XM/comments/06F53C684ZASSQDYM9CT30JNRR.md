[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492C50WM7V2NE0WZB3774XM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492C50WM7V2NE0WZB3774XM`.
- Optimistic claim succeeded (`expectedRevision=06F51ZC265RSM12ZJMH1E13SEC`, `currentRevision=06F53959J5R4F2WBT14NW5C7E0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492C50WM7V2NE0WZB3774XM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492C50WM7V2NE0WZB3774XM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an' from source '260385051596f4f0e6f34426ddff7b405edc51a9'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F492C50WM7V2NE0WZB3774XM-story-add-query-shape-performance-diagnostics-an` as `09137f07cc3b`.

Open questions / Risiken
- `DataVaultDiagnosticsResult.ReadShape` and the public read-shape records are already public snapshot-backed API, so any non-additive change will break consumers and the public API snapshot.
- PIT and provider-neutral fallback paths can perform materially separate steps; collapsing them into one synthetic join count or one synthetic index hint will mislead consumers.
- Provider caveats that drift from capability profiles, provider behavior profiles, or read-strategy fallback facts will over-promise provider behavior and misdirect tuning work.
- Support-bundle JSON is redaction-safe by design; leaking raw request keys, raw as-of values, or SQL text through new performance fields would violate the existing support contract.
- Split recommendation: No split recommended; current branch evidence supports one additive extension ticket on the existing `ReadShape` surface.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8383`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `49859d19e1904bc9a2dcbd2fe7425451`
- completed-at-utc: `<redacted>-22T21:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492C50WM7V2NE0WZB3774XM/runs/20260522T215640606Z-49859d19e1904bc9a2dcbd2fe7425451.json`