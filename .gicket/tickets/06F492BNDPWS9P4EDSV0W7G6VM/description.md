<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the v0.17.0 documentation task against the current v0.16.0 public baseline, the completed EF safety/preflight stories, and the no-diff scratch branch state; the ticket is ready for PO-critic and no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- `docs/releases/v0.16.0.md` and `docs/production-adoption-checklist.md` establish v0.16.0 as the current public documentation baseline, so this ticket should promote v0.17.0 to that role rather than invent a parallel release posture.
- Completed prerequisite tickets already ratify the behavior boundaries this docs pass must summarize: `DMV1910` and `DMV1911` misuse analyzers, `UseDataVaultSaveChangesGuardInterceptor(...)`, `DataVaultModelDriftPreflightReporter.Compare(...)`, expanded provider explain and read-shape diagnostics, strengthened migration guardrail reports, and `DataVaultPreflight.Run(...)`.
- `src/DCoding.Data.DVault.Analyzers/README.md` already documents the EF misuse analyzer slice and project-local analyzer installation guidance, so v0.17 docs should keep those ids and boundaries consistent across release notes and public guidance.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` already fixes the consumer-owned `DbContext` and design-time factory boundary and explicitly excludes a DVault-owned standalone CLI, `dotnet ef` interception, automatic migration execution, and automatic snapshot discovery.
- `git diff --name-only cc92ab9c283838606e0af88035661ac8452d5b62 HEAD` returned no changed files, so there is no in-branch docs draft to preserve; this ticket should author the documentation pass from the checked-in feature baseline.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass.

### Scope In
- Author v0.17.0 release notes for the coordinated seven-package DVault family, summarizing the EF safety and preflight slice as the next public baseline after v0.16.0.
- Update the existing public adoption, setup, and design-time guidance surfaces that currently carry the baseline so they point at v0.17.0 and explain the new analyzer, guard, drift, guardrail, provider-explain, and aggregate preflight capabilities with correct default-versus-opt-in boundaries.
- Document the public APIs and diagnostic identifiers consumers need to use or recognize, including `DMV1910`, `DMV1911`, `DataVaultPreflight.Run(...)`, `DataVaultModelDriftPreflightReporter.Compare(...)`, `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`, `UseDataVaultSaveChangesGuardInterceptor(...)`, and the relevant diagnostics, read-shape, and support-bundle result surfaces.
- Include bounded migration and drift examples that show consumer-owned validate, drift, guardrail, and preflight flows, safe, risky, and incompatible migration outcomes, and explicit snapshot-model or reviewed-artifact inputs.
- Explain provider-capability, provider-behavior, save and read strategy, and read-shape diagnostics as bounded explainability features and keep redaction, no raw SQL, and no secret-bearing output boundaries explicit.
- Keep non-goals explicit so the docs reinforce DVault as an EF Core library rather than a standalone platform, orchestration layer, or automatic tooling product.

### Scope Out
- No new runtime, analyzer, drift, migration, provider, or telemetry behavior; this ticket only documents the already-ratified implementation.
- No standalone DVault CLI, `dotnet ef` shim, automatic migration or live-schema execution, automatic snapshot discovery, or automatic representative request generation in examples.
- No performance or benchmark contract work from epic `06F492BTNHRPBC7D24E13ECFKM` or story `06F492BZPP5YT9SJSPDHQBGF3R`.
- No provider-specific SQL walkthroughs, raw support-bundle dumps, connection details, or secret-bearing examples.
- No reopening of completed feature boundaries such as the built-in registry-backed model-cache guarantee, the consumer-owned `IModelCacheKeyFactory` customization path, or the opt-in nature of runtime guard and telemetry surfaces.

## Acceptance Criteria
- A new or updated `docs/releases/v0.17.0.md` presents the coordinated seven-package release, identifies the EF safety and preflight highlights ratified by the completed prerequisite stories, and keeps publication, manual-release, and non-goal boundaries explicit.
- The public docs surfaces that currently define adoption, setup, and design-time guidance are updated to treat v0.17.0 as the current baseline and to align installation snippets, analyzer guidance, and preflight, guard, and drift workflow wording with the checked-in APIs.
- Release notes and adoption docs name the shipped EF misuse analyzer ids `DMV1910` and `DMV1911`, explain their supported and non-supported patterns at a bounded level, and keep `DCoding.Data.DVault.Analyzers` as project-local tooling.
- Runtime guard documentation explains that `UseDataVaultSaveChangesGuardInterceptor(...)` is explicit opt-in, separate from `AddDVault()`, supports warning and blocking modes, coexists with `UseDataVaultSaveChangesMetadataInterceptor(...)`, and does not replace `IDataVaultSaveService` as the default write boundary.
- Preflight and drift documentation shows the consumer-owned workflow around `IDataVaultDiagnosticsService.Analyze(DbContext)`, `DataVaultModelDriftPreflightReporter.Compare(...)`, `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`, and `DataVaultPreflight.Run(...)` without implying `ModelSnapshot` coupling, repository scanning, or a DVault-owned CLI.
- Provider explainability and support-bundle guidance documents capability profile, provider-behavior profile, save and read strategy diagnostics, and request-bound read-shape diagnostics as deterministic redacted explain surfaces rather than raw SQL or provider-magic claims.
- At least one migration example and one drift or preflight example are updated so readers can distinguish safe, risky, and incompatible guardrail outcomes plus artifact-versus-design-time and snapshot-model preflight lanes.
- The documentation keeps non-goals explicit across release notes and public guidance: no automatic migration execution, no automatic schema repair, no automatic live-schema gate, no dashboards, and no standalone DVault platform.

## Definition of Done
- All affected public documentation surfaces and the v0.17.0 release notes are internally consistent on version numbers, API names, diagnostic ids, and default-versus-opt-in behavior.
- The docs use the completed ticket contracts and checked-in repository docs as the authoritative source for feature scope instead of inventing new APIs, relation semantics, or broader provider guarantees.
- Examples and snippets remain bounded to consumer-owned EF Core workflows and do not require unsupported repository discovery, `ModelSnapshot` public contracts, or provider-specific magic.
- The current v0.16.0 baseline references in public guidance are advanced to v0.17.0 wherever this ticket owns the public current-release posture.
- The documentation pass completes without child-ticket creation, relation rewrites, description updates, attachments, or planning-document materialization.

## Implementation Notes
- Use `docs/releases/v0.16.0.md` as the structural baseline for release-note scope, then add the v0.17 items from the completed EF safety and preflight stories rather than reshaping the coordinated release format.
- Use the completed story contracts as the authoritative input map: `06F492A8WV0EP2V03CWXXWH71G` for guardrail outcome and report wording, `06F492AE2C8XBDXDH4V2JPTJDR` for snapshot-model drift, `06F492AKGMKPCRJYF4Z1EC9WY4` for model-cache guidance, `06F492ARW2N6SNYJH15RHMZEN8` for analyzer ids, `06F492AYE4A3PKA2D20DDPQ37C` for runtime guard, `06F492B40K7B0WWPKH8N3PPG3G` for provider explain output, `06F492B9PR036PDNN52S06S9BC` for read-shape diagnostics, and `06F492BG6BZYYFMBE5WK7CB024` for the aggregate preflight facade.
- Keep analyzer docs limited to the documented high-confidence misuse slice and preserve the existing analyzer-package README conventions around project-local installation and bounded suppression guidance.
- Keep design-time and preflight examples aligned with the architecture note: consumer-owned `DbContext`, design-time factory, command host, explicit reviewed-artifact inputs, and explicit snapshot-model inputs.
- When documenting provider explanations or support-bundle output, emphasize deterministic redacted fields such as provider and profile names, strategy status, fallback causes, and read-shape facts, not raw SQL, exception text, hash keys, or credentials.
- No durable planning writes were applied in this pass.

## Open Questions
- none

## Follow-Up Questions
- After v0.17 documentation ships, should a later tutorial ticket add a full end-to-end sample application that combines analyzer, guard, drift, and preflight flows without expanding this release-note task?
- Should the later performance documentation work under `06F492BTNHRPBC7D24E13ECFKM` and `06F492BZPP5YT9SJSPDHQBGF3R` add a separate evidence and reporting guide instead of widening this EF safety and preflight docs pass?
- Once adopters use representative request-bound diagnostics in practice, should a later docs pass publish one canonical support-bundle or request-diagnostics example payload?

## Risks
- If the docs blur default and opt-in boundaries, consumers may incorrectly assume `AddDVault()` enables runtime guard, telemetry, or representative preflight diagnostics automatically.
- If release notes or examples imply a DVault-owned CLI, automatic `dotnet ef` interception, or automatic snapshot or live-schema discovery, the documentation will contradict the checked-in architecture contract.
- If the docs omit the consumer-owned model-cache and snapshot-model boundaries, adopters may overread the current implementation as protecting arbitrary caller-owned model-shaping state or accepting EF `ModelSnapshot` as a DVault public contract.
- If provider explanations are presented as provider-specific SQL validation instead of bounded capability, strategy, and read-shape explainability, the release notes will overpromise what v0.17 actually does.

## Split Recommendations
- No split is recommended; repository evidence shows the implementation tickets are already complete and this task can stay bounded to one coordinated v0.17 documentation and release-note pass.
- Keep any future end-to-end tutorial or sample-app expansion as a separate follow-up ticket rather than widening the release-note task.
- Keep performance-evidence documentation under the separate performance epic and story instead of folding it into this EF safety and preflight docs slice.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Document the v0.17 EF Core safety/preflight scope, public APIs, analyzer IDs, runtime guard behavior, provider explanations, and migration/drift examples. Keep non-goals explicit so DVault remains a focused EF Core library.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Authored the v0.17.0 EF safety and aggregate preflight documentation pass.
- Promoted public current-baseline references from v0.16.0 to v0.17.0 where this ticket owns the release posture.
- Kept historical v0.16.0 release notes historical.

Repository artifacts
- `docs/releases/v0.17.0.md`
- `README.md`
- `docs/production-adoption-checklist.md`
- `src/DCoding.Data.DVault.Analyzers/README.md`
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`
- `docs/model-first-governance.md`
- `docs/plans/fluent-code-first-api-contract.md`

