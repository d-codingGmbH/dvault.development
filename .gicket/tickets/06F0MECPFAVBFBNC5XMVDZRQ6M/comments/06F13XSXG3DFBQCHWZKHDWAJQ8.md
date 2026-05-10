[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p' and commit '6438c5bbc042' for ticket '06F0MECPFAVBFBNC5XMVDZRQ6M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MECPFAVBFBNC5XMVDZRQ6M`.
- Optimistic claim succeeded (`expectedRevision=06F13HZV09CT61AKJ8STT6D7XM`, `currentRevision=06F13KQE3D1H8SBWRYEB4KCDM8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive developer scratch worktree for target branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p' from source 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p'.
- Triggered developer repair attempt 1/3 after isolated workspace test failure.
- Triggered developer repair attempt 2/3 after isolated workspace test failure.
- Planned implementation step: Reproduced the reported unit failure against the built unit test assembly and confirmed it was the core public API snapshot comparing TProjection against compiled TProjection? nullability metadata.
- Planned implementation step: Updated the core DCoding.Data.DVault public API approved snapshot for both explicit-metadata and registry-backed typed ReadLatestSatelliteAsync overloads.
- Planned implementation step: Verified the filtered snapshot test, the built unit test assembly, the built integration test assembly, and the repository format check within the sandbox constraints.
- Resolved branch route (fallback): base 'develop', work 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p'.
- 10 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: Local solution build/test commands that require restore are blocked in this sandbox by denied nuget.org access and a missing restored Microsoft.EntityFrameworkCore.Analyzers package; built test executables were used for targeted verification instead.

Next steps
- Push branch 'ticket/06F0MECPFAVBFBNC5XMVDZRQ6M-task-add-typed-latest-and-as-of-satellite-read-p' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9478`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `28b82e7432b64addab992df3c95ff238`
- completed-at-utc: `<redacted>-10T12:57:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MECPFAVBFBNC5XMVDZRQ6M/runs/20260510T125755694Z-28b82e7432b64addab992df3c95ff238.json`