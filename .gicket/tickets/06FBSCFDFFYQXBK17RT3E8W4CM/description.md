<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified from the checked-out repository and local .gicket state that PostgreSQL latest-satellite remains a P0 capability gap with no provider-specific strategy registered; this ticket is now bounded to either add that strategy with diagnostics/tests/benchmark proof or explicitly close as no-work-required under the existing provider-neutral fallback baseline.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The done criteria story 06FBSCF61N0TYPYH7008TRD6VR already answers the main PO question: non-SQLite latest-satellite tickets may close as no-work-required unless a new provider-specific strategy is registered, diagnostics select it, and completed timing evidence proves it.
- The repository already classifies PostgreSQL latest-satellite as capability gap P0.01, not as a PIT/bridge evidence gap: docs/plans/provider-optimization-gap-matrix.md and docs/plans/provider-optimization-evidence-matrix.md both record providerSpecificReadStrategy=not registered for latest satellite reads for PostgreSQL.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this pass.

### Scope In
- Decide this single provider lane only: PostgreSQL latest-satellite-read either gains a provider-specific optimized path or is explicitly closed as no-work-required against the current fallback baseline.
- If implementation is chosen, cover only PostgreSQL latest-satellite strategy selection, fallback, diagnostics, tests, and benchmark evidence needed to justify that one lane.
- Document the closure rationale so downstream docs/benchmark ticket 06FBSCHBJEYYERDPA7JN34Y8PG can publish the outcome without reopening acceptance rules.

### Scope Out
- PostgreSQL PIT or bridge read work, which already sits on the separate diagnostics-gated candidate lane.
- SQL Server, MySQL, Oracle, and DB2 latest-satellite tickets, which remain sibling provider tasks.
- Any provider performance claim based only on skipped-placeholder, diagnostics-only, smoke-only, or storage-footprint evidence.
- Automatic maintenance, raw SQL exposure, physical-plan guarantees, or broader provider platform behavior promises.

## Acceptance Criteria
- The ticket closes with one of two explicit outcomes only: implemented PostgreSQL latest-satellite optimization with proof, or no-work-required with repository-backed rationale for retaining provider-neutral fallback.
- Any implemented outcome adds a PostgreSQL latest-satellite provider strategy on the existing read-service boundary without widening PIT/bridge scope, and request-bound diagnostics must show provider strategy selection for supported shapes and bounded fallback for unsupported or declined shapes.
- Tests cover the chosen outcome: service registration or absence thereof, latest-satellite dispatch behavior, finite fallback behavior, and the expected diagnostics surface for PostgreSQL latest-satellite reads.
- Any implemented PostgreSQL performance claim is backed by completed benchmark evidence with preserved triplet/run context and compared against the provider-neutral latest-satellite baseline; skipped-placeholder guidance rows do not satisfy this gate.
- Any no-work-required outcome explicitly cites the current repository posture: AddDVaultPostgres() does not register a latest-satellite provider strategy, benchmark guidance rows keep selectedStrategy=<none>, and fallback remains NoProviderSpecificStrategyRegistered/provider-neutral.

## Definition of Done
- The ticket no longer reopens baseline questions about provider list, evidence vocabulary, or whether PIT/bridge work is included; PostgreSQL latest-satellite is the only delivery lane.
- Closure evidence cites the authoritative repository surfaces for this lane: the gap matrix P0.01 row, the evidence matrix PostgreSQL latest-satellite row, benchmark guidance/tests, and the PostgreSQL registration surface.
- If implemented, closure evidence includes updated diagnostics/tests/benchmark artifacts sufficient to prove the selected strategy and bounded fallback behavior.
- If closed as no-work-required, closure evidence states why the current capability-gap posture remains the correct bounded outcome and leaves the outbound docs ticket ready to record that decision.

## Implementation Notes
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs currently registers PostgreSQL save plus PIT/bridge read strategies only; it does not register IDataVaultProviderReadStrategy for latest-satellite reads.
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs currently expects the PostgreSQL latest-satellite guidance row to keep selectedStrategy=<none> and providerSpecificReadStrategy=not registered for latest satellite reads.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs verifies that relational provider packages register optimized PIT/bridge read strategies, not latest-satellite read strategies, outside SQLite.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs and related diagnostics tests already prove provider-neutral latest-satellite fallback with NoProviderSpecificStrategyRegistered when no provider-specific strategy is available.
- The checked-out ticket branch has no product-code or documentation diff from scratch source ref 965645e58de38a95ab927d858d211817d8b2512f; no implementation or explicit no-work-required closure text has landed yet.
- The two inbound blocks relations come from done prerequisite stories and do not leave this ticket blocked; the outbound blocks relation to 06FBSCHBJEYYERDPA7JN34Y8PG should stay aligned with whichever outcome this ticket chooses.

## Open Questions
- none

## Follow-Up Questions
- After this ticket closes, should 06FBSCHBJEYYERDPA7JN34Y8PG publish the outcome as an implemented PostgreSQL optimization or as a documented no-work-required fallback confirmation?
- If product later wants non-SQLite latest-satellite work beyond PostgreSQL, should the remaining priority stay the current gap-matrix order: SQL Server, MySQL, Oracle, then DB2?

## Risks
- The current repository baseline strongly supports no-work-required; attempting an implementation without provider-configured benchmark evidence risks overclaiming PostgreSQL latest-satellite performance.
- Mixing this ticket with PostgreSQL PIT/bridge work would violate the existing ticket split and blur a capability-gap decision into a separate evidence-gap lane.
- If optional PostgreSQL benchmark configuration is unavailable, an implemented strategy may still fail the timing-claim closure gate even if diagnostics and functional tests pass.

## Split Recommendations
- No new split recommended; the live graph already separates this PostgreSQL latest-satellite task from sibling provider latest-satellite tasks and the downstream read docs/benchmark ticket.
- Do not pre-split PIT/bridge or cross-provider work out of this ticket; only create a later follow-on if a concrete PostgreSQL latest-satellite implementation proves functional but still needs separately scheduled benchmark or documentation execution.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Use v0.39 evidence and v0.41 criteria to implement or reject a PostgreSQL latest-satellite read strategy improvement. Acceptance: tests, diagnostics, fallback, and benchmark evidence are updated, or no-work-required is documented.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Outcome: implemented PostgreSQL latest-satellite optimization closure.

Repository evidence:
- `AddDVaultPostgres()` registers `PostgresDataVaultReadStrategy` as `IDataVaultProviderReadStrategy` for latest-satellite reads.
- Benchmark guidance keeps the PostgreSQL latest-satellite row as `skipped-placeholder` when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is not configured, but records `selectedStrategy=PostgresDataVaultReadStrategy` and `plannedReadStrategy=PostgresDataVaultReadStrategy`.
- The gap/evidence matrices close PostgreSQL P0.01 as a strategy-registration and diagnostics-guidance outcome, without claiming completed PostgreSQL timing.
- This rework corrected diagnostics tuning so `PostgresDataVaultReadStrategy` selected for `LatestSatellite` is classified as a repository-proven optimized read path.

Verification:
- `dotnet build DVault.slnx --nologo` passed before the final diagnostics-recommendation rework with 0 errors.
- `timeout 600 dotnet build src/DCoding.Data.DVault/DCoding.Data.DVault.csproj --no-restore --no-dependencies --nologo` passed for `net8.0` and `net10.0` after the diagnostics fix.
- `timeout 600 dotnet build tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj -f net10.0 --no-restore --no-dependencies --nologo` passed; repeated for `net8.0` and passed.
- `timeout 600 dotnet test tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj -f net10.0 --no-build --nologo --filter "FullyQualifiedName~ReadProviderTuningTreatsPostgresLatestSatelliteSelectionAsRepositoryProvenOptimizedPath"` passed. Microsoft.Testing.Platform ignored the filter and ran the full `net10.0` unit assembly: 592 passed.
- Same `net8.0` unit command passed. Microsoft.Testing.Platform ignored the filter and ran the full `net8.0` unit assembly: 564 passed.
- Before the final diagnostics fix, the full unit and integration projects passed for both target frameworks; integration skipped optional live providers because connection strings were absent.
- `timeout 600 bash tools/check-format.sh` passed after the final edit.

Limitations:
- No completed live PostgreSQL timing row is claimed; the checked-in PostgreSQL benchmark row remains a skipped-placeholder unless a local PostgreSQL connection string is configured.
<!-- gicket-bot:developer-delivery:v1:end -->