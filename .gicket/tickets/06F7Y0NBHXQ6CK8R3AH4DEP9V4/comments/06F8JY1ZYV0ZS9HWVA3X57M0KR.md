[gicket-bot] PO refinement contract

Summary
- Refined this as a bounded documentation-alignment task that advances the public baseline to v0.26.0 by documenting already-landed provider-tuning diagnostics, benchmark-verifier evidence, migration/idempotency guardrails, and the stored-procedure artifact boundary; no child tickets, relation writes, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository docs still show older public baselines such as `docs/performance-profiles.md` status `v0.24.0` and `docs/production-adoption-checklist.md` treating `v0.25.0` release notes as current, so this ticket's bounded purpose is to advance those surfaces to a coherent `v0.26.0` baseline.
- Related completed tickets already bound the content inputs: provider-tuning diagnostics story `06F7Y0JZKTVBGGQ9Q4EBC2PCDG`, benchmark verifier story `06F7Y0K95VW0PX21F6R2YGP8DM`, and stored-procedure boundary task `06F7Y0MCR3GXCE741BR2D06TV4`.
- Documentation must stay on EF Core library usage and consumer-owned operations, so migration guardrails and idempotency preflight are documented as caller-owned checks rather than new runtime automation.
- No bounded planning writes were materialized because current ticket and repository evidence already justify a direct PO contract without a split.

Scope In
- Update README, `docs/performance-profiles.md`, `docs/production-adoption-checklist.md`, relevant architecture notes, and `v0.26.0` release notes to present one coherent public documentation baseline.
- Document the closed provider-tuning recommendation baseline already established by the related diagnostics work: the four checked-in performance-profile categories, bounded save-threshold facts, supported read kinds, and SQLite-only proven optimized read posture.
- Add bounded examples for provider eligibility diagnostics, benchmark verifier evidence, migration guardrails, idempotency preflight, and the stored-procedure artifact boundary.
- Refresh baseline cross-references so `v0.26.0` becomes current and older release notes remain historical feature-introduction records.

Scope Out
- Any library/runtime code changes, new diagnostics fields, new verifier rules, benchmark reruns, or benchmark artifact schema changes.
- Stored-procedure implementation, provider-specific SQL generation, runtime dispatch, migration automation, DBA workflows, or deployment automation.
- New provider performance claims beyond the checked-in benchmark evidence, including non-SQLite optimized read claims the repository does not prove.
- Raw benchmark tables, raw SQL, query plans, credentials, connection strings, exception text, or other unredacted operational data.

Open questions
- none

Follow-up questions
- After `v0.26.0` lands, should a separate follow-up verify README and checklist benchmark citations mechanically the same way `docs/performance-profiles.md` is verified today?
- When additional read-path benchmark evidence exists, should a later documentation ticket add finite provider-specific read-threshold guidance instead of the current profile-based posture?
- If a future provider-specific artifact experiment is approved, should documentation add a worked example for one representative provider, or keep the current generic boundary-only guidance?

Risks
- Documentation can drift from the checked-in diagnostics and verifier contracts if any surface rephrases recommendation categories, thresholds, or provider claims instead of reusing the established bounded vocabulary.
- The docs can overpromise unsupported behavior if provider-specific read guidance or stored-procedure language goes beyond the SQLite-proven read baseline or beyond the explicit opt-in artifact boundary.
- Because the current checklist and performance-profile docs still advertise older baselines, partial updates could leave contradictory current-release signals across surfaces.

Split recommendations
- No immediate split is needed; the current task remains bounded as documentation alignment over already-completed diagnostics, verifier, and stored-procedure-boundary work.
- If the team later wants new benchmark-backed profile categories, provider-specific tutorials, release-process automation, or broader operational governance material, open separate follow-up tickets instead of widening this documentation task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment