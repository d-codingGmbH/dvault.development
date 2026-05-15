[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the persisted contract is source-backed, tightly bounded to provider-neutral CreateTableOperation guardrail coverage, and has no unresolved PO questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGH42B6BT1708MYGMXP5GM/description.md contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` with `- none`.
- src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs analyzes Add/Drop/Alter/Rename column, Create/Drop/Rename index, Add/Drop primary key, and DropTable operations, but `AnalyzeOperation(...)` has no `CreateTableOperation` case.
- Command `rg -n "CreateTableOperation" /mnt/c/Projects/DVault/src /mnt/c/Projects/DVault/tests` returned no matches, so the current source and tests have no explicit CreateTable guardrail lane.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs asserts exact existing paths such as `migration/AddColumn/HubCustomer/CustomerStatus` and `migration/DropPrimaryKey/PitCustomerContact/PkPitCustomerContactCustomerHashKeyLoadTimestamp`, confirming the deterministic path/order contract the ticket references.
- src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs currently defines DVM2001-DVM2004 in add/drop/alter/recreate terms, matching the ticket's note that only narrow catalog wording broadening may be needed for CreateTable reuse.
- src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs still routes the `guardrail` verb through `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)`, so the public command surface boundary the ticket preserves is directly source-backed.
- `git diff --name-only develop...ticket/06F2PGH42B6BT1708MYGMXP5GM-task-add-migration-guardrail-rule-coverage` lists only `.gicket/tickets/06F2PGH42B6BT1708MYGMXP5GM/...` files, and `git log --oneline -5 -- .gicket/tickets/06F2PGH42B6BT1708MYGMXP5GM` shows only PO/po-critic metadata commits up to `c470db378`, confirming the branch is still pre-development rather than partially implemented code.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give a concrete example of a correct CreateTableOperation whose primary key is omitted inline and added later through a separate AddPrimaryKeyOperation; it only implies the existing AddPrimaryKey lane should own that case.
- The contract does not include an explicit quiet-case example where provider-specific CreateTable facets (store type, default SQL, collation, annotations) vary while the provider-neutral DVault structure remains correct.

Risky assumptions
- The implementation will compare only provider-neutral structural invariants from `DataVaultDiagnosticsResult.Explain.Entities`, even though `DataVaultPropertyExplain` also exposes provider profile, store type, and value-format fields that the ticket says to ignore.
- EF `CreateTableOperation` metadata is assumed to be sufficient to emit deterministic `migration/CreateTable/...` member paths without changing the public diagnostics issue shape.

AC / test suggestions
- Add one happy-path test where a DVault CreateTableOperation matches the explain baseline and expresses the correct primary key inline.
- Add one deterministic-ordering test where CreateTable findings coexist with a separate AddPrimaryKeyOperation or CreateIndexOperation finding for the same table.
- Add one quiet-path test proving provider-specific annotations or facets on an otherwise structurally correct CreateTableOperation do not create findings.

Implementation watchouts
- Keep CreateTable findings on the existing `migration/{Operation}/{Target}/{Member?}` path pattern and preserve the exact deterministic ordering already asserted by `DataVaultMigrationOperationDiagnosticsTests`.
- If DVM2001-DVM2004 catalog text changes, keep it narrow and backward-compatible; `DataVaultDiagnosticCatalog.cs` currently phrases those definitions around add/drop/alter/recreate behavior.
- Do not widen the lane into RenameTableOperation, missing-table inference, live-schema drift, or `DataVaultDesignTimeCommand` verb changes; the persisted contract explicitly scopes those out.

Non-blocking notes
- The branch is metadata-only today; absence of source changes is expected at this PO gate and is not a blocker.
- Broad README/release-note rollout is already tracked separately by ticket `06F2PGHA0EXJRGDHM4GQM7NPYR`, which lets this ticket stay focused on guardrail rule coverage.

Split recommendations
- No new split is needed beyond the existing contract: keep RenameTableOperation and absence-based drift inference in later follow-ups, and keep broad v0.11 documentation work in `06F2PGHA0EXJRGDHM4GQM7NPYR`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment