[gicket-bot] Run report (outcome: dev-workflow-apply)

Summary
- Developer workflow prepared branch 'ticket/06FGX5QAZSAB0M0W8FW807GQQR-task-add-privacy-support-bundle-facts-for-alias' and commit '91f28958e302' for ticket '06FGX5QAZSAB0M0W8FW807GQQR'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5QAZSAB0M0W8FW807GQQR`.
- Optimistic claim succeeded (`expectedRevision=06FH1T6MXAPVY2038QC967HX2R`, `currentRevision=06FH2734NSR0DMRF1XMRX63F60`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded developer ticket mutation surface enabled: gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-relation.
- Prepared interactive developer scratch worktree for target branch 'ticket/06FGX5QAZSAB0M0W8FW807GQQR-task-add-privacy-support-bundle-facts-for-alias' from source 'ticket/06FGX5QAZSAB0M0W8FW807GQQR-task-add-privacy-support-bundle-facts-for-alias'.
- Planned implementation step: Added core privacy diagnostics records and the DataVaultDiagnosticsResult.Privacy surface for provider-native boundary, key-provider posture, alias coverage, and personal-data marker coverage facts.
- Planned implementation step: Added IDataVaultPrivacyAliasCoverageProvider in core and registered an optional privacy-package implementation that maps registered aliases and EF converter wiring without inverting package dependencies.
- Planned implementation step: Refactored personal-data coverage evaluation so existing warning/error semantics and new structured statuses are produced from the same diagnostics path.
- Planned implementation step: Added object-model and support-bundle JSON tests for proof missing, alias unregistered, unusable key-provider posture, proof unavailable, no observable converter wiring, converter-alias mismatch, and covered cases; updated the public API snapshot.
- Resolved branch route (fallback): base 'develop', work 'ticket/06FGX5QAZSAB0M0W8FW807GQQR-task-add-privacy-support-bundle-facts-for-alias'.
- Executed runtime-orchestration sync-first fetch/pull before workspace automation.
- Checked out existing branch 'ticket/06FGX5QAZSAB0M0W8FW807GQQR-task-add-privacy-support-bundle-facts-for-alias'.
- 26 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Risk: The solution build emits pre-existing warning noise, including sandbox read-only NuGet vulnerability-cache NU1900 warnings and existing analyzer/test warnings; no errors were present.
- Risk: The privacy facts are additive public API and additive support-bundle JSON under the existing schema version; consumers that reject unknown diagnostics properties should be checked downstream.

Next steps
- Push branch 'ticket/06FGX5QAZSAB0M0W8FW807GQQR-task-add-privacy-support-bundle-facts-for-alias' manually if remote collaboration is required.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9743`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `dev`
- run-id: `676ff7f04874442e9c70b31511ccd6c5`
- completed-at-utc: `<redacted>-29T03:06:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5QAZSAB0M0W8FW807GQQR/runs/20260629T030631963Z-676ff7f04874442e9c70b31511ccd6c5.json`