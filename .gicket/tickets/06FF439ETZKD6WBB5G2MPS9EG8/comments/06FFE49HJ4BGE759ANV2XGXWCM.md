[gicket-bot] PO refinement contract

Summary
- Fresh repository inspection shows this is a bounded docs-alignment ticket: separate completed PIT/bridge read evidence from PIT maintenance evidence, cite the existing v0.45.0 maintenance contract, and keep bridge maintenance push-down deferred.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already has the maintenance evidence contract: docs/performance-profiles.md treats the v0.45.0 PIT maintenance prototypes as source/test evidence, not benchmark-backed maintenance timing.
- docs/architecture/dvault-v1-pit-bridge-boundary.md already fixes the behavior boundary that PIT and bridge reads consume already-maintained rows and do not perform maintenance.
- Maintained bridge read rows remain read-side evidence only; bridge maintenance push-down is still explicitly deferred in the architecture note, performance guide, v0.45.0 release notes, and provider-optimization gap matrix.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Clarify in docs/performance-profiles.md that completed latest-satellite, PIT, and bridge read timings measure reads over already-maintained read-model rows and must not be cited as provider maintenance timing.
- Align docs/architecture/dvault-v1-pit-bridge-boundary.md so the read boundary, PIT maintenance evidence boundary, and deferred bridge-maintenance push-down posture tell one consistent story.
- Cite the existing maintenance evidence contract surfaces instead of inventing new benchmark claims or a parallel evidence taxonomy.

Scope Out
- Any product-code, diagnostics, benchmark, or provider-registration change.
- Any new PIT or bridge maintenance benchmark lane or new provider-maintenance timing claim.
- Reopening bridge maintenance push-down, MySQL PIT maintenance implementation, or provider-evidence matrix semantics.
- Historical release-note rewrites beyond the citations needed to anchor the current docs.

Open questions
- none

Follow-up questions
- If a future ticket adds benchmark-backed PIT maintenance timings, which single adopter-facing document should become the canonical maintenance timing citation surface?
- If bridge-maintenance hotspot evidence appears later, should the first reopened slice stay limited to many-to-many full rebuild push-down before hierarchy or incremental variants?

Risks
- If only one of the two live docs is updated, the remaining surface can still let readers infer that completed read rows prove provider-maintenance timing.
- Citing the 2026-06-23 provider optimization closure bundle without the maintained-row disclaimer could reintroduce confusion between read-side evidence and maintenance-side evidence.
- Expanding wording beyond the existing v0.45.0 maintenance boundary could accidentally imply benchmark-backed PIT maintenance claims that the repository does not currently prove.

Split recommendations
- No split recommended; the current branch evidence bounds this to one documentation-alignment task across the performance and architecture surfaces.

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