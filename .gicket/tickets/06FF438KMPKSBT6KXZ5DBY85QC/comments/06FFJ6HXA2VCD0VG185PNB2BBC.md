[gicket-bot] PO refinement contract

Summary
- Reclassified the ticket as an implementation task: current branch evidence does not show landed PIT maintenance timing-row documentation, so closure-only handling is rejected and the remaining work is the required contract update.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The closure-only expectation is removed. This ticket remains an implementation task because the current branch context does not show landed documentation changes for PIT maintenance timing rows; the required next step is development work on the evidence-contract docs, not closure.
- critic-item-2: `answered` - Closure-only handling is not intended. No exact landed commit/path evidence for completed PIT maintenance timing-row documentation is present in the provided current branch context, so the contract keeps the documentation update as unapplied implementation scope instead of claiming closure.
- critic-item-3: `answered` - Accepted. The branch evidence in this prompt does not contain the required maintenance-row documentation changes, so the Definition of Done is not currently satisfied and the ticket must stay framed as pending documentation implementation.
- critic-item-4: `answered` - The routing mismatch is corrected by explicitly classifying this as an implementation-task refinement. The contract keeps the unapplied documentation work in scope on the existing matrix and supporting-contract surfaces and no longer leaves room for a closure-only interpretation.

Clarifications
- This ticket is not a closure-only audit. It owns documentation implementation that is still unapplied in the current branch context.
- The authoritative surface remains the existing provider evidence matrix, with a shared benchmark-artifact contract edit only if maintenance-row field wording needs explicit clarification there.
- The bounded v1 maintenance scope stays limited to PIT full-rebuild evidence for the provider-neutral comparator plus the PostgreSQL and SQL Server provider-specific lanes already split to sibling tickets 06FF43BPP5NRJR3JTY48ZNEKHM, 06FF43AH9SK6J07GV5EKYV3AMM, and 06FF43AYQYZKFF400CK5Q84WYR.
- No child-ticket, relation, description, attachment, or planning-document write was applied in this refinement pass.

Scope In
- Update the provider evidence contract so PIT full-rebuild maintenance timing rows are explicitly distinct from existing `pit-as-of-read` and `bridge-traversal-read` rows.
- Define the maintenance-row contract slice for the provider-neutral comparator lane plus the PostgreSQL and SQL Server provider-specific PIT full-rebuild lanes.
- Require each maintenance timing claim to capture scenario identity, provider, baseline/comparator identity, selected strategy or provider-neutral fallback posture, bounded fallback causes when present, run context, and artifact-triplet links.
- Keep the contract aligned with the existing PIT maintenance boundary: PostgreSQL full rebuilds may cover ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs; SQL Server full rebuilds stay limited to clean ordinary hub-parent PITs.

Scope Out
- Landing the benchmark artifacts or completed timing rows themselves; sibling tickets own the provider-neutral, PostgreSQL, and SQL Server evidence generation.
- Bridge-maintenance push-down or bridge-maintenance timing rows.
- PIT `MaintainParentsAsync(...)` timing expansion.
- New provider scope beyond the current PostgreSQL and SQL Server PIT maintenance boundary.
- Automatic maintenance, scheduling, EF `SaveChanges` interception, or other runtime behavior changes.

Open questions
- none

Follow-up questions
- If later work wants PIT parent-maintenance timing claims, should it add separate maintenance scenarios instead of overloading the PIT full-rebuild row family?
- If bridge-maintenance push-down is reopened later, should it extend the same matrix only after a dedicated provider seam, bridge-specific diagnostics vocabulary, parity coverage, and benchmark artifact triplet exist?

Risks
- If maintenance scenario naming remains ambiguous in the docs update, downstream work could continue to misread `pit-as-of-read` rows as PIT maintenance evidence.
- Until sibling tickets land preserved provider-neutral, PostgreSQL, and SQL Server maintenance artifacts, this ticket can only define the contract and cannot itself promote completed maintenance timing claims.
- If the evidence matrix and shared benchmark artifact contract diverge on required maintenance-row fields or token mapping, later evidence rows could become inconsistent across lanes.

Split recommendations
- No additional split is justified; the existing decomposition already separates this contract update from the provider-neutral comparator row work and the PostgreSQL and SQL Server provider-specific benchmark lanes.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment