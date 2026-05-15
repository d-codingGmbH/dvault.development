[gicket-bot] PO refinement contract

Summary
- Refinement ratifies this as a bounded consumer-owned design-time command task over existing DVault APIs, with no standalone CLI shim, no package-family expansion, and no additional split required.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the design-time boundary to a consumer-owned `IDesignTimeDbContextFactory<TContext>` plus preflight entrypoint in the same project as the configured `DbContext`; this ticket should add reusable command runners around that boundary, not a DVault-owned `dotnet ef` shim, `IDesignTimeServices`, or EF CLI interception.
- Current branch APIs already cover the four underlying behaviors this command surface should wrap: `IDataVaultDiagnosticsService.Analyze(...)` for validation, `DataVaultModelArtifactExporter.ExportJson(...)` for canonical artifact export, `DataVaultModelDriftReporter` plus `DataVaultLiveSchemaReader`/`DataVaultLiveSchemaDriftReporter` for drift, and `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)` for guardrail checks.
- The drift command can treat the completed live-schema-reader story `06F2PGFZWC5PXSDH46RCZPN1CG` as satisfied baseline input and reuse its first-class provider result statuses instead of reopening provider-support questions.
- Current repo evidence does not expose a public export-from-`DbContext` API, so export must use one explicit consumer-supplied source compatible with current `DataVaultModelArtifactExporter` inputs rather than trying to reconstruct artifacts from EF model state.
- Live relation state still shows an incoming `blocks` link from done ticket `06F2PGFZWC5PXSDH46RCZPN1CG`; no relation write was materialized in this refinement pass, so treat that link as historical/stale scheduling context rather than new scope.

Scope In
- Add a small reusable command surface in `DCoding.Data.DVault` that a consumer-owned executable can host for the four verbs `validate`, `export`, `drift`, and `guardrail`.
- Add the minimal host/runner abstraction needed for a consumer project to provide its configured design-time `DbContext`, an explicit export source, and migration-operation resolution without magical repository or EF CLI interception.
- `validate`: analyze the configured design-time model with existing diagnostics APIs and report deterministic success/failure.
- `export`: emit canonical `dvault.model.v1` JSON from an explicit consumer-supplied export source supported by the current exporter.
- `drift`: import a reviewed artifact and compare it against the current design-time model by default, with an opt-in live-schema lane that uses the existing live-schema reader and drift reporter surfaces.
- `guardrail`: evaluate scaffolded migration `UpOperations` against the configured design-time model with the existing migration guardrail API.
- Add parser/help/exit-code coverage plus representative command tests that prove each verb wraps the existing deterministic report surfaces without redefining them.

Scope Out
- No standalone DVault executable, `dotnet` tool package, `dotnet ef` shim, `IDesignTimeServices`, or EF CLI interception.
- No new NuGet package family member or broader package-shape change; keep the command surface inside the existing consumer-owned design-time workflow.
- No widening of migration-rule taxonomy beyond the current guardrail surface already owned by story `06F2PGGW8ZBW80V6B8RPWNVM70` and task `06F2PGH42B6BT1708MYGMXP5GM`.
- No new provider live-schema reader implementation work beyond consuming the completed reader baseline.
- No CI workflow snippet authoring or broad README/release-note rollout beyond the minimal command help/XML text needed for the API itself; those belong to `06F2PGGR30XXCDKCZ8W2J2WX8C` and `06F2PGHA0EXJRGDHM4GQM7NPYR`.
- No automatic migration execution, schema repair, SQL parsing, or database update behavior.

Open questions
- none

Follow-up questions
- After the command surface lands, should a later additive tooling ticket expose optional JSON command output, or is deterministic text plus exit codes sufficient because callers can already consume the structured APIs directly?
- Should the later CI/documentation tickets show one consolidated sample consumer host that wires factory creation, export-source selection, and migration resolution together, or keep each verb documented independently?

Risks
- Export is the easiest place to overreach: current repo evidence does not provide a public export-from-`DbContext` path, so any attempt at implicit EF-model reconstruction would create brittle, under-documented behavior.
- If live-schema drift becomes the default instead of an opt-in lane, external-provider availability and `UnsupportedProvider`/`Unavailable` outcomes could make routine local command use noisy or misleading.
- Because the ticket currently still carries a historical incoming `blocks` link from a done dependency, release-order views can look more constrained than the actual repository baseline.

Split recommendations
- No additional split is recommended once the ticket is bounded to reusable consumer-owned command runners over existing APIs; CI examples and v0.11.0 docs already remain separated in `06F2PGGR30XXCDKCZ8W2J2WX8C` and `06F2PGHA0EXJRGDHM4GQM7NPYR`.

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