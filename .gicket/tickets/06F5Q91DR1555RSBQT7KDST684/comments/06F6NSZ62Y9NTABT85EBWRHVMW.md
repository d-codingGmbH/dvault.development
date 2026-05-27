[gicket-bot] PO refinement contract

Summary
- Refined this into an evidence-focused follow-up over completed link-parent PIT, multi-active PIT, and delete-aware bridge capabilities; no child tickets, relation edits, description updates, attachments, or planning documents were applied.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Ticket evidence shows upstream stories 06F5Q90SX5AQ07M4PQKDR4BZD8, 06F5Q9102970H1VQN16QWRGQX0, and 06F5Q916BXE2N372SWMH1X776G are done, so this ticket assumes link-parent PIT, shared-driving-key multi-active PIT, and delete-aware bridge maintenance as completed baselines rather than re-opening core feature design.
- The verified repo baseline already has read-shape diagnostics through IDataVaultReadDiagnosticsService and explicit PIT/bridge maintenance result summaries; this story extends those evidence surfaces instead of inventing a separate public maintenance-diagnostics service.
- The visible public API still has registry-backed PIT maintenance requests but no registry-backed PIT as-of read request, so 'registry-backed PIT diagnostics' here is bounded to registry resolution and equivalence evidence around PIT maintenance and explicit PIT read diagnostics after metadata resolution.
- The current benchmark baseline only covers ordinary PIT as-of and hierarchy bridge traversal scenarios, so new measured evidence must either add completed-shape rows to the existing root triplet or add a checked-in before/after bundle under artifacts/benchmarks/... that follows the benchmark artifact contract.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement pass; the ticket remains unblocked, the outgoing blocks link to documentation task 06F5Q91M0PM17RP43ZQRPBDXP0 still makes sense, and the incoming blocks links from the three done upstream stories were left as historical sequencing context.

Scope In
- Add diagnostics coverage that proves registry-backed PIT resolution, explicit PIT read diagnostics, and completed bridge read diagnostics stay consistent with the completed feature contracts.
- Add link-parent PIT evidence that preserves ParentHashKey as the link hash key, shows the ordered snapshot-column contract, and records provider-neutral fallback when provider-specific PIT optimization does not accept the shape.
- Add multi-active PIT evidence that preserves tuple-aware row identity, driving-key projection, and expected index/read-shape semantics for the bounded shared-driving-key baseline.
- Add delete-aware bridge evidence that connects the explicit shrink-safe maintenance path to correct many-to-many and hierarchy bridge read behavior without implying automatic maintenance.
- Add benchmark rows or checked-in artifact bundles only where release or documentation claims depend on measured behavior, using diagnostics-backed execution detail and the existing benchmark artifact contract.

Scope Out
- Re-implementing core link-parent PIT, multi-active PIT, or delete-aware bridge functionality already owned by the completed upstream stories.
- Adding a new public maintenance-diagnostics service or a new public registry-backed PIT as-of read request.
- Provider-specific link-parent or multi-active PIT read optimization, provider-specific delete-aware bridge maintenance optimization, raw SQL or physical-plan promises, or automatic index creation.
- General README, architecture, production-checklist, or release-note completeness work already blocked behind task 06F5Q91M0PM17RP43ZQRPBDXP0, except for narrowly necessary benchmark-surface or diagnostics-contract wording that must ship with the evidence itself.
- Automatic PIT or bridge maintenance orchestration, new model-first PIT artifact shapes, or broader benchmark-schema changes outside docs/plans/performance-evidence-benchmark-artifact-contract.md.

Open questions
- none

Follow-up questions
- Should a separate additive ticket introduce a public DataVaultRegistryPitAsOfReadRequest so PIT read diagnostics can mirror the registry-backed latest-satellite and bridge read request surfaces?
- Should provider-specific PIT strategies later accept link-parent or multi-active PIT shapes, or should those scenarios remain permanently documented as provider-neutral fallback baselines?
- If future release guidance wants performance language for delete-aware bridge maintenance itself, should that be handled as a separate benchmark and evidence ticket instead of broadening this story?

Risks
- The current public diagnostics surface is read-oriented, so expanding this ticket into a new maintenance-diagnostics API would materially enlarge scope and compatibility risk.
- The current root benchmark matrix only covers ordinary PIT and bridge read scenarios; new rows or bundles must make fallback and strategy status explicit or the evidence could be misread as provider-specific optimization support.
- Broader README and release-note completeness work is intentionally downstream, so weak handoff between this evidence ticket and 06F5Q91M0PM17RP43ZQRPBDXP0 could leave published guidance behind the verified behavior.

Split recommendations
- No additional split is recommended; the remaining work is one cohesive evidence pass across the existing read-diagnostics surface and the established benchmark artifact contract.
- If stakeholders want a new public registry-backed PIT read API or a dedicated maintenance-diagnostics API, split that into a separate additive API ticket instead of enlarging this evidence story.
- If the broader README, architecture, or release-note completeness pass grows beyond narrowly necessary benchmark-surface wording, keep it on the existing downstream documentation task 06F5Q91M0PM17RP43ZQRPBDXP0.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment