[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGJ28KVSZAAFRA40D94128'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGJ28KVSZAAFRA40D94128`.
- Optimistic claim succeeded (`expectedRevision=06F2PNJTPVZ9G8DGP23VQK8K4M`, `currentRevision=06F2W4TPEVMPB7WXKKDZJ95X6G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGJ28KVSZAAFRA40D94128': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGJ28KVSZAAFRA40D94128': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGJ28KVSZAAFRA40D94128-task-document-analyzer-configuration-and-suppres' from source '7fac033fbdf0fa7801d64a31eeb029018bd46e33'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGJ28KVSZAAFRA40D94128-task-document-analyzer-configuration-and-suppres` as `250c8a5402ca`.

Open questions / Risiken
- If documentation names diagnostics or suppression paths that are not actually present in `CodeFirstDiagnosticCatalog`, consumer guidance will diverge from shipped analyzer behavior.
- If this ticket expands into a repo-wide `v0.12.0` documentation or version sweep, it will overlap the separate release-closure ticket and increase merge churn without clarifying analyzer usage.
- If suppression guidance is framed too broadly, consumers may silence analyzer coverage that should remain enabled by default instead of treating suppression as an intentional exception path.
- Split recommendation: No additional split is recommended; the current ticket is already a bounded documentation slice under story `06F2PGHQ2GATEM13M5QK1MSX1G`.
- Split recommendation: Keep implementation of new analyzer rules in `06F2PGHWEWYJZSRQ9QPT4NJ0QM`, broader `v0.12.0` documentation and release-note wrap-up in `06F2PGJYY6S97B4Z8044D34K5C`, and later code-fix ergonomics in `06F2PGJBRXFCP038CN6XVAYSZM`.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9575`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `bd84a7c1eba84c2788b4aea5837ce096`
- completed-at-utc: `<redacted>-16T00:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGJ28KVSZAAFRA40D94128/runs/20260516T000838089Z-bd84a7c1eba84c2788b4aea5837ce096.json`