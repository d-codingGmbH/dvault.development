<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- The epic is ready for independent PO-critic closure audit: all deferred-capability child tickets are complete, and the bridge child contract has been aligned so its former `ready_for_dev` wording is explicitly historical.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This epic remains a tracking-only closure umbrella with no parent-owned implementation, test, or documentation slice.
- The existing child split remains architecture 06EZ0NSBM3GD7DY11Y4PZMXD28, PIT 06EZ0NSXY2Y1JZ8SSCX177C770, bridge 06EZ0NTV4SVAKV98C418T8A3CC, multi-active satellites 06EZ0NVN71BN0QWJDCWGVZ2PYG, and advanced hooks 06EZ0NWKC9ZME5BSCJFSQEQ02R.
- Bridge workflow evidence indicates the narrow hierarchy-validation gap was implemented, tested, accepted, and contract-aligned on child 06EZ0NTV4SVAKV98C418T8A3CC.
- docs/plans/deferred-data-vault-capabilities.md remains the umbrella decision record, with docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md and docs/plans/optional-advanced-configuration-hooks.md as bounded supporting contracts.
- No new child tickets, relation mutations, attachments, or planning documents are needed; the remaining epic work is PO-critic closure audit of the completed child set.

### Scope In
- Track and ratify the combined provider-neutral deferred-capability baseline already delivered or bounded in the five existing child tickets.
- Preserve epic-level guardrails that deferred capabilities remain opt-in, additive, compatibility-reviewed where public surface changes occur, and non-disruptive to the established default DVault path.
- Align the epic's closure language with durable child-ticket state and route the completed child set to PO-critic closure audit.

### Scope Out
- Any new parent-owned product-code, repository-test, or repository-documentation implementation slice.
- Reopening bridge runtime maintenance, bridge row population, PIT refresh or population, link-based PIT, multi-active PIT semantics, provider-specific optimization, or unbounded Data Vault pattern variants.
- Changing the established `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, explicit `IDataVaultSaveService`, deterministic convention, or SQLite-default baseline without separate follow-up work.
- Creating replacement implementation tickets for already integrated bridge validation work.

## Acceptance Criteria
- The epic contract states explicitly that ticket 06EZ0NS59T2SW9976HHSGP2GF0 is a tracking-only closure umbrella with delivery owned by its existing child tickets.
- The parent contract points to child tickets 06EZ0NSBM3GD7DY11Y4PZMXD28, 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, 06EZ0NVN71BN0QWJDCWGVZ2PYG, and 06EZ0NWKC9ZME5BSCJFSQEQ02R plus the governing planning documents as one bounded provider-neutral deferred-capability baseline.
- The combined child outcome preserves the current default DVault path and keeps deferred capabilities opt-in and additive, including `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, explicit `IDataVaultSaveService`, required record-source lineage, UTC load-timestamp semantics, deterministic defaults, and the SQLite default capability profile when no hook is configured.
- The epic may cite the child tester verification and integrator `ACCEPT` on branch `ticket/06EZ0NTV4SVAKV98C418T8A3CC-story-add-bridge-table-modeling-and-generation` at commit `9a5d5de0980b`, plus the contract-aligned child description, as concrete closure evidence for the bridge gap.
- Epic closure review remains based on combined child outcomes, durable docs, source/tests, and compatibility guardrails rather than child-ticket existence or status alone.

## Definition of Done
- A reviewer can trace the epic to its five existing child tickets and governing plan documents without reading the parent as an implementation story.
- Parent contract language and child durable contract language agree that the bridge hierarchy-validation baseline is complete.
- Child 06EZ0NTV4SVAKV98C418T8A3CC durable contract, later workflow history, and epic closure language all tell the same completed-delivery story for PO-critic audit.
- The combined child outcome preserves the established default DVault path while exposing only the bounded opt-in deferred capabilities already documented in current plans and source.

## Implementation Notes
- Use docs/plans/deferred-data-vault-capabilities.md as the umbrella decision record; use the bridge v1 contract and optional advanced hooks plan as supporting detail instead of reopening architecture from scratch.
- Concrete bridge-closure evidence already exists in child 06EZ0NTV4SVAKV98C418T8A3CC history: tester verification reported 6/6 acceptance criteria and 4/4 definition-of-done on branch `ticket/06EZ0NTV4SVAKV98C418T8A3CC-story-add-bridge-table-modeling-and-generation` at commit `9a5d5de0980b`, and a later integrator comment recorded `ACCEPT` for squash integration.
- Manual repair aligned the existing child delivery contract rather than creating extra artifacts; PO-critic can now audit closure against the completed child set and durable evidence.

## Open Questions
- none

## Follow-Up Questions
- Should README or docs-index discoverability link directly to the umbrella deferred-capability decision record and the supporting bridge and hooks contracts?
- If later work reopens PIT refresh, bridge runtime maintenance, or multi-active PIT behavior, should each remain a narrow follow-up ticket rather than expanding this closure umbrella?

## Risks
- Later deferred-capability work could erode opt-in or deterministic guardrails if epic closure prose is read as blanket approval instead of bounded child-owned delivery.

## Split Recommendations
- No additional implementation split is recommended; the existing five-child structure remains the correct decomposition.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: implement the first release-quality slice of the deferred Data Vault capabilities that were intentionally excluded from the MVP.

Scope:
- Define the extension architecture and compatibility guardrails for advanced Data Vault patterns.
- Add baseline PIT table modeling and generation.
- Add baseline bridge table modeling and generation.
- Add baseline multi-active satellite modeling and persistence semantics.
- Add advanced hooks needed by those capabilities without weakening deterministic defaults.

Out of scope:
- Replacing existing hub/link/satellite APIs for the MVP scenarios.
- Provider-specific performance work; that is tracked in the provider optimization epic.
- Unbounded automation for every Data Vault pattern variation. Each capability must document supported cases and explicit limitations.

Acceptance Criteria:
- Each capability has tests, documentation, and a clear provider-neutral baseline.
- Advanced capabilities do not silently change existing v0.4 behavior.
- API additions are reviewed through compatibility snapshots or an explicit documented decision.
- The epic cannot close merely because child tickets exist; the combined child outcome must be reviewed as satisfying the deferred capability goal.