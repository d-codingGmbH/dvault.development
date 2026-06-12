[gicket-bot] PO refinement contract

Summary
- Refined the ticket against the current hash-key storage and benchmark baselines: the existing repository-local benchmark harness and artifact contract should be extended to compare four bounded hash-key variants, with no child tickets, relation edits, description updates, attachments, or planning documents materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The benchmark baseline already exists in `benchmarks/DCoding.Data.DVault.Benchmarks`, the shared artifact triplet contract already exists in `docs/plans/performance-evidence-benchmark-artifact-contract.md`, and this ticket should extend those surfaces rather than introduce a second benchmark harness or artifact schema.
- The storage-profile baseline is already finite in repository code and docs: `DataVaultHashKeyStorageProfile` exposes only `HexString` and `Binary`, and `BuiltInStableHashService` exposes `sha256-v1`, `sha1-v1`, `sha256-128-v1`, and `sha256-160-v1`.
- For this ticket, ratify `sha256-128-v1` as the single shorter opt-in comparison baseline instead of reopening the full non-default algorithm matrix; current provider-matrix and live-schema fixture tests already use `sha256-128-v1`, and it gives the clearest bounded footprint delta.
- Current benchmark code is still hard-wired to SHA-256-shaped hash assertions in the DVault save benchmarks and has no storage-profile or stable-hash option surface in `BenchmarkOptions`/`BenchmarkRunner`, so this ticket includes benchmark-harness generalization work, not only running an existing command.
- Live relation context was verified locally: parent epic `06F9GF5A8V7G3PAKGRXNYEBW5C` still owns this ticket, this ticket still blocks documentation task `06F9GF6CX7WE2JGBDW3QH1GX98`, and the incoming blocks relation from done task `06F9GF60BKEW0CC9FCZRPVX0SR` is historical and non-blocking because the related ticket is already `done`.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement run.

Scope In
- Extend the existing benchmark harness, options, and artifact output so it can compare `sha256-v1` hex, `sha256-v1` binary, `sha256-128-v1` hex, and `sha256-128-v1` binary within the current repository-local benchmark project.
- Measure repository-local insert/save cost on the existing DVault benchmark scenarios for the four bounded variants instead of inventing a separate workload family.
- Measure repository-local latest-satellite repeated-write lookup behavior and index-shape sensitivity by reusing the existing `--latest-indexes` path or an equivalent bounded extension of that same benchmark surface.
- Measure repository-local read and join-style workloads on the existing latest-satellite read, PIT as-of read, and bridge traversal read scenarios for the same four bounded variants where execution is feasible.
- Produce timing, allocation, and supporting footprint evidence through the existing benchmark artifact triplet, with same-label sidecar files when exact SQL or storage-footprint evidence needs supplemental capture.

Scope Out
- A full comparison matrix across every non-default built-in stable-hash algorithm.
- Production telemetry, dashboards, hosted observability, or non-repository-local evidence collection.
- New public API shapes, caller-facing hash-key type changes, or changes to the canonical lowercase-hex boundary.
- Automatic rehash, migration, dual-write, backfill, or repair tooling.
- Mandatory completed external-provider execution across PostgreSQL, SQL Server, MySQL, and Oracle when those environments are not locally configured.
- A shared benchmark artifact contract rewrite unless a later separate contract ticket explicitly approves one.

Open questions
- none

Follow-up questions
- After the SQLite-local evidence lands, should a follow-up ticket execute the same four-variant benchmark matrix on one or more optional external providers to validate provider-specific storage savings beyond the required local baseline?
- Should later documentation expand beyond the bounded `sha256-128-v1` comparison and add advisory notes for `sha1-v1` or `sha256-160-v1`, or should those remain out of scope unless a later evidence ticket measures them directly?

Risks
- The current benchmark suite assumes SHA-256 digest shape in multiple verification points; if that generalization is incomplete, shorter-algorithm rows may fail falsely or silently compare the wrong invariant.
- Storage-footprint claims can drift into one-off local notes if supplemental measurements are not kept under the same benchmark label and artifact-bundle discipline as the timing rows.
- Optional provider execution remains environment-gated; expecting completed cross-provider rows in this ticket without local provider infrastructure would expand the scope beyond the current bounded baseline.
- Because documentation ticket `06F9GF6CX7WE2JGBDW3QH1GX98` depends on this ticket, under-scoping the measured variant matrix would push unmeasured generalization pressure downstream into documentation work.

Split recommendations
- No split is recommended while the work stays inside the existing benchmark harness, artifact contract, and bounded four-variant comparison baseline.
- If stakeholders later require completed optional-provider matrices or a broader multi-algorithm comparison beyond `sha256-128-v1`, create follow-up tickets instead of broadening this task in place.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment