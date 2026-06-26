[gicket-bot] PO refinement contract

Summary
- Fresh repo inspection shows repeated same-hub links already work in metadata/runtime when participant roles are explicit; this ticket is refined to generated typed link-mapper parity, explicit-save-path verification, and doc/diagnostic alignment.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current runtime save path already supports repeated same-hub links when link metadata carries distinct produced participant names such as SourceCustomer and MatchedCustomer; the resolved save path reads participant values by produced participant name.
- This ticket does not need a new persistence boundary. The additive goal is generated typed link-mapper parity so same-hub role-bearing links flow through the existing IDataVaultLinkMapper<TSource> and IDataVaultSaveService helper path.
- For this ticket, DataVaultLinkParticipantBindingAttribute should be treated as binding the produced participant name: the hub name for ordinary links, or the explicit participant role name for repeated same-hub links.

Scope In
- Generated typed link-mapper support for repeated same-hub links when every participant name is explicit, non-blank, and unique by StringComparer.Ordinal.
- Alignment of analyzer diagnostics, XML/docs/contract text, and generated helper metadata so the supported shape is described as role-bearing produced participant names rather than distinct hub types.
- Verification that generated same-hub link mappers work through the existing explicit save-service helper flow without introducing implicit persistence behavior.

Scope Out
- Ambiguous repeated same-hub links without explicit distinct participant names or roles.
- New implicit persistence, SaveChanges interception, or a replacement for the existing IDataVaultSaveService boundary.
- Broader typed-helper parity work outside repeated same-hub links, including typed read-model generation, link-parent satellite helper expansion, effectivity-specific APIs, or model-first support-bundle changes.

Open questions
- none

Follow-up questions
- Should a later compatibility cleanup add a clearer additive alias or obsoletion path for ParticipantHubName-style naming, which now semantically represents a produced participant name when same-hub roles are explicit?

Risks
- The public names ParticipantHubName and ParticipantHubNames are misleading for same-hub role-bearing mappings; incomplete doc alignment could leave the supported pattern hard to discover.
- If implementation expands into a new public same-hub-specific save contract instead of reusing the current mapper and save-service path, scope and compatibility risk grow unnecessarily.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment