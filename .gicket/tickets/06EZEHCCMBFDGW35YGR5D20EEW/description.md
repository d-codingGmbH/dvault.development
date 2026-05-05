<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified ticket, relation, comment, and repository evidence for 06EZEHCCMBFDGW35YGR5D20EEW. The story is already materialized with parent/block relations and a planning document, and the remaining PO contract is a single bounded documentation and closure-alignment pass with no blocking open questions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- README.md already sets the safe v1 default: five provider-specific save-strategy entry points exist (`AddDVaultSqlite`, `AddDVaultPostgres`, `AddDVaultSqlServer`, `AddDVaultOracle`, `AddDVaultMySql`) and all keep the provider-neutral `AddDVault()` writer as the caller-visible fallback.
- Visible provider-name capability-profile auto-registration is narrower than the save-strategy surface: `DataVaultProviderCapabilityProfileSelection.Register(...)` is evidenced in the SQLite and MySQL startup extensions only, not in the Postgres, SQL Server, or Oracle startup extensions.
- Oracle is not compatibility-only in the current source baseline: `OracleDataVaultSaveStrategy` owns an optimized path for clean `Oracle.EntityFrameworkCore` hub/link batches and declines unsupported shapes so the provider-neutral writer handles them.
- Superseding the stale closure narrative in `06EZ0N8HW9PZAFKMM5WQD564VR`, `06EZ0NB4965QZZYG0Z1PG5YY7C`, and `06EZ0NCAFFJSSRFFEG66AYG8XC` does not require reopening those done tickets; this follow-up story and the aligned repo docs become the epic-closure source of truth.
- Persisted planning context already exists: `docs/plans/provider-optimization-closure-alignment-follow-up-06EZ0MHBC3DGRJCHQ91E89HABM.md` is present, the epic has an incoming `parentOf` relation to this story, this story has an outgoing `blocks` relation to the epic, and the ticket has no human comments adding new scope.

### Scope In
- Align closure-facing documentation across `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` to one release posture.
- Ratify the current release baseline as five provider-specific save-strategy entry points plus provider-neutral fallback.
- Correct provider-name capability-profile auto-registration claims so they match the visible startup code surface.
- Document Oracle's intentionally narrower optimized scope and fallback behavior.
- Make this story the single cited owner of the remaining provider-optimization closure blocker for epic `06EZ0MHBC3DGRJCHQ91E89HABM`.

### Scope Out
- Implementing new provider save strategies, fallback behavior, or capability-profile registration code.
- Expanding the benchmark runner or report artifact to add SQL Server, Oracle, or MySQL rows.
- Changing NuGet publish automation, package versions, or release-process tooling.
- Reopening completed provider stories for code changes instead of superseding their stale closure narrative here.

## Acceptance Criteria
- The closure posture in `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` is internally consistent about current provider support.
- No closure prose in the aligned docs or this story describes SQL Server, Oracle, or MySQL as compatibility-only packages in the current save-strategy baseline.
- No closure prose claims provider-name capability-profile auto-registration for Oracle, PostgreSQL, or SQL Server; only the SQLite and MySQL auto-registration surface is described as evidenced by the visible startup code.
- Oracle documentation explicitly states that optimized behavior is limited to clean `Oracle.EntityFrameworkCore` hub/link batches and that unsupported shapes fall back through the provider-neutral writer.
- The benchmark README explains that SQLite baseline rows and optional PostgreSQL rows are a benchmark-scope choice, not a claim that SQL Server, Oracle, or MySQL lack provider-specific optimized strategies.
- Epic `06EZ0MHBC3DGRJCHQ91E89HABM` can cite this story as the persisted owner of the remaining closure-alignment blocker without reopening prior done stories.

## Definition of Done
- The repository docs named above no longer contradict one another on provider optimization posture, capability-profile auto-registration scope, or benchmark scope.
- A reviewer can verify each remaining claim directly from the visible startup extension files and `OracleDataVaultSaveStrategy` without inferring unsupported provider behavior.
- The existing planning document and ticket relation set remain sufficient; no additional child ticket split is required for this story.
- The story contract clearly supersedes the stale closure narrative from the earlier done stories for epic-closure review.

## Implementation Notes
- Use the current `README.md` provider package section as the default posture anchor: five provider-specific entry points exist today, but provider-name capability-profile auto-registration is only visibly wired for SQLite and MySQL.
- Update the architecture note's capability matrix and ownership bullets so Oracle, SQL Server, and PostgreSQL are not described as owning provider-name capability-profile registration unless the visible startup code proves it.
- Update `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` to separate benchmark artifact scope from release posture: SQL Server, Oracle, and MySQL are out of the v1 benchmark artifact, not out of the provider-optimization baseline.
- Preserve the Oracle nuance already proved by source: optimized clean hub/link batches only, with unsupported request shapes routed through the provider-neutral fallback writer.
- No new planning writes were needed during refinement because the story, its `parentOf` and `blocks` relations, and the planning document at `docs/plans/provider-optimization-closure-alignment-follow-up-06EZ0MHBC3DGRJCHQ91E89HABM.md` already materialize the bounded plan.

## Open Questions
- none

## Follow-Up Questions
- After this closure-alignment story lands, should the epic review checklist explicitly reference this story instead of the earlier done stories to prevent stale closure prose from being quoted again?
- Should a later non-blocking documentation or benchmark story explain future conditions for adding SQL Server, Oracle, or MySQL rows to the benchmark artifact once that comparison scope is intentionally expanded?
- Should provider-name capability-profile auto-registration remain intentionally narrow outside SQLite and MySQL, or should a separate future implementation story evaluate expanding it for PostgreSQL, SQL Server, or Oracle?

## Risks
- The benchmark README is the highest-risk stale artifact; if it is only partially updated, reviewers may keep reading absent benchmark rows as proof of compatibility-only provider posture.
- The architecture note currently mixes correct save-strategy posture with overstated capability-registration language; a shallow edit could fix one contradiction while leaving the other in place.
- Because the superseded stories remain historically done, epic reviewers may still quote them unless this story and the updated docs are treated as the current closure authority.

## Split Recommendations
- No further split is recommended. The remaining work is one bounded closure-alignment pass across existing docs and closure narrative, already backed by ticket `06EZEHCCMBFDGW35YGR5D20EEW`, its epic relations, and the persisted planning document.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Provider Optimization Closure-Alignment Follow-Up

Parent epic: `06EZ0MHBC3DGRJCHQ91E89HABM`

## Purpose

Persist the exact remaining closure-alignment work that still blocks the provider-optimization epic from returning to PO-critic as a clean tracking-only closure epic.

## Why This Exists

The current epic contract already narrows the parent to tracking-only closure work, but the remaining blockers are still carried only as parent prose:

- done story `06EZ0N8HW9PZAFKMM5WQD564VR` still describes a SQLite-only optimization baseline and compatibility-only posture for PostgreSQL, SQL Server, Oracle, and MySQL
- done story `06EZ0NB4965QZZYG0Z1PG5YY7C` still claims Oracle capability registration that the visible source does not prove
- done story `06EZ0NCAFFJSSRFFEG66AYG8XC` and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` still describe SQL Server, Oracle, and MySQL as compatibility-only packages

The current source and docs baseline is narrower and more specific:

- `README.md` and `docs/architecture/dvault-v1-explicit-save-service.md` describe five provider-specific save-strategy entry points
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` and `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` are the only visible provider startup paths that call `DataVaultProviderCapabilityProfileSelection.Register(...)`
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` register save strategies but do not prove provider-name capability-profile auto-registration
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs` proves an Oracle-optimized path only for clean Oracle hub/link batches and keeps unsupported shapes on provider-neutral fallback

## Required Follow-Up Ticket Contract

Create one follow-up story with a title equivalent to:

`Story: Align provider optimization closure contracts and release posture`

That story should:

1. Supersede the stale closure narrative in `06EZ0N8HW9PZAFKMM5WQD564VR`, `06EZ0NB4965QZZYG0Z1PG5YY7C`, and `06EZ0NCAFFJSSRFFEG66AYG8XC` for epic-closure purposes.
2. Align `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` to one release posture.
3. Preserve the distinction between:
   - five provider-specific save-strategy entry points with provider-neutral fallback
   - the narrower provider-name capability-profile auto-registration surface currently evidenced only for SQLite and MySQL
4. State that Oracle optimization is intentionally narrower than the other provider stories:
   - clean `Oracle.EntityFrameworkCore` hub/link batches are optimized
   - unsupported shapes fall back through the provider-neutral writer

## Acceptance Boundary For The Follow-Up

- no closure prose says SQL Server, Oracle, or MySQL are compatibility-only packages in the current save-strategy baseline
- no closure prose claims Oracle provider-name capability-profile auto-registration that the visible source does not prove
- the benchmark README is consistent with the root README and the architecture note on benchmark scope and release posture
- epic `06EZ0MHBC3DGRJCHQ91E89HABM` can cite this follow-up as the persisted owner of the remaining closure blocker

## Materialization

This plan has been materialized as follow-up story `06EZEHCCMBFDGW35YGR5D20EEW`.

The follow-up is assigned to the same provider-specific release and milestone as the parent epic, is related through `parentOf`, and blocks parent epic `06EZ0MHBC3DGRJCHQ91E89HABM` until the closure-alignment work is complete.

The earlier runtime attempt to create this ticket through `gicket-create-ticket` was rejected with `BOT-LOCAL-TOOL-TRUST-BLOCKED`; the bot fix should preserve persistent planning artifacts during stop-the-line writeback so this kind of document is not left as an untracked local file again.