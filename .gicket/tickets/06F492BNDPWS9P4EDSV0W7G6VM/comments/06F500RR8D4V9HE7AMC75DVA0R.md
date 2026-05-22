[gicket-bot] PO refinement contract

Summary
- Refined the v0.17.0 documentation task against the current v0.16.0 public baseline, the completed EF safety/preflight stories, and the no-diff scratch branch state; the ticket is ready for PO-critic and no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- `docs/releases/v0.16.0.md` and `docs/production-adoption-checklist.md` establish v0.16.0 as the current public documentation baseline, so this ticket should promote v0.17.0 to that role rather than invent a parallel release posture.
- Completed prerequisite tickets already ratify the behavior boundaries this docs pass must summarize: `DMV1910` and `DMV1911` misuse analyzers, `UseDataVaultSaveChangesGuardInterceptor(...)`, `DataVaultModelDriftPreflightReporter.Compare(...)`, expanded provider explain and read-shape diagnostics, strengthened migration guardrail reports, and `DataVaultPreflight.Run(...)`.
- `src/DCoding.Data.DVault.Analyzers/README.md` already documents the EF misuse analyzer slice and project-local analyzer installation guidance, so v0.17 docs should keep those ids and boundaries consistent across release notes and public guidance.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` already fixes the consumer-owned `DbContext` and design-time factory boundary and explicitly excludes a DVault-owned standalone CLI, `dotnet ef` interception, automatic migration execution, and automatic snapshot discovery.
- `git diff --name-only cc92ab9c283838606e0af88035661ac8452d5b62 HEAD` returned no changed files, so there is no in-branch docs draft to preserve; this ticket should author the documentation pass from the checked-in feature baseline.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass.

Scope In
- Author v0.17.0 release notes for the coordinated seven-package DVault family, summarizing the EF safety and preflight slice as the next public baseline after v0.16.0.
- Update the existing public adoption, setup, and design-time guidance surfaces that currently carry the baseline so they point at v0.17.0 and explain the new analyzer, guard, drift, guardrail, provider-explain, and aggregate preflight capabilities with correct default-versus-opt-in boundaries.
- Document the public APIs and diagnostic identifiers consumers need to use or recognize, including `DMV1910`, `DMV1911`, `DataVaultPreflight.Run(...)`, `DataVaultModelDriftPreflightReporter.Compare(...)`, `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`, `UseDataVaultSaveChangesGuardInterceptor(...)`, and the relevant diagnostics, read-shape, and support-bundle result surfaces.
- Include bounded migration and drift examples that show consumer-owned validate, drift, guardrail, and preflight flows, safe, risky, and incompatible migration outcomes, and explicit snapshot-model or reviewed-artifact inputs.
- Explain provider-capability, provider-behavior, save and read strategy, and read-shape diagnostics as bounded explainability features and keep redaction, no raw SQL, and no secret-bearing output boundaries explicit.
- Keep non-goals explicit so the docs reinforce DVault as an EF Core library rather than a standalone platform, orchestration layer, or automatic tooling product.

Scope Out
- No new runtime, analyzer, drift, migration, provider, or telemetry behavior; this ticket only documents the already-ratified implementation.
- No standalone DVault CLI, `dotnet ef` shim, automatic migration or live-schema execution, automatic snapshot discovery, or automatic representative request generation in examples.
- No performance or benchmark contract work from epic `06F492BTNHRPBC7D24E13ECFKM` or story `06F492BZPP5YT9SJSPDHQBGF3R`.
- No provider-specific SQL walkthroughs, raw support-bundle dumps, connection details, or secret-bearing examples.
- No reopening of completed feature boundaries such as the built-in registry-backed model-cache guarantee, the consumer-owned `IModelCacheKeyFactory` customization path, or the opt-in nature of runtime guard and telemetry surfaces.

Open questions
- none

Follow-up questions
- After v0.17 documentation ships, should a later tutorial ticket add a full end-to-end sample application that combines analyzer, guard, drift, and preflight flows without expanding this release-note task?
- Should the later performance documentation work under `06F492BTNHRPBC7D24E13ECFKM` and `06F492BZPP5YT9SJSPDHQBGF3R` add a separate evidence and reporting guide instead of widening this EF safety and preflight docs pass?
- Once adopters use representative request-bound diagnostics in practice, should a later docs pass publish one canonical support-bundle or request-diagnostics example payload?

Risks
- If the docs blur default and opt-in boundaries, consumers may incorrectly assume `AddDVault()` enables runtime guard, telemetry, or representative preflight diagnostics automatically.
- If release notes or examples imply a DVault-owned CLI, automatic `dotnet ef` interception, or automatic snapshot or live-schema discovery, the documentation will contradict the checked-in architecture contract.
- If the docs omit the consumer-owned model-cache and snapshot-model boundaries, adopters may overread the current implementation as protecting arbitrary caller-owned model-shaping state or accepting EF `ModelSnapshot` as a DVault public contract.
- If provider explanations are presented as provider-specific SQL validation instead of bounded capability, strategy, and read-shape explainability, the release notes will overpromise what v0.17 actually does.

Split recommendations
- No split is recommended; repository evidence shows the implementation tickets are already complete and this task can stay bounded to one coordinated v0.17 documentation and release-note pass.
- Keep any future end-to-end tutorial or sample-app expansion as a separate follow-up ticket rather than widening the release-note task.
- Keep performance-evidence documentation under the separate performance epic and story instead of folding it into this EF safety and preflight docs slice.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment