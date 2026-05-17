[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGKJBG7NGNVBN0ZDSBE6B8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKJBG7NGNVBN0ZDSBE6B8`.
- Optimistic claim succeeded (`expectedRevision=06F3DYB7XGNKKGE6XXQN6GPXVG`, `currentRevision=06F3DYHECQ3CX01HTPR5RYDYMC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project' from source '1aeb7256d3f6ab82a921bcb2fd60dae524668f3a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGKJBG7NGNVBN0ZDSBE6B8-task-test-link-parent-satellite-metadata-project` as `a67c3da0b912`.

Open questions / Risiken
- Blocking finding: The acceptance criteria describe repository state that already exists in the current tree, but the ticket does not name the uncovered regression, missing assertion, or missing suite delta that still requires developer work.
- Blocking finding: The contract points to code-first test patterns without source-backed evidence that link-parent satellites are declarable through the code-first surface, so the intended developer target is ambiguous.
- Required PO action: Decide whether this ticket should be closed as no-work-required/already covered, or rewritten around a concrete uncovered gap that is not already satisfied by `ApplyDataVaultMetadataTranslatesLinkParentSatellites` and the existing snapshot coverage.
- Required PO action: If work is still intended, update the acceptance criteria to name the exact missing contract and the exact target suite instead of restating the already-present baseline.
- Required PO action: If code-first coverage is intended, provide source-backed justification for that surface; otherwise narrow the ticket to the metadata-first / EF projection path that is actually supported.
- Risky assumption: That link-parent satellite projection is currently untested, despite direct existing unit and snapshot coverage.
- Risky assumption: That code-first suites are an appropriate target for this ticket, despite no visible code-first link-parent satellite builder API.
- Risky assumption: That a developer can infer the remaining scope without a human clarification comment or a delta-oriented acceptance criterion.
- Split recommendation: No split is needed if PO closes the ticket as already covered. If PO identifies more than one real uncovered layer, split the minimal missing test surface from any broader diagnostics/parity follow-up.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8696`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `a2aa2498ee214020a5af370495c25f9e`
- completed-at-utc: `<redacted>-17T17:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKJBG7NGNVBN0ZDSBE6B8/runs/20260517T173223745Z-a2aa2498ee214020a5af370495c25f9e.json`