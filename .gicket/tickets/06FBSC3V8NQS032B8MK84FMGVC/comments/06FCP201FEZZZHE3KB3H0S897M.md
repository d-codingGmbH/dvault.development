[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The ticket closes its open-question gate, stays bounded to a v1 provider-evidence manifest contract, and points to live repository sources that already define the row families, closed vocabularies, and deterministic JSON precedent.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06FBSC3V8NQS032B8MK84FMGVC says ## Open Questions is none, PO handoff is ready_for_po_critic, and the scope is limited to one v1 provider-evidence manifest contract rather than an exporter or runtime lane.
- At /mnt/c/Projects/DVault, git rev-parse --abbrev-ref HEAD returned ticket/06FBSC3V8NQS032B8MK84FMGVC-task-add-provider-evidence-manifest-shape.
- At /mnt/c/Projects/DVault, git diff --name-only bba2049331e7b5cbad987d674ee473a9914952ec...HEAD returned no paths, so the branch is still at pre-development PO-critic review rather than presenting implementation evidence.
- docs/plans/performance-evidence-benchmark-artifact-contract.md already defines the benchmark row contract and skipped-row rules, including scenario, provider, baseline, strategy family, dataset size, change ratio, execution status, skip reason, iterations, execution detail, and persisted outcome plus deterministic skipped-row handling.
- docs/plans/provider-optimization-evidence-matrix.md already defines the evidence postures completed-timing, skipped-placeholder, diagnostics-only, smoke-only, and storage-footprint, and cites the fallback enum files as authoritative vocabulary sources.
- benchmark-summary.md and benchmark-summary.json on the current branch contain completed SQLite provider-evidence rows plus skipped PostgreSQL, SQL Server, MySQL, and Oracle save and read guidance rows that already carry planned-path facts such as selectedStrategy, plannedReadStrategy, providerSpecificReadStrategy, readShape, transfer, stagedBulkBoundary, and cleanupBoundary.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs hard-codes the expected skipped provider read and save guidance rows and asserts detail tokens such as readShape=LatestSatellite|PitAsOf|Bridge, plannedReadStrategy=..., selectedStrategy=..., transfer=COPY, transfer=SqlBulkCopy, and stagedOracleBulk=not-selected-no-measured-win.
- src/DCoding.Data.DVault/DataVaultReadShapeKind.cs, src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs, src/DCoding.Data.DVault/DataVaultSaveStrategyFallbackCauseKind.cs, and src/DCoding.Data.DVault/DataVaultChunkedSaveStateFallbackCauseKind.cs provide the closed repository vocabulary named by the ticket.
- src/DCoding.Data.DVault/DataVaultSqlArtifactManifestExporter.cs provides a directly observed deterministic JSON precedent with schemaVersion, camelCase serialization, ordered properties, and no runtime deployment or machine-specific payload behavior.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add one explicit docs-only manifest example that shows how diagnostics-only or smoke-only rows populate or omit workload, readShape, and result fields.
- Make the null-versus-omission rules concrete across one completed SQLite row, one skipped external-provider row, and one docs-only row so downstream tickets do not invent placeholders differently.

Risky assumptions
- The ticket assumes one manifest identity can cover both benchmark-backed rows and docs-owned evidence-matrix rows without needing extra identity fields beyond the bounded provider-evidence family.
- The ticket assumes current deterministic executionDetail fragments are sufficient interim source material for planned-path mapping until a shared mapper replaces ad hoc prose parsing.

AC / test suggestions
- Add golden examples or tests that project one completed SQLite row, one skipped PostgreSQL or SQL Server or MySQL or Oracle guidance row, and one docs-only posture row into the same manifest shape.
- Add assertions that the manifest preserves existing vocabulary spellings exactly: LatestSatellite, PitAsOf, Bridge, completed-timing, skipped-placeholder, diagnostics-only, smoke-only, and the current fallback enum member names.
- Add mapping tests that prove selectedStrategy, plannedReadStrategy, fallback causes, and bounded path fields come from direct repository sources rather than free-form markdown prose.

Implementation watchouts
- Keep the manifest contract documentation-only or mapping-focused for this ticket; the follow-up question already reserves any dedicated generated artifact or export lane for later work.
- Do not require downstream docs or tests to scrape semicolon-delimited executionDetail text ad hoc; one shared mapping surface should own that translation.
- Follow the existing deterministic JSON precedent from DataVaultSqlArtifactManifestExporter: schemaVersion, camelCase, deterministic ordering, no timestamps, no secrets, and no machine-specific paths.

Non-blocking notes
- The live repository already extends the prompt seed with skipped external-provider read guidance rows in the root benchmark triplet, so implementation should treat the checked-in branch files as the source of truth.
- The empty diff from the scratch ref is consistent with a pre-development quality gate and is not a readiness problem by itself.

Split recommendations
- No split recommended. Defining the manifest shape, pinning the source vocabularies, and proving the row-to-manifest mapping still fits one bounded developer ticket.
- If the team later wants a generated or checked-in provider-evidence manifest artifact, track that as the separate follow-up already called out in the ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment