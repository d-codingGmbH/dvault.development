[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F1XQ25KK4VY4MYJSDG9V4BZM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ25KK4VY4MYJSDG9V4BZM`.
- Optimistic claim succeeded (`expectedRevision=06F2HEKKEPC4M0BZWH2F9WWC28`, `currentRevision=06F2HFNBZZCR4PEGCHA5FFXNYW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample' from source 'b03576728d530872b40df343e21caa675943dd62'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F1XQ25KK4VY4MYJSDG9V4BZM-task-add-provider-container-fixture-sample` as `5b4a385e838b`.

Open questions / Risiken
- Risky assumption: The docker.io/postgres:18 image baseline is local release-document evidence; implementation should avoid implying DVault owns image lifecycle or container provisioning.
- Risky assumption: Podman and Docker networking can differ by host, so the sample must make connection-string overrides visible rather than assuming localhost always works.
- Split recommendation: No split recommended; the ticket is already scoped as the first PostgreSQL provider-container fixture sample while the wider provider matrix remains out of scope.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8920`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `593a94a9d8f84d9db524bcf1540eeec6`
- completed-at-utc: `<redacted>-14T23:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ25KK4VY4MYJSDG9V4BZM/runs/20260514T231140264Z-593a94a9d8f84d9db524bcf1540eeec6.json`