Verification
- `bash tools/check-format.sh` passed.
- `timeout 600 dotnet build DVault.slnx --nologo` completed with 0 errors. The build reported existing warning classes, including NuGet `NU1900` read-only vulnerability-cache warnings under the sandbox and EF/xUnit analyzer warnings in test projects.

Notes
- Full `dotnet test DVault.slnx --nologo` was not run after the documentation-only pass because formatting and solution build verification completed successfully.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

Summary
- Authored the v0.17.0 EF safety and aggregate preflight documentation pass.
- Promoted public current-baseline references from v0.16.0 to v0.17.0 on the owned public guidance surfaces.
- Rework response: restored docs/plans/fluent-code-first-api-contract.md to its develop-baseline planning text so this ticket no longer materializes a planning-document change.

Repository artifacts
- README.md
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md
- docs/model-first-governance.md
- docs/production-adoption-checklist.md
- docs/releases/v0.17.0.md
- src/DCoding.Data.DVault.Analyzers/README.md
- docs/plans/fluent-code-first-api-contract.md was touched only to remove the unintended branch diff; tester should confirm it has no effective diff from develop.

Verification
- bash tools/check-format.sh passed.
- dotnet build DVault.slnx --nologo passed with 0 errors; restore/build still emitted existing NU1900 read-only vulnerability-cache warnings plus existing analyzer warnings.
- dotnet test DVault.slnx --nologo passed; external-provider integration tests without local connection strings were skipped by their documented opt-in guards.
- git diff --exit-code develop -- docs/plans/fluent-code-first-api-contract.md passed with no output after the rework.
- git diff --name-only develop -- . excluding .gicket paths now lists README.md, docs/architecture/dvault-dotnet-ef-design-time-workflow.md, docs/model-first-governance.md, docs/production-adoption-checklist.md, docs/releases/v0.17.0.md, and src/DCoding.Data.DVault.Analyzers/README.md; it no longer lists docs/plans/fluent-code-first-api-contract.md.

Tester rework response
- The tester-returned planning-document concern is materially resolved: docs/plans/fluent-code-first-api-contract.md now keeps its historical v0.16.0 planning-contract pointer, while v0.17.0 current-release guidance remains on public documentation surfaces.
<!-- gicket-bot:developer-delivery:v1:end -->