[gicket-bot] PO refinement contract

Summary
- Refined the blocker task as a bounded PIT maintenance evidence-contract update: extend the existing provider evidence matrix/supporting contract for PIT full-rebuild timing rows, keep bridge and parent-maintenance expansion out of scope, and leave the concrete benchmark rows to the already-split sibling tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The canonical surface remains the existing provider evidence matrix and its supporting contract; this ticket should extend that surface instead of creating a parallel maintenance-evidence document.
- The bounded v1 maintenance timing scope is PIT full-rebuild evidence only, aligned to sibling tickets `06FF43AH9SK6J07GV5EKYV3AMM`, `06FF43AYQYZKFF400CK5Q84WYR`, and `06FF43BPP5NRJR3JTY48ZNEKHM`.
- Repository evidence already fixes the provider boundary: PostgreSQL full-rebuild rows may cover ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs; SQL Server full-rebuild rows stay limited to clean ordinary hub-parent rebuilds.
- No child-ticket, relation, description, attachment, or planning-document write was applied in this refinement pass.

Scope In
- Document maintenance timing rows so they are clearly distinct from existing `pit-as-of-read` and `bridge-traversal-read` evidence rows.
- Define the contract slice for PIT full-rebuild timing rows across the provider-neutral comparator, PostgreSQL optimized lane, and SQL Server optimized lane.
- Require each maintenance timing row to carry scenario, provider, baseline/comparator identity, selected strategy or provider-neutral fallback posture, bounded fallback causes when present, run context, and artifact-triplet links.
- Keep maintenance timing claims gated by preserved benchmark artifacts and the shared benchmark artifact contract rather than prose-only notes or skipped placeholders.

Scope Out
- Bridge maintenance push-down or bridge-maintenance timing rows.
- PIT/bridge read-row rewrites beyond making maintenance rows non-confusable with existing read evidence.
- Automatic maintenance, scheduling, `SaveChanges` interception, or background orchestration.
- PIT `MaintainParentsAsync(...)` timing expansion unless a later ticket adds separate benchmark lanes and contract language.
- New provider scope beyond the current PostgreSQL and SQL Server PIT full-rebuild evidence slice.

Open questions
- none

Follow-up questions
- If later work wants PIT parent-maintenance timing claims, should it add separate maintenance scenarios instead of overloading the full-rebuild row family?
- If bridge-maintenance push-down is reopened later, should it extend the same matrix only after a dedicated provider seam, bridge-specific diagnostics vocabulary, parity coverage, and benchmark artifact triplet exist?

Risks
- If maintenance scenario naming stays ambiguous in implementation, downstream docs could continue misreading `pit-as-of-read` rows as maintenance evidence.
- Until `06FF43AH9SK6J07GV5EKYV3AMM` and `06FF43AYQYZKFF400CK5Q84WYR` land preserved provider-configured artifacts, this ticket can define the contract but cannot itself satisfy provider timing claims.
- If `06FF43BPP5NRJR3JTY48ZNEKHM` does not normalize the provider-neutral comparator row, fallback-cause comparisons across providers may remain inconsistent.

Split recommendations
- No additional split is justified; the existing decomposition already separates the contract task from PostgreSQL lane, SQL Server lane, and provider-neutral comparator-row work.

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