[gicket-bot] PO-critic review contract

Summary
- Approve: the ticket contract is explicit, bounded, and repo-consistent for a pre-development tracking story; no open questions remain.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4RJ4CC2YRVK0P98NBSXRKC/description.md contains `## Open Questions` = `none`, five acceptance criteria, four definition-of-done items, and split recommendations covering tickets 06FE4RJD5Z6MWC2E66YB3EZ5YW, 06FE4RK80ZXGCZ62CMSAYP164W, 06FE4RJP5KG02DF7AEMCQYGNVW, 06FE4RJZ4PA0DZ3HXDSEG2BQMM, and 06FE4RKGASKV6F7DF0RD1WTAV4.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService` inside `AddDVault()`, confirming today's maintenance boundary is provider-neutral and caller-invoked.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs and src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs register provider save/read strategies only; neither file shows a provider-specific maintenance strategy registration seam.
- docs/architecture/dvault-v1-pit-bridge-boundary.md states PIT/bridge tables are explicit read models, reads consume already-maintained rows, unsupported or stale cases fall back to provider-neutral reads, and `Unsupported In V1` includes provider-specific PIT or bridge maintenance strategies.
- docs/plans/provider-specific-sql-artifact-contract.md keeps provider-authored SQL artifacts design-time only and explicitly excludes runtime dispatch, automatic deployment, migration synchronization, and a standalone DVault CLI, matching this ticket's non-goals.
- `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD` returns only `.gicket/tickets/06FE4RJ4CC2YRVK0P98NBSXRKC/...` files, and commit `e978a7fc02` is the PO->PO-critic handoff updating ticket metadata rather than repo source/docs.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Downstream work should make the dry-run diagnostics examples concrete for each stop path: unknown provider, unsupported maintenance shape, missing diagnostics evidence, and strategy-declined fallback.
- If bridge diagnostics are allowed before bridge feasibility closes, child tickets should state whether those diagnostics may only report declined paths or may also report hypothetical translated targets without implying implementation.

Risky assumptions
- This approval assumes the parent remains a boundary/tracking story and that downstream child tickets inherit its provider baseline and automation non-goals instead of redefining them.

AC / test suggestions
- Keep an explicit acceptance/test check that maintenance dry-run diagnostics may identify translated target tables and provider selection/fallback, but must not include raw SQL, query plans, request values, or credentials.
- Keep a downstream parity check that explicit `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService` entry points remain the caller-visible API even when a provider prototype performs server-side work internally.
- Add a downstream diagnostics test matrix for provider-neutral fallback causes across unknown provider, unsupported shape, missing evidence, and declined strategy.

Implementation watchouts
- Current core registration exposes provider-neutral maintenance services only through `AddDVault()`; any shared provider-specific maintenance seam is new API design work, not an already-approved extension point.
- The existing PIT/bridge boundary already treats stale maintenance evidence as a read fallback signal; child tickets should not blur read dispatch fallback with maintenance execution semantics.
- src/DCoding.Data.DVault/DataVaultActivityTracing.cs defines maintenance span names and generic failure tags, but no maintenance-specific fallback-cause vocabulary, so diagnostics/telemetry additions need a finite shared contract.
- The existing SQL artifact contract is review-only design-time output; push-down diagnostics or prototypes must not imply runtime dispatch, deployment orchestration, startup hooks, or background scheduling.

Non-blocking notes
- The current branch is ticket-metadata-only so far: `git diff --name-only develop...HEAD` shows `.gicket/tickets/06FE4RJ4CC2YRVK0P98NBSXRKC/...` only.
- The referenced follow-on tickets exist and match the intended decomposition, but they are not yet PO-refined individually; treat this parent as the approved boundary, not as proof that each child ticket is ready to execute today.

Split recommendations
- Keep 06FE4RJD5Z6MWC2E66YB3EZ5YW (PIT dry-run diagnostics), 06FE4RK80ZXGCZ62CMSAYP164W (bridge feasibility), 06FE4RJP5KG02DF7AEMCQYGNVW (PostgreSQL PIT prototype), 06FE4RJZ4PA0DZ3HXDSEG2BQMM (SQL Server PIT prototype), and 06FE4RKGASKV6F7DF0RD1WTAV4 (documentation updates) as separate follow-on tickets.
- Do not open a bridge implementation ticket until 06FE4RK80ZXGCZ62CMSAYP164W closes the bounded feasibility question.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment