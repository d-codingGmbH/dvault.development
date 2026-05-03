[gicket-bot] PO refinement contract

Summary
- Refined the epic as a tracking-only closure umbrella over its four existing child tickets, with README.md plus benchmarks/DCoding.Data.DVault.Benchmarks as the approved runnable-example surfaces and no new child-ticket, relation, attachment, or planning-document write needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Resolved the closure audit by making the parent epic explicitly tracking-only and closure-only: it owns no direct implementation slice and can close only after its four existing child tickets deliver the bounded documentation and benchmark outcomes and those outputs are mutually consistent.
- critic-item-2: `answered` - Resolved the persisted contract gap by explicitly stating that this tracking-only epic has no parent-owned implementation work. The parent only ratifies the approved completion surfaces for child delivery: the README.md quickstart and the existing benchmarks/DCoding.Data.DVault.Benchmarks scenarios and guidance, with no standalone examples/ tree required for this epic.

Clarifications
- This parent epic is tracking-only and closure-only; it owns no direct parent implementation slice and exists to coordinate and close out child delivery.
- The epic already has four persisted child tickets via parentOf: 06EXB7QYF1BB1REM7HQZ4WWVMM, 06EXB7RPKGTEW4RZKYQ2DXS554, 06EXB7SEAWB2KSBQSHQB2MVV38, and 06EXB7T62EMCD7CSHS9PE501SC.
- The epic remains related to charter ticket 06EXB4MDREV2T51VJNJEP6R0WR and no new child-ticket, relation, attachment, or planning-document write was needed in this pass.
- No human comments or ticket attachments add scope beyond the existing contract context.
- For epic closure, the approved runnable-example surfaces are the repository README.md quickstart and the existing benchmarks/DCoding.Data.DVault.Benchmarks scenarios and guidance; the examples/ area is future follow-up only and is not part of this epic's closure scope.

Scope In
- Tracking and closure coordination across the four existing child tickets that deliver the bounded documentation, example, and benchmark work for this epic.
- Ratifying README.md as the canonical beginner quickstart and benchmarks/DCoding.Data.DVault.Benchmarks as the approved comparison-example surface for v1 closure.
- Cross-story consistency review so README quickstart, benchmark guidance, and supporting architecture notes stay aligned to the SQLite-first v1 contract.
- Epic-level closure verification that child-delivered documentation and benchmark outputs satisfy the agreed bounded scope without introducing new parent-owned implementation work.

Scope Out
- Any direct parent-owned implementation, documentation, benchmark, or product-code work under this epic.
- Creating a separate standalone examples/ asset tree for this epic.
- New core runtime features beyond what the existing child tickets need to demonstrate the current DVault API surface.
- Provider baselines beyond SQLite for this v1 epic.
- NuGet publication guidance, package-version install instructions, release/distribution work, and deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.

Open questions
- none

Follow-up questions
- After the SQLite-first epic lands, should a separate follow-up epic add provider-specific documentation and example material once additional provider profiles exist?
- If onboarding feedback later shows the README quickstart is insufficient, should a separate follow-up ticket create a dedicated examples/ tree after MVP rather than expanding this epic now?
- Should benchmark evidence eventually be published as a checked-in report or attached release artifact instead of remaining primarily runnable from the benchmark project?
- Once package publication exists, should the quickstart be split into separate source-consumption and NuGet-consumption guides?

Risks
- If child stories update README quickstart and benchmark guidance inconsistently, closure readiness may still be confusing even with the parent epic now marked tracking-only.
- README.md currently references examples/ as a future-use area, so child documentation changes must avoid reintroducing ambiguity between that placeholder and the approved current runnable surfaces.
- Benchmark results will be misleading if the conventional EF and DVault baselines stop using the same scenario contracts, data volume, or lineage and timestamp assumptions.
- If benchmark artifacts are cited without provider and environment context, future reviewers may misread machine-specific timings as general performance claims.
- If contributors reopen parent-owned implementation tasks on this epic instead of its child stories, the tracking-only closure boundary may blur again.

Split recommendations
- No additional split is recommended at the epic level; the existing four child tickets already carry the bounded delivery work while this epic remains the tracking and closure umbrella.
- Any future standalone examples/ tree, provider-specific documentation, or broader benchmark publication work should be scheduled as separate follow-up tickets or epics instead of enlarging this SQLite-first MVP epic.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment