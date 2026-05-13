[gicket-bot] PO refinement contract

Summary
- Ratified the existing four-story lifecycle-guardrails split and the current repo baselines for diagnostics, migration guardrails, consumer-owned dotnet-ef preflight, and artifact/ModelSnapshot/live-schema drift; no blocking PO questions remain.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Existing `parentOf` relations already materialize the epic split across four done child stories: 06F1XPS7KGKBP5SVMQPJC49J2G for stable diagnostic codes, 06F1XPTCGWTJHHQVNPN13KANMG for EF migration guardrails, 06F1XPVPKVGYKCV04PY98TSS78 for the consumer-owned design-time workflow, and 06F1XPWB8DZR4J8EZ00V8DT25G for ModelSnapshot plus optional live-schema drift comparison.
- The existing attachment `v0.8.0-lifecycle-guardrails-plan.md` already fixes the intended sequence as diagnostics, migration guardrails, design-time preflight, then drift comparison.
- Repository evidence already fixes the design-time boundary: the supported `dotnet ef` lane is a consumer-owned single-project `IDesignTimeDbContextFactory<TContext>` workflow with explicit preflight steps, not a DVault-owned `IDesignTimeServices` package or CLI shim.
- Stable diagnostic naming is already ratified by current repository evidence: model/import/projection validation uses the `DMV####` family and migration guardrails use the existing `DVM2001` through `DVM2006` baseline; this epic should not reopen diagnostic-prefix or code-band naming.
- No new child tickets, relation changes, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Track the lifecycle-guardrail slice through the four established child stories: stable diagnostic catalog, migration-operation guardrails, consumer-owned design-time preflight workflow, and artifact/ModelSnapshot/optional live-schema drift comparison.
- Reuse the current provider-neutral baselines already visible in repo code and docs: `DataVaultDiagnosticCatalog`, `IDataVaultDiagnosticsService.Analyze(DbContext)`, `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`, and `DataVaultModelDriftReporter.Compare(...)`.
- Keep validation and drift evidence design-time/local first: <redacted> live database is required for model validation or ModelSnapshot comparison, and optional live-schema evidence uses the current SQLite-first lane.
- Ship release and workflow documentation that explains the guarded EF lifecycle: validate the configured model, scaffold the migration, run migration preflight, then decide whether to apply the update.

Scope Out
- No automatic migration execution, rollback, or migration rewriting.
- No DVault-owned `IDesignTimeServices`, custom `dotnet ef` shim, or repo-owned `Microsoft.EntityFrameworkCore.Design` dependency.
- No provider-specific online migration engine or broad multi-provider live-schema promise beyond the documented SQLite-first optional lane and explicit unsupported or unavailable results.
- No breaking change to existing v0.7.0 runtime APIs unless separately documented and guarded.
- No expansion into later runtime ergonomics work already tracked by downstream blocked tickets.

Open questions
- none

Follow-up questions
- After this epic lands, should consumers get one consolidated preflight entrypoint that can run model validation, migration guardrails, ModelSnapshot comparison, and live-schema comparison through a shared command or report format?
- Which provider should be the next live-schema implementation after the current SQLite-first optional lane?
- If consumer-owned preflight proves too heavy, does later work need a separate ticket for packaged tooling or broader multi-project design-time layouts?

Risks
- Release notes or overview docs may over-promise support if they blur the boundary between consumer-owned preflight workflows and unsupported DVault-owned CLI or design-time tooling.
- Drift guidance can become confusing if docs do not clearly separate metadata-only ModelSnapshot comparison from optional physical live-schema evidence.
- The repository currently has release notes only through `v0.7.0`; if the v0.8 lifecycle-guardrail summary is left implicit, the epic's documentation acceptance criterion will be easy to miss during closure.

Split recommendations
- No additional implementation split is recommended; the epic already has the correct four-story decomposition and all four child stories are done.
- If any remaining work grows beyond a concise release-note and workflow write-up, split only a docs-focused follow-up and keep later runtime ergonomics in the already-blocked downstream tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment