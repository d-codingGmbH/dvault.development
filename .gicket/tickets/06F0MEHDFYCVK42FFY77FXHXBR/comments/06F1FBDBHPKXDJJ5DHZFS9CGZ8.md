[gicket-bot] PO refinement contract

Summary
- Refined bridge traversal query helper contract against the existing bridge metadata v1 planning baseline, v0.5/v0.6 release notes, current source annotations, and related read-helper tickets. The ticket is bounded to API contract design and is ready for PO-critic review without child-ticket or relation changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 bridge helper contract is a read/query API design ticket, not the implementation ticket; implementation remains covered by related ticket 06F0MEHKYTBJEJH2DVZ2CFH9Z0.
- The helper must use the existing bridge metadata baseline: bridge kinds are many-to-many and hierarchy, bridge projections are provider-neutral EF shared-type tables, and hierarchy depth is represented by the existing BridgeDepth role/logical kind rather than satellite payload semantics.
- The contract must distinguish supported baseline traversal from future graph behavior: no full recursive graph engine, unbounded traversal, provider-specific tuning, bridge row maintenance, PIT interaction, or multi-active interaction is included here.
- The related provider-specific read strategy ticket 06F0MEJ7NANHCP64VR1SH3S3G8 is not a blocker for this contract; the baseline helper contract must remain provider-neutral and compatible with later strategy selection hooks.
- The done documentation/release ticket 06F0MEDJC732GDD77H60R259P0 is historical context only and does not reopen any PO decision for this ticket.

Scope In
- Define the public request and response contract for many-to-many bridge traversal over generated bridge tables with ordered from/to endpoint hash-key semantics.
- Define the public request and response contract for bounded hierarchy traversal using ancestor, descendant, and TraversalDepth semantics from the bridge metadata baseline.
- Define unsupported-shape and missing-row failure behavior, including unsupported traversal depth, missing bridge declaration, missing endpoint binding, and bridge metadata outside the v1 projection baseline.
- Keep the contract suitable for future typed projection by separating traversal row identity/hash-key data from caller-owned projection shape.
- Provide examples using only current bridge metadata concepts from the v1 planning contract.

Scope Out
- Implementing the bridge traversal read service or EF query logic.
- Provider-specific query tuning, strategy implementations, or provider-specific SQL.
- Full recursive graph-query behavior, arbitrary path finding, unbounded hierarchy traversal, or graph mutation semantics.
- Bridge row population, traversal maintenance, closure-table refresh behavior, migrations, EF foreign keys, or navigations.
- PIT-backed reads, multi-active satellite interactions, and model-first import/export behavior.

Open questions
- none

Follow-up questions
- After the baseline implementation lands, decide whether provider-specific read strategy hooks should optimize bridge traversal alongside satellite and PIT reads.
- Future architecture work can decide whether unbounded recursive traversal, path payloads, closure maintenance, or graph-query composition should become separate advanced bridge capabilities.
- Typed projection convenience overloads may be considered after the baseline row/request contract has proven stable.

Risks
- Over-specifying graph behavior in this contract would conflict with the deferred-capabilities decision record and make the implementation ticket larger than intended.
- Provider-specific assumptions in the contract would make the provider-neutral fallback and later strategy-hook work harder to preserve.
- Ambiguous hierarchy depth semantics could lead to partial or misleading traversal results, so unsupported depth must fail clearly.

Split recommendations
- No split is recommended for this contract ticket; the implementation and provider-strategy work are already represented by related tickets 06F0MEHKYTBJEJH2DVZ2CFH9Z0 and 06F0MEJ7NANHCP64VR1SH3S3G8.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment