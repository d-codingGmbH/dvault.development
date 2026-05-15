[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGFT8Z406HFBJGQSY7YRJ0/description.md` contains the authoritative delivery contract and its `## Open Questions` section is `- none`.
- Live `parentOf` relation files exist at `.gicket/relations/J0/CG/06F2PGFT8Z406HFBJGQSY7YRJ0--06F2PGFZWC5PXSDH46RCZPN1CG--parentOf.json`, `J0/1M/06F2PGFT8Z406HFBJGQSY7YRJ0--06F2PGGEY26Y65G97NGFKH381M--parentOf.json`, `J0/70/06F2PGFT8Z406HFBJGQSY7YRJ0--06F2PGGW8ZBW80V6B8RPWNVM70--parentOf.json`, and `J0/YR/06F2PGFT8Z406HFBJGQSY7YRJ0--06F2PGHA0EXJRGDHM4GQM7NPYR--parentOf.json`.
- `git diff --name-only develop..HEAD` listed only `.gicket/tickets/06F2PGFT8Z406HFBJGQSY7YRJ0/*`; no `src/`, `tests/`, `docs/`, or `examples/` files changed on the epic branch.
- `git log --oneline --decorate --max-count=12` shows the child work already integrated on `develop`: `522bf6258` (`06F2PGFZWC5PXSDH46RCZPN1CG`), `2b68c2101` (`06F2PGGEY26Y65G97NGFKH381M`), `b48552dd9` (`06F2PGGW8ZBW80V6B8RPWNVM70`), and `a5fb735fb` (`06F2PGHA0EXJRGDHM4GQM7NPYR`).
- `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` parses and runs `validate`, `export`, `drift`, and `guardrail`, and `src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs` keeps `CreateDbContext`, `ExportSource`, `ResolveMigrationOperations`, and optional `LiveSchemaReader` consumer-owned.
- `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` declares built-in provider dispatch for SQLite, PostgreSQL, SQL Server, Oracle, `MySql.EntityFrameworkCore`, and `Pomelo.EntityFrameworkCore.MySql`; `src/DCoding.Data.DVault/DataVaultLiveSchemaReadStatus.cs` defines `Succeeded`, `UnsupportedProvider`, and `Unavailable`.
- Tests back those boundaries: `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaReaderTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs` cover live-schema outcomes, drift, and guardrail behavior.
- The public docs named in the contract are present and aligned: `README.md`, `examples/README.md`, `docs/production-adoption-checklist.md`, `docs/model-first-governance.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, and `docs/releases/v0.11.0.md` all describe the consumer-owned command host, `drift --artifact` as the default blocking gate, opt-in `--live-schema`, and no standalone CLI/auto-migration behavior.
- All inspected ticket comments under `.gicket/tickets/06F2PGFT8Z406HFBJGQSY7YRJ0/comments/*.md` were bot claim/handoff/report comments; no human clarification comments were present.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- This approves workflow handoff on the assumption that the epic is intentionally a ratifying roll-up ticket; `develop..HEAD` contains only `.gicket` changes, so no new implementation appears to remain on the epic branch itself.
- This assumes the existing downstream sequencing is still correctly modeled by the nine live `blocks` relation files already referenced in the contract, since the PO pass explicitly verified them without rewriting relations.

AC / test suggestions
- Use the existing verification anchors already in the repo for handoff evidence: `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs`, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs`, `tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaReaderTests.cs`, and `tests/DCoding.Data.DVault.Tests/Integration/SqliteLiveSchemaDriftTests.cs`.
- If the next role needs a quick acceptance checklist, anchor it to the current doc set plus `docs/releases/v0.11.0.md` instead of adding new epic-level criteria.

Implementation watchouts
- Treat this as an epic roll-up: the current branch only changes `.gicket` metadata, so follow-on work should not reopen the already-integrated child scopes without new repository evidence.
- Preserve the documented consumer-owned single-project design-time boundary; the repo explicitly excludes a standalone DVault CLI, `dotnet ef` interception, automatic migration execution, and schema repair.
- Keep non-SQLite live-schema validation opt-in and consumer-managed; the docs and tests treat SQLite as the default local proof and external providers as operational evidence.

Non-blocking notes
- none

Split recommendations
- No additional split recommended; the epic already has four live `parentOf` children and the stated downstream analyzer/generator work remains separate via existing `blocks` relations.
- Keep the v0.12.0 analyzer/code-fix/source-generator tickets separate from this v0.11.0 roll-up rather than widening the epic.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment