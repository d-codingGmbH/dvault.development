[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZV18BQ0GN3CE4G02ATVA0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZV18BQ0GN3CE4G02ATVA0`.
- Optimistic claim succeeded (`expectedRevision=06F8M02YPJSK6RBJ79MMBFM6K0`, `currentRevision=06FA23YASJKT55QGRPX2FAB0TM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZV18BQ0GN3CE4G02ATVA0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZV18BQ0GN3CE4G02ATVA0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo' from source 'ed72de4558604bfa754f5e5608c6ded31de68fc4'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZV18BQ0GN3CE4G02ATVA0-story-add-dry-run-artifact-manifest-prototype-fo` as `88ee539afa10`.

Open questions / Risiken
- The SQL Server benchmark rows exist in the checked-in artifact triplet but are currently skipped because the external provider is not configured locally, so this ticket must not be treated as evidence completion or production-ready artifact approval.
- If implementation starts inferring provider/workload evidence when request-bound diagnostics are absent, the manifest could create unreviewed provider-specific claims.
- If developers widen this slice into automatic invocation, deployment, or migration synchronization, they will violate the parent contract already marked done in `06F8KZTNG44XDPMVTVCV4WJSHG`.
- A metadata-only dry-run manifest can be misread as a deployable artifact unless the explicit dry-run indicator and consumer-owned operational boundary stay visible.
- Split recommendation: No new split is justified now; the existing epic/parent/evidence/prototype/documentation separation is sufficient for this refinement pass.
- Split recommendation: If later work wants provider-matrix coverage, deployable sidecar SQL payload emission, runtime invocation helpers, or provider-specific validators, create separate follow-up tickets instead of widening this first prototype.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9493`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `6f80e0e5961a45bab8b845ee8061f510`
- completed-at-utc: `<redacted>-07T08:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZV18BQ0GN3CE4G02ATVA0/runs/20260607T080627812Z-6f80e0e5961a45bab8b845ee8061f510.json`