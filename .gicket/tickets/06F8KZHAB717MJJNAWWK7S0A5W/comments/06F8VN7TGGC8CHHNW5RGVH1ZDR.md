[gicket-bot] PO refinement contract

Summary
- Refined this into a bounded v0.27 documentation-alignment task: roll the coordinated docs baseline forward, keep DMV1912-DMV1914 lifecycle guidance consistent across public docs, and preserve the analyzer-only no-runtime-change posture.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket is documentation alignment for already-landed analyzer and test behavior; it does not reopen analyzer implementation or runtime design.
- Treat `docs/architecture/dvault-ef-compiled-compatibility.md` as the authoritative lifecycle and compiled-compatibility source; the root `dvault-ef-compiled-compatibility.md` remains a validation-facing entrypoint, not a second contract document.
- The v0.27 analyzer slice is the existing EfCore misuse surface for `DMV1910` through `DMV1914`, with `DMV1912` through `DMV1914` limited to high-confidence, source-visible lifecycle misuse around caller-owned cache keys, `UseModel(...)`, and direct `AddDbContextPool<TContext>(...)` usage.
- Registry-backed `UseDataVaultMetadata(...)` paths, the documented fixed-shape `UseModel(runtimeModel)` lane, stable direct EF compiled queries over generated shared-type tables, and fixed-shape options-only pooling remain documented non-diagnostic baselines.
- Related contract, implementation, and fixture tickets `06F8KZGC4NY41PRYB2RP00ZA1M`, `06F8KZGNRG5FY4WWCY3FAX2NS4`, and `06F8KZGZND5ZCH147PVBRWXYN4` are already `done` and should be treated as completed evidence, not blockers for this ticket.

Scope In
- Roll the current coordinated documentation baseline from v0.26.0 to v0.27.0 in the root README, production checklist, and release-note surface.
- Align `src/DCoding.Data.DVault.Analyzers/README.md`, README install snippets, and other versioned examples with the v0.27.0 documentation baseline while preserving the existing no-publication disclaimer.
- Document the lifecycle analyzer boundary for `DMV1912`, `DMV1913`, and `DMV1914` alongside the carried-forward analyzer catalog and limitations.
- Keep EF compiled-model, compiled-query, and `DbContext` pooling guidance consistent across README, checklist, compiled-compatibility note, and release notes.
- Add the v0.27 validation-evidence and non-goal story for EF lifecycle analyzer guardrails, including the explicit no-runtime-change posture.

Scope Out
- Analyzer implementation changes, descriptor changes, severity changes, or new analyzer test coverage.
- New runtime guards, interceptors, compiled-model generators, provider-specific lifecycle behavior, or whole-application inference.
- Broadening diagnostics beyond the documented direct `ApplyDataVaultMetadata(...)`, `UseModel(...)`, and direct `AddDbContextPool<TContext>(...)` evidence lanes.
- NuGet publication records, package hashes, release approval records, or release-automation work.
- Benchmark reruns or new performance/provider claims unrelated to the lifecycle analyzer documentation rollout.

Open questions
- none

Follow-up questions
- none

Risks
- The repository is already partially updated for `DMV1912` through `DMV1914`, so the main delivery risk is leaving a mixed baseline where README or checklist sections still present v0.26.0 or the older `DMV1910`/`DMV1911`-only story as current.
- A new v0.27 release note can easily over-claim runtime or provider behavior unless it mirrors the accepted contract and existing test evidence exactly.
- Versioned install snippets can imply package availability if the existing no-publication disclaimer is weakened or removed during the v0.27 roll-forward.
- The root compiled-compatibility entrypoint can drift from the architecture note if both files are expanded independently instead of keeping one authoritative source.

Split recommendations
- No further split is recommended; contract, implementation, fixtures, and documentation are already separated across sibling tickets, and this ticket is now a bounded documentation-alignment task.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment