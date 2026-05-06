<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the bridge-metadata contract to pin the supported single-link hierarchy boundary and one concrete invalid self-cycle example, so bridge validation and negative tests are auditable against the current hub/link/satellite baseline.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket still owns bridge metadata declarations and bridge-specific validation only; provider-neutral EF mapping remains split to 06EZ0NV7KG94MTMNXMGVRYVW9C and documentation/example work remains split to 06EZ0NVE88WW9PMM04NVAZHRG0.
- Bridge support stays an opt-in deferred v0.5 capability and must not change ordinary hub, link, satellite, AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), or explicit save-service behavior by default.
- The current modeling baseline remains the provider-neutral DataVaultMetadataModel aggregate plus ApplyDataVaultMetadata(), so bridge metadata should extend that surface additively rather than replace existing hub, link, and satellite contracts.
- The one supported bounded hierarchy slice in this ticket is one recursive link plus two explicit, distinct participant selectors for one directional edge; this ticket does not authorize multi-link hierarchy composition or bridge-to-bridge traversal.
- No child tickets, relation updates, or planning documents were materialized in this refinement pass because the existing sibling split already bounds the remaining work.

### Scope In
- Add provider-neutral bridge metadata declarations alongside the existing hub, link, and satellite metadata model.
- Represent one many-to-many traversal shape by explicitly tying a source hub, one traversed link, and a target hub into one bridge definition.
- Represent one bounded hierarchy traversal shape over one declared recursive link with explicit ancestor-side and descendant-side participant selectors for a single recursive edge.
- Add bridge-specific validation for unknown references, invalid participant selectors, ambiguous endpoint selection, and the unsupported self-cycle pattern where both hierarchy selectors resolve to the same participant.
- Keep existing hub, link, and satellite callers backward-compatible by making bridge support additive and opt-in.

### Scope Out
- Provider-neutral EF Core mapping for bridge tables; that belongs to 06EZ0NV7KG94MTMNXMGVRYVW9C.
- User-facing bridge documentation and example scenarios; that belongs to 06EZ0NVE88WW9PMM04NVAZHRG0.
- Bridge row materialization, refresh or maintenance strategy, load workflows, SaveChanges interception, or explicit save-service behavior.
- Provider-specific DDL, indexing strategy, SQL optimization, migrations, or benchmark posture.
- Traversal expansion beyond the bounded v0.5 slice, including multi-link hierarchy composition, bridge-to-bridge chaining, alternate cycle policies, business-rule-driven pruning, or other advanced bridge patterns.

## Acceptance Criteria
- The metadata model can declare an opt-in many-to-many bridge by naming the source hub, traversed link, target hub, and the deterministic traversal references needed to distinguish the path.
- The metadata model can declare one baseline hierarchy bridge by naming one recursive link plus explicit ancestor-side and descendant-side participant selectors that resolve to two distinct participants on that link for one directional recursive edge.
- Bridge validation fails deterministically when a bridge references a hub or link that is not declared in the same aggregate metadata model, when a selected participant does not belong to the referenced link, when endpoint selection is ambiguous, or when a hierarchy bridge resolves both selectors to the same participant.
- Concrete invalid-cycle example: a hierarchy bridge over 'EmployeeReportsTo(Employee, Employee)' must be rejected when both selectors resolve to participant ordinal 0, or otherwise to the same selected participant; the same link is only supported when the selectors resolve to two different participants.
- The bridge contract is additive: existing hub, link, and satellite metadata callers continue to work without providing bridge metadata, and no existing default-path behavior changes when no bridge is declared.
- The delivery makes the bridge surface boundary auditable: either exported bridge metadata is covered by the core package API snapshot in the same change, or the implementation is explicitly documented as internal and leaves the approved snapshots unchanged.

## Definition of Done
- Unit tests cover successful many-to-many and successful single-link hierarchy bridge declarations plus rejected cases for unknown references, invalid selectors, ambiguous selections, and the concrete same-participant self-cycle example.
- The implementation preserves existing hub, link, and satellite modeling and current non-bridge translation behavior, with bridge support reachable only when explicitly declared.
- Any new public bridge types or members are reflected in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt; otherwise the final delivery summary states that bridge metadata remained internal and no approved snapshot changed.
- No EF mapping, provider-specific behavior, or documentation/example deliverables remain on this ticket after completion.

## Implementation Notes
- Follow the existing provider-neutral modeling pattern in DCoding.Data.DVault.Modeling and keep bridge declarations separate from provider-specific packages.
- Extend the aggregate metadata model additively rather than changing the current hub, link, and satellite caller contract in a breaking way.
- Use explicit bridge-owned participant selection semantics such as declaration-order selectors or an equivalent deterministic selector; hierarchy direction is defined only by those selectors, not by hub names alone.
- The supported v0.5 hierarchy baseline is one recursive DataVaultLinkMetadata traversed once with two distinct participant selectors. The first disallowed cyclical shape is a bridge whose ancestor-side and descendant-side selectors resolve to the same participant in that link.
- Negative tests should use a concrete repeated-hub example such as 'EmployeeReportsTo(Employee, Employee)': selector pair 0->1 or 1->0 is the bounded supported edge, but 0->0 or 1->1 must fail as unsupported self-cycle metadata.
- Keep bridge validation scoped to bridge definitions so current satellite parent-reference behavior and other non-bridge tests do not regress unintentionally.
- If hierarchy support reveals a deeper need to change core link participant identity beyond this ticket's bounded bridge metadata scope, stop at the minimal bridge contract and raise a follow-up instead of broadening into a link or save-service redesign.

## Open Questions
- none

## Follow-Up Questions
- When bridge row materialization is scheduled, should the first shipped bridge capability persist only generated bridge tables or also expose computed or query-time traversal helpers?
- If the first bridge metadata surface stays internal for v0.5, when should it be promoted to a stable public modeling API for external consumers?
- If later requirements need multi-link hierarchy composition or bridge-to-bridge chaining, should that arrive as a dedicated path-model ticket instead of widening this single-link v0.5 bridge contract?

## Risks
- The current metadata stack does not perform aggregate cross-reference validation today, so bridge validation must be introduced carefully to avoid accidental regressions in existing hub, link, and satellite flows.
- Recursive links that repeat the same hub type depend on stable participant selector identity; if selector resolution is not kept explicit and deterministic, hierarchy validation and tests will become brittle.
- Changing the public DataVaultMetadataModel surface in place could create avoidable compatibility churn if existing hub, link, and satellite callers are not kept backward-compatible.

## Split Recommendations
- No further split is required inside current bridge scope; this ticket stays limited to metadata plus validation while EF mapping and docs remain in the existing sibling tickets.
- If future work needs multi-link hierarchy composition, bridge-to-bridge chaining, or a broader redesign of core link participant identity, create a follow-up ticket instead of expanding this v0.5 metadata task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: define metadata for bridge table scenarios over hubs and links.

Acceptance Criteria:
- Metadata can represent many-to-many traversal through a link and a baseline hierarchy traversal shape.
- Validation detects missing hub/link references, cycles where unsupported, and ambiguous relationship definitions.
- Public API shape is documented or explicitly marked internal.