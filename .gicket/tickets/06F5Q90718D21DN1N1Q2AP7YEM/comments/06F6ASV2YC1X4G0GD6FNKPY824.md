[gicket-bot] PO refinement contract

Summary
- Refined the v0.20.0 documentation task against the existing v0.19.0 public baseline and live relation state; no child tickets, relation edits, description edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already establishes v0.19.0 as the current public baseline and explicitly keeps staged provider bulk ingestion outside that release's claim set, so this ticket should document the v0.20.0 boundary rather than reopen the v0.19.0 decision.
- The shared benchmark artifact contract in `docs/plans/performance-evidence-benchmark-artifact-contract.md` is the v1 evidence baseline; staged-bulk benchmark guidance should reuse that contract instead of inventing ticket-specific artifact formats.
- The existing parent relation from `06F5Q8YBVRS2EZVMJK5EATV9AR` is sufficient for tracking; no further ticket split or relation cleanup is justified from the current evidence.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement run.

Scope In
- Update README guidance to position staged bulk ingestion as the preferred provider-optimized bulk path for v0.20.0 while keeping the existing explicit-save surfaces aligned with their documented roles.
- Update `docs/production-adoption-checklist.md` so adopter guidance points to the v0.20.0 documentation boundary and explicitly avoids presenting automatic stored-procedure generation as the default path.
- Update benchmark-facing and release-note documentation so staged-bulk performance claims reuse the existing benchmark summary triplet and shared artifact contract instead of defining a new evidence schema.
- Document stored procedures only as an explicit design-time or provider-specific escape hatch that requires confirmed provider evidence and migration-synchronization guidance.

Scope Out
- Implementing staged bulk ingestion, provider-native chunk execution, or automatic stored-procedure generation behavior in product code.
- Introducing new benchmark artifact schemas, new performance harnesses, or release automation changes.
- Designing generic stored-procedure scaffolding or migration helpers beyond documenting the boundary and caveats.

Open questions
- none

Follow-up questions
- After the upstream implementation and evidence blockers close, should the v0.20.0 evidence story keep using the root benchmark summary triplet as the public baseline, or should release-specific before/after bundles also be published under the existing artifact contract?
- Once provider evidence is stable, does the roadmap want a future provider-by-provider decision matrix covering staged bulk, chunked explicit save, and any explicit stored-procedure escape hatches?

Risks
- The current ticket remains relation-blocked by `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`; if upstream implementation or evidence shifts, the documentation boundary may need wording changes before release.
- Because three downstream tickets are currently blocked by this documentation ticket, ambiguity in the write-path hierarchy or stored-procedure caveats will propagate quickly.
- If provider evidence or migration-synchronization rules are incomplete at doc-authoring time, the stored-procedure section can overclaim unsupported automation.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment