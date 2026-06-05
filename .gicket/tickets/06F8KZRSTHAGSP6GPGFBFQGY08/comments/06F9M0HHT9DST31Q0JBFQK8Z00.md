[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F8KZRSTHAGSP6GPGFBFQGY08'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZRSTHAGSP6GPGFBFQGY08`.
- Optimistic claim succeeded (`expectedRevision=06F9KYW5WJ3K1KX01VZ0AWQHWW`, `currentRevision=06F9KZ37CHFH2S3HWH0CGKKB28`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZRSTHAGSP6GPGFBFQGY08-task-add-bounded-performance-decision-tree-docum' from source '130fe2afc7444b7ad87a8e4420f8cb806dc56361'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F8KZRSTHAGSP6GPGFBFQGY08-task-add-bounded-performance-decision-tree-docum` as `0b04fcc3eb35`.

Open questions / Risiken
- Blocking finding: The ticket no longer describes a clear repository change: the examples, fallback branches, and rerun/stop-condition guidance it asks a developer to add are already present in `docs/performance-profiles.md`, so a dev handoff would duplicate landed documentatio...
- Blocking finding: The ticket's Definition of Done requires a repository diff that updates `docs/performance-profiles.md` outside `.gicket`, but the current branch diff against `develop` contains only ticket-metadata files under `.gicket`, which is consistent with the scope alr...
- Required PO action: Reconcile ticket `06F8KZRSTHAGSP6GPGFBFQGY08` against current `develop` and either close it as already satisfied/no-work-required or rewrite it around a specific remaining documentation gap that is not already present in `docs/performance-profiles.md`.
- Required PO action: If the ticket stays open, replace the current broad acceptance criteria with delta-based criteria that name the exact missing section(s), example(s), or wording still absent on `develop`, and remove expectations that are already landed in `docs/performance-...
- Risky assumption: This review assumes `develop` is the correct pre-development baseline for developer handoff; all inspected branch-history evidence points to that baseline.
- Risky assumption: This review assumes the already-landed `docs/performance-profiles.md` sections satisfy the currently written contract; if PO sees a qualitative gap, that gap is not expressed concretely enough in the ticket or branch diff to hand to a developer safely.
- Split recommendation: No split recommended. Reconcile or close this ticket first; only create a new follow-up if PO can name a specific residual documentation gap against the current `develop` baseline.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9012`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `e16aa1992c7a4976a9bf73c6e0ffd33e`
- completed-at-utc: `<redacted>-05T22:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZRSTHAGSP6GPGFBFQGY08/runs/20260605T225815623Z-e16aa1992c7a4976a9bf73c6e0ffd33e.json`