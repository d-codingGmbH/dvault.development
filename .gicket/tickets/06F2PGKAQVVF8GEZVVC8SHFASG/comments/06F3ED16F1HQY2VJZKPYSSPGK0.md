[gicket-bot] PO refinement contract

Summary
- Refined the story as the separate fluent Code-First feature that adds link-parent satellite declarations on links, keeps implementation on the existing metadata-model pipeline, and leaves README/release-note authoring on blocked task 06F2PGM9038RXVJH0RJFYEJEV0.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows link-parent satellites already exist in metadata-first projection and tests, including the CustomerOrder/State baseline that produces SatCustomerOrderState, but the current Code-First surface still excludes them: DataVaultCodeFirstLinkBuilder only exposes Participant<TEntity>() and DataVaultCodeFirstModelBuilder currently materializes satellites only from hub declarations.
- Done child ticket 06F2PGKJBG7NGNVBN0ZDSBE6B8 closed the already-covered test-only work and explicitly pushed any fluent link-parent satellite capability into a separate feature ticket; this story is that feature ticket, not a reopening of the closure-only child.
- The bounded default for this story is additive link-parent satellite declaration on existing Link(...) builders, using a caller-chosen CLR type at the satellite method and the same Payload(...) plus optional DrivingKey(...) selector rules already used by DataVaultCodeFirstSatelliteBuilder<T>.
- Existing relations remain coherent and were not changed in this run: the story stays under epic 06F2PGK4QJ0YGXK5479W83Z2J0, keeps done child 06F2PGKJBG7NGNVBN0ZDSBE6B8, and continues to block effectivity story 06F2PGKV9AFAMKGJEKKZ3AXHGC plus documentation task 06F2PGM9038RXVJH0RJFYEJEV0.
- No new child tickets, relation edits, attachments, or planning documents were materialized during this refinement.

Scope In
- Add link-parent satellite declaration to the public Code-First link builder via Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null) while keeping existing Link(...) overloads and Participant<TEntity>() semantics.
- Project code-first link-parent satellites into DataVaultMetadataModel as DataVaultSatelliteMetadata with parent DataVaultMetadataReference.Link(linkName).
- Reuse existing selector validation, duplicate logical-name rejection, and optional DrivingKey(...) multi-active tuple capture through DataVaultCodeFirstSatelliteBuilder<T>.
- Update representative parity and export coverage so Code-First now produces the same link-parent satellite metadata and EF schema shape that metadata-first already produces for CustomerOrder/State.
- Keep the implementation on the metadata-projection path only and let downstream documentation delivery be handled by blocked task 06F2PGM9038RXVJH0RJFYEJEV0.

Scope Out
- Effectivity satellites, same-as links, dependent child keys, PIT or bridge changes, and other advanced link variants.
- Typed save-helper expansion, compile-time mapping or source-generator feature work, or SaveChanges interception changes.
- README and release-note authoring itself, which is already isolated in task 06F2PGM9038RXVJH0RJFYEJEV0.
- Reopening the already-done coverage-only child 06F2PGKJBG7NGNVBN0ZDSBE6B8 or broad provider-matrix hardening beyond representative parity and regression evidence.

Open questions
- none

Follow-up questions
- After this story lands, should compile-time or source-generator mapping parity for link-parent satellites be tracked as a separate ticket, since current public mapping attributes cover hubs, links, and hub-parent satellites but not a dedicated link-parent satellite attribute surface?
- After documentation lands, do we want a separate quickstart or example ticket that demonstrates end-to-end save and read usage for a Code-First link-parent satellite?

Risks
- The main scope-creep risk is accidentally folding effectivity, same-as, dependent-child-key, or typed-save-helper work into this story because those capabilities are adjacent but separately tracked.
- Public documentation currently still describes link-parent satellite declarations as metadata-first only; if task 06F2PGM9038RXVJH0RJFYEJEV0 is not updated promptly after delivery, shipped behavior and docs will diverge.
- If only the API surface changes without updating Code-First parity and export baselines, regressions could slip past because metadata-first tests already cover SatCustomerOrderState while current Code-First baselines do not.

Split recommendations
- No additional split recommended. Existing child 06F2PGKJBG7NGNVBN0ZDSBE6B8 already closed the coverage-only work, and blocked task 06F2PGM9038RXVJH0RJFYEJEV0 already isolates documentation and release-note follow-through.
- Keep any later advanced link-satellite variants or compile-time mapping parity as separate follow-up tickets rather than expanding this story.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment