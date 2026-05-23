[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is well grounded in repository evidence, but it still lacks a source-of-truth intended release date for v0.18.0 even though the acceptance criteria require that date in docs/releases/v0.18.0.md.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492D05THPGQVT3B3K7853A0/description.md has `## Open Questions` set to `none`, so the persisted contract itself has no unresolved open-question block.
- git rev-parse --abbrev-ref HEAD returned `ticket/06F492D05THPGQVT3B3K7853A0-task-update-v0-18-0-documentation-and-release-no`, and `git diff --name-status 980857a7f42861ce99f1c0f120d5118000801740..HEAD -- README.md docs benchmark-summary.md benchmark-summary.csv benchmark-summary.json artifacts/benchmarks .gicket` returned no changed files on the reviewed surfaces.
- `docs/releases/` currently contains `v0.5.0` through `v0.17.0`; there is no `docs/releases/v0.18.0.md` yet.
- `README.md:10-16`, `README.md:686-702`, `docs/production-adoption-checklist.md:9`, and `docs/model-first-governance.md:3-5` still point to `v0.17.0` as the current baseline, matching the ticket scope.
- `docs/architecture/dvault-ef-compiled-compatibility.md` says SQLite is the required local compiled-model/query/pooling baseline and excludes provider-specific compiled guarantees; `benchmark-summary.md:51-56` contains the `compiled-model-startup`, `compiled-query-hub-read`, and `dbcontext-pooling-dvault-operation` SQLite rows.
- `docs/plans/performance-evidence-benchmark-artifact-contract.md` requires the benchmark summary triplet and visible skipped optional-provider rows; `benchmark-summary.md:57-64` keeps PostgreSQL, SQL Server, MySQL, and Oracle `provider-native-bulk-ingestion` rows visible as `skipped` with normalized reasons.
- Artifact bundles exist at `artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations`, `artifacts/benchmarks/06F492CFSJHN0RGXXRG3KT63FM-explicit-save-change-tracker`, and `artifacts/benchmarks/06F492CTREZEDXVKJ839YGCPWW-provider-optimization-regression-baselines`, each with checked-in before/after `benchmark-summary.md/.csv/.json` files.
- Direct repo evidence exists for the query-shape diagnostics surface the ticket references: `README.md:523,537,697`, `docs/releases/v0.17.0.md:166,215`, and `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` define `IDataVaultDiagnosticsService` and `DataVaultReadShapeDiagnostics`.

Blocking findings
- Acceptance Criteria require `docs/releases/v0.18.0.md` to record an intended release date, but the persisted ticket contract does not supply one, and local release metadata does not fill the gap: `.gicket/releases/06F492A0EZ3N8E2T605F7ZHHB0.json` and `.gicket/milestones/06F492A2MRBR0G0137V22KAGKG.json` contain names/descriptions/status only and no planned date field. A developer would have to invent the date.

Required PO actions
- Add the exact intended release date for `v0.18.0` to the delivery contract, or point to one authoritative local release-planning artifact that supplies the date the release note must copy.
- If no exact date is currently approved, relax the acceptance criterion so the release note can use an explicitly authorized placeholder or cross-reference instead of forcing the developer to guess.

Open issues ledger
- critic-item-1 [required-po-action] Add the exact intended release date for `v0.18.0` to the delivery contract, or point to one authoritative local release-planning artifact that supplies the date the release note must copy.
- critic-item-2 [required-po-action] If no exact date is currently approved, relax the acceptance criterion so the release note can use an explicitly authorized placeholder or cross-reference instead of forcing the developer to guess.
- critic-item-3 [blocking-finding] Acceptance Criteria require `docs/releases/v0.18.0.md` to record an intended release date, but the persisted ticket contract does not supply one, and local release metadata does not fill the gap: `.gicket/releases/06F492A0EZ3N8E2T605F7ZHHB0.json` and `.gicket/milestones/06F492A2MRBR0G0137V22KAGKG.json` contain names/descriptions/status only and no planned date field. A developer would have to invent the date.

Missing examples / edge cases
- The ticket names three current-baseline files, but `README.md` also has current-baseline narrative sections at `README.md:686-702`; the rollout needs to update both version-install snippets and current-baseline prose, not only the package version lines.
- The optional external-provider `provider-native-bulk-ingestion` rows are currently present only as skipped rows in `benchmark-summary.md:57-64`; the release note should treat that visible skipped state as intentional evidence, not as missing data.

Risky assumptions
- Assuming the intended `v0.18.0` release date can be inferred from ticket creation time, release object creation time, prior release cadence, or the current calendar date.
- Assuming only the three named files need baseline-pointer edits when the repo already shows additional current-baseline prose in `README.md`.

AC / test suggestions
- Add one explicit AC that the release note must cite the exact checked-in artifact bundle labels and the root `benchmark-summary.md/.csv/.json` evidence set it summarizes.
- Add one explicit AC that query-shape guidance should anchor to the existing request-bound diagnostics surface (`IDataVaultDiagnosticsService`, `DataVaultReadShapeDiagnostics`) rather than generic prose about diagnostics.

Implementation watchouts
- Keep compiled-model/query/pooling guidance SQLite-only and bounded to `UseModel(runtimeModel)`, stable direct EF shared-type-table queries, and one fixed model shape for pooled contexts.
- Do not describe DVault as a raw-SQL advisor, automatic index advisor, or provider-specific physical-plan guide; the existing read-shape diagnostics surface is deterministic explain output, not a SQL-tuning promise.
- Keep optional PostgreSQL/SQL Server/MySQL/Oracle rows visible as completed or skipped, and do not add per-scenario SQL-capture promises to compiled-model/query/pooling prose where the claim is only timing/allocation based.

Non-blocking notes
- The branch currently has no reviewed-surface diff against `980857a7f42861ce99f1c0f120d5118000801740`; for a pre-development PO gate, that is a handoff watchout rather than a blocker.

Split recommendations
- No split recommended; the benchmark/profiling work is already isolated in done sibling tickets, and this ticket is appropriately scoped as one documentation and release-note rollup once the release-date gap is resolved.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment