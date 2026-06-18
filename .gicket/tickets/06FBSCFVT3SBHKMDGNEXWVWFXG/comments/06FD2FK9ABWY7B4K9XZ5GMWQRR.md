[gicket-bot] PO-critic review contract

Summary
- Ticket 06FBSCFVT3SBHKMDGNEXWVWFXG is refined clearly enough for developer handoff: the persisted contract has no open questions, the MySQL latest-satellite baseline is repository-backed, and the current branch contains only .gicket workflow updates.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSCFVT3SBHKMDGNEXWVWFXG/description.md contains PO handoff ready_for_po_critic and ## Open Questions = none.
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs registers IDataVaultProviderPitReadStrategy and IDataVaultProviderBridgeReadStrategy for MySQL, but no IDataVaultProviderReadStrategy latest-satellite registration; src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs does register IDataVaultProviderReadStrategy for SQLite.
- src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs implements only PIT/bridge checks, and src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs exposes MySQL evaluation for PIT/bridge while GetKnownLatestSatelliteGateRequirements(...) only recognizes SqliteDataVaultReadStrategy for latest-satellite gating.
- benchmark-summary.md:81, docs/plans/provider-optimization-evidence-matrix.md:261, docs/plans/provider-optimization-gap-matrix.md:53, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:460 all pin the current MySQL latest-satellite-read baseline to selectedStrategy=<none> and providerSpecificReadStrategy=not registered for latest satellite reads.
- git diff --name-only 77272d144..HEAD -- . ':(exclude).gicket' returned no files, and git show --name-only f717674c3 shows the PO handoff commit changed only .gicket/tickets/06FBSCFVT3SBHKMDGNEXWVWFXG/...; there is no branch-local code/doc/test delta yet, which is consistent with a pre-development handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developer handoff assumes any implementation path will preserve the existing provider-neutral fallback boundary for unsupported latest-satellite shapes.
- Developer handoff assumes any no-work-required closure will land explicit repository documentation or evidence updates rather than closing the ticket with comments alone.
- Developer handoff assumes no one will treat the skipped MySQL benchmark row as measured timing while DVAULT_TEST_MYSQL_CONNECTION_STRING remains unset in the checked-in run.

AC / test suggestions
- Keep the acceptance boundary tied to the existing evidence surfaces by requiring the final outcome to reconcile benchmark-summary.*, the provider evidence or gap matrices, and BenchmarkScenarioExecutionTests for the MySQL latest-satellite row.
- Whichever path is chosen, require explicit proof of the fallback boundary for provider mismatch and unsupported latest-satellite shapes so the repository does not rely on implicit behavior.

Implementation watchouts
- Do not regress the existing MySQL PIT and bridge posture while resolving the latest-satellite decision.
- Do not restate skipped-placeholder MySQL benchmark guidance as measured external-provider performance.

Non-blocking notes
- The ticket is presently todo with critic-needed and no assignees, which is compatible with forwarding the ticket to the dev role after this review.

Split recommendations
- No split recommended at PO level; the contract is already scoped to one provider and one read shape, and the repository evidence does not expose a second independent PO problem.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment