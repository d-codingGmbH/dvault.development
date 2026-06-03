[gicket-bot] PO refinement contract

Summary
- Refined the implementation story around DMV1912-DMV1914 using the completed lifecycle contract and the current analyzer/test baseline; no bounded planning writes or relation changes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence shows the EfCore analyzer slice currently implements only DMV1910 and DMV1911, so this story is the implementation lane for the next contiguous lifecycle diagnostics rather than a runtime or documentation change.
- Ticket context shows no recent human comments or closure-evidence amendments that reopen or narrow the lifecycle contract.
- Related-ticket evidence already separates lifecycle contract work in done ticket 06F8KZGC4NY41PRYB2RP00ZA1M from this implementation story and from sibling fixture/documentation stories 06F8KZGZND5ZCH147PVBRWXYN4 and 06F8KZHAB717MJJNAWWK7S0A5W, so no additional child-ticket split is justified from the visible scope.
- No bounded ticket write, relation update, attachment, or planning document was applied in this refinement run.

Scope In
- Implement Roslyn diagnostics DMV1912, DMV1913, and DMV1914 inside the existing EfCore analyzer slice for source-visible DVault EF lifecycle misuse.
- Add the analyzer metadata, messaging, and bounded detection logic for missing visible model-cache discriminators, unsafe variable-shape UseModel(...) lanes, and unsafe AddDbContextPool<TContext>(...) lanes.
- Add focused analyzer unit coverage for the new diagnostic and non-diagnostic cases needed to prove the implementation without taking on the separate broad fixture-matrix story.

Scope Out
- Whole-application DI inference, runtime tenant-state inference, generated compiled-model artifact inspection, or proof over opaque helper and service-registration code.
- Runtime behavior changes, SaveChanges guards, provider-specific validation, or changes to existing DMV1910 and DMV1911 behavior.
- Broad fixture expansion and documentation or public release-note updates, which stay with sibling tickets 06F8KZGZND5ZCH147PVBRWXYN4 and 06F8KZHAB717MJJNAWWK7S0A5W.

Open questions
- none

Follow-up questions
- After implementation lands, decide whether later backlog work should extend the same lifecycle analysis to AddPooledDbContextFactory<TContext> or other pooling entrypoints.
- After the analyzer wording stabilizes, decide whether the ticket description should mirror the accepted DMV1912-DMV1914 contract or continue to rely on the existing contract and documentation tickets.

Risks
- If implementation expands beyond direct source-visible evidence, it is likely to create false positives against safe fixed-model compiled-model and registry-backed metadata lanes.
- If implementation remains intentionally high-confidence, some real unsafe caller-owned variable-shape cases will remain guidance-only and must be explained by the separate documentation lane.
- The boundary between targeted implementation tests here and the broader fixture story must stay explicit or the ticket will regrow into already-split test-surface work.

Split recommendations
- No further split is recommended; visible evidence already separates contract, implementation, fixtures, and documentation into dedicated tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment