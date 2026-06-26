[gicket-bot] PO refinement contract

Summary
- Revised the ticket again to remove unsupported public-API inferences from the contract. Current branch evidence proves repeated same-hub role support in model export and Code-First translation, while the support-bundle work is framed only as an additive change on top of the visible explain surface (`DataVaultExplainDiagnostics` and `DataVaultEntityExplain`).

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now cites only visible current-branch evidence. `DataVaultModelArtifactExporter` exports ordered link participants with `hub` plus optional `role`; `DataVaultCodeFirstLinkTests` proves repeated same-hub role names and produced names; and the visible explain contract in prompt context is limited to `DataVaultExplainDiagnostics` and `DataVaultEntityExplain`, so the ordered participant surface is explicitly treated as additive work this ticket may create.
- critic-item-2: `answered` - The contract no longer assumes an existing participant-specific public explain API. It now requires ordered participant facts to become reachable from the public explain surface by an additive change to the visible explain contract, whether that is achieved by extending the visible types or by introducing a new adjacent public contract in this ticket.
- critic-item-3: `answered` - The unsupported claim about an already-existing public participant explain type has been removed from the contract. The revised scope and acceptance criteria avoid naming any unverified participant explain type and instead require the needed ordered participant representation to be added explicitly as part of this ticket.

Clarifications
- This ticket remains fact-only and does not change same-hub typed mapper parity; current typed link mapper evidence still keeps repeated same-hub typed mappings out of scope in `docs/architecture/dvault-v1-typed-row-mapper-contract.md` and `src/DCoding.Data.DVault/IDataVaultLinkMapper.cs`.
- Current branch evidence proves repeated same-hub role support in model/export and Code-First translation: `DataVaultModelArtifactExporter` writes ordered link `participants` with `hub` plus optional `role`, and `DataVaultCodeFirstLinkTests` verifies `CustomerIdentityMatch` with `SourceCustomer`/`MatchedCustomer` and produced columns `SourceCustomerHashKey`/`MatchedCustomerHashKey`.
- Visible explain-surface evidence in the current prompt is limited to `DataVaultExplainDiagnostics` and `DataVaultEntityExplain`; the contract therefore asks for additive ordered participant facts without assuming a pre-existing participant-specific public explain type.
- The branch snapshot shows no checked-in `dvault.model.v1`, `dvault.support-bundle.v1`, `diagnostics.explain`, or `diagnostics.readShape` artifact files, so this ticket is about exported runtime explain output rather than a repository artifact baseline.

Scope In
- Add additive ordered link-participant facts to support-bundle explain output for link entities, including referenced hub name, logical participant role/name, and association to translated produced property or column names.
- Preserve authoritative participant order for both ordinary distinct-hub links and repeated same-hub role-bearing links.
- Add or extend only the minimal public explain-contract surface needed to carry those participant facts.

Scope Out
- Changing `IDataVaultLinkMapper`, `DataVaultLinkParticipantBindingAttribute`, or current compile-time typed link-mapping semantics to support same-hub typed link mappings.
- Changing PIT or bridge request-bound `diagnostics.readShape` behavior or typed read-helper generation.
- Changing existing `dvault.model.v1` repeated same-hub role rules or Code-First same-hub validation already evidenced in the current branch.

Open questions
- none

Follow-up questions
- Should later same-hub typed mapper work extend the public compile-time mapping attributes, or should it consume only the new support-bundle explain facts added here?
- After this additive explain work lands, do we want a follow-up ticket to reconcile public documentation and generator diagnostics around unique-participant-only typed link mappings?

Risks
- Because the visible public explain surface currently has no ordered participant facts, the additive shape must remain backward compatible for existing support-bundle consumers.
- If implementation exports only produced names and omits logical participant role/name, repeated same-hub links remain ambiguous under future naming-policy changes.
- Current public typed link-mapping evidence is still unique-participant-only, so developers may accidentally widen mapper scope unless this ticket stays fact-only.

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