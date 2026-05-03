[gicket-bot] PO refinement contract

Summary
- Refined the epic against current repo and ticket evidence: the repository already contains a source-consumed quickstart in `README.md`, an existing benchmark harness in `benchmarks/DCoding.Data.DVault.Benchmarks`, and four persisted child tickets, so the epic can be locked to SQLite-first documentation/examples plus reproducible DVault-vs-normal-EF benchmark evidence without further PO blockers.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- No human comments or ticket attachments add scope; the only current comments are automation claim/lease records.
- The epic already has four persisted `parentOf` child relations: `06EXB7QYF1BB1REM7HQZ4WWVMM`, `06EXB7RPKGTEW4RZKYQ2DXS554`, `06EXB7SEAWB2KSBQSHQB2MVV38`, and `06EXB7T62EMCD7CSHS9PE501SC`.
- The epic remains related to charter ticket `06EXB4MDREV2T51VJNJEP6R0WR` and no additional planning document, attachment, or child-ticket write was needed in this PO pass.
- Current repository evidence already includes a beginner quickstart in `README.md` and a benchmark project under `benchmarks/DCoding.Data.DVault.Benchmarks`, so this epic is about aligning and completing those assets rather than defining a new documentation or benchmark architecture.

Scope In
- Beginner-focused English documentation and runnable examples for the current source-consumed `DCoding.Data.DVault` package.
- SQLite-backed example scenarios that reflect the current MVP Data Vault concepts and explicit save flow.
- Normal EF baseline implementations that are directly comparable to the DVault scenarios.
- Benchmark execution guidance and benchmark evidence for the bounded comparison scenarios already hosted in `benchmarks/DCoding.Data.DVault.Benchmarks`.

Scope Out
- New core runtime features beyond what is required to demonstrate the current DVault API surface.
- Provider baselines beyond SQLite for this v1 epic.
- NuGet publication guidance, package-version install instructions, or release/distribution work.
- Deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.
- Advanced configuration-hook implementation work beyond documenting current convention-first defaults where relevant.

Open questions
- none

Follow-up questions
- After the SQLite-first epic lands, should a separate post-MVP documentation epic add provider-specific examples once additional provider profiles exist?
- Should benchmark evidence eventually be published as a checked-in report or attached release artifact instead of remaining primarily runnable from the benchmark project?
- Once package publication exists, should the README quickstart be split into separate source-consumption and NuGet-consumption guides?

Risks
- If docs or benchmarks do not explicitly say that v1 is SQLite-first, readers may infer unsupported provider breadth from the general EF wording in the README.
- Benchmark results will be misleading if the normal EF baseline and DVault baseline do not use the same scenario shape, data volume, and lineage/timestamp assumptions.
- If comparison evidence is not preserved in a reproducible form, future reviewers may have difficulty validating performance claims across environments.

Split recommendations
- No additional split is recommended at the epic level in this PO pass; the epic is already decomposed through four persisted child tickets and should remain the umbrella for cross-cutting documentation/example/benchmark alignment.
- Treat any future provider-specific documentation or advanced Data Vault capability expansion as separate follow-up epics rather than enlarging this MVP-scoped epic.

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