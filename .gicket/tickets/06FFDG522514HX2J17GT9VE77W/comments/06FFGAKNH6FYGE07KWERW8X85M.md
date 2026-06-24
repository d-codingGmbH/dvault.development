[gicket-bot] PO-critic review contract

Summary
- Return to PO: the persisted ticket is a clear pre-development implementation task, but this run is explicitly auditing it as a closure-only ticket and the branch shows only ticket-metadata changes.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FFDG522514HX2J17GT9VE77W/description.md has '## Open Questions' = 'none', but its Scope In / Acceptance Criteria / Definition of Done still require adding a MySQL IDataVaultProviderPitMaintenanceStrategy, shared diagnostics changes, unit/integration tests, and documentation updates.
- git diff --name-only a0e5d80ecc..HEAD lists only .gicket/tickets/06FFDG522514HX2J17GT9VE77W/... files; no src/, tests/, or docs/ implementation files are present on this branch.
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs currently registers MySQL save and read strategies only; src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs already registers IDataVaultProviderPitMaintenanceStrategy; src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs replaces IDataVaultPitMaintenanceService.
- src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs exposes EvaluatePostgres(...) and a TryEvaluateKnownStrategy switch that recognizes only 'PostgresDataVaultPitMaintenanceStrategy'; src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs does not yet include explicit rollback/savepoint decline causes.
- .gicket/tickets/06FF43CJ9CJMG7J917RW22QKJC/ticket.json shows the source evaluation ticket is 'done', so the implementation boundary is already established upstream.

Blocking findings
- The runtime marks this review as a 'closure-only-ticket', but the persisted contract is still a normal implementation ticket that explicitly requires future source, test, and documentation work. Under the provided closure-only audit rule, that mismatch is a blocker.
- There is no landed repository evidence for a closure-only claim: the current branch diff from a0e5d80ecc to HEAD contains only ticket metadata changes, while the contract itself still describes unimplemented work.

Required PO actions
- Make the ticket mode consistent. If this is a normal pre-development implementation ticket, remove or fix the closure-only routing and send it forward as a dev handoff.
- If the ticket must remain closure-only, rewrite the contract to an evidence-only closure scope and attach concrete landed repository evidence or point to the actual implementation ticket or commit that already delivered the work.
- Keep the provider boundary explicit after rerouting: official MySql.EntityFrameworkCore only for the maintenance lane, with Pomelo, multi-active hub-parent PITs, link-parent PITs, and timing claims deferred.

Open issues ledger
- critic-item-1 [required-po-action] Make the ticket mode consistent. If this is a normal pre-development implementation ticket, remove or fix the closure-only routing and send it forward as a dev handoff.
- critic-item-2 [required-po-action] If the ticket must remain closure-only, rewrite the contract to an evidence-only closure scope and attach concrete landed repository evidence or point to the actual implementation ticket or commit that already delivered the work.
- critic-item-3 [required-po-action] Keep the provider boundary explicit after rerouting: official MySql.EntityFrameworkCore only for the maintenance lane, with Pomelo, multi-active hub-parent PITs, link-parent PITs, and timing claims deferred.
- critic-item-4 [blocking-finding] The runtime marks this review as a 'closure-only-ticket', but the persisted contract is still a normal implementation ticket that explicitly requires future source, test, and documentation work. Under the provided closure-only audit rule, that mismatch is a blocker.
- critic-item-5 [blocking-finding] There is no landed repository evidence for a closure-only claim: the current branch diff from a0e5d80ecc to HEAD contains only ticket metadata changes, while the contract itself still describes unimplemented work.

Missing examples / edge cases
- If the ticket stays closure-only, it needs a concrete landed-evidence example tying the claimed MySQL PIT maintenance outcome to specific src/, tests/, and docs/ paths or commits; no such example exists in the current branch.
- If rerouted back to normal dev work, acceptance text should keep an explicit decline example for Pomelo.EntityFrameworkCore.MySql maintenance selection so the official-provider-only boundary cannot be misread.

Risky assumptions
- Assuming the closure-only runtime context is harmless would let an implementation ticket bypass the required mode or routing correction.
- Assuming existing dual-provider MySQL capability registration implies dual-provider maintenance support would overstate the current repository evidence; the contract and docs only justify the official MySql.EntityFrameworkCore lane.

AC / test suggestions
- If rerouted to normal dev, add one explicit acceptance or test bullet that KnownProviderNames.MySqlOracle is the accepted maintenance provider and KnownProviderNames.MySqlPomelo still declines to provider-neutral fallback.
- If rerouted to normal dev, keep one explicit rollback-clean test case for locally owned transactions and one explicit fallback case for current transactions without verified savepoint support.

Implementation watchouts
- The ticket already correctly points at shared maintenance seams, so implementation evidence will have to touch more than DVaultMySqlServiceCollectionExtensions.cs; shared gate and fallback vocabulary are part of the promised scope.
- MaintainParentsAsync(...) must stay on the provider-neutral path; DefaultDataVaultPitMaintenanceService.cs currently dispatches provider strategies only from RebuildAsync(...).

Non-blocking notes
- The persisted contract is otherwise well-scoped for a normal dev ticket: .gicket/.../description.md has Open Questions = none and the source feasibility ticket 06FF43CJ9CJMG7J917RW22QKJC is already done.
- Repository documentation already supports the narrow MySQL lane and distinguishes PIT read timing from PIT maintenance proof in docs/architecture/dvault-v1-pit-bridge-boundary.md and docs/performance-profiles.md.

Split recommendations
- No technical split is needed if the ticket is rerouted as a normal implementation handoff; the current implementation slice is already narrow.
- If product insists on keeping this ticket closure-only, split the real implementation into a separate dev ticket and leave this ticket as evidence-only closure work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment