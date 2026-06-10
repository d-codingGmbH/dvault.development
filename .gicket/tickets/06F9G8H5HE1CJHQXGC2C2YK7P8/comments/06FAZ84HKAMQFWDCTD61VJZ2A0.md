[gicket-bot] PO-critic review contract

Summary
- Ticket refinement is clear and evidence-backed; approve for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9G8H5HE1CJHQXGC2C2YK7P8/description.md sets PO Handoff to ready_for_po_critic and Open Questions to none, so the explicit approval gate is satisfied.
- Related prerequisite tickets are already closed in repository state: .gicket/tickets/06F9G8GS08VNH0DT09Q4PC2HRC/ticket.json is done for the DB2 capability contract and .gicket/tickets/06F9G8GZ384VKA7RVF039WKX1M/ticket.json is done for the DB2 package story; the downstream integration story .gicket/tickets/06F9G8HBXS7Y42J7XFSQKZ2AZ8/ticket.json remains todo.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs maps IBM.EntityFrameworkCore to DataVaultProviderCapabilityProfiles.Db2, src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs includes DataVaultProviderCapabilityProfiles.Db2.WithLoadTimestampStorage(...), and src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers AddDVaultDb2, which supports the ticket's scope-out of package wiring.
- src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs currently defines db2-v1 with DB2 mappings but without explicit maximumIdentifierLength: 128 or allowsIndexesCoveredByPrimaryKey: false; src/DCoding.Data.DVault/DataVaultProviderIdentifierPreflight.cs IsSupportedProfile(...) lists only sqlite/oracle/postgres/sqlserver/mysql; tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstSchemaParityTests.cs still enumerates only five built-in profiles; and src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs dispatches only SQLite/PostgreSQL/SqlServer/Oracle/MySQL and otherwise returns UnsupportedProvider. These directly match the refined story scope.
- git -C /mnt/c/Projects/DVault diff --name-only develop..HEAD lists only .gicket/tickets/06F9G8H5HE1CJHQXGC2C2YK7P8/*, and git log --oneline --decorate -n 6 -- .gicket/tickets/06F9G8H5HE1CJHQXGC2C2YK7P8 shows recent commits are PO/PO-critic handoff and lease-claim commits, so this branch is still planning-ticket-only rather than a partial implementation review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A concrete acceptance-test example for a DB2 identifier over 128 characters or a post-projection collision would make the preflight expectation easier to spot, but the current contract is still sufficient for dev handoff.
- A concrete example of the expected diagnostic/result shape when DB2 remains on the explicit UnsupportedProvider live-schema path would help test naming, but the acceptance criteria already require deterministic behavior.

Risky assumptions
- The story assumes the done contract ticket 06F9G8GS08VNH0DT09Q4PC2HRC remains the authoritative source for DB2 guardrail facts and that developers will not reopen provider-name, identifier-limit, include-column, or timestamp-storage decisions.
- Repository docs still contain older five-provider/seven-package language, for example docs/releases/v0.11.0.md and docs/plans/provider-specific-sql-artifact-contract.md; the ticket assumes that documentation drift is handled later by the follow-up documentation task rather than inside this story.

AC / test suggestions
- Add one explicit DB2 preflight case for >128 identifiers and one for reserved-word/collision projection so the deterministic diagnostic path is pinned in tests.
- Mirror the Oracle redundant-secondary-index guardrail test pattern for DB2 to pin AllowsIndexesCoveredByPrimaryKey = false and the effective included-index behavior.
- If no DB2 live-schema reader is implemented here, add design-time and live-schema tests that assert IBM.EntityFrameworkCore returns UnsupportedProvider instead of falling through to another reader.

Implementation watchouts
- Do not let DB2 inherit SQLite/default identifier-preflight behavior; DataVaultProviderIdentifierPreflight.IsSupportedProfile(...) currently omits db2-.
- Do not let IBM.EntityFrameworkCore dispatch to another live-schema reader; DataVaultLiveSchemaReader currently recognizes only SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Keep DB2 validation opt-in and secret-free by default, with any live DB2 coverage gated behind DVAULT_TEST_DB2_CONNECTION_STRING or an equivalent non-secret marker.

Non-blocking notes
- The ticket is refined enough for dev even though the current branch contains only ticket metadata; this is a pre-development gate, not an implementation review.
- .gicket/tickets/06F9G8H5HE1CJHQXGC2C2YK7P8/comments/06FAZ67ZWN0344AE7T7XKAK8VG.md records the downstream blocks follow-up for 06F9G8HBXS7Y42J7XFSQKZ2AZ8 and drops the obsolete blocked-by follow-up from done ticket 06F9G8GZ384VKA7RVF039WKX1M, so relation housekeeping is not a PO blocker here.

Split recommendations
- No split recommended; the epic already separates contract, package, schema/guardrail, integration, package-verification, and documentation work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment