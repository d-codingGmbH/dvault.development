[gicket-bot] PO refinement contract

Summary
- Rebounded the ticket around source-backed diagnostics/export/drift/guardrail APIs, explicitly allowing creation of any missing command host/runner types; no child tickets, relation writes, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced the earlier inferred command-surface assumption with source-backed API evidence. The current branch visibly exposes `IDataVaultDiagnosticsService.Analyze(DbContext)`, `DataVaultModelArtifactExporter.ExportJson(...)`, `DataVaultModelDriftReporter.Compare(...)`, `DataVaultLiveSchemaReader.ReadAsync(...)`, `DataVaultLiveSchemaDriftReporter.Compare/CompareAsync(...)`, and `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`. The branch does not show a public command host/runner API, so this contract now treats any minimal host/runner types as new deliverables for this ticket rather than as pre-existing APIs.
- critic-item-2: `answered` - The contract no longer depends on an unseen existing public command API/type. Acceptance criteria and implementation notes now state that this task may introduce the minimal public host/runner abstractions it needs inside `DCoding.Data.DVault`, and that any newly public types must be covered by the existing core public API snapshot.
- critic-item-3: `answered` - The split decision is now grounded in live ticket structure instead of an inferred existing command API. The broader design-time command surface is already split at story level: story `06F2PGGEY26Y65G97NGFKH381M` parents this implementation task and sibling CI/examples task `06F2PGGR30XXCDKCZ8W2J2WX8C`, while release-facing documentation remains separate in `06F2PGHA0EXJRGDHM4GQM7NPYR`. The completed live-schema-reader story `06F2PGFZWC5PXSDH46RCZPN1CG` remains done and source-backed for the drift baseline.

Clarifications
- Repository evidence keeps the design-time boundary consumer-owned and single-project: the application that owns the configured `DbContext` also owns `IDesignTimeDbContextFactory<TContext>`, the preflight entrypoint, and any executable `Main`; `DCoding.Data.DVault` stays free of `Microsoft.EntityFrameworkCore.Design`, `IDesignTimeServices`, and `dotnet ef` interception.
- Source-backed underlying APIs already present on the branch are `IDataVaultDiagnosticsService.Analyze(DbContext)`, `DataVaultModelArtifactExporter.ExportJson(...)`, `DataVaultModelDriftReporter.Compare(...)`, `DataVaultLiveSchemaReader.ReadAsync(...)`, `DataVaultLiveSchemaDriftReporter.Compare/CompareAsync(...)`, and `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`.
- The current core package does not expose a public command host/runner or verb API in the approved public snapshot, so this ticket may add the minimal public host/runner abstractions it needs inside `DCoding.Data.DVault`.
- Export must use one explicit consumer-supplied source already supported by `DataVaultModelArtifactExporter`; the exporter explicitly accepts Code-First declaration callbacks, `DataVaultMetadataModel`, and `DataVaultMetadataRegistry`, and explicitly does not accept EF `ModelBuilder` state or reflective runtime `DbContext` export.
- Drift should keep artifact-versus-design-time-model comparison as the default lane and treat live-schema comparison as opt-in; the existing live-schema surfaces already classify `Succeeded`, `UnsupportedProvider`, and `Unavailable`.
- No child tickets, relation updates, or planning documents were materialized in this refinement pass; live relation state still includes a historical incoming `blocks` link from done story `06F2PGFZWC5PXSDH46RCZPN1CG`.

Scope In
- Add the minimal reusable command surface inside `DCoding.Data.DVault` for verbs `validate`, `export`, `drift`, and `guardrail`; if the branch lacks required public host/runner types, create them in this ticket.
- Keep executable hosting consumer-owned: the consumer project supplies the configured design-time `DbContext`, explicit export source, migration type/operation resolution, and `Main` entrypoint.
- `validate`: analyze the configured design-time model through `IDataVaultDiagnosticsService.Analyze(DbContext)` and surface deterministic success/failure.
- `export`: emit canonical `dvault.model.v1` JSON from an explicit consumer-supplied exporter input already supported by `DataVaultModelArtifactExporter`.
- `drift`: import a reviewed artifact and compare it to the current design-time model by default, with an opt-in live-schema lane that uses `DataVaultLiveSchemaReader` and `DataVaultLiveSchemaDriftReporter`.
- `guardrail`: evaluate scaffolded migration `UpOperations` against the configured design-time model via `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`.
- Add parser/help/exit-code coverage, representative verb tests, and core public API snapshot updates when new public types are introduced.

Scope Out
- No standalone DVault executable, `dotnet` tool package, `dotnet ef` shim, `IDesignTimeServices`, or EF CLI interception.
- No new NuGet package family member, no new packable project, and no broader package-shape change.
- No implicit export-from-`DbContext` or export-from-`ModelBuilder` path; explicit exporter inputs only.
- No widening of migration-rule taxonomy beyond the guardrail surfaces already tracked by `06F2PGGW8ZBW80V6B8RPWNVM70` and `06F2PGH42B6BT1708MYGMXP5GM`.
- No new provider live-schema reader implementation work beyond consuming the completed reader baseline.
- No CI workflow snippet authoring or broad README/release-note rollout; those remain in `06F2PGGR30XXCDKCZ8W2J2WX8C` and `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- No automatic migration execution, schema repair, SQL parsing, or database update behavior.

Open questions
- none

Follow-up questions
- After the command surface lands, should a later additive tooling ticket expose optional JSON command output, or are deterministic text plus exit codes sufficient because callers can already consume the structured APIs directly?
- Should the later CI/documentation tickets show one consolidated sample consumer host that wires factory creation, export-source selection, and migration resolution together, or keep each verb documented independently?

Risks
- Because the current public API snapshot shows no command host/runner surface, over-designing the new public API beyond minimal consumer hosting would create unnecessary long-term support obligations.
- Export is still the easiest place to overreach: the current exporter explicitly excludes EF `ModelBuilder` state and reflective `DbContext` export paths.
- If live-schema drift becomes the default instead of an opt-in lane, external-provider availability and `UnsupportedProvider`/`Unavailable` outcomes could make routine local command use noisy or misleading.
- Live relation state still contains a historical incoming `blocks` relation from done story `06F2PGFZWC5PXSDH46RCZPN1CG`; no relation cleanup was materialized in this pass, so schedule views may look more constrained than the actual baseline.

Split recommendations
- No additional split is recommended inside this ticket because the broader design-time command-surface breakdown is already materialized: story `06F2PGGEY26Y65G97NGFKH381M` parents this implementation task and sibling CI/examples task `06F2PGGR30XXCDKCZ8W2J2WX8C`, while documentation/release-note rollout remains separate in `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- Migration guardrail rule taxonomy and coverage expansion remain outside this ticket and continue to live in `06F2PGGW8ZBW80V6B8RPWNVM70` and `06F2PGH42B6BT1708MYGMXP5GM`.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment