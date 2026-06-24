[gicket-bot] Run report (outcome: test-workflow-awaiting-integrator)

Summary
- Tester workflow verified branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi' for ticket '06FF438KMPKSBT6KXZ5DBY85QC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF438KMPKSBT6KXZ5DBY85QC`.
- Optimistic claim succeeded (`expectedRevision=06FFJF9XE6TPKE07V4DJNKJ4RW`, `currentRevision=06FFJJ3ZB58M8BEQ0QC22BRJ0M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Selected verification source branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi' and commit '909d0259bc33' (verification-source contract).
- Prepared interactive tester scratch worktree for target branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi' from source '909d0259bc33'.
- Interactive tester tool loop completed review for branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi'.
- Evidence: git log --oneline --max-count=8 shows the functional documentation change landed at commit 909d0259bc33; git diff --name-only 909d0259bc33...HEAD -- docs returns no output, so the verified docs content at branch HEAD matches the implementation handoff.
- Evidence: git diff --name-only develop...909d0259bc33 -- docs lists only docs/architecture/dvault-v1-pit-bridge-boundary.md, docs/plans/performance-evidence-benchmark-artifact-contract.md, and docs/plans/provider-optimization-evidence-matrix.md.
- Evidence: docs/plans/provider-optimization-evidence-matrix.md adds PIT full-rebuild maintenance usage rules, manifest-field rules for workloadShape=pit-full-rebuild-maintenance and readShape=null, a dedicated PIT Full-Rebuild Maintenance Row Contract section, and a maintenance...
- Evidence: That matrix section defines exactly three maintenance lanes: provider-neutral comparator, PostgreSQL PIT full rebuild, and SQL Server PIT full rebuild.
- Evidence: The same matrix requires benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json under the sibling benchmark ticket's preserved artifact label, plus run context, before a completed maintenance timing claim is valid.
- Evidence: docs/plans/performance-evidence-benchmark-artifact-contract.md now says maintenance rows must use scenario pit-full-rebuild-maintenance, maintenanceScope=FullRebuild, bounded fallback causes, run context, and the supporting artifact triplet, and it maps maintenance f...
- 46 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- No tester findings were reported.

Next steps
- Proceed to integrator; no tester rework is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8663`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `test`
- run-id: `6561731b41454b80b4686578a6f39bba`
- completed-at-utc: `<redacted>-24T11:05:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF438KMPKSBT6KXZ5DBY85QC/runs/20260624T110507947Z-6561731b41454b80b4686578a6f39bba.json`