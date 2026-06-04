[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract has no open questions, the repo confirms the missing v0.29.0 baseline doc and current v0.28.0 public baseline, and the cited provider/guardrail anchors exist locally.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZNNS76TD9Z7ESB173FZ68/description.md contains `## Open Questions` followed by `- none`, plus acceptance criteria requiring a new `docs/releases/v0.29.0.md` and coordinated `README.md` / `docs/production-adoption-checklist.md` updates.
- `ls docs/releases` ended at `v0.28.0.md`, and `test -f docs/releases/v0.29.0.md` returned `1`; the v0.29.0 release-note file does not exist yet.
- `README.md` still installs `0.28.0` packages and says `The current coordinated release baseline is DVault v0.28.0 Release Notes`; `docs/production-adoption-checklist.md` line 9 still says `v0.28.0 release notes` are the current public baseline.
- A repo search for `0.29.0` across `README.md`, `docs/`, `src/`, `tests/`, and the ticket folder only returned hits in the ticket artifacts, not in checked-in product docs.
- `docs/plans/provider-identifier-ddl-guardrail-contract.md` defines the finite supported-provider baseline as SQLite, Oracle, PostgreSQL, SQL Server, and MySQL and states that unrecognized providers must not inherit provider-specific DDL safety claims.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` defines built-in profiles `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, and `mysql-pomelo-v1`; the MySQL profile enforces `maximumIdentifierLength: 64` and `unsupportedIncludedIndexColumnMode: Ignore`, and Oracle sets `allowsIndexesCoveredByPrimaryKey: false`.
- `src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs` registers exactly those five capability profiles in `CreateProviderCapabilityProfiles(...)`, and the contract-cited files `DataVaultAnnotationNames.cs`, `DataVaultDiagnostics.cs`, `DataVaultMigrationOperationDiagnostics.cs`, and `DataVaultMigrationGuardrailReport.cs` exist under `src/DCoding.Data.DVault/`.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` documents the adopter workflow `validate`, `drift --artifact`, and `guardrail --migration`, and `src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs` / `DataVaultMigrationGuardrailReport.cs` expose deterministic DVM-based guardrail reporting.
- `git rev-parse HEAD` returned `38be2f42712935d87ec8cc6099f2f57ace1cd3e8`, matching the supplied `scratch-source-ref`, and `git diff --name-only 38be2f42712935d87ec8cc6099f2f57ace1cd3e8..HEAD -- . ':(exclude).gicket/**'` returned no paths, so the owner branch currently carries no repository-file implementation changes yet.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not explicitly require a user-facing scenario for the unrecognized-provider path, even though it does require stating that fallback providers do not inherit provider-specific safety guarantees.
- The contract requires at least one concrete example, but it does not force coverage of both identifier-length caveats and index-shape caveats; one of those could remain implicit unless the writer broadens the examples section.

Risky assumptions
- Current provider profile names, diagnostic names, and guardrail terminology in `DataVaultDiagnostics` / `DataVaultMigrationOperationDiagnostics` will remain stable between refinement and implementation.
- Updating `README.md`, `docs/production-adoption-checklist.md`, and the new `docs/releases/v0.29.0.md` will be enough for discoverability; `docs/model-first-governance.md` still calls `docs/releases/v0.26.0.md` the current public baseline.

AC / test suggestions
- During doc review, verify that `docs/releases/v0.29.0.md` stays publication-neutral while still naming the coordinated seven-package DVault family.
- Check that at least one example shows both the guardrail/DDL outcome and the adopter response, not just background theory.
- Do a targeted doc QA pass to ensure touched sections do not leave mixed `0.28.0` and `0.29.0` version references.

Implementation watchouts
- Keep provider support bounded to the five built-in profiles; the docs must not imply provider-specific safety for fallback or unrecognized providers.
- Reuse the existing design-time workflow boundary from `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`: guardrails analyze scaffolded migrations before apply and do not repair schema, intercept `dotnet ef`, or run migrations automatically.
- Match public wording to source-anchored facts such as MySQL `maximumIdentifierLength: 64`, MySQL include-column `Ignore`, and Oracle `allowsIndexesCoveredByPrimaryKey: false`.
- Because the branch currently has no repository-file implementation changes yet, the first developer pass must create the release note and align touched docs from scratch.

Non-blocking notes
- The local ticket history now contains bot operational comments beyond the original prompt snapshot, but none reopen `## Open Questions` or change the refined delivery contract.
- A broader discoverability pass is already captured as a follow-up question in the contract, so it does not need to block developer handoff.
- No split is justified by current repo and ticket evidence; the scope remains one bounded documentation slice.

Split recommendations
- No split recommended; the current contract already bounds the work to the missing `docs/releases/v0.29.0.md` plus coordinated public-doc updates for the provider schema guardrail slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment