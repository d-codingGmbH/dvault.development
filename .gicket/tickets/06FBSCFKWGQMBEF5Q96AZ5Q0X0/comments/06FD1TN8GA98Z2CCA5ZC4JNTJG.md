[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the ticket is a specific, repo-grounded SQL Server latest-satellite capability-gap closure with no unresolved Open Questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket Delivery Contract sets Open Questions to none and narrows scope to SQL Server latest/current/as-of satellite reads, fallback behavior, tests, and evidence/document updates.
- src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs already defines IDataVaultProviderReadStrategy for provider-specific latest/as-of satellite reads behind IDataVaultReadService.
- src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs currently registers SqlServerDataVaultReadStrategy only as IDataVaultProviderPitReadStrategy and IDataVaultProviderBridgeReadStrategy; it does not register IDataVaultProviderReadStrategy.
- src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultReadStrategy.cs currently inherits DataVaultRelationalPitBridgeReadStrategy and exposes PIT/bridge gate methods, while src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs implements IDataVaultProviderReadStrategy plus PIT/bridge interfaces.
- src/DCoding.Data.DVault/DefaultDataVaultReadService.cs dispatches latest-satellite overrides from IReadOnlyList<IDataVaultProviderReadStrategy>, so current SQL Server DI cannot be selected for latest-satellite reads.
- benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json currently keep the SQL Server latest-satellite-read row as skipped with selectedStrategy=<none>, plannedReadStrategy=<none>, and providerSpecificReadStrategy=not registered for latest satellite reads.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs hard-codes the same SQL Server latest-satellite expectation, while its SQL Server PIT and bridge rows already expect plannedReadStrategy=SqlServerDataVaultReadStrategy.
- docs/architecture/dvault-v1-pit-bridge-boundary.md and docs/performance-profiles.md currently describe AddDVaultSqlServer() as a diagnostics-gated PIT/bridge path while SQLite remains the only optimized latest-satellite read provider.
- docs/plans/provider-optimization-gap-matrix.md marks SQL Server latest-satellite-read as capability gap P0.02, and docs/plans/provider-optimization-evidence-matrix.md records no SQL Server latest-satellite optimization claim.
- src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs already contains CreateSqlServerLatestSatelliteHashDiffCommandText and latest-hash-diff filtering helpers, matching the ticket's bounded reuse note.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add explicit parity coverage for empty parent-hash-key input and duplicate parent-hash-key requests so SQL Server latest-satellite dispatch matches the provider-neutral path.
- Add explicit parity coverage for as-of reads and same-timestamp ties, since the contract asks for current/as-of behavior but does not spell out tie examples.
- Add projection-path coverage alongside record-path coverage so ReadLatestSatelliteProjectionRowsAsync and row materialization stay aligned under SQL Server.
- Keep unsupported latest-satellite shapes explicit in tests: non-hub parents, multi-active driving keys, and provider mismatches should fall back deterministically.

Risky assumptions
- The existing SQL Server latest-hash-diff query helpers in SqlServerDataVaultSaveStrategy can be reused for read-path semantics without introducing behavioral drift from the provider-neutral latest-satellite pipeline.
- Existing read diagnostics and fallback vocabularies are sufficient for the new SQL Server latest-satellite path, so no additional public diagnostics surface is needed.
- If DVAULT_TEST_SQLSERVER_CONNECTION_STRING stays unset, skipped-placeholder benchmark artifacts with corrected planned/selected path tokens will be accepted as sufficient evidence for this ticket.

AC / test suggestions
- Add a registration assertion that AddDVaultSqlServer() contributes an IDataVaultProviderReadStrategy in addition to the existing PIT and bridge registrations.
- Update BenchmarkScenarioExecutionTests and the checked-in benchmark artifact triplet so the SQL Server latest-satellite row no longer expects selectedStrategy=<none> or providerSpecificReadStrategy=not registered for latest satellite reads.
- Add SQL Server parity coverage for latest current and as-of satellite reads against the provider-neutral path for supported hub-parent, non-multi-active satellite shapes.
- Add diagnostics/fallback assertions for provider mismatch, unsupported satellite parent, multi-active driving keys, and retained provider-neutral fallback when SQL Server latest-satellite gates decline.

Implementation watchouts
- Keep the strategy identity SqlServerDataVaultReadStrategy stable across DI registration, diagnostics, benchmark executionDetail tokens, and docs.
- Do not accidentally widen latest-satellite support claims for PostgreSQL, MySQL, Oracle, or DB2 while updating shared benchmark and evidence documents.
- Preserve deterministic provider-neutral fallback causes for unsupported latest-satellite shapes and unconfigured benchmark lanes.
- If the SQL Server benchmark lane remains opt-in and unconfigured, artifact updates must stay in skipped-placeholder posture and avoid new timing claims.

Non-blocking notes
- The contract's Follow-Up Questions are separate from Open Questions, and Open Questions is explicitly none.
- The ticket snapshot notes live relation/comment refresh was trust-policy blocked, but no blocker is visible in the supplied context.

Split recommendations
- No split recommended; the current contract is already bounded to one provider and to existing read-strategy, diagnostics, benchmark, and documentation surfaces.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment