[gicket-bot] PO-critic review contract

Summary
- Ticket 06FFDG522514HX2J17GT9VE77W is now a consistent pre-development implementation handoff: the delivery contract has no open questions, the scope boundary is explicit, and the cited repository seams match the requested work, so it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket-comments` includes the later PO refinement comment stating this is a normal implementation ticket, not a closure-only claim, and that no landed implementation evidence is being claimed, which resolves the earlier closure-only mismatch raised in the historical PO-critic comment.
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` currently registers MySQL provider behavior plus save and read/PIT/bridge read strategies, but no `IDataVaultProviderPitMaintenanceStrategy`, which matches the contract's identified implementation seam.
- `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` currently exposes `EvaluatePostgres(...)` and recognizes only `PostgresDataVaultPitMaintenanceStrategy` in `TryEvaluateKnownStrategy(...)`, directly supporting the contract's shared-gate extension scope for MySQL.
- `src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs` currently contains `ProviderNameMismatch`, `UnknownOrUnregisteredProviderName`, `NoProviderSpecificStrategyRegistered`, `DirtyDbContext`, `UnsupportedPitShape`, `IncompleteMaintenanceShapeEvidence`, and `StrategyDeclined`, which corroborates the ticket's need for explicit MySQL-visible rollback/savepoint decline vocabulary.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` states that current MySQL PIT read timing is not maintenance push-down evidence and describes MySQL PIT maintenance as a deliberately narrow future full-rebuild strategy lane, aligning repository docs with the ticket contract.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add explicit examples in the eventual test plan for caller-transaction handling that distinguish local-transaction rollback success from provider-neutral fallback when savepoint support is absent or unverified.
- Add an explicit negative example that `Pomelo.EntityFrameworkCore.MySql` remains provider-neutral for PIT maintenance even though MySQL save/read capability registration already covers Pomelo.

Risky assumptions
- The official `MySql.EntityFrameworkCore` provider can prove rollback-clean behavior clearly enough to separate accepted savepoint-backed cases from provider-neutral fallback cases.
- Existing MySQL capability-profile registration for both provider names will not accidentally widen PIT maintenance selection beyond the official-provider lane.
- Documentation and tests will keep optimized PIT read evidence separate from PIT maintenance proof so timing claims are not overstated.

AC / test suggestions
- Add paired gate tests for `MySql.EntityFrameworkCore` versus `Pomelo.EntityFrameworkCore.MySql` so the official-provider-only selection rule is explicit.
- Add separate fallback assertions for provider mismatch, unknown or unregistered provider name, dirty context, incomplete evidence, unsupported PIT shape, and rollback/savepoint boundary decline instead of collapsing them into generic strategy decline behavior.
- Add a negative test that `MaintainParentsAsync(...)` never selects the MySQL provider strategy even when provider name and PIT metadata otherwise match the accepted rebuild lane.

Implementation watchouts
- The MySQL package already registers provider capability profiles for both provider names, so maintenance selection must stay narrower than existing save/read registration behavior.
- Shared gate-evaluator and fallback-cause changes affect behavior outside MySQL; preserving current PostgreSQL semantics and provider-neutral fallback behavior is part of the implementation risk surface.
- Repository docs already state that MySQL PIT read timing is not maintenance evidence; the implementation must preserve that documentation boundary.

Non-blocking notes
- No landed implementation evidence is currently claimed, which is appropriate because this is a normal pre-development handoff rather than a closure-only or post-development review.

Split recommendations
- No additional split is needed for the current official-provider ordinary hub-parent full-rebuild lane.
- If product later wants Pomelo live PIT maintenance, multi-active hub-parent PIT rebuilds, or link-parent PIT rebuilds, keep those as separate follow-up tickets rather than widening this slice.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment