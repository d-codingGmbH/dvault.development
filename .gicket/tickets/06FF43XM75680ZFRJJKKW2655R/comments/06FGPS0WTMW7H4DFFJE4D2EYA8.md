[gicket-bot] PO refinement contract

Summary
- Fresh repository and relation inspection shows this story is an aggregate parent for already-completed repeated same-hub parity slices; refine it as the bounded v1 tracking contract and move it to PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already proves repeated same-hub links in the modeling and runtime baseline: code-first requires an explicit relationship name plus distinct non-blank roles, and persistence keys link participants by produced participant name.
- Generated typed link-mapper parity is already bounded to explicit unique produced participant names such as SourceCustomer and MatchedCustomer; ambiguous duplicate names remain invalid.
- Support-bundle and explain guidance must preserve ordered same-hub participant facts, but typed helper generation remains support-bundle-driven and does not parse raw dvault.model.v1 files or source-visible declarations directly.
- Adjacent scope stays deferred: dependent child key modeling remains out of the current public baseline, and effectivity remains the existing generic link-parent satellite pattern.
- Repository relation state shows this parent already owns the bounded child slices: 06FF43Y6JE9NQWTAQRQXV2YS80, 06FF43YPV3WYDQHEGZSW4T296C, 06FF440F02AFQNQ0A3XNA2ZS3W, 06FF441DM4F4ZDTHY9ZZD9RA8R, 06FF442BD5V9CTTNXQQAR3EQTW, and 06FF4430YGFJV43ZS54RXEJD5R are done; relation state also shows 06FF43Z97VRFNMVKPZ13CKPN1C duplicated to 06FF43YPV3WYDQHEGZSW4T296C.

Scope In
- Ratify the finite v1 same-hub story boundary across support-bundle facts, generated typed link-mapper parity, and documentation or contract alignment.
- Require explicit relationship names and distinct role-bearing produced participant names for repeated same-hub links so metadata names, produced columns, and generated bindings stay deterministic.
- Keep same-hub generator parity provider-neutral and on the existing IDataVaultLinkMapper<TSource> plus IDataVaultSaveService explicit-save boundary.
- Carry forward the already-decided nearby boundaries for deferred dependent child modeling and effectivity-as-link-parent-satellite guidance.

Scope Out
- Ambiguous repeated same-hub links that omit explicit roles or reuse the same produced participant name.
- New implicit persistence behavior, SaveChanges-driven write paths, provider-specific SQL generation, or a separate same-hub save contract.
- Raw dvault.model.v1 direct typed-helper generation, source-visible direct helper inference, or wider typed-helper parity beyond this same-hub story.
- New dependent child metadata concepts, effectivity-specific fluent APIs, or other broader modeling expansions.

Open questions
- none

Follow-up questions
- If the live parent ticket description should become the authoritative handoff surface, should a follow-up pass write this aggregate contract into the ticket body now that the child-slice decisions are stable?
- If product later wants model-first same-hub typed mapper generation or clearer public naming than ParticipantHubName, should that land as a separate additive compatibility ticket rather than widening this closed v1 story?

Risks
- The live parent ticket description is still the short legacy draft, so readers who do not inspect child tickets or repository docs may miss the bounded aggregate decision until the description is rewritten.
- Public names such as ParticipantHubName and ParticipantHubNames remain semantically awkward for same-hub role-bearing mappings, so incomplete documentation alignment can still make the supported pattern harder to discover.
- Because one child relation points to duplicate ticket 06FF43Z97VRFNMVKPZ13CKPN1C rather than only to the done representative 06FF43YPV3WYDQHEGZSW4T296C, some aggregate views may still look noisier than the real active scope.

Split recommendations
- No additional split recommended; the existing child-ticket breakdown already covers support-bundle facts, generated mapper parity, documentation alignment, and the nearby defer-now decisions.

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