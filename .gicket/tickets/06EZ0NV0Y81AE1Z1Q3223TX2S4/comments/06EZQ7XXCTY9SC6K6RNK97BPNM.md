[gicket-bot] PO refinement contract

Summary
- Refined the bridge-metadata task to an additive opt-in modeling contract: bridge definitions extend the existing provider-neutral metadata model, cover one many-to-many path plus one bounded hierarchy path, add bridge-specific validation, and follow the established API snapshot or internal-surface rule.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket owns bridge metadata declarations and bridge-specific validation only; provider-neutral EF mapping is already split to 06EZ0NV7KG94MTMNXMGVRYVW9C and documentation/example work to 06EZ0NVE88WW9PMM04NVAZHRG0.
- Bridge support stays an opt-in deferred v0.5 capability and must not change ordinary hub, link, satellite, AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), or explicit save-service behavior by default.
- The current public modeling baseline is the provider-neutral DCoding.Data.DVault.Modeling metadata family plus the public ApplyDataVaultMetadata() entry point, so bridge metadata should extend that aggregate additively rather than replace existing hub, link, and satellite contracts.
- Bridge validation in this ticket is bridge-specific; it should not retroactively tighten current non-bridge behavior such as the existing satellite metadata translation tests that allow unresolved parent names.
- If this ticket exports new public bridge modeling types, the same change must update the core public API snapshot; if the first bridge slice stays internal, the final delivery note must say so explicitly and approved snapshots should remain unchanged.

Scope In
- Add provider-neutral bridge metadata declarations alongside the existing hub, link, and satellite metadata model.
- Represent one many-to-many traversal shape by explicitly tying a source hub, one traversed link, and a target hub into one bridge definition.
- Represent one bounded hierarchy traversal shape over one declared link with explicit ancestor-side and descendant-side participant selection for a single recursive edge.
- Add bridge-specific validation for unknown references, invalid participant selectors, ambiguous endpoint selection, and unsupported metadata-level cycles.
- Keep existing hub, link, and satellite callers backward-compatible by making bridge support additive and opt-in.

Scope Out
- Provider-neutral EF Core mapping for bridge tables; that belongs to 06EZ0NV7KG94MTMNXMGVRYVW9C.
- User-facing bridge documentation and example scenarios; that belongs to 06EZ0NVE88WW9PMM04NVAZHRG0.
- Bridge row materialization, refresh or maintenance strategy, load workflows, SaveChanges interception, or explicit save-service behavior.
- Provider-specific DDL, indexing strategy, SQL optimization, migrations, or benchmark posture.
- Unbounded traversal variants such as multi-link hierarchy composition, alternate cycle policies, business-rule-driven pruning, or other advanced bridge patterns.

Open questions
- none

Follow-up questions
- When bridge row materialization is scheduled, should the first shipped bridge capability persist only generated bridge tables or also expose computed or query-time traversal helpers?
- If the first bridge metadata surface stays internal for v0.5, when should it be promoted to a stable public modeling API for external consumers?

Risks
- The current metadata stack does not perform aggregate cross-reference validation today, so bridge validation must be introduced carefully to avoid accidental regressions in existing hub, link, and satellite flows.
- Hierarchy scenarios become ambiguous if implementation relies only on hub names instead of an explicit participant selector, especially when the same hub appears more than once in a relationship.
- Changing the public DataVaultMetadataModel surface in place could create avoidable compatibility churn if existing hub, link, and satellite callers are not kept backward-compatible.

Split recommendations
- No further split is required inside bridge scope; this ticket stays limited to metadata plus validation while EF mapping and docs remain in the existing sibling tickets.
- If supporting the bounded hierarchy shape requires a broader redesign of core link participant identity or the explicit save-service link contract, create a follow-up ticket for that base-link gap instead of expanding this metadata task.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment