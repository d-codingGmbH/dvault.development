<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Reframed the parent as a tracking-only closure epic over existing children 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0N9TJSXFXH0YZRA3QN2S14, 06EZ0NADTKZP9J1YCVNMDH60WC, 06EZ0NB4965QZZYG0Z1PG5YY7C, 06EZ0NBPWEWAP264B4XP36CXC8, and 06EZ0NCAFFJSSRFFEG66AYG8XC, but kept the ticket in PO because stale child/doc closure language still blocks PO-critic return.

### PO Handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

### Clarifications
- This ticket should be restated as a tracking-only closure epic. It owns cross-child closure verification and contract alignment, not direct implementation work.
- The existing child split is already materialized through parentOf relations: 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0N9TJSXFXH0YZRA3QN2S14, 06EZ0NADTKZP9J1YCVNMDH60WC, 06EZ0NB4965QZZYG0Z1PG5YY7C, 06EZ0NBPWEWAP264B4XP36CXC8, and 06EZ0NCAFFJSSRFFEG66AYG8XC.
- Repository evidence separates two surfaces that must not be conflated: five-provider save-strategy optimization and the narrower capability-profile selection surface used by ApplyDataVaultMetadata(...).
- Source-evidenced provider-name capability-profile auto-registration currently exists only for SQLite and MySQL. PostgreSQL, SQL Server, and Oracle only show save-strategy registration in the visible snapshot.
- Oracle's optimized path is intentionally narrower than the other provider stories: only clean Oracle.EntityFrameworkCore hub/link batches are optimized, and unsupported shapes fall back through the provider-neutral writer.
- Follow-up story 06EZEHCCMBFDGW35YGR5D20EEW has been materialized with parentOf and blocks relations; it now owns the remaining closure-alignment work for stale child/doc closure language.

### Scope In
- Track closure of the provider-optimization epic through the existing child-owned delivery slices rather than through parent-owned implementation work.
- Ratify five-provider save-strategy support plus provider-neutral fallback as the delivery baseline for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Require closure-time agreement across the parent contract, relevant child contracts, README.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md.
- Require the Oracle closure narrative to match visible source by limiting optimized behavior to clean Oracle hub/link batches and preserving fallback for unsupported shapes.
- Track the narrower capability-profile selection baseline as explicit UseDataVault(providerCapabilities) plus only the provider-name auto-registration behavior the source actually proves.

### Scope Out
- Adding new provider-name capability-profile auto-registration for PostgreSQL, SQL Server, or Oracle in this epic.
- Adding new provider save-strategy implementation work directly in the parent ticket.
- Treating SQL Server, Oracle, or MySQL as required benchmark providers in the current v1 artifact.
- CI-managed provisioning or mandatory unattended local databases for PostgreSQL, SQL Server, Oracle, or MySQL.
- Broader provider-aware metadata-profile expansion or Oracle satellite optimization outside separate follow-up work.

## Acceptance Criteria
- The parent contract explicitly states that this ticket is a tracking-only closure epic with no remaining parent-owned implementation slice.
- The parent maps each delivery obligation to child-owned work and treats any remaining closure blocker as child or follow-up scope rather than direct parent implementation scope.
- Parent, child contracts, README.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md consistently distinguish five-provider save-strategy support from the narrower capability-profile selection surface.
- The closure narrative states that AddDVaultSqlite(), AddDVaultPostgres(), AddDVaultSqlServer(), AddDVaultOracle(), and AddDVaultMySql() are provider-specific save-strategy entry points with provider-neutral fallback when CanSave declines.
- The closure narrative states that provider-name capability-profile auto-registration is source-evidenced only for SQLite and MySQL and does not claim Oracle, PostgreSQL, or SQL Server auto-registration.
- The closure narrative states that SQLite is the required local benchmark baseline, PostgreSQL is the only optional external benchmark/reporting participant in v1, and SQL Server, Oracle, and MySQL are not described as compatibility-only packages.
- The Oracle closure narrative matches visible source by limiting optimized behavior to clean Oracle hub/link batches and by keeping unsupported shapes on the provider-neutral fallback.

## Definition of Done
- The parent description marks the epic as tracking-only and names the child-owned delivery slices explicitly.
- The stale compatibility-only or Oracle capability-registration claims in child contracts and repository docs are corrected or superseded by a persisted child or follow-up path before parent closure.
- README.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md present one consistent release posture.
- The child set collectively proves the five-provider save-strategy baseline, the narrower capability-profile selection baseline, and the bounded Oracle fallback rules before the parent returns to PO-critic.
- Broader profile auto-registration parity, wider benchmark coverage, CI provisioning, and Oracle satellite optimization remain outside this epic unless reopened separately.

## Implementation Notes
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs keeps explicit UseDataVault(providerCapabilities) and ApplyDataVaultMetadata(...) as the general metadata-profile selection path.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfileSelection.cs falls back to DataVaultProviderCapabilityProfiles.Sqlite when no provider-name registration exists.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs and src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs are the only visible AddDVault* paths that call DataVaultProviderCapabilityProfileSelection.Register(...).
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs, src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs, and src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs only register provider save strategies in the visible snapshot.
- src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs proves Oracle's clean-context, hub/link-only optimized boundary and fallback requirement for unsupported shapes.
- README.md and docs/architecture/dvault-v1-explicit-save-service.md already describe SQL Server, Oracle, and MySQL as optimized save-strategy packages, but benchmarks/DCoding.Data.DVault.Benchmarks/README.md still carries the stale compatibility-only sentence.
- Done child contracts 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0NB4965QZZYG0Z1PG5YY7C, and 06EZ0NCAFFJSSRFFEG66AYG8XC still contain stale closure language and should not be treated as closure proof until corrected or superseded.

## Open Questions
- Persisted closure-alignment path: follow-up story 06EZEHCCMBFDGW35YGR5D20EEW supersedes the stale closure narrative in 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0NB4965QZZYG0Z1PG5YY7C, and 06EZ0NCAFFJSSRFFEG66AYG8XC for epic-closure purposes.
- Repository-document alignment is still incomplete because benchmarks/DCoding.Data.DVault.Benchmarks/README.md remains inconsistent with README.md and docs/architecture/dvault-v1-explicit-save-service.md on SQL Server, Oracle, and MySQL release posture.

## Follow-Up Questions
- Should a later follow-up ticket add provider-name capability-profile auto-registration for PostgreSQL, SQL Server, and Oracle?
- After v0.5, should benchmark artifacts expand beyond the SQLite required baseline and optional PostgreSQL comparison path to include SQL Server, Oracle, and MySQL?
- Should later infrastructure work provision CI-managed external database lanes for PostgreSQL, SQL Server, Oracle, and MySQL instead of keeping those live validations developer-managed and ProviderIntegration.ExternalOptIn?
- Should Oracle receive a separate follow-up for optimized satellite persistence instead of widening the current hub/link-only optimized boundary?

## Risks
- Leaving the stale benchmark README or stale child contracts in place will continue to mislead readers and will keep causing closure-audit failures.
- Because provider-name capability-profile auto-registration is partial, consumers may wrongly assume five-provider save-strategy support also means uniform metadata-profile auto-selection.
- Oracle's optimized path is intentionally narrower than the other provider strategies, so unsupported request shapes continue to depend on provider-neutral fallback.
- Live validation for PostgreSQL, SQL Server, Oracle, and MySQL remains developer-managed and opt-in, so default unattended validation does not exercise every external-provider lane end to end.

## Split Recommendations
- Use follow-up story 06EZEHCCMBFDGW35YGR5D20EEW as the dedicated closure-alignment slice that fixes README.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md to one release posture.
- Use follow-up story 06EZEHCCMBFDGW35YGR5D20EEW to supersede the Oracle closure claim so no closure prose claims provider-name capability registration that the visible source does not prove.
- Use follow-up story 06EZEHCCMBFDGW35YGR5D20EEW to supersede the SQLite-only optimization baseline wherever it is still being read as closure truth after the provider stories landed.
- Keep broader profile auto-registration parity, wider benchmark coverage, CI or database provisioning, and Oracle satellite optimization as separate follow-up tickets rather than widening the parent again.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: introduce provider-specific persistence optimization for the existing provider projects without weakening the provider-neutral fallback.

Scope:
- Use the existing provider projects for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL as the extension points.
- Keep the core DVault package provider-neutral and deterministic.
- Implement provider capability selection through explicit contracts instead of provider-name string checks in application code.
- Require integration tests or documented opt-in smoke tests for each provider-specific path.
- Require benchmark evidence comparing the optimized path with the provider-neutral fallback and the classic EF baseline where feasible.

Out of scope:
- Automatic package publishing.
- New Data Vault modeling features such as PIT, bridge, or multi-active satellite generation; those are tracked in the deferred capability epic.