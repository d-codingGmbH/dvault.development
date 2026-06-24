<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Reclassified the ticket as an implementation task: current branch evidence does not show landed PIT maintenance timing-row documentation, so closure-only handling is rejected and the remaining work is the required contract update.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is not a closure-only audit. It owns documentation implementation that is still unapplied in the current branch context.
- The authoritative surface remains the existing provider evidence matrix, with a shared benchmark-artifact contract edit only if maintenance-row field wording needs explicit clarification there.
- The bounded v1 maintenance scope stays limited to PIT full-rebuild evidence for the provider-neutral comparator plus the PostgreSQL and SQL Server provider-specific lanes already split to sibling tickets 06FF43BPP5NRJR3JTY48ZNEKHM, 06FF43AH9SK6J07GV5EKYV3AMM, and 06FF43AYQYZKFF400CK5Q84WYR.
- No child-ticket, relation, description, attachment, or planning-document write was applied in this refinement pass.

### Scope In
- Update the provider evidence contract so PIT full-rebuild maintenance timing rows are explicitly distinct from existing `pit-as-of-read` and `bridge-traversal-read` rows.
- Define the maintenance-row contract slice for the provider-neutral comparator lane plus the PostgreSQL and SQL Server provider-specific PIT full-rebuild lanes.
- Require each maintenance timing claim to capture scenario identity, provider, baseline/comparator identity, selected strategy or provider-neutral fallback posture, bounded fallback causes when present, run context, and artifact-triplet links.
- Keep the contract aligned with the existing PIT maintenance boundary: PostgreSQL full rebuilds may cover ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs; SQL Server full rebuilds stay limited to clean ordinary hub-parent PITs.

### Scope Out
- Landing the benchmark artifacts or completed timing rows themselves; sibling tickets own the provider-neutral, PostgreSQL, and SQL Server evidence generation.
- Bridge-maintenance push-down or bridge-maintenance timing rows.
- PIT `MaintainParentsAsync(...)` timing expansion.
- New provider scope beyond the current PostgreSQL and SQL Server PIT maintenance boundary.
- Automatic maintenance, scheduling, EF `SaveChanges` interception, or other runtime behavior changes.

## Acceptance Criteria
- The authoritative provider evidence contract explicitly distinguishes PIT maintenance timing rows from PIT and bridge read rows so read evidence cannot be cited as maintenance evidence.
- The documented maintenance slice is limited to PIT full-rebuild evidence for the provider-neutral comparator plus the PostgreSQL and SQL Server provider-specific lanes already owned by sibling benchmark tickets.
- Maintenance timing claims require scenario, provider, baseline/comparator, selected strategy or provider-neutral fallback posture, bounded fallback causes when present, run context, and links to the supporting benchmark artifact triplet.
- The contract reuses the existing supported-shape boundary: PostgreSQL full rebuilds on ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active PITs; SQL Server full rebuilds on clean ordinary hub-parent PITs only.
- Skipped, unconfigured, diagnostics-only, or docs-only guidance rows are not maintenance timing claims; completed maintenance timing claims require preserved artifact triplets and run context.

## Definition of Done
- The authoritative evidence-contract document set is updated on the existing matrix and supporting-contract surfaces to describe PIT full-rebuild maintenance timing rows without creating a parallel document.
- The updated contract stays consistent with the PIT maintenance boundary in docs/architecture/dvault-v1-pit-bridge-boundary.md and with the shared benchmark artifact contract.
- Sibling benchmark tickets can add provider-neutral, PostgreSQL, and SQL Server PIT full-rebuild maintenance rows without reopening provider boundary, artifact, or non-goal decisions.
- No blocking PO question remains about whether bridge maintenance, parent maintenance, or additional providers belong in this ticket.

## Implementation Notes
- Prefer extending docs/plans/provider-optimization-evidence-matrix.md; touch docs/plans/performance-evidence-benchmark-artifact-contract.md only if maintenance-row token mapping or required field wording needs explicit clarification.
- Reuse the repository-fixed maintenance vocabulary where available, including the PIT full-rebuild semantics already documented in docs/architecture/dvault-v1-pit-bridge-boundary.md.
- Do not promote existing `pit-as-of-read` or `bridge-traversal-read` rows into maintenance evidence; this ticket must introduce an explicitly separate maintenance row family.
- Do not invent a new maintenance-specific fallback taxonomy in prose; reuse bounded fallback facts from the comparator and provider benchmark work and otherwise document provider-neutral fallback posture only.
- Current branch context does not contain the landed documentation change yet; this ticket is handing forward unapplied documentation work for implementation rather than claiming closure.

## Open Questions
- none

## Follow-Up Questions
- If later work wants PIT parent-maintenance timing claims, should it add separate maintenance scenarios instead of overloading the PIT full-rebuild row family?
- If bridge-maintenance push-down is reopened later, should it extend the same matrix only after a dedicated provider seam, bridge-specific diagnostics vocabulary, parity coverage, and benchmark artifact triplet exist?

## Risks
- If maintenance scenario naming remains ambiguous in the docs update, downstream work could continue to misread `pit-as-of-read` rows as PIT maintenance evidence.
- Until sibling tickets land preserved provider-neutral, PostgreSQL, and SQL Server maintenance artifacts, this ticket can only define the contract and cannot itself promote completed maintenance timing claims.
- If the evidence matrix and shared benchmark artifact contract diverge on required maintenance-row fields or token mapping, later evidence rows could become inconsistent across lanes.

## Split Recommendations
- No additional split is justified; the existing decomposition already separates this contract update from the provider-neutral comparator row work and the PostgreSQL and SQL Server provider-specific benchmark lanes.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Extend the provider evidence matrix or supporting contract so PIT maintenance timing rows are distinguishable from PIT/bridge read rows. Acceptance: maintenance rows include scenario, provider, selected strategy, fallback causes, run context, and artifact links.