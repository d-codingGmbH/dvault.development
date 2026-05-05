[gicket-bot] PO-critic review contract

Summary
- Ready for dev: the contract is bounded to a documentation-only closure-alignment pass, has no open questions, and is grounded in direct repo/ticket evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZEHCCMBFDGW35YGR5D20EEW/description.md contains '## Open Questions' followed by '- none'.
- .gicket/relations/BM/EW/06EZ0MHBC3DGRJCHQ91E89HABM--06EZEHCCMBFDGW35YGR5D20EEW--parentOf.json and .gicket/relations/EW/BM/06EZEHCCMBFDGW35YGR5D20EEW--06EZ0MHBC3DGRJCHQ91E89HABM--blocks.json materialize the epic/story link.
- docs/plans/provider-optimization-closure-alignment-follow-up-06EZ0MHBC3DGRJCHQ91E89HABM.md exists and says the follow-up was materialized as story 06EZEHCCMBFDGW35YGR5D20EEW.
- README.md lists AddDVaultSqlite, AddDVaultPostgres, AddDVaultSqlServer, AddDVaultOracle, and AddDVaultMySql, and says incompatible provider strategies fall back to the provider-neutral writer.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs and src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs call DataVaultProviderCapabilityProfileSelection.Register(...); the Postgres, SqlServer, and Oracle service-collection extensions do not.
- src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs gates Oracle optimization to provider Oracle.EntityFrameworkCore, clean contexts, and request batches with no satellite operations; src/DCoding.Data.DVault/DataVaultSaveService.cs iterates strategies and falls back to the provider-neutral writer when none accept the batch.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md still says SQL Server, Oracle, and MySQL are compatibility-only, and docs/architecture/dvault-v1-explicit-save-service.md still says 'Oracle capability registration plus ...'; those are the concrete stale contradictions this story is scoped to fix.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- When the docs explain Oracle fallback, call out the two source-visible rejection shapes: dirty tracked EF changes and request batches containing satellite operations.
- When the docs explain capability-profile auto-registration, keep it separate from save-strategy registration so readers do not infer broader startup wiring.

Risky assumptions
- The contract assumes the three named docs and this story are the only closure artifacts reviewers will use; implementation should search for any remaining compatibility-only or capability-registration prose before closing the story.

AC / test suggestions
- Map each acceptance criterion to one direct source proof: README provider package section, the five provider startup extension files, DataVaultSaveService.cs, and OracleDataVaultSaveStrategy.cs.
- Search the three in-scope docs for compatibility-only and capability-registration wording before closing the story.
- Verify the benchmark README ends by framing SQLite as required benchmark scope and PostgreSQL as optional benchmark scope, not as a statement about release support for SQL Server, Oracle, or MySQL.

Implementation watchouts
- The current architecture note is mostly aligned already; the highest-risk stale line there is the Oracle ownership bullet that claims capability registration not proven by visible startup code.
- The benchmark README contains the strongest misleading statement because it explicitly calls SQL Server, Oracle, and MySQL compatibility-only.
- Do not blur save-strategy availability with provider-name capability-profile auto-registration; the ticket deliberately separates those surfaces.

Non-blocking notes
- README.md already matches the intended five-provider release posture, so the remaining work is bounded rather than exploratory.
- Comment 06EZESRHR4MTK0JV7ZY1C6F4C8.md shows an epic follow-up comment was queued for the epic owner branch, but the parentOf and blocks relation files already exist.

Split recommendations
- No split recommended; the observed contradictions are limited to existing documentation and closure narrative, and the contract already bounds the work to that alignment pass.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment