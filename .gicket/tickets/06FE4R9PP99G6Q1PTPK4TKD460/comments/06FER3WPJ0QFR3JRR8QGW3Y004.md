[gicket-bot] PO refinement contract

Summary
- Refined the story into an architecture-contract lane for an optional, opt-in DVault privacy add-on for EU/DSGVO-oriented projects: provider-neutral, explicit-service-friendly, and explicitly out of compliance-guarantee, KMS, and automatic-deletion territory. No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This is a definition-only boundary story, not a product-code implementation story.
- Current repository evidence already fixes the baseline: DVault is an EF Core library family with explicit save/read surfaces and provider-specific packages behind provider-neutral seams, so the privacy add-on should be additive and opt-in rather than a new platform layer.
- Application owners remain responsible for compliance interpretation, provider selection, database provisioning, credentials, key lifecycle, transactions, scheduling, deployment, and operational retention or deletion workflows.
- The contract must explicitly state that DVault does not become a DSGVO/GDPR compliance guarantee, a key-management platform, or an automatic deletion engine.
- The privacy boundary should preserve caller-driven behavior and should not introduce implicit background workflows or default SaveChanges-style automation as the primary lane.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Define the architecture contract for an optional DVault privacy add-on aimed at EU/DSGVO-oriented projects.
- Define the default v1 posture that the add-on is opt-in and does not change behavior for existing callers unless they explicitly enable it.
- Define how that add-on composes with the existing DI, metadata, save, and read surfaces as an additive library boundary.
- Define the provider-neutral EF Core boundary between shared DVault abstractions and any future provider-specific implementation details.
- Define the ownership split between DVault responsibilities and application or operator responsibilities for privacy-sensitive deployments.
- Define the explicit non-goals and guardrails that keep the feature out of compliance-certification, KMS, and workflow-orchestration territory.

Scope Out
- Implementing concrete privacy behavior in product code.
- Shipping a compliance certification, legal opinion, or claim that using DVault makes a system DSGVO/GDPR compliant.
- Building a key-management platform, secret vault, HSM integration layer, or key rotation orchestration inside DVault.
- Building automatic deletion, retention scheduling, backfill, purge orchestration, or records-of-processing workflows.
- Provider-specific DDL, migrations, or storage optimizations unless later implementation tickets require them behind the approved boundary.

Open questions
- none

Follow-up questions
- After the boundary is approved, which concrete privacy capabilities, if any, should be implemented first as separate tickets: field-level encryption, pseudonymization helpers, redaction or export controls, or retention metadata?
- Do any downstream provider-specific tickets need separate package or extension points once real implementation evidence exists?
- Should later reader-facing documentation use GDPR, DSGVO, or both as the primary terminology once the optional add-on has an approved contract?

Risks
- The story can mislead downstream work if the contract is written too loosely and gets interpreted as a compliance guarantee rather than a library boundary.
- Provider-neutral API shape can be damaged if provider-specific privacy behavior is promised before there is implementation evidence for multiple providers.
- Scope can expand uncontrollably if key lifecycle, retention orchestration, deletion workflows, or operational governance are not kept explicitly outside the DVault boundary.

Split recommendations
- No split is needed for this ticket if it remains definition-only and produces the authoritative privacy-boundary contract.
- Create follow-on implementation tickets only after the boundary contract is accepted, with one ticket per concrete capability or provider-specific lane instead of broadening this story.

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