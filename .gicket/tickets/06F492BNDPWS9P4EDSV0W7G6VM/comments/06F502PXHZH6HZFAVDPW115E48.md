[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the persisted contract is specific, the prerequisite stories are done, the named APIs and diagnostics exist in source, and the ticket has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492BNDPWS9P4EDSV0W7G6VM/description.md contains the persisted delivery contract with `## Open Questions` = `none`, 8 acceptance-criteria bullets, 5 definition-of-done bullets, and implementation notes naming the prerequisite tickets.
- Comment `.gicket/tickets/06F492BNDPWS9P4EDSV0W7G6VM/comments/06F500RR8D4V9HE7AMC75DVA0R.md` records PO handoff decision `ready_for_po_critic` and states that no child tickets, relation writes, description updates, attachments, or planning documents were materialized.
- `README.md:10-16` still installs all seven packages at `0.16.0`, and `docs/production-adoption-checklist.md:9` still treats `releases/v0.16.0.md` as the current public baseline.
- `ls docs/releases` lists release-note files through `v0.16.0.md`, and `[ -e docs/releases/v0.17.0.md ]` returned `missing`, which matches the pre-development documentation scope.
- `src/DCoding.Data.DVault.Analyzers/README.md:7-8,15-23,35-39` documents `DMV1910`, `DMV1911`, and `DCoding.Data.DVault.Analyzers` as project-local tooling with `PrivateAssets=all` guidance.
- `src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs:101-144`, `src/DCoding.Data.DVault/DataVaultModelDriftPreflightReporter.cs:10-57`, `src/DCoding.Data.DVault/DataVaultPreflight.cs:8-39,73-118`, and `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:717-757` contain the named guard, drift, preflight, and diagnostics APIs referenced by the ticket.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:8-10,237-262,264-309` already fixes the consumer-owned design-time boundary and explicitly excludes a DVault CLI, `dotnet ef` interception, automatic snapshot discovery, and automatic migration execution.
- `git diff --name-only cc92ab9c283838606e0af88035661ac8452d5b62 HEAD -- . ':(exclude).gicket/**'` returned no output, and `git diff --quiet -- . ':(exclude).gicket/**'; echo $?` returned `0`, so the branch currently has no non-ticket-metadata changes.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Keep the docs examples explicit about the difference between runtime-guard warning/blocking modes and migration-guardrail safe/risky/incompatible outcomes.
- If both reviewed-artifact and snapshot-model preflight lanes are shown, make the optional-versus-required inputs and skipped-lane behavior obvious.

Risky assumptions
- The developer can find every owned current-baseline reference by grepping remaining `0.16.0` and `v0.16.0` mentions; the contract names key surfaces but not an exhaustive file list.
- Doc examples will stay within the verified public API surface and will not invent a DVault CLI, `ModelSnapshot` public contract, or automatic artifact discovery.
- Provider explainability prose will remain at capability/profile/strategy/read-shape level and will not drift into raw SQL or provider-magic claims.

AC / test suggestions
- Use a repo-wide review pass for `0.16.0` and `v0.16.0` in public docs before closing the ticket so stale baseline references do not remain.
- Cross-check every named doc/API identifier against source before publish: `DMV1910`, `DMV1911`, `IDataVaultDiagnosticsService.Analyze(DbContext)`, `DataVaultModelDriftPreflightReporter.Compare(...)`, `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`, `DataVaultPreflight.Run(...)`, and `UseDataVaultSaveChangesGuardInterceptor(...)`.
- Review at least one migration example and one artifact/snapshot preflight example against `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` so the consumer-owned design-time boundary stays consistent.

Implementation watchouts
- Do not imply `AddDVault()` enables runtime guard, telemetry, or representative request diagnostics automatically; the guard is a separate opt-in `DbContextOptionsBuilder` extension and telemetry remains opt-in in the current public baseline docs.
- Do not describe a DVault-owned CLI, `dotnet ef` interception, automatic migration execution, automatic live-schema gating, automatic snapshot discovery, or `ModelSnapshot` as a public DVault contract.
- Keep analyzer installation project-local and keep `DMV1910` and `DMV1911` scoped to the bounded shared-type misuse slice already documented in `src/DCoding.Data.DVault.Analyzers/README.md`.
- The branch currently has no non-`.gicket` diff and `docs/releases/v0.17.0.md` is missing, so the developer will need to author the documentation pass from the checked-in baselines rather than preserve an existing draft.

Non-blocking notes
- Performance tickets `06F492BTNHRPBC7D24E13ECFKM` and `06F492BZPP5YT9SJSPDHQBGF3R` remain `todo`, but the delivery contract explicitly scopes that work out of this documentation task.

Split recommendations
- No split recommended; the prerequisite stories are already `done` and the remaining work is one coordinated documentation and release-notes pass.
- Keep any later end-to-end tutorial or sample-app expansion separate from this release-note task.
- Keep performance-evidence and benchmark-reporting documentation under `06F492BTNHRPBC7D24E13ECFKM` and `06F492BZPP5YT9SJSPDHQBGF3R` rather than widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment