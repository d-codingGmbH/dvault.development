<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the current v1 DVault design-time command baseline: a consumer-owned command surface for validate, export, drift, and guardrail, with focused CI examples split from broader v0.11 documentation rollout.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Ratify the current public v1 command surface already present on the branch: DataVaultDesignTimeCommand, DataVaultDesignTimeCommandHost, DataVaultDesignTimeExportSource, and the verbs validate, export, drift, and guardrail.
- Keep the supported design-time boundary consumer-owned and single-project: the application that owns the configured DbContext also owns IDesignTimeDbContextFactory<TContext>, the executable entrypoint, export-source selection, migration resolution, and dotnet ef invocation.
- Ratify the explicit export-source baseline already supported by the repository: export comes from Code-First declarations, DataVaultMetadataModel, or DataVaultMetadataRegistry, not reflective DbContext or ModelBuilder export.
- Use artifact-versus-design-time-model comparison as the default drift lane; live-schema drift stays opt-in through the existing live-schema APIs and must not become the default blocking gate.
- This story owns the command surface and its usage guidance; migration-rule taxonomy expansion, additional live-schema reader work, and broader release-note cleanup stay in separate tickets.

### Scope In
- Provide or maintain the minimal reusable command surface in DCoding.Data.DVault for validate, export, drift, and guardrail.
- Keep hosting consumer-owned: the consumer project supplies the configured design-time DbContext factory, diagnostics service wiring, explicit export source, migration UpOperations resolver, and optional live-schema reader.
- Define deterministic parser, help, and exit-code behavior for the four verbs so local scripts and CI can treat the surface as automation-safe.
- Document the command surface in the existing design-time workflow and focused adopter examples/checklist guidance needed for pre-integration use.
- Cover the public command surface with unit tests and public API snapshot updates where applicable.

### Scope Out
- No standalone DVault executable or dotnet tool package, no dotnet ef shim, no IDesignTimeServices integration, and no EF CLI interception.
- No startup-project or target-project split workflow, host auto-discovery, or multi-project design-time orchestration beyond the current single-project v1 boundary.
- No reflective export-from-DbContext or export-from-ModelBuilder path; export stays limited to the explicit source types already supported by DataVaultModelArtifactExporter.
- No new live-schema reader implementations or provider-support expansion in this story; the command surface only consumes the existing live-schema drift contract.
- No migration-rule taxonomy hardening, automatic migration execution, schema repair, SQL parsing, or broad v0.11 README and release-note consolidation beyond focused command-surface guidance.

## Acceptance Criteria
- A consumer-owned executable can host the four verbs through DataVaultDesignTimeCommand and DataVaultDesignTimeCommandHost without adding Microsoft.EntityFrameworkCore.Design to the core package or introducing a DVault-owned CLI package.
- Validate runs IDataVaultDiagnosticsService.Analyze(DbContext), prints deterministic diagnostics text, returns exit code 0 when valid, 1 when invalid, and 2 on usage errors.
- Export emits deterministic canonical dvault.model.v1 JSON from DataVaultDesignTimeExportSource, supports optional file output, returns 0 on success, 1 on export failure, and 2 on usage errors.
- Drift imports a reviewed artifact path, compares it to the current design-time model by default, supports an opt-in live-schema lane, and returns 0 only when no blocking differences exist.
- Guardrail resolves a named migration's UpOperations, runs DataVaultMigrationOperationDiagnostics.AnalyzeReport(...), prints deterministic guardrail output, and returns 0 only when the report is valid with no findings.
- Automated coverage includes help and usage parsing plus at least one success and failure path for each verb, and the approved public API snapshot reflects any newly public command-surface types.

## Definition of Done
- Only the minimal host, runner, and export-source surface needed for consumer hosting is public; the executable entrypoint, design-time factory wiring, artifact paths, and migration lookup remain consumer-owned.
- Source, tests, architecture guidance, and focused examples all use the same single-project consumer-owned boundary and the same four verb names.
- Command output reuses the existing deterministic diagnostics, drift-report, and migration-guardrail display surfaces instead of creating a second reporting taxonomy.
- The core package remains design-package-free and does not change package-publication scope.
- The existing split remains intact: command implementation and CI examples are part of this story boundary, while broader v0.11 documentation and release-note cleanup continues separately.

## Implementation Notes
- Use docs/architecture/dvault-dotnet-ef-design-time-workflow.md as the architectural anchor for command behavior and supported ownership boundaries.
- Reuse the existing underlying APIs rather than inventing parallel flows: IDataVaultDiagnosticsService.Analyze(DbContext), DataVaultModelArtifactExporter, DataVaultModelArtifactImporter, DataVaultModelDriftReporter, DataVaultLiveSchemaDriftReporter, and DataVaultMigrationOperationDiagnostics.AnalyzeReport(...).
- Keep drift artifact-based by default; only the live-schema lane should depend on async live-schema reading and classified unsupported or unavailable outcomes.
- Keep parser, help text, and exit codes deterministic; the current bounded baseline is 0 for success or help, 1 for command failure, invalid findings, or blocking differences, and 2 for usage errors.
- Treat export as artifact maintenance or reviewed refresh workflow support, not as the default blocking CI gate.
- Migration resolution remains consumer-owned and operates on scaffolded Migration.UpOperations only; do not parse SQL text or drive database update behavior.

## Open Questions
- none

## Follow-Up Questions
- Should a later tooling ticket add optional JSON command output, or are deterministic text plus direct structured APIs sufficient for v1 consumers?
- Should the broader v0.11 documentation task add one consolidated consumer-host example in README or release notes, or keep the detailed wiring only in the design-time workflow document?
- Should future design-time work broaden startup-project and target-project split support, or keep the current single-project composition boundary as the long-term default?

## Risks
- Over-designing the public command host or runner API beyond the current minimal surface would create avoidable long-term support obligations.
- If documentation teaches live-schema drift or export as the default blocking gate, adopters may build noisy or unsafe CI checks instead of using validate and reviewed-artifact drift by default.
- If command examples blur the consumer-owned boundary, teams may incorrectly assume DVault intercepts dotnet ef or applies migrations automatically.
- Broader v0.11 documentation and release-note rollout is separate work; if it slips, discoverability may lag behind the implemented command surface.

## Split Recommendations
- Keep the current split: one track for the core command implementation, one for CI and adopter examples, and one for broader v0.11 documentation and release-note cleanup.
- Keep migration-guardrail rule hardening and live-schema provider evolution outside this story so the command surface stays bounded to hosting and orchestration concerns.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Provide a small command surface for validation, artifact export, drift reports, and guardrail checks.

## Scope
- Refine and complete the work for "Add DVault design-time command surface" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.