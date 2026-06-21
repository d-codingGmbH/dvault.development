<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story into an architecture-contract lane for an optional, opt-in DVault privacy add-on for EU/DSGVO-oriented projects: provider-neutral, explicit-service-friendly, and explicitly out of compliance-guarantee, KMS, and automatic-deletion territory. No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This is a definition-only boundary story, not a product-code implementation story.
- Current repository evidence already fixes the baseline: DVault is an EF Core library family with explicit save/read surfaces and provider-specific packages behind provider-neutral seams, so the privacy add-on should be additive and opt-in rather than a new platform layer.
- Application owners remain responsible for compliance interpretation, provider selection, database provisioning, credentials, key lifecycle, transactions, scheduling, deployment, and operational retention or deletion workflows.
- The contract must explicitly state that DVault does not become a DSGVO/GDPR compliance guarantee, a key-management platform, or an automatic deletion engine.
- The privacy boundary should preserve caller-driven behavior and should not introduce implicit background workflows or default SaveChanges-style automation as the primary lane.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass.

### Scope In
- Define the architecture contract for an optional DVault privacy add-on aimed at EU/DSGVO-oriented projects.
- Define the default v1 posture that the add-on is opt-in and does not change behavior for existing callers unless they explicitly enable it.
- Define how that add-on composes with the existing DI, metadata, save, and read surfaces as an additive library boundary.
- Define the provider-neutral EF Core boundary between shared DVault abstractions and any future provider-specific implementation details.
- Define the ownership split between DVault responsibilities and application or operator responsibilities for privacy-sensitive deployments.
- Define the explicit non-goals and guardrails that keep the feature out of compliance-certification, KMS, and workflow-orchestration territory.

### Scope Out
- Implementing concrete privacy behavior in product code.
- Shipping a compliance certification, legal opinion, or claim that using DVault makes a system DSGVO/GDPR compliant.
- Building a key-management platform, secret vault, HSM integration layer, or key rotation orchestration inside DVault.
- Building automatic deletion, retention scheduling, backfill, purge orchestration, or records-of-processing workflows.
- Provider-specific DDL, migrations, or storage optimizations unless later implementation tickets require them behind the approved boundary.

## Acceptance Criteria
- A reviewed architecture contract, following the existing docs/architecture convention, defines the optional privacy add-on as additive to the current DVault library family and compatible with the existing explicit AddDVault, metadata, save, and read architecture.
- The contract states that enablement is explicit and opt-in, and that existing callers keep their current behavior unless they intentionally adopt the privacy add-on.
- The contract states that provider and database configuration, credentials, deployment, transactions, scheduling, compliance interpretation, and operational retention or deletion remain application-owned responsibilities.
- The contract makes the provider-neutral EF Core boundary explicit and says any provider-specific behavior must sit behind the same kind of extension and package seams already used elsewhere in DVault.
- The contract explicitly excludes compliance guarantees, key-management-platform behavior, and automatic deletion workflows from this story's scope.
- The contract identifies downstream implementation work as follow-on tickets instead of widening this story into code delivery.

## Definition of Done
- The ticket has a clear architecture-level contract that downstream dev tickets can implement without reopening the baseline boundary questions.
- The contract keeps DVault positioned as an opt-in EF Core library extension rather than an application platform or governance system.
- The contract preserves provider-neutral core abstractions and avoids promising provider-specific privacy behavior on the shared surface without separate evidence.
- The contract documents the non-goals strongly enough that later work cannot reasonably interpret this story as approval for compliance guarantees, KMS ownership, or automatic deletion orchestration.

## Implementation Notes
- Use the current repository baseline as the v1 anchor: explicit IDataVaultSaveService and read-diagnostics boundaries, provider-specific packages behind provider-neutral abstractions, and application-owned operational concerns documented in the getting-started guidance.
- A reasonable deliverable shape is a new docs/architecture contract document rather than a product-code change or a broad multi-document implementation wave.
- If a package or profile is proposed, keep it opt-in and additive to AddDVault plus existing metadata and service-registration patterns rather than changing the default runtime path.
- Keep privacy-related behavior caller-driven and explicit; do not define the add-on primarily through implicit interception, automatic scheduling, or background workflow ownership.
- Built-in stable hashing and telemetry surfaces can inform boundary language, but they must not be reframed as encryption, KMS, or compliance controls.
- The existing live relation graph already fans this story out to downstream privacy and security work, so this ticket should remain the authoritative boundary-definition lane and should not absorb child implementation scope.

## Open Questions
- none

## Follow-Up Questions
- After the boundary is approved, which concrete privacy capabilities, if any, should be implemented first as separate tickets: field-level encryption, pseudonymization helpers, redaction or export controls, or retention metadata?
- Do any downstream provider-specific tickets need separate package or extension points once real implementation evidence exists?
- Should later reader-facing documentation use GDPR, DSGVO, or both as the primary terminology once the optional add-on has an approved contract?

## Risks
- The story can mislead downstream work if the contract is written too loosely and gets interpreted as a compliance guarantee rather than a library boundary.
- Provider-neutral API shape can be damaged if provider-specific privacy behavior is promised before there is implementation evidence for multiple providers.
- Scope can expand uncontrollably if key lifecycle, retention orchestration, deletion workflows, or operational governance are not kept explicitly outside the DVault boundary.

## Split Recommendations
- No split is needed for this ticket if it remains definition-only and produces the authoritative privacy-boundary contract.
- Create follow-on implementation tickets only after the boundary contract is accepted, with one ticket per concrete capability or provider-specific lane instead of broadening this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: define an optional DVault privacy add-on for EU/DSGVO-oriented projects. Acceptance: no compliance guarantee, no key-management platform, no automatic deletion workflow, and clear provider-neutral EF Core boundaries.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- Added `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` as the developer architecture contract for this story.
- The contract preserves an optional, opt-in, additive privacy boundary; keeps provider configuration, credentials, deployment, transactions, scheduling, compliance interpretation, key lifecycle, and retention or deletion workflows application-owned; and explicitly excludes compliance guarantees, KMS ownership, and automatic deletion workflows.
- Verification: `bash tools/check-format.sh` passed. `dotnet build DVault.slnx --nologo --no-restore` was attempted but could not complete because required NuGet packages were missing locally and NuGet access was unreachable in the bounded run.
<!-- gicket-bot:developer-delivery:v1:end -->