<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined bridge traversal query helper contract against the existing bridge metadata v1 planning baseline, v0.5/v0.6 release notes, current source annotations, and related read-helper tickets. The ticket is bounded to API contract design and is ready for PO-critic review without child-ticket or relation changes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 bridge helper contract is a read/query API design ticket, not the implementation ticket; implementation remains covered by related ticket 06F0MEHKYTBJEJH2DVZ2CFH9Z0.
- The helper must use the existing bridge metadata baseline: bridge kinds are many-to-many and hierarchy, bridge projections are provider-neutral EF shared-type tables, and hierarchy depth is represented by the existing BridgeDepth role/logical kind rather than satellite payload semantics.
- The contract must distinguish supported baseline traversal from future graph behavior: no full recursive graph engine, unbounded traversal, provider-specific tuning, bridge row maintenance, PIT interaction, or multi-active interaction is included here.
- The related provider-specific read strategy ticket 06F0MEJ7NANHCP64VR1SH3S3G8 is not a blocker for this contract; the baseline helper contract must remain provider-neutral and compatible with later strategy selection hooks.
- The done documentation/release ticket 06F0MEDJC732GDD77H60R259P0 is historical context only and does not reopen any PO decision for this ticket.

### Scope In
- Define the public request and response contract for many-to-many bridge traversal over generated bridge tables with ordered from/to endpoint hash-key semantics.
- Define the public request and response contract for bounded hierarchy traversal using ancestor, descendant, and TraversalDepth semantics from the bridge metadata baseline.
- Define unsupported-shape and missing-row failure behavior, including unsupported traversal depth, missing bridge declaration, missing endpoint binding, and bridge metadata outside the v1 projection baseline.
- Keep the contract suitable for future typed projection by separating traversal row identity/hash-key data from caller-owned projection shape.
- Provide examples using only current bridge metadata concepts from the v1 planning contract.

### Scope Out
- Implementing the bridge traversal read service or EF query logic.
- Provider-specific query tuning, strategy implementations, or provider-specific SQL.
- Full recursive graph-query behavior, arbitrary path finding, unbounded hierarchy traversal, or graph mutation semantics.
- Bridge row population, traversal maintenance, closure-table refresh behavior, migrations, EF foreign keys, or navigations.
- PIT-backed reads, multi-active satellite interactions, and model-first import/export behavior.

## Acceptance Criteria
- The contract names supported traversal kinds explicitly as many-to-many and bounded hierarchy and rejects advanced graph behavior as out of scope.
- Many-to-many request semantics identify one bridge declaration plus from/to endpoint direction and endpoint hash-key inputs without requiring provider-specific query details.
- Hierarchy request semantics identify one hierarchy bridge declaration, ancestor/descendant direction, requested depth constraints, and TraversalDepth interpretation using the existing bridge depth metadata role.
- Response semantics represent zero rows, matched rows, and unsupported requests deterministically and leave room for future typed projection without committing to reflection-based DTO binding.
- Failure modes are documented for missing bridge metadata, unsupported bridge kind, unsupported depth, malformed endpoint bindings, and valid metadata that asks for behavior outside the provider-neutral baseline.
- Examples use the documented CustomerOrder many-to-many and SalesRegionHierarchy hierarchy concepts or equivalent current bridge metadata terms only.

## Definition of Done
- A developer can implement the contract without reopening naming, ownership, or baseline metadata decisions already fixed by the bridge metadata v1 planning document.
- The contract clearly separates this API design ticket from the provider-neutral implementation ticket and the provider-specific strategy-hook ticket.
- The contract aligns with current repository conventions around DataVault metadata annotations, property roles, provider-neutral read services, and caller-owned projection delegates.
- No remaining PO-level blocker exists for PO-critic review.

## Implementation Notes
- Prefer placing the eventual public helper near the existing read-model API surface in the core DCoding.Data.DVault package, with tests under the existing DVault test roots; exact type and method names may be chosen by the implementer following local conventions.
- Use DataVaultMetadataModel bridge declarations and generated EF shared-type bridge tables as the authoritative source; do not infer relationships from EF foreign keys or navigations.
- Many-to-many rows should be modeled around ordered endpoint hash-key columns from the bridge declaration, with empty bridge tables producing an empty result rather than an error.
- Hierarchy rows should include or expose TraversalDepth when present and treat unsupported depth requests as clear diagnostics rather than partial graph answers.
- Projection should follow the v0.6 read-helper pattern of caller-owned projection rather than adding a model-first DTO binding contract in this ticket.
- Provider-specific optimization should be left to later read-strategy work; this contract should be implementable by a correct provider-neutral fallback.

## Open Questions
- none

## Follow-Up Questions
- After the baseline implementation lands, decide whether provider-specific read strategy hooks should optimize bridge traversal alongside satellite and PIT reads.
- Future architecture work can decide whether unbounded recursive traversal, path payloads, closure maintenance, or graph-query composition should become separate advanced bridge capabilities.
- Typed projection convenience overloads may be considered after the baseline row/request contract has proven stable.

## Risks
- Over-specifying graph behavior in this contract would conflict with the deferred-capabilities decision record and make the implementation ticket larger than intended.
- Provider-specific assumptions in the contract would make the provider-neutral fallback and later strategy-hook work harder to preserve.
- Ambiguous hierarchy depth semantics could lead to partial or misleading traversal results, so unsupported depth must fail clearly.

## Split Recommendations
- No split is recommended for this contract ticket; the implementation and provider-strategy work are already represented by related tickets 06F0MEHKYTBJEJH2DVZ2CFH9Z0 and 06F0MEJ7NANHCP64VR1SH3S3G8.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Design a bounded bridge traversal helper API for the bridge metadata baseline without promising a full graph-query engine.

## Scope In

- Many-to-many bridge traversal request/response shape.
- Bounded hierarchy traversal semantics using existing bridge metadata fields.
- Failure modes for unsupported traversal depth or missing bridge rows.

## Scope Out

- Full recursive graph engine.
- Provider-specific query tuning.

## Acceptance Criteria

- Contract distinguishes implemented baseline traversal from future advanced graph behavior.
- Request and response types can support typed projection later.
- Examples use current bridge metadata concepts only.