<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- The remaining PO blockers are resolved: follow-up story 06EZEHCCMBFDGW35YGR5D20EEW is the persisted closure-alignment owner, and the current benchmark README aligns with README.md and docs/architecture/dvault-v1-explicit-save-service.md on release posture, so this epic can continue as a tracking-only closure epic.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- 06EZ0MHBC3DGRJCHQ91E89HABM remains a tracking-only closure epic and does not regain any direct implementation slice.
- Existing child-owned delivery slices remain 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0N9TJSXFXH0YZRA3QN2S14, 06EZ0NADTKZP9J1YCVNMDH60WC, 06EZ0NB4965QZZYG0Z1PG5YY7C, 06EZ0NBPWEWAP264B4XP36CXC8, and 06EZ0NCAFFJSSRFFEG66AYG8XC.
- Follow-up story 06EZEHCCMBFDGW35YGR5D20EEW is already materialized as the persisted closure-alignment owner and is the approved superseding path for stale child and doc closure prose.
- Five-provider save-strategy support and the narrower capability-profile auto-registration surface remain distinct; visible provider-name capability-profile auto-registration is still source-evidenced only for SQLite and MySQL.
- The benchmark README is now aligned with README.md and docs/architecture/dvault-v1-explicit-save-service.md on release posture and benchmark scope.
- Oracle optimization remains intentionally narrower: only clean Oracle.EntityFrameworkCore hub/link batches use the optimized path, and unsupported shapes fall back through the provider-neutral writer.

### Scope In
- Track epic closure through the existing child-owned slices and follow-up story 06EZEHCCMBFDGW35YGR5D20EEW rather than through parent-owned implementation work.
- Ratify the five-provider save-strategy baseline with provider-neutral fallback for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Require closure-time consistency across the parent contract, the follow-up closure-alignment path, README.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md.
- Require the Oracle closure narrative to match the visible clean-context hub/link-only optimized boundary plus provider-neutral fallback for unsupported shapes.

### Scope Out
- Adding new parent-owned implementation work for provider save strategies or metadata-profile registration.
- Adding new provider-name capability-profile auto-registration for PostgreSQL, SQL Server, or Oracle in this epic.
- Requiring SQL Server, Oracle, or MySQL benchmark rows in the current v1 benchmark artifact.
- Introducing CI-managed or mandatory unattended external database provisioning for PostgreSQL, SQL Server, Oracle, or MySQL.
- Widening Oracle optimization to satellite operations or other broader provider-aware metadata work in this epic.

## Acceptance Criteria
- The parent contract explicitly states that this ticket is a tracking-only closure epic with no remaining direct implementation slice.
- The parent cites follow-up story 06EZEHCCMBFDGW35YGR5D20EEW as the persisted owner of the remaining closure-alignment work and does not treat stale prose in 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0NB4965QZZYG0Z1PG5YY7C, or 06EZ0NCAFFJSSRFFEG66AYG8XC as closure proof.
- README.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md consistently distinguish five provider-specific save-strategy entry points with provider-neutral fallback from the narrower provider-name capability-profile auto-registration surface.
- Closure prose states that provider-name capability-profile auto-registration is source-evidenced only for SQLite and MySQL and does not claim Oracle, PostgreSQL, or SQL Server parity.
- Closure prose states that SQLite is the required local benchmark baseline, PostgreSQL is the only optional external benchmark participant in v1, and the absence of SQL Server, Oracle, and MySQL rows in the benchmark artifact is not a release-posture claim.
- Closure prose states that Oracle optimization is limited to clean Oracle.EntityFrameworkCore hub/link batches and that unsupported shapes fall back through the provider-neutral writer.

## Definition of Done
- The parent description remains tracking-only and names the existing child-owned slices plus follow-up story 06EZEHCCMBFDGW35YGR5D20EEW as the persisted closure-alignment owner.
- The epic no longer carries unresolved PO blockers about closure ownership or benchmark README alignment.
- The visible closure-evidence documents remain mutually consistent on release posture, benchmark scope, the narrower capability-profile registration surface, and Oracle fallback boundaries.
- Epic closure depends on child and follow-up completion for any remaining closure work rather than on new parent implementation.
- Broader auto-registration parity, wider benchmark coverage, CI provisioning, and Oracle satellite optimization remain separate follow-up scope unless reopened explicitly.

## Implementation Notes
- Use follow-up story 06EZEHCCMBFDGW35YGR5D20EEW as the persisted owner of stale closure-alignment work; for epic-closure purposes it supersedes stale closure prose in 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0NB4965QZZYG0Z1PG5YY7C, and 06EZ0NCAFFJSSRFFEG66AYG8XC.
- README.md and docs/architecture/dvault-v1-explicit-save-service.md describe the five provider-specific save-strategy baseline, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md now matches that release posture while keeping benchmark scope narrower.
- src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs keeps explicit UseDataVault(providerCapabilities) plus ApplyDataVaultMetadata(...) as the metadata-profile selection surface.
- Visible provider-name capability-profile registration exists in src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs and src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs; the visible PostgreSQL, SQL Server, and Oracle startup paths register save strategies only.
- src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs keeps the bounded Oracle optimized path: Oracle.EntityFrameworkCore provider name, clean DbContext, and hub/link-only request batches without satellite operations before optimized execution is allowed.

## Open Questions
- none

## Follow-Up Questions
- Should a later follow-up add provider-name capability-profile auto-registration for PostgreSQL, SQL Server, and Oracle?
- After v0.5, should benchmark coverage expand beyond the required SQLite baseline and optional PostgreSQL comparison path to include SQL Server, Oracle, and MySQL?
- Should later infrastructure work provision CI-managed external database lanes instead of keeping PostgreSQL, SQL Server, Oracle, and MySQL validation developer-managed and opt-in?
- Should Oracle satellite optimization be handled in a separate future ticket if broader Oracle optimization is needed?

## Risks
- If later child or follow-up closure prose drifts again from source-evidenced behavior, the epic can regress into closure-audit inconsistency.
- Consumers may still incorrectly infer uniform metadata-profile auto-selection from five-provider save-strategy support unless the narrower registration surface remains explicit in closure prose.
- Oracle's optimized path remains intentionally narrower and continues to rely on provider-neutral fallback for dirty contexts or request batches containing unsupported shapes.
- Developer-managed opt-in validation still means unattended default validation does not exercise every external-provider lane end to end.

## Split Recommendations
- No additional PO split is needed for this clarification pass; continue to use follow-up story 06EZEHCCMBFDGW35YGR5D20EEW as the dedicated closure-alignment slice.
- Keep broader profile auto-registration parity, wider benchmark coverage, CI or database provisioning, and Oracle satellite optimization as separate future tickets rather than widening this parent epic again.

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