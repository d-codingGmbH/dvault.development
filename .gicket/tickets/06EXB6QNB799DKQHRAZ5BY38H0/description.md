<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket into a documentation-only task for the v1 DVault default convention policy. Repository evidence supports a bounded foundation-stage default: no source or test layout exists yet, so the policy should define deterministic provider-neutral conventions without requiring implementation or migration work.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket is scoped to drafting the policy, not implementing persistence behavior in code.
- The v1 policy should use deterministic defaults as the baseline and identify override points as future extension hooks rather than required deliverables.
- Provider-neutral behavior means the policy must avoid database-specific storage features, SQL dialect assumptions, or provider-only metadata semantics.
- Because the repository has no current source or test roots, no existing code naming convention is reopened; the policy should establish the initial DVault persistence convention baseline.

### Scope In
- Document default persistence object naming conventions for DVault artifacts, including deterministic table, collection, index, and metadata names where applicable.
- Document required metadata columns or fields for persisted DVault records, including stable identifiers, content hash, creation/update timestamps where needed, and version or schema metadata needed for future compatibility.
- Document hashing defaults, including canonicalization expectations, deterministic algorithm selection, encoding, and how hashes are used for identity, deduplication, or integrity checks.
- Document provider-neutral behavior and constraints so the same logical defaults can be mapped across relational, document, and other supported persistence providers.
- Identify supported override categories without requiring override implementation details, APIs, or provider-specific configuration mechanisms.

### Scope Out
- Implementing persistence providers, schema generators, migrations, hashing code, or runtime configuration APIs.
- Choosing or documenting every future provider-specific physical schema variant.
- Defining public method names, helper class names, package layout, or other implementation-shape details.
- Creating subtickets or expanding this task into full persistence architecture implementation.
- Changing workflow metadata, status labels, board configuration, or Gicket runtime policy.

## Acceptance Criteria
- The policy defines deterministic v1 defaults for names, metadata fields, hashing behavior, and provider-neutral mapping behavior.
- Each default includes enough detail that two implementers would derive the same logical persistence shape without additional PO clarification.
- The policy explicitly distinguishes required defaults from optional override points.
- The policy states that overrides must preserve deterministic behavior unless a later ticket explicitly approves a different contract.
- The policy avoids provider-specific assumptions while allowing provider adapters to map logical conventions to their native storage primitives.
- The policy records any intentionally deferred decisions as follow-up items rather than leaving the v1 baseline ambiguous.

## Definition of Done
- A planning or documentation artifact exists under an approved planning/documentation path and captures the v1 default convention policy.
- The artifact covers default names, metadata fields, hashing defaults, provider-neutral behavior, and override categories.
- The artifact is internally consistent with the current foundation-stage repository state and does not depend on source or test roots that do not yet exist.
- No implementation work is required to satisfy this ticket.
- The resulting ticket description is specific enough for a developer/documentation owner to complete without further PO clarification.

## Implementation Notes
- Prefer a concise policy document in docs/plans or .gicket-bot/planning, matching the repository's current planning-first state.
- Use normative language such as MUST, SHOULD, and MAY where it helps separate hard defaults from extension points.
- Treat logical convention names as the source of truth; physical provider names can be derived by adapters while preserving stable logical meaning.
- Recommended v1 metadata coverage includes record identity, content hash, hash algorithm/version, created timestamp, updated timestamp if mutable records are allowed, and schema or convention version.
- Recommended v1 hashing coverage includes canonical input formation, stable text encoding, a named algorithm, deterministic output encoding, and separation between content identity and storage location.
- When naming examples are included, keep them illustrative unless the policy marks them as required defaults.

## Open Questions
- none

## Follow-Up Questions
- Which concrete persistence providers should get first-class adapter guidance after the provider-neutral policy is accepted?
- Should future implementation tickets expose override points through configuration files, code-first APIs, attributes/annotations, or a combination of these?
- Should the project later define formal migration/versioning rules for changes to the convention policy after v1?

## Risks
- A policy that is too abstract may still leave implementers with divergent physical schemas; acceptance criteria require deterministic logical defaults to reduce that risk.
- Provider-neutral language can accidentally hide provider-specific constraints; examples should be clearly labeled and avoid becoming unofficial provider commitments.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Describe the default conventions that make DVault persistence work with minimal setup.

## Scope
- Cover default names, metadata columns, hashing defaults, and provider-neutral behavior.

## Acceptance Criteria
- Defaults are deterministic and documented.
- Overrides are identified but not required.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.