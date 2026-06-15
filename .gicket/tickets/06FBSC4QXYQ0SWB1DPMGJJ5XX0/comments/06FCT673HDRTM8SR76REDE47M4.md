[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded docs-only v0.39 update, verified the repository evidence baseline, and materialized a ticket-bound planning note at `docs/plans/provider-optimization-evidence-docs-v0.39-refinement.md`.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The authoritative fact source is `docs/plans/provider-optimization-evidence-matrix.md`; the authoritative future-work source is `docs/plans/provider-optimization-gap-matrix.md`.
- The v0.39 documentation update should touch `docs/performance-profiles.md`, add `docs/releases/v0.39.0.md`, and update `CHANGELOG.md` rather than duplicating matrix content into new ad hoc tables.
- A ticket-bound refinement note was materialized at `docs/plans/provider-optimization-evidence-docs-v0.39-refinement.md` to persist the verified scope, release-note target, and evidence boundaries.
- Use the established coordinated release-label pattern for the new release note unless separate release-planning work changes it: v0.39.0 over the visible `8.39.0` and `10.39.0` consumer package-version lines.

Scope In
- Update `docs/performance-profiles.md` so measured evidence is explicitly separated from follow-up recommendations and readers are directed to the evidence and gap matrices for the canonical details.
- Create `docs/releases/v0.39.0.md` to record the v0.39 provider-evidence-matrix documentation baseline, caveats, and follow-up recommendation posture.
- Add the matching `CHANGELOG.md` entry that points to the new v0.39.0 release note.

Scope Out
- Rerunning benchmarks, generating new benchmark artifact triplets, or changing the benchmark schema or row contract.
- Adding provider implementations, changing diagnostics behavior, or widening provider claims beyond the checked-in evidence baseline.
- Treating skipped-placeholder, diagnostics-only, smoke-only, or storage-footprint rows as completed external-provider timing evidence.
- Broad documentation sweeps outside the bounded performance docs and release-note surfaces unless a later ticket opts into them explicitly.

Open questions
- none

Follow-up questions
- After the v0.39 documentation baseline lands, should a later docs ticket propagate the same evidence-matrix and gap-matrix cross-links into other adopter-facing surfaces such as `docs/production-adoption-checklist.md` if drift appears there?
- When provider-configured benchmark bundles are added later, which gap-matrix rows should be promoted first from follow-up recommendations into release-note-ready completed timing claims?

Risks
- The live gicket comment and relation reads were trust-policy blocked in this run, so duplicate and relation decisions rely on the provided ticket snapshot and repository evidence rather than a fresh persisted relation read.
- If separate release-planning work changes the established dual package-version-line pattern for v0.39, the new release note wording will need to be adjusted to match that later release decision.

Split recommendations
- No split recommended. The visible repository evidence supports one bounded documentation task across `docs/performance-profiles.md`, `docs/releases/v0.39.0.md`, and `CHANGELOG.md`, and the ticket-bound refinement note has already been materialized to preserve that scope.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment