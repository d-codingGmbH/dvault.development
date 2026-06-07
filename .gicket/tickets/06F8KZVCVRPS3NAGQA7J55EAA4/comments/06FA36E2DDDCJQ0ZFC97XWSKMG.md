[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari' for ticket '06F8KZVCVRPS3NAGQA7J55EAA4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZVCVRPS3NAGQA7J55EAA4`.
- Optimistic claim succeeded (`expectedRevision=06FA34JEEJ5DJJNV1A4F70ZZ2W`, `currentRevision=06FA34SHNWXHTPX14Z0VXNRTD4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari' and commit 'd1f010f1fd99' (developer-delivery-outcome contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari' from source 'd1f010f1fd99'.
- Interactive tester tool loop completed review for branch 'ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari'.
- Evidence: `git -C /mnt/c/Projects/DVault rev-parse --abbrev-ref HEAD` returned `ticket/06F8KZVCVRPS3NAGQA7J55EAA4-task-define-artifact-benchmark-and-semantic-pari`.
- Evidence: `git -C /mnt/c/Projects/DVault rev-parse --short HEAD` returned `dcab491f5`.
- Evidence: `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD -- docs src tests benchmark-summary.md benchmark-summary.csv benchmark-summary.json` returned no paths, and the same diff against `d1f010f1fd99...HEAD` also returned no paths.
- Evidence: `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD -- .gicket/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/description.md` returned `.gicket/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/description.md`, and the diff shows the legacy one-line draft replaced by the full delive...
- Evidence: `docs/plans/performance-evidence-benchmark-artifact-contract.md:16-29` defines the shared benchmark triplet and comparable before/after inputs; `:77-99` defines the SQLite baseline scenarios and skipped optional-provider visibility rules.
- Evidence: `docs/performance-profiles.md:11-13,25,33-35,85,278,289,297` points to the root triplet, preserves skipped optional-provider rows, requires exact-request diagnostics, and requires semantic-parity evidence before provider-specific claims.
- 61 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Handoff to integrator.
- Keep future provider-specific implementation tickets bound to one exact provider/workload pair with matching diagnostics and comparable benchmark triplet evidence.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8589`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `0a3ccf46f583446aa29cfa9eddd9a5c0`
- completed-at-utc: `<redacted>-07T10:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/runs/20260607T102109092Z-0a3ccf46f583446aa29cfa9eddd9a5c0.json`