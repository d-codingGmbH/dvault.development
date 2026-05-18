[gicket-bot] PO-critic review contract

Summary
- The contract is detailed and has no open questions, but repository and branch history do not support this as a clean pre-development implementation handoff: the current ticket branch carries only ticket metadata while the cited provider-strategy work is already present on develop under other ticket history.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/description.md` contains a detailed delivery contract and `## Open Questions` = `none`.
- `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F2PGNGVQ3TZZWSABAK5SNFK4/*`; no `src/` or `tests/` files are unique to the current story branch versus `develop`.
- `git log --grep='06F2PGNGVQ3TZZWSABAK5SNFK4' -- src/DCoding.Data.DVault.Postgres src/DCoding.Data.DVault.SqlServer src/DCoding.Data.DVault.MySql src/DCoding.Data.DVault.Oracle tests/DCoding.Data.DVault.Tests/Integration` returned no matching implementation/test commits for this story.
- `git show --name-only b95ad09f9` for done child `06F2PGNT7DF4DVNKYWDFZC8DEM` changed `src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs`, `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs`, and provider bulk integration tests including `ExternalProviderBulkSaveAssertions.cs`, `MySqlExplicitDataVaultSaveServiceTests.cs`, `OracleDataVaultSmokeTests.cs`, `PostgresOptimizedDataVaultSaveServiceTests.cs`, and `SqlServerDataVaultSmokeTests.cs`.
- `git show --name-only 6e833b1a7` changed all four provider strategy files (`PostgresDataVaultSaveStrategy.cs`, `SqlServerDataVaultSaveStrategy.cs`, `MySqlDataVaultSaveStrategy.cs`, `OracleDataVaultSaveStrategy.cs`) under different ticket history before this story branch carried any code delta.
- Current source still directly proves the technical scope exists in-repo: `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` contains the provider gate evaluator, and the provider packages register strategies through `DVaultPostgresServiceCollectionExtensions.cs`, `DVaultSqlServerServiceCollectionExtensions.cs`, `DVaultMySqlServiceCollectionExtensions.cs`, and `DVaultOracleServiceCollectionExtensions.cs`.
- `docs/architecture/dvault-v1-explicit-save-service.md` describes Oracle ordinary-satellite eligibility, while `docs/plans/provider-optimization-closure-alignment-follow-up.md` still records an older Oracle hub/link-only posture.

Blocking findings
- This is not currently a clean developer handoff branch for an implementation story: relative to `develop`, the branch contains only `.gicket` metadata updates and no code/test delta for the work the contract says dev should implement.
- The claimed ownership split is not reconciled with repository history: the contract says this story owns provider-native strategy implementation while done child `06F2PGNT7DF4DVNKYWDFZC8DEM` already landed changes in provider strategy and provider bulk test surfaces.

Required PO actions
- Clarify delivery state. If the native-strategy implementation is already landed on `develop`, re-route or reclassify this ticket as closure/test-ready/no-work instead of handing it to dev as a fresh implementation story.
- If developer work still remains, identify the exact pending code delta and concrete file surfaces not yet in `develop`, then update Scope In, Acceptance Criteria, and Definition of Done to match that remaining work.

Open issues ledger
- critic-item-1 [required-po-action] Clarify delivery state. If the native-strategy implementation is already landed on `develop`, re-route or reclassify this ticket as closure/test-ready/no-work instead of handing it to dev as a fresh implementation story.
- critic-item-2 [required-po-action] If developer work still remains, identify the exact pending code delta and concrete file surfaces not yet in `develop`, then update Scope In, Acceptance Criteria, and Definition of Done to match that remaining work.
- critic-item-3 [blocking-finding] This is not currently a clean developer handoff branch for an implementation story: relative to `develop`, the branch contains only `.gicket` metadata updates and no code/test delta for the work the contract says dev should implement.
- critic-item-4 [blocking-finding] The claimed ownership split is not reconciled with repository history: the contract says this story owns provider-native strategy implementation while done child `06F2PGNT7DF4DVNKYWDFZC8DEM` already landed changes in provider strategy and provider bulk test surfaces.

Missing examples / edge cases
- The contract does not explain the handling path when the intended implementation is already present on `develop` but the ticket remains in a pre-development `todo` state.
- If the remaining work is only closure, release posture, or validation alignment, that edge case is not modeled explicitly in this ticket and is currently implied only through branch history.

Risky assumptions
- Assumes a developer can take meaningful implementation action from this branch even though `git diff --name-only develop..HEAD` is metadata-only.
- Assumes done child `06F2PGNT7DF4DVNKYWDFZC8DEM` is only live-coverage work despite its integration commit changing provider strategy and provider bulk test files.
- Assumes readers will ignore stale older planning prose about Oracle scope and provider registration behavior in favor of current source and architecture notes.

AC / test suggestions
- If this remains a pre-dev story, add one acceptance criterion that names the exact remaining code delta relative to `develop`.
- If the remaining work is only validation/closure, replace implementation-oriented acceptance criteria with state/closure criteria and route the ticket accordingly.

Implementation watchouts
- `docs/plans/provider-optimization-closure-alignment-follow-up.md` is stale against current source and `docs/architecture/dvault-v1-explicit-save-service.md`; downstream docs work should reconcile Oracle scope and provider registration posture.
- Any remaining work must avoid reopening the bulk SPI or fallback-writer boundaries already assigned to done tickets `06F2PGMSQ4D4FV8W5ZERD4GS8C` and `06F2PGN4GPQCGC5WHZQBGP4SD0`.

Non-blocking notes
- The persisted delivery contract is otherwise specific and internally structured well: `## Open Questions` is `none`, and the AC/DoD sections are concrete.
- Ticket comments are bot-only claim/refinement/handoff comments; there is no human clarification thread resolving the branch-history mismatch.
- An additional done blocker relation exists at `.gicket/relations/J0/K4/06F2PGK4QJ0YGXK5479W83Z2J0--06F2PGNGVQ3TZZWSABAK5SNFK4--blocks.json`; automation treated it as obsolete because the source ticket is already `done` on `develop`.

Split recommendations
- If actual implementation work is finished, do not send this ticket to dev as-is; convert it to closure/no-work and let docs `06F2PGP2B2RZGGK3CVKK5WRRP8` and benchmarks `06F2PGNZBRNCQ1SV2KKP6F3BA8` carry the remaining follow-up.
- If implementation work is not finished, split the still-unlanded delta from the already-landed provider strategy/test history so developer ownership maps to real code changes.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment