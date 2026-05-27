[gicket-bot] PO refinement contract

Summary
- Rewrote the story around the actual bridge baseline: PIT work stays evidence-focused, and bridge work is limited to diagnostics and benchmark evidence over append-only MaintainBridgeAsync(...) plus RebuildBridgeAsync(...) for shrink; no child tickets, relation changes, description updates, attachments, or planning documents were applied in this refinement pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The delivery contract is revised to match the actual bridge baseline: MaintainBridgeAsync(...) is append-only, RebuildBridgeAsync(...) remains the shrink-safe path, and this story no longer assumes a delivered delete-aware bridge maintenance capability or depends on an unverified implementation ticket.
- critic-item-2: `answered` - Scope-in and acceptance criteria now state that bridge work here is limited to diagnostics and benchmark evidence over the current non-delete-aware contract. This story does not add a new delete-aware maintenance path; it verifies diagnostics, maintenance-result visibility, post-maintenance reads, and measured artifacts only where release or documentation claims require measurement.
- critic-item-3: `answered` - The revised contract ratifies the repository baseline instead of the prior false assumption: bridge maintenance is still explicit and non-delete-aware, and shrink or row-removal scenarios rely on RebuildBridgeAsync(...) rather than incremental delete-aware reconciliation.
- critic-item-4: `answered` - The ambiguity is removed by bounding this ticket to evidence over existing PIT and bridge contracts. Developers are not asked to implement delete-aware bridge maintenance here; any future additive delete-aware bridge capability remains a separate follow-up, while this ticket stays focused on diagnostics, correctness evidence, and benchmark artifacts over the current baseline.

Clarifications
- This story now assumes the actual bridge baseline: bridge maintenance remains explicit and non-delete-aware, with append-only MaintainBridgeAsync(...) and RebuildBridgeAsync(...) as the current shrink-safe recomputation path.
- Registry-backed PIT coverage stays bounded to PIT maintenance-name resolution plus explicit DataVaultPitAsOfReadRequest diagnostics after metadata resolution because the visible API still has no DataVaultRegistryPitAsOfReadRequest equivalent.
- The verified repo baseline already has read-shape diagnostics through IDataVaultReadDiagnosticsService and PIT/bridge maintenance result summaries, so this story extends those evidence surfaces instead of inventing a separate maintenance-diagnostics API.
- Current relation context remains unchanged: the ticket still blocks 06F5Q91M0PM17RP43ZQRPBDXP0, remains a child of 06F5Q90CSKMGK3NZZ25XTW6W4C, and retains incoming historical blocks links from done upstream stories including 06F5Q916BXE2N372SWMH1X776G.
- No child tickets, relation changes, description updates, attachments, or planning documents were applied in this refinement pass.

Scope In
- Add diagnostics coverage that proves registry-backed PIT maintenance-name resolution, explicit PIT read diagnostics, and current bridge read diagnostics stay consistent with the implemented feature contracts.
- Add link-parent PIT evidence that preserves ParentHashKey as the link hash key, shows ordered snapshot-column behavior, and records provider-neutral fallback when provider-specific PIT optimization declines the request shape.
- Add multi-active PIT evidence that preserves tuple-aware row identity, driving-key projection, and expected index and read-shape semantics for the bounded shared-driving-key baseline.
- Add bridge evidence only for the current explicit non-delete-aware maintenance contract: append-only MaintainBridgeAsync(...) result visibility, compatible post-maintenance many-to-many and hierarchy reads, and RebuildBridgeAsync(...) as the shrink-safe path when row removal or increased TraversalDepth must be handled.
- Add benchmark rows or checked-in artifact bundles only where release or documentation claims depend on measured behavior, using diagnostics-backed execution detail and the existing benchmark artifact contract.

Scope Out
- Adding a new delete-aware bridge maintenance behavior, API, or incremental shrink-safe reconciliation path in this story.
- Re-implementing core link-parent PIT, multi-active PIT, PIT maintenance, or bridge maintenance functionality already present in the repository baseline.
- Adding a new public maintenance-diagnostics service or a new public registry-backed PIT as-of read request.
- Provider-specific link-parent or multi-active PIT read optimization, provider-specific bridge maintenance optimization, raw SQL or physical-plan promises, or automatic index creation.
- Automatic PIT or bridge maintenance orchestration, new model-first PIT artifact shapes, or benchmark-schema changes outside docs/plans/performance-evidence-benchmark-artifact-contract.md.
- Broader README, architecture, release-note, or production-checklist completeness work already tracked by 06F5Q91M0PM17RP43ZQRPBDXP0 except for narrowly necessary wording that must ship with the evidence itself.

Open questions
- none

Follow-up questions
- Should a separate additive ticket introduce a public DataVaultRegistryPitAsOfReadRequest so PIT read diagnostics can mirror the registry-backed latest-satellite and bridge read request surfaces?
- If stakeholders later want diagnostics or benchmark evidence for a delete-aware bridge maintenance path, should that be handled only after a real delete-aware bridge capability lands in a separate additive ticket?
- Should provider-specific PIT strategies later accept link-parent or multi-active PIT shapes, or should those scenarios remain documented as provider-neutral fallback baselines?

Risks
- If implementation or docs reintroduce delete-aware bridge wording, this ticket could again overclaim behavior the repository does not currently implement.
- The current root benchmark matrix only covers ordinary PIT and bridge read scenarios; new rows or bundles must make fallback and strategy status explicit or the evidence could be misread as provider-specific optimization support.
- Bridge shrink scenarios can cause scope creep toward a new delete-aware maintenance capability unless development stays bounded to current RebuildBridgeAsync(...) evidence.
- Broader README and release-note completeness work is intentionally downstream, so weak handoff between this evidence ticket and 06F5Q91M0PM17RP43ZQRPBDXP0 could leave published guidance behind the verified behavior.

Split recommendations
- No additional split is recommended if this story stays evidence-only over the existing PIT and non-delete-aware bridge contracts.
- If stakeholders want a new public delete-aware bridge maintenance path or incremental shrink-safe reconciliation behavior, create a separate additive capability ticket instead of broadening this evidence story.
- If broader README, architecture, or release-note completeness work grows beyond narrowly necessary benchmark-surface wording, keep it on 06F5Q91M0PM17RP43ZQRPBDXP0.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment