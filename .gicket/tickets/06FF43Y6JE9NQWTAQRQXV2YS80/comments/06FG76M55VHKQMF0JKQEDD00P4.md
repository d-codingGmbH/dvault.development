[gicket-bot] PO refinement contract

Summary
- Revised the contract to remove unsupported public-API inferences: current branch evidence proves repeated same-hub roles in `dvault.model.v1` export and Code-First/EF translation, and the ticket now treats the support-bundle participant explain surface as additive work to create explicitly where the current public explain model has no ordered participant descriptor.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract no longer assumes an existing public support-bundle participant explain API. Current branch evidence shows public explain types `DataVaultExplainDiagnostics` and `DataVaultEntityExplain`, but neither exposes ordered link participant facts, so this ticket may add a new additive participant explain representation instead of reusing a missing type.
- critic-item-2: `answered` - The unsupported existing-public-type inference has been removed. The only visible role-bearing participant name in current branch source is `internal string SourceEndpointName` on `DataVaultLinkParticipantMetadata`, so the contract now treats the public support-bundle explain participant surface as something this ticket may add explicitly rather than something already present.
- critic-item-3: `answered` - The summary is now source-backed only. Current prompt evidence proves repeated same-hub roles in model-artifact export and Code-First/EF translation, while `IDataVaultLinkMapper` still documents unique-participant-only typed link mappings. The ticket therefore remains fact-only and does not claim existing same-hub typed mapper parity or an already-existing public explain participant API.

Clarifications
- This ticket is scoped to additive `dvault.support-bundle.v1` and `diagnostics.explain` facts, not to shipping same-hub typed mapper parity.
- Current branch evidence proves repeated same-hub role support in `dvault.model.v1` export and Code-First/EF translation: `DataVaultModelArtifactExporter` writes ordered `participants` with `hub` plus optional `role`, and `DataVaultCodeFirstLinkTests` verifies `CustomerIdentityMatch` with logical participant names `SourceCustomer` and `MatchedCustomer` and produced columns `SourceCustomerHashKey` and `MatchedCustomerHashKey`.
- Current public explain surface does not yet expose ordered link participant facts: `DataVaultExplainDiagnostics` exposes `Entities`, and `DataVaultEntityExplain` exposes table/property/index/constraint data plus `ProducedName`. This ticket may therefore add a new additive participant descriptor instead of assuming one already exists.
- `DataVaultLinkParticipantMetadata.SourceEndpointName` is internal runtime metadata. Its logical naming semantics may inform the implementation, but it is not itself the public support-bundle contract.

Scope In
- Add additive ordered link-participant facts to the support-bundle explain surface for link entities, including referenced hub name and resolved logical participant name or role.
- Preserve authoritative participant order and expose enough information to associate repeated same-hub participants with their translated produced property or column names.
- Cover ordinary distinct-hub links and repeated same-hub role-bearing links across code-first, metadata-first, and model-first projections already evidenced in the branch.

Scope Out
- Changing `IDataVaultLinkMapper`, `DataVaultLinkParticipantBindingAttribute`, or the existing compile-time mapping source generator to support same-hub typed link mappings.
- Claiming or relying on an already-existing public participant explain type that is not visible in current branch evidence.
- Adding PIT, bridge, or other request-bound `diagnostics.readShape` work unrelated to link participant facts.

Open questions
- none

Follow-up questions
- Should later same-hub typed mapper work extend the public compile-time mapping attributes, or should it consume only the new support-bundle explain facts added here?
- After this additive explain work lands, do we want a follow-up ticket to reconcile public documentation and generator diagnostics around unique-participant-only typed link mappings?

Risks
- Because the current public explain surface has no participant descriptor, the new additive shape must remain backward compatible for existing support-bundle consumers.
- If implementation keys only off produced property names and omits logical participant identity, repeated same-hub roles remain ambiguous under future naming-policy changes.
- Current `IDataVaultLinkMapper` and `DataVaultLinkParticipantBindingAttribute` evidence still models unique participant hub names, so developers may accidentally widen mapper scope unless this ticket stays fact-only.

Split recommendations
- No split is required for the support-bundle fact work itself; the bounded branch evidence supports one additive explain-contract ticket.
- If same-hub typed mapper emission is desired later, keep it as a separate follow-up ticket that consumes these new facts and updates mapper or generator contracts independently.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment