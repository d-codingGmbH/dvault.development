[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded additive support-bundle explain-contract ticket: repeated same-hub links already exist in metadata/model artifacts and explicit save operations, but the support bundle still needs explicit ordered role-aware participant facts so later typed mapper generation can bind those links deterministically without widening mapper runtime in this ticket.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket is scoped to additive `dvault.support-bundle.v1` facts, not to shipping same-hub generated mapper parity. Current repository evidence already supports repeated same-hub links in metadata, model artifacts, EF translation, and explicit save operations when participant keys use distinct produced participant names.
- The new facts belong in `diagnostics.explain`, not `diagnostics.readShape`, because repeated same-hub participant roles are metadata-bound link shape, not request-bound read evidence.
- Use the projected logical participant name already carried by `DataVaultLinkParticipantMetadata.SourceEndpointName` as the authoritative participant name in the exported facts. For ordinary links that value collapses to the hub name, so the bundle should expose one consistent logical participant field for both ordinary and repeated same-hub links while also carrying the referenced hub name separately.

Scope In
- Add additive support-bundle explain facts for link participants that preserve authoritative participant order for both ordinary links and repeated same-hub role-bearing links.
- Expose enough per-participant data for downstream typed mapper generation to distinguish underlying hub name from logical participant name/role and to bind the translated produced participant column/property deterministically.
- Cover Code-First, metadata-first, and model-first projections that already preserve repeated same-hub role metadata.

Scope Out
- Changing `IDataVaultLinkMapper`, `DataVaultLinkParticipantBindingAttribute`, or the existing compile-time mapping source generator to ship same-hub generated mapper parity in this ticket.
- Changing explicit save-service persistence semantics for repeated same-hub links, which already work when participant hash keys are keyed by distinct produced participant names.
- Adding request-bound `readShape` facts, PIT/bridge helper changes, or any non-link support-bundle redesign unrelated to repeated same-hub participant roles.

Open questions
- none

Follow-up questions
- Should the follow-up same-hub typed mapper work stay support-bundle-driven, or should the public compile-time mapping attributes also grow a role-aware link participant contract for manual and generated parity?
- After these facts land, do we want a separate ticket to reconcile the current typed row mapper documentation and generator diagnostics that still describe unique-participant-only link mapping?

Risks
- Because current typed mapper docs and generator diagnostics still enforce unique participant names, implementers may accidentally widen mapper or runtime behavior inside this ticket; keeping this ticket fact-only avoids mixing support-bundle contract work with public mapper contract changes.
- Additive support-bundle changes must remain backward compatible for existing consumers that only read current `diagnostics.explain.entities[].properties[]` shapes.
- If exported facts rely on provider-specific produced names without also preserving logical participant identity, downstream generator work could still be ambiguous for repeated same-hub roles or future naming-policy changes.

Split recommendations
- No split is required for the support-bundle fact work itself; it is a bounded additive explain-contract change with focused tests.
- If same-hub typed mapper emission is also desired, keep it as a separate child or follow-up ticket that consumes these new facts and updates the public mapper or generator contract independently.

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