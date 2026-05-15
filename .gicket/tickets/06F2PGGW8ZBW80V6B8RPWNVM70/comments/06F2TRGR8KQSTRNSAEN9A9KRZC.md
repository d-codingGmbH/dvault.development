[gicket-bot] PO-critic review contract

Summary
- The delivery contract is specific, bounded, and grounded in current local ticket, relation, source, test, and branch-history evidence; there are no unresolved open questions, so this story is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGGW8ZBW80V6B8RPWNVM70/description.md:11-16 and :53-54 bound the story to provider-neutral guardrail hardening, explicitly keep Open Questions at 'none', and state that command-surface work is already upstream while docs rollout stays separate.
- .gicket/relations/J0/70/06F2PGFT8Z406HFBJGQSY7YRJ0--06F2PGGW8ZBW80V6B8RPWNVM70--parentOf.json, .gicket/relations/70/GM/06F2PGGW8ZBW80V6B8RPWNVM70--06F2PGH42B6BT1708MYGMXP5GM--parentOf.json, and .gicket/relations/70/YR/06F2PGGW8ZBW80V6B8RPWNVM70--06F2PGHA0EXJRGDHM4GQM7NPYR--blocks.json confirm the epic-parent link, the existing child split, and the separate docs follow-up relation in the local ticket store.
- src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs:138-152 already wires the existing guardrail verb to DataVaultMigrationOperationDiagnostics.AnalyzeReport(...) and returns exit code 1 when findings exist.
- src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs:121-156 dispatches CreateTableOperation plus add/drop/alter/rename-column, index, primary-key, and drop-table operations; :159-253 implements create-table checks; :467-468 emits deterministic migration/{Operation}/{Target}/{Member?} paths.
- src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs:141-182 defines the stable DVM2001 through DVM2006 migration-guardrail catalog.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs:70-90 keeps non-DVault and matching DVault create-table cases quiet; :93-164 asserts deterministic create-table findings and ordering; :166-342 asserts exact code, severity, path, remediation, and display output for the wider migration-operation matrix.
- docs/architecture/dvault-dotnet-ef-design-time-workflow.md:230-239 shows the consumer-owned preflight calling AnalyzeReport(...); docs/production-adoption-checklist.md:26-32 tells adopters to run guardrail --migration <name> as a blocking CI step.
- .gicket/tickets/06F2PGH42B6BT1708MYGMXP5GM/comments/06F2TMXBGVCJVKD5NSC3WVNHER.md:5-80 records tester evidence that 6/6 acceptance criteria and 5/5 definition-of-done items were satisfied on commit b8f61830cb7c for the child rule-coverage task.
- git diff --name-only develop..ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce listed only .gicket/tickets/06F2PGGW8ZBW80V6B8RPWNVM70/** files, so the story branch itself is currently ticket-metadata-only and does not reopen product code already represented elsewhere.
- test -f docs/releases/v0.11.0.md returned non-zero and ls docs/releases listed v0.10.0.md through v0.5.0.md only, matching the contract statement that broader v0.11 documentation work remains in ticket 06F2PGHA0EXJRGDHM4GQM7NPYR.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- RenameTableOperation coverage is explicitly deferred to a later follow-up ticket.
- Missing-table or renamed-table detection that depends on model snapshot or prior-schema state is explicitly out of scope.
- Provider-specific facet checks such as store type, default SQL, collation, or annotation analysis are explicitly out of scope.

Risky assumptions
- The story assumes current CreateTableOperation plus existing column/index/primary-key/drop-table coverage is sufficient for a blocking CI baseline without table-rename or prior-schema inference.
- The handoff assumes the missing docs/releases/v0.11.0.md file remains acceptable here because that documentation rollout is explicitly separated into ticket 06F2PGHA0EXJRGDHM4GQM7NPYR.

AC / test suggestions
- Keep future guardrail additions pinned to exact DVM code, severity, path, and ordering assertions in the existing migration diagnostics test suite.
- If RenameTableOperation or prior-schema inference is later added, give that work its own acceptance criteria instead of widening this story retroactively.

Implementation watchouts
- Stay inside DataVaultMigrationOperationDiagnostics for this story; do not reopen DataVaultDesignTimeCommand, DataVaultDesignTimeCommandHost, or the public command surface already delivered by 06F2PGGEY26Y65G97NGFKH381M.
- Keep the work provider-neutral; provider-specific SQL or store-facet checks are out of scope for this ticket.
- Do not pull README or v0.11 release-note rollout back into this story; that work is explicitly parked in 06F2PGHA0EXJRGDHM4GQM7NPYR.
- Because the current story branch differs from develop only in ticket metadata, any further code work should be traced to the child task lineage or a new focused follow-up rather than assumed from this branch itself.

Non-blocking notes
- The current branch HEAD is f7c4aa54980ebe8124b4065c4fc7af21d823ec2e, matching the provided scratch-source-ref; branch history around this story is ticket-metadata-only.

Split recommendations
- Keep implementation-level rule coverage in existing child 06F2PGH42B6BT1708MYGMXP5GM.
- Keep broader v0.11 documentation and release-note work in 06F2PGHA0EXJRGDHM4GQM7NPYR.
- Create separate future tickets for RenameTableOperation, missing-table/prior-schema inference, or provider-specific facet checks if they become necessary.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment