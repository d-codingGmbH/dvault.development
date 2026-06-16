[gicket-bot] PO-critic review contract

Summary
- Ticket contract is coherent and directly backed by repository evidence; approve for dev as a closure/ratification handoff, not fresh implementation.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket snapshot delivery contract has `## Open Questions` = `none`, so the PO gate is not blocked by unresolved contract questions.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-25` shows `AddDVaultSqlServer()` registering the SQL Server capability profile plus `SqlServerDataVaultSaveStrategy` and `SqlServerDataVaultReadStrategy` PIT/bridge services.
- `src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs:16-17,27-31,254-270` and `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs:65-84` show the bounded SQL Server save gate: provider must match SQL Server, the context must be clean, total operations must be at least 50, and satellite operations must be at most 500.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:742-768` exports SQL Server threshold facts for 50 total operations and 500 satellite operations, and `:<redacted>` asserts fallback causes for small batch, too many satellites, dirty context, multi-active satellite operations, and provider-name mismatch.
- `tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs:168-232` covers ordered bulk save, staged-bulk replay idempotency, caller-transaction rollback, and cancellation; the same file also contains representative hub/link/satellite save smoke tests earlier in the file.
- `benchmark-summary.md:12,66-80` preserves SQL Server skipped-placeholder benchmark rows with `transfer=SqlBulkCopy`, `nativeBulkBoundary=50-plus-operations`, and PIT/bridge guidance rows while explicitly keeping latest-satellite as `selectedStrategy=<none>`; `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:441-459,691-700` verifies those SQL Server benchmark-detail tokens and skip reasons.
- `docs/plans/provider-optimization-gap-matrix.md:12-13,52` keeps SQL Server latest-satellite as a future capability gap and treats provider-native bulk/PIT/bridge timing as evidence gaps when `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset, matching the ticket's scope-out and follow-up boundaries.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- This review assumes the prompt snapshot reflects the latest persisted ticket description and `Recent comments: <none>` state because no callable `gicket-read-ticket` or `gicket-read-ticket-comments` tool was exposed in this Codex runtime.
- This approval assumes downstream roles will follow the delivery-contract clarification that this ticket ratifies already-landed SQL Server bulk-save work instead of reopening implementation design.

AC / test suggestions
- If PO later opens the follow-up evidence ticket mentioned in the contract, cite the exact benchmark row identity `scenario=provider-native-bulk-ingestion; provider=SQL Server external provider; baseline=dvault-adddvaultsqlserver-optimized` so timing collection stays separate from this closure-oriented ticket.

Implementation watchouts
- Treat the next handoff as closure/ratification work; the current branch already matches scratch ref `6e7f65112a5f11c3e1572b5e2193f28a6b05231c` with no additional repository delta.
- Do not let later comments or closure text overclaim completed SQL Server timing evidence or a SQL Server latest-satellite optimization; the checked-in benchmark row is still `skipped` and the gap matrix keeps latest-satellite as follow-up scope.

Non-blocking notes
- The contract/title mismatch is handled by the authoritative delivery-contract clarification, so it is a workflow watchout rather than a PO blocker.
- No split is needed for this ticket itself because the repository already contains the bounded code, tests, and benchmark-contract surfaces the contract cites.

Split recommendations
- No split recommended; later SQL Server latest-satellite work and provider-configured timing collection already have explicit follow-up lanes in `docs/plans/provider-optimization-gap-matrix.md`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment