[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FE4QRMXVGJVA65ZR5MZ817K8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QRMXVGJVA65ZR5MZ817K8`.
- Optimistic claim succeeded (`expectedRevision=06FEEN4ES24YDJ9D8PDCB5NQG0`, `currentRevision=06FEENBENCJ4EJD6WZNP6GDF04`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0' from source 'd93dc7652eb3085b56a02af63e5a8b85391791f8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FE4QRMXVGJVA65ZR5MZ817K8-task-update-provider-performance-matrices-and-v0` as `4c306c066b43`.

Open questions / Risiken
- Risky assumption: Developers will continue treating docs/plans/provider-optimization-evidence-matrix.md as the only promotable evidence surface and will not cite gap-matrix rows or root skipped rows as completed timing.
- Risky assumption: Any future SQL Server latest-satellite promotion will go through a dedicated evidence ticket rather than reusing the incidental row in the 2026-06-20 bulk-threshold bundle.
- Risky assumption: The scope remains doc-only as described in description.md:44-50; if the work expands into benchmark reruns or provider capability changes, the ticket needs re-refinement.
- Split recommendation: No split recommended; the remaining PostgreSQL/SQL Server/Oracle latest-satellite and PostgreSQL/MySQL/Oracle bulk follow-up work is already tracked as explicit gap-matrix rows and follow-up questions.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9027`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7b4f6af3a2004d37a37a97e7bd0198ae`
- completed-at-utc: `<redacted>-20T23:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QRMXVGJVA65ZR5MZ817K8/runs/20260620T232724932Z-7b4f6af3a2004d37a37a97e7bd0198ae.json`