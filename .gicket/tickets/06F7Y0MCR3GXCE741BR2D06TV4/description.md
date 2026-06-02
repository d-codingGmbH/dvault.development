<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this into a documentation-only decision gate: provider-specific stored-procedure or SQL artifact ideas remain opt-in, design-time-only, consumer-deployed, non-default DVault behavior and must meet the same evidence-first posture already used for staged provider bulk ingestion.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current repository context already shows the comparable optional-provider pattern in docs/performance-profiles.md: staged provider bulk ingestion is explicit, diagnostics-gated, and benchmark-backed rather than a default runtime path.
- Repository-visible runtime surfaces today are provider-neutral save/read/PIT/bridge operations and metadata annotations; there is no existing stored-procedure dispatcher, deployment owner, or automatic migration-synchronization surface to preserve.
- Ticket comments are lease metadata only and the ticket has no attachments, so refinement relies on the checked-in documentation and code evidence already referenced by the ticket.
- The live relation set already fits the intended gate behavior: this ticket remains a child of 06F7Y0J8PRFRSSWZ3GGT91S0TW, is blocked by 06F7Y0K95VW0PX21F6R2YGP8DM, and blocks 06F7Y0NBHXQ6CK8R3AH4DEP9V4; no relation change is needed.

### Scope In
- Document that any stored-procedure or provider-specific SQL artifact path is a future additive option only when explicitly opted into by a consumer.
- Define the hard boundary: design-time generation only, consumer-owned deployment and lifecycle, no default runtime execution path, and no automatic migration synchronization.
- Compare that boundary to the existing staged provider bulk-ingestion precedent so future tickets reuse the same diagnostics-first and benchmark-evidence-first posture.
- Record the decision gate future implementation tickets must satisfy before provider-specific artifact generation or execution work can enter scope.

### Scope Out
- Implementing stored procedures, SQL artifact generators, runtime dispatchers, EF interceptors, migration hooks, or deployment automation.
- Creating provider-specific performance promises or default provider routing without checked-in benchmark evidence for the exact provider and workload.
- Changing the current provider-neutral public save/read/PIT/bridge boundaries or typed read-model generator behavior.

## Acceptance Criteria
- Documentation explicitly states that stored procedures or provider-specific SQL artifacts are not DVault's default path and require explicit consumer opt-in.
- Documentation explicitly states that any approved artifacts are design-time outputs only and remain consumer-owned for deployment, invocation, versioning, and rollback.
- Documentation explicitly states that DVault will not auto-create runtime dispatch, auto-run artifacts, or automatically synchronize them with migrations or model changes.
- Documentation compares the proposal to staged provider bulk-ingestion guidance and requires representative diagnostics review plus benchmark evidence before any future implementation ticket.
- Future tickets can reference the document as the authoritative gate for prerequisites, non-goals, and evidence expectations.

## Definition of Done
- A reviewed documentation surface records the boundary and cites the existing staged provider-ingestion evidence posture as the comparison baseline.
- The ticket contract leaves no ambiguity about runtime defaults, deployment ownership, or migration synchronization for stored-procedure artifacts.
- Downstream tickets can consume this ticket as the authoritative boundary without reopening whether stored procedures are a default DVault feature.

## Implementation Notes
- Anchor the narrative to the existing provider-neutral runtime baseline already visible in the repository: IDataVaultSaveService and IDataVaultReadService stay the default execution surfaces, while provider-specific behavior remains optional and diagnostics-gated.
- Reuse the docs/performance-profiles.md staged-provider guidance as the comparison model: optional provider extensions, request-bound eligibility, skipped optional-provider rows when evidence is missing, and no performance claim without measured artifacts.
- Use the existing typed read-model generator contract as a contrast point: the repository already allows opt-in design-time artifacts while explicitly excluding provider-specific SQL generation, so this ticket should keep stored-procedure artifacts outside the default generator and runtime baseline.
- Call out that any future artifact work, if ever approved, must be design-time generation from explicit reviewed workflows rather than runtime metadata inspection or automatic deployment ownership.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- If a future additive experiment is approved, which provider and representative workload should supply the first checked-in benchmark triplet and diagnostics evidence?
- Once this boundary lands, does downstream ticket 06F7Y0NBHXQ6CK8R3AH4DEP9V4 need a provider-specific worked example, or is the generic gate sufficient?

## Risks
- If the boundary text does not sharply separate design-time artifact generation from runtime execution, downstream work may incorrectly expand into dispatcher, migration, or deployment scope.
- The repository's optional-provider benchmark posture already shows skipped non-SQLite rows when local provider evidence is absent; future teams may over-read the precedent unless the document explicitly forbids unmeasured performance claims.
- Because this ticket currently blocks 06F7Y0NBHXQ6CK8R3AH4DEP9V4, ambiguous wording here would propagate delay or rework downstream.

## Split Recommendations
- No immediate split: keep this ticket as the single documentation and evidence-gate artifact, and open separate provider-specific experiment tickets only after benchmark evidence exists.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Record the DVault boundary for optional provider-specific stored-procedure or SQL artifact ideas without making them a default feature.

# Scope In
- Document that such artifacts require explicit opt-in, design-time generation only, no deployment ownership, no default runtime path, no automatic migration synchronization, and benchmark evidence first.
- Compare this boundary with existing staged provider bulk strategies.

# Scope Out
No stored-procedure implementation, artifact generator, runtime dispatcher, or deployment automation.

# Acceptance Criteria
- Documentation clearly says stored procedures are not DVault's default path.
- Future tickets can use this as a decision gate.