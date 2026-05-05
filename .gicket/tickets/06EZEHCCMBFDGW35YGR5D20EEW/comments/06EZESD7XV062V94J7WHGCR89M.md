[gicket-bot] PO refinement contract

Summary
- Verified ticket, relation, comment, and repository evidence for 06EZEHCCMBFDGW35YGR5D20EEW. The story is already materialized with parent/block relations and a planning document, and the remaining PO contract is a single bounded documentation and closure-alignment pass with no blocking open questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- README.md already sets the safe v1 default: five provider-specific save-strategy entry points exist (`AddDVaultSqlite`, `AddDVaultPostgres`, `AddDVaultSqlServer`, `AddDVaultOracle`, `AddDVaultMySql`) and all keep the provider-neutral `AddDVault()` writer as the caller-visible fallback.
- Visible provider-name capability-profile auto-registration is narrower than the save-strategy surface: `DataVaultProviderCapabilityProfileSelection.Register(...)` is evidenced in the SQLite and MySQL startup extensions only, not in the Postgres, SQL Server, or Oracle startup extensions.
- Oracle is not compatibility-only in the current source baseline: `OracleDataVaultSaveStrategy` owns an optimized path for clean `Oracle.EntityFrameworkCore` hub/link batches and declines unsupported shapes so the provider-neutral writer handles them.
- Superseding the stale closure narrative in `06EZ0N8HW9PZAFKMM5WQD564VR`, `06EZ0NB4965QZZYG0Z1PG5YY7C`, and `06EZ0NCAFFJSSRFFEG66AYG8XC` does not require reopening those done tickets; this follow-up story and the aligned repo docs become the epic-closure source of truth.
- Persisted planning context already exists: `docs/plans/provider-optimization-closure-alignment-follow-up-06EZ0MHBC3DGRJCHQ91E89HABM.md` is present, the epic has an incoming `parentOf` relation to this story, this story has an outgoing `blocks` relation to the epic, and the ticket has no human comments adding new scope.

Scope In
- Align closure-facing documentation across `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` to one release posture.
- Ratify the current release baseline as five provider-specific save-strategy entry points plus provider-neutral fallback.
- Correct provider-name capability-profile auto-registration claims so they match the visible startup code surface.
- Document Oracle's intentionally narrower optimized scope and fallback behavior.
- Make this story the single cited owner of the remaining provider-optimization closure blocker for epic `06EZ0MHBC3DGRJCHQ91E89HABM`.

Scope Out
- Implementing new provider save strategies, fallback behavior, or capability-profile registration code.
- Expanding the benchmark runner or report artifact to add SQL Server, Oracle, or MySQL rows.
- Changing NuGet publish automation, package versions, or release-process tooling.
- Reopening completed provider stories for code changes instead of superseding their stale closure narrative here.

Open questions
- none

Follow-up questions
- After this closure-alignment story lands, should the epic review checklist explicitly reference this story instead of the earlier done stories to prevent stale closure prose from being quoted again?
- Should a later non-blocking documentation or benchmark story explain future conditions for adding SQL Server, Oracle, or MySQL rows to the benchmark artifact once that comparison scope is intentionally expanded?
- Should provider-name capability-profile auto-registration remain intentionally narrow outside SQLite and MySQL, or should a separate future implementation story evaluate expanding it for PostgreSQL, SQL Server, or Oracle?

Risks
- The benchmark README is the highest-risk stale artifact; if it is only partially updated, reviewers may keep reading absent benchmark rows as proof of compatibility-only provider posture.
- The architecture note currently mixes correct save-strategy posture with overstated capability-registration language; a shallow edit could fix one contradiction while leaving the other in place.
- Because the superseded stories remain historically done, epic reviewers may still quote them unless this story and the updated docs are treated as the current closure authority.

Split recommendations
- No further split is recommended. The remaining work is one bounded closure-alignment pass across existing docs and closure narrative, already backed by ticket `06EZEHCCMBFDGW35YGR5D20EEW`, its epic relations, and the persisted planning document.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment