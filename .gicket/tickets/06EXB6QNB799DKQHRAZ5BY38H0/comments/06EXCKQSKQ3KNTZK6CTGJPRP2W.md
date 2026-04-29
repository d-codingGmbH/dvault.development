[gicket-bot] PO refinement contract

Summary
- Refined the ticket into a documentation-only task for the v1 DVault default convention policy. Repository evidence supports a bounded foundation-stage default: no source or test layout exists yet, so the policy should define deterministic provider-neutral conventions without requiring implementation or migration work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket is scoped to drafting the policy, not implementing persistence behavior in code.
- The v1 policy should use deterministic defaults as the baseline and identify override points as future extension hooks rather than required deliverables.
- Provider-neutral behavior means the policy must avoid database-specific storage features, SQL dialect assumptions, or provider-only metadata semantics.
- Because the repository has no current source or test roots, no existing code naming convention is reopened; the policy should establish the initial DVault persistence convention baseline.

Scope In
- Document default persistence object naming conventions for DVault artifacts, including deterministic table, collection, index, and metadata names where applicable.
- Document required metadata columns or fields for persisted DVault records, including stable identifiers, content hash, creation/update timestamps where needed, and version or schema metadata needed for future compatibility.
- Document hashing defaults, including canonicalization expectations, deterministic algorithm selection, encoding, and how hashes are used for identity, deduplication, or integrity checks.
- Document provider-neutral behavior and constraints so the same logical defaults can be mapped across relational, document, and other supported persistence providers.
- Identify supported override categories without requiring override implementation details, APIs, or provider-specific configuration mechanisms.

Scope Out
- Implementing persistence providers, schema generators, migrations, hashing code, or runtime configuration APIs.
- Choosing or documenting every future provider-specific physical schema variant.
- Defining public method names, helper class names, package layout, or other implementation-shape details.
- Creating subtickets or expanding this task into full persistence architecture implementation.
- Changing workflow metadata, status labels, board configuration, or Gicket runtime policy.

Open questions
- none

Follow-up questions
- Which concrete persistence providers should get first-class adapter guidance after the provider-neutral policy is accepted?
- Should future implementation tickets expose override points through configuration files, code-first APIs, attributes/annotations, or a combination of these?
- Should the project later define formal migration/versioning rules for changes to the convention policy after v1?

Risks
- A policy that is too abstract may still leave implementers with divergent physical schemas; acceptance criteria require deterministic logical defaults to reduce that risk.
- Provider-neutral language can accidentally hide provider-specific constraints; examples should be clearly labeled and avoid becoming unofficial provider commitments.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment