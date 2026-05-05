[gicket-bot] PO refinement contract

Summary
- The remaining PO blockers are resolved: follow-up story 06EZEHCCMBFDGW35YGR5D20EEW is the persisted closure-alignment owner, and the current benchmark README aligns with README.md and docs/architecture/dvault-v1-explicit-save-service.md on release posture, so this epic can continue as a tracking-only closure epic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- Persisted closure-alignment path: follow-up story 06EZEHCCMBFDGW35YGR5D20EEW supersedes the stale closure narrative in 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0NB4965QZZYG0Z1PG5YY7C, and 06EZ0NCAFFJSSRFFEG66AYG8XC for epic-closure purposes.: `answered` - Answered. Follow-up story 06EZEHCCMBFDGW35YGR5D20EEW is already the persisted owner of the remaining closure-alignment work, so this parent epic should treat the stale closure prose in those older done stories as superseded for epic-closure proof and rely on the follow-up ticket for the remaining alignment slice.
- Repository-document alignment is still incomplete because benchmarks/DCoding.Data.DVault.Benchmarks/README.md remains inconsistent with README.md and docs/architecture/dvault-v1-explicit-save-service.md on SQL Server, Oracle, and MySQL release posture.: `answered` - Answered. The current benchmarks README now states that SQL Server, Oracle, and MySQL are omitted from the v1 benchmark artifact without treating that omission as release posture, and it preserves the bounded Oracle optimized path plus provider-neutral fallback. That matches the root README and the architecture note.

Clarifications
- 06EZ0MHBC3DGRJCHQ91E89HABM remains a tracking-only closure epic and does not regain any direct implementation slice.
- Existing child-owned delivery slices remain 06EZ0N8HW9PZAFKMM5WQD564VR, 06EZ0N9TJSXFXH0YZRA3QN2S14, 06EZ0NADTKZP9J1YCVNMDH60WC, 06EZ0NB4965QZZYG0Z1PG5YY7C, 06EZ0NBPWEWAP264B4XP36CXC8, and 06EZ0NCAFFJSSRFFEG66AYG8XC.
- Follow-up story 06EZEHCCMBFDGW35YGR5D20EEW is already materialized as the persisted closure-alignment owner and is the approved superseding path for stale child and doc closure prose.
- Five-provider save-strategy support and the narrower capability-profile auto-registration surface remain distinct; visible provider-name capability-profile auto-registration is still source-evidenced only for SQLite and MySQL.
- The benchmark README is now aligned with README.md and docs/architecture/dvault-v1-explicit-save-service.md on release posture and benchmark scope.
- Oracle optimization remains intentionally narrower: only clean Oracle.EntityFrameworkCore hub/link batches use the optimized path, and unsupported shapes fall back through the provider-neutral writer.

Scope In
- Track epic closure through the existing child-owned slices and follow-up story 06EZEHCCMBFDGW35YGR5D20EEW rather than through parent-owned implementation work.
- Ratify the five-provider save-strategy baseline with provider-neutral fallback for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Require closure-time consistency across the parent contract, the follow-up closure-alignment path, README.md, docs/architecture/dvault-v1-explicit-save-service.md, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md.
- Require the Oracle closure narrative to match the visible clean-context hub/link-only optimized boundary plus provider-neutral fallback for unsupported shapes.

Scope Out
- Adding new parent-owned implementation work for provider save strategies or metadata-profile registration.
- Adding new provider-name capability-profile auto-registration for PostgreSQL, SQL Server, or Oracle in this epic.
- Requiring SQL Server, Oracle, or MySQL benchmark rows in the current v1 benchmark artifact.
- Introducing CI-managed or mandatory unattended external database provisioning for PostgreSQL, SQL Server, Oracle, or MySQL.
- Widening Oracle optimization to satellite operations or other broader provider-aware metadata work in this epic.

Open questions
- none

Follow-up questions
- Should a later follow-up add provider-name capability-profile auto-registration for PostgreSQL, SQL Server, and Oracle?
- After v0.5, should benchmark coverage expand beyond the required SQLite baseline and optional PostgreSQL comparison path to include SQL Server, Oracle, and MySQL?
- Should later infrastructure work provision CI-managed external database lanes instead of keeping PostgreSQL, SQL Server, Oracle, and MySQL validation developer-managed and opt-in?
- Should Oracle satellite optimization be handled in a separate future ticket if broader Oracle optimization is needed?

Risks
- If later child or follow-up closure prose drifts again from source-evidenced behavior, the epic can regress into closure-audit inconsistency.
- Consumers may still incorrectly infer uniform metadata-profile auto-selection from five-provider save-strategy support unless the narrower registration surface remains explicit in closure prose.
- Oracle's optimized path remains intentionally narrower and continues to rely on provider-neutral fallback for dirty contexts or request batches containing unsupported shapes.
- Developer-managed opt-in validation still means unattended default validation does not exercise every external-provider lane end to end.

Split recommendations
- No additional PO split is needed for this clarification pass; continue to use follow-up story 06EZEHCCMBFDGW35YGR5D20EEW as the dedicated closure-alignment slice.
- Keep broader profile auto-registration parity, wider benchmark coverage, CI or database provisioning, and Oracle satellite optimization as separate future tickets rather than widening this parent epic again.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment