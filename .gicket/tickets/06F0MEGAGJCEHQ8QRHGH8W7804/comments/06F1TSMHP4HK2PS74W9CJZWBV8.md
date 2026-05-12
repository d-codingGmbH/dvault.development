[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F0MEGAGJCEHQ8QRHGH8W7804'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEGAGJCEHQ8QRHGH8W7804`.
- Optimistic claim succeeded (`expectedRevision=06F1TPPSTQCQ55YRNHED0965H0`, `currentRevision=06F1TPY99XZMZV8E9KHD9V1F5W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow' from source '2157ec4745dbe334c24bbdcc92289563c7a019c1'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow` as `f7e194f790ad`.

Open questions / Risiken
- Blocking finding: The delivery contract tells developers to make deferred model-first import/export tooling a current limitation, but current branch source and public API snapshots show implemented public import, export, projection, and drift surfaces. That contradiction can l...
- Blocking finding: The contract relies on v0.6.0 release notes as a limitation baseline without clearly separating historical v0.6.0 guidance from the active v0.7.0 release scope and already-done related tickets.
- Required PO action: Revise the contract to distinguish historical v0.6.0 limitations from current v0.7.0 branch capabilities.
- Required PO action: State whether the docs must name the existing public APIs DataVaultModelArtifactImporter.ImportJson, DataVaultModelArtifactExporter.ExportJson, DataVaultModelDriftReporter.Compare, and UseDataVaultMetadata(DataVaultModelImportResult), or deliberately keep t...
- Required PO action: Replace the blanket deferred import/export tooling limitation with precise remaining limitations such as no CLI commands, no CI gates, no direct YAML ingestion, no live database drift introspection, and no public raw Code-First-to-registry export bridge.
- Risky assumption: Assuming README.md and docs/releases/v0.6.0.md are the current capability source is risky because the active ticket release is v0.7.0 and source now contains public model-first APIs.
- Split recommendation: No split recommended; the issue is contract refinement, not scope size.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9322`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d5f88c20799b4764a0c961df5536dac1`
- completed-at-utc: `<redacted>-12T18:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/runs/20260512T181520953Z-d5f88c20799b4764a0c961df5536dac1.json`