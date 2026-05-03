[gicket-bot] PO refinement contract

Summary
- Verified the parent epic is already refined as a coordination-only closure umbrella over four existing done child stories, with README.md and benchmarks/DCoding.Data.DVault.Benchmarks as the approved closure surfaces; no new child tickets, relations, attachments, or planning documents are needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent epic stays coordination-only and owns no direct implementation slice. The bounded delivery work already sits on the four existing child stories, all four child tickets are persisted and currently done, and any future residual scope must be scheduled as separate follow-up work rather than reopened on the parent epic.

Clarifications
- This parent epic is tracking-only and closure-only; it owns no direct parent implementation, documentation, benchmark, or product-code slice.
- The epic already has four persisted child tickets and they are currently done: 06EXB7QYF1BB1REM7HQZ4WWVMM (Write getting started documentation), 06EXB7RPKGTEW4RZKYQ2DXS554 (Build example scenario for customer profile history), 06EXB7SEAWB2KSBQSHQB2MVV38 (Build example scenario for orders and product relationships), and 06EXB7T62EMCD7CSHS9PE501SC (Build benchmark harness for normal EF versus DVault).
- The epic remains related to charter ticket 06EXB4MDREV2T51VJNJEP6R0WR.
- No parent ticket attachment files were found for this ticket, and the comments in scope are automation and handoff records rather than new human scope changes.
- For epic closure, the approved runnable-example surfaces are the README.md quickstart and the existing benchmarks/DCoding.Data.DVault.Benchmarks scenarios and guidance; examples/ remains future follow-up only.
- No new child-ticket, relation, attachment, or planning-document write was needed in this pass.

Scope In
- Tracking and closure coordination across the four existing child tickets that carry the bounded documentation, example, and benchmark work for this epic.
- Ratifying README.md as the canonical beginner quickstart and benchmarks/DCoding.Data.DVault.Benchmarks as the approved comparison-example surface for v1 closure.
- Cross-story consistency review so README quickstart, benchmark guidance, and supporting architecture notes stay aligned to the SQLite-first v1 contract.
- Epic-level closure verification that child-delivered documentation and benchmark outputs satisfy the agreed bounded scope without introducing new parent-owned implementation work.

Scope Out
- Any direct parent-owned implementation, documentation, benchmark, or product-code work under this epic.
- Creating a separate standalone examples/ asset tree as part of this epic.
- Provider baselines beyond SQLite for this v1 epic.
- NuGet publication guidance, package-version install instructions, or release and distribution work.
- Deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.
- Any newly discovered residual scope after this refinement; that belongs in a separate follow-up ticket or epic, not back on this parent epic.

Open questions
- none

Follow-up questions
- After the SQLite-first epic lands, should a separate follow-up epic add provider-specific documentation and example material once additional provider profiles exist?
- If onboarding feedback later shows the README quickstart is insufficient, should a separate follow-up ticket create a dedicated examples/ tree after MVP rather than expanding this epic now?
- Should benchmark evidence eventually be published as a checked-in report or attached release artifact instead of remaining primarily runnable from the benchmark project?
- Once package publication exists, should the quickstart be split into separate source-consumption and NuGet-consumption guides?

Risks
- If README quickstart and benchmark guidance drift apart across child outputs, closure readiness will still be confusing even with the parent epic kept coordination-only.
- README.md still reserves examples/ for future use, so later edits must avoid implying that a standalone examples/ tree is required for this epic.
- Benchmark comparisons will mislead reviewers if the conventional EF and DVault baselines stop using the same scenario contracts, data volume, lineage assumptions, or timestamp assumptions.
- If benchmark artifacts are cited without provider and environment context, future readers may misread machine-specific timings as general performance claims.
- If contributors reopen parent-owned implementation work on this epic instead of creating follow-up work, the coordination-only closure boundary will blur again.

Split recommendations
- No additional split is recommended at the epic level; the existing four child tickets already carry the bounded delivery work while this epic remains the coordination-only closure umbrella.
- Any future standalone examples/ tree, provider-specific documentation, broader benchmark publication, or post-NuGet quickstart split should be scheduled as separate follow-up tickets or epics instead of enlarging this SQLite-first MVP epic.

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