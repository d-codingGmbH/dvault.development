[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git branch --show-current returned ticket/06F0MECWYMPQ4R0KWV1R637RT0-story-add-developer-diagnostics-and-starter-exam and git rev-parse HEAD returned 38982be92753546a1b0a51c8768268e40b9fdd67, matching the scratch source ref.
- git log --oneline -n 12 shows develop already contains AUTO-INTEGRATION squashes for child tickets 06F0MEDJC732GDD77H60R259P0, 06F0MEDBFZ25YA1M7RJ71Z7ZCM, and 06F0MED4P7HMBDZVMPWQZ5A7PC before the parent PO/PO-critic orchestration commits.
- .gicket/relations/T0/PC/06F0MECWYMPQ4R0KWV1R637RT0--06F0MED4P7HMBDZVMPWQZ5A7PC--parentOf.json, .gicket/relations/T0/CM/06F0MECWYMPQ4R0KWV1R637RT0--06F0MEDBFZ25YA1M7RJ71Z7ZCM--parentOf.json, and .gicket/relations/T0/P0/06F0MECWYMPQ4R0KWV1R637RT0--06F0MEDJC732GDD77H60R259P0--parentOf.json all exist and point from the parent story to the named child tickets.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs defines IDataVaultDiagnosticsService plus structured diagnostics records for validation, explain entities/properties/keys/indexes/constraints, provider behavior/profile, and save-strategy status/candidates/fallback causes.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers IDataVaultDiagnosticsService to DefaultDataVaultDiagnosticsService inside AddDVault().
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs covers NotEvaluated validation-only diagnostics, registry and Code-First result-shape parity, built-in provider profiles and load timestamp storage variants, and provider save-strategy fallback causes; tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs covers DbContext NotEvaluated behavior, provider strategy selection, ordered bulk requests, registry request resolution, dirty-context fallback, and candidate ordering.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt includes IDataVaultDiagnosticsService and DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated in the public API snapshot.
- examples/README.md documents dotnet build DVault.slnx --nologo plus exact dotnet run commands for SQLite and PostgreSQL quickstarts, and explains the shared registry-backed metadata path.
- examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs uses a temporary SQLite database path, AddDVault(options => options.UseMetadataModel(QuickstartHistoryFlow.MetadataModel)), AddDVaultSqlite(), and UseDataVaultMetadata().
- examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs reads only DVAULT_TEST_POSTGRES_CONNECTION_STRING, prints the documented skip message and returns when it is missing, then uses AddDVaultPostgres() and the same UseDataVaultMetadata() registry-backed path when configured.
- examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs creates schema with EnsureCreatedAsync, saves hub plus two customer profile versions through IDataVaultSaveService/DataVaultRegistrySaveRequest, and prints latest and as-of typed read projections via IDataVaultReadService.ReadLatestSatelliteAsync.
- README.md documents Code-First as the v0.6.0 recommended path, explicit IDataVaultSaveService saves, typed latest/as-of reads, raw ReadLatestSatelliteRowsAsync as the lower-level escape hatch, metadata-first/registry-backed compatibility, diagnostics behavior, and deferred limitations.
- docs/releases/v0.6.0.md covers the six package names, highlights for Code-First, typed reads, diagnostics, and quickstarts, compatibility notes including no public Code-First-to-registry bridge, known limitations, and the release validation boundary.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Final NuGet publication and tag-time audited package evidence remain outside this story by contract and are captured as release-operator follow-up, not a blocker for this parent aggregation handoff.

AC / test suggestions
- Keep the existing diagnostics tests as the required developer guardrails: NotEvaluated without a save request, provider strategy selection, provider-neutral fallback causes, provider profile/load timestamp coverage, and public API snapshot coverage.
- Keep the quickstart validation boundary explicit in downstream testing: SQLite should remain runnable without external infrastructure, and PostgreSQL should continue to pass the missing-env-var skip path without opening a connection.

Implementation watchouts
- Do not re-open a public Code-First-to-registry bridge or CLI diagnostics command under this parent story; both are explicitly scope-out/future work in the persisted contract.
- Provider capability auto-registration and provider-specific save-strategy registration are separate surfaces; documentation and diagnostics changes should keep that distinction visible.
- If provider strategy gates change later, update both fallback-cause diagnostics and shared tests together so diagnostics do not drift from runtime dispatch behavior.

Non-blocking notes
- The working tree status showed only local metadata modifications under .gicket-bot/.gitignore, .gicket/.gitignore, .gicket/project.json, and .gicket/types.json; no reviewed product contract path was dirty.
- The release notes intentionally retain a pre-publication validation note requiring the release operator to replace it with final audited pass/fail evidence before NuGet publication.

Split recommendations
- No new split is recommended. The parent already links to done child tickets for diagnostics, quickstart examples, and README/release docs; future CLI diagnostics, Code-First-to-registry bridging, extra provider quickstarts, and post-tag publication work should stay as separate future tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment