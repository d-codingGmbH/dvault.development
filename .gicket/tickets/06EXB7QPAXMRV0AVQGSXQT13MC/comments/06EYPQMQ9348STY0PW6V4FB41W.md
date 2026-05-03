[gicket-bot] PO refinement contract

Summary
- Refined the epic so it is explicitly coordination-only over its existing child stories and so the runnable-example requirement is satisfied by `README.md` plus the existing benchmark scenarios, with no standalone `examples/` tree required; no new child tickets, relations, attachments, or planning documents were created in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This epic is coordination-only. It does not add new epic-level developer work beyond aligning its existing child stories, and epic completion is reached once those child tickets deliver the bounded documentation and benchmark outcomes and the resulting repository guidance is mutually consistent.
- critic-item-2: `answered` - The runnable-example requirement for this epic is explicitly satisfied by the repository-root `README.md` quickstart together with the existing benchmark scenarios under `benchmarks/DCoding.Data.DVault.Benchmarks`; this epic does not require separate standalone assets under `examples/`.
- critic-item-3: `answered` - Resolved the ambiguity by ratifying a single completion target: the canonical runnable example surface is `README.md`, and the comparison example surface is the existing benchmark scenarios; standalone `examples/` assets are out of scope for this epic unless a later follow-up ticket introduces them.

Clarifications
- No human comments or ticket attachments add scope in the provided ticket context.
- The epic already has four persisted `parentOf` child tickets: `06EXB7QYF1BB1REM7HQZ4WWVMM`, `06EXB7RPKGTEW4RZKYQ2DXS554`, `06EXB7SEAWB2KSBQSHQB2MVV38`, and `06EXB7T62EMCD7CSHS9PE501SC`.
- The epic remains related to charter ticket `06EXB4MDREV2T51VJNJEP6R0WR`, and no additional child-ticket, relation, attachment, or planning-document write was needed in this refinement pass.
- This epic is an umbrella for cross-story alignment and closure criteria, not a separate bucket for new product-code work once the child stories are in flight.
- For this epic, the required runnable-example surface is the repository `README.md` quickstart plus the benchmark scenarios already hosted in `benchmarks/DCoding.Data.DVault.Benchmarks`; a standalone `examples/` directory is not required.

Scope In
- Update and align beginner-focused English documentation in `README.md` as the canonical runnable quickstart for the current source-consumed `DCoding.Data.DVault` package.
- Deliver SQLite-backed example flows through the README quickstart and the existing benchmark scenarios so the current MVP hub, link, satellite, and explicit save concepts are demonstrable from repository code.
- Maintain conventional EF baseline implementations that are directly comparable to the DVault scenarios for the bounded benchmark cases.
- Provide benchmark execution guidance and reproducible evidence for the existing customer-profile and order-product fulfillment comparisons in `benchmarks/DCoding.Data.DVault.Benchmarks`.

Scope Out
- Creating a separate standalone `examples/` asset tree for this epic.
- New epic-level implementation work outside the already-related child stories.
- New core runtime features beyond what is required to demonstrate the current DVault API surface.
- Provider baselines beyond SQLite for this v1 epic.
- NuGet publication guidance, package-version install instructions, or release/distribution work.
- Deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.

Open questions
- none

Follow-up questions
- After the SQLite-first epic lands, should a separate follow-up epic add provider-specific documentation and example material once additional provider profiles exist?
- If onboarding feedback later shows the README quickstart is insufficient, should a separate follow-up ticket create a dedicated `examples/` tree after MVP rather than expanding this epic now?
- Should benchmark evidence eventually be published as a checked-in report or attached release artifact instead of remaining primarily runnable from the benchmark project?
- Once package publication exists, should the quickstart be split into separate source-consumption and NuGet-consumption guides?

Risks
- If child stories update the README quickstart and benchmark guidance inconsistently, the now-explicit decision that those surfaces satisfy the example requirement may still be confusing to readers.
- If docs or benchmarks do not explicitly repeat that v1 is SQLite-first, readers may infer unsupported provider breadth from general EF wording.
- Benchmark results will be misleading if the conventional EF and DVault baselines stop using the same scenario shape, data volume, or lineage and timestamp assumptions.
- If produced benchmark artifacts are referenced without their provider and environment context, future reviewers may misread machine-specific timings as general performance claims.

Split recommendations
- No additional split is recommended at the epic level in this PO pass; the existing four child tickets should continue to carry the bounded delivery work while the epic remains the coordination and closure umbrella.
- Any future standalone `examples/` tree, provider-specific documentation, or broader benchmark publication work should be scheduled as separate follow-up tickets or epics instead of enlarging this MVP-scoped epic.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment