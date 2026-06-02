[gicket-bot] PO refinement contract

Summary
- Refined this into a documentation-only decision gate: provider-specific stored-procedure or SQL artifact ideas remain opt-in, design-time-only, consumer-deployed, non-default DVault behavior and must meet the same evidence-first posture already used for staged provider bulk ingestion.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository context already shows the comparable optional-provider pattern in docs/performance-profiles.md: staged provider bulk ingestion is explicit, diagnostics-gated, and benchmark-backed rather than a default runtime path.
- Repository-visible runtime surfaces today are provider-neutral save/read/PIT/bridge operations and metadata annotations; there is no existing stored-procedure dispatcher, deployment owner, or automatic migration-synchronization surface to preserve.
- Ticket comments are lease metadata only and the ticket has no attachments, so refinement relies on the checked-in documentation and code evidence already referenced by the ticket.
- The live relation set already fits the intended gate behavior: this ticket remains a child of 06F7Y0J8PRFRSSWZ3GGT91S0TW, is blocked by 06F7Y0K95VW0PX21F6R2YGP8DM, and blocks 06F7Y0NBHXQ6CK8R3AH4DEP9V4; no relation change is needed.

Scope In
- Document that any stored-procedure or provider-specific SQL artifact path is a future additive option only when explicitly opted into by a consumer.
- Define the hard boundary: design-time generation only, consumer-owned deployment and lifecycle, no default runtime execution path, and no automatic migration synchronization.
- Compare that boundary to the existing staged provider bulk-ingestion precedent so future tickets reuse the same diagnostics-first and benchmark-evidence-first posture.
- Record the decision gate future implementation tickets must satisfy before provider-specific artifact generation or execution work can enter scope.

Scope Out
- Implementing stored procedures, SQL artifact generators, runtime dispatchers, EF interceptors, migration hooks, or deployment automation.
- Creating provider-specific performance promises or default provider routing without checked-in benchmark evidence for the exact provider and workload.
- Changing the current provider-neutral public save/read/PIT/bridge boundaries or typed read-model generator behavior.

Open questions
- none

Follow-up questions
- If a future additive experiment is approved, which provider and representative workload should supply the first checked-in benchmark triplet and diagnostics evidence?
- Once this boundary lands, does downstream ticket 06F7Y0NBHXQ6CK8R3AH4DEP9V4 need a provider-specific worked example, or is the generic gate sufficient?

Risks
- If the boundary text does not sharply separate design-time artifact generation from runtime execution, downstream work may incorrectly expand into dispatcher, migration, or deployment scope.
- The repository's optional-provider benchmark posture already shows skipped non-SQLite rows when local provider evidence is absent; future teams may over-read the precedent unless the document explicitly forbids unmeasured performance claims.
- Because this ticket currently blocks 06F7Y0NBHXQ6CK8R3AH4DEP9V4, ambiguous wording here would propagate delay or rework downstream.

Split recommendations
- No immediate split: keep this ticket as the single documentation and evidence-gate artifact, and open separate provider-specific experiment tickets only after benchmark evidence exists.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment