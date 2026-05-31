[gicket-bot] PO refinement contract

Summary
- Repo evidence already contains the v0.23.0 performance guide, the activity tracing contract, and the supporting test and benchmark surfaces; this ticket is now a bounded documentation-consolidation pass to align README.md, docs/production-adoption-checklist.md, and a missing docs/releases/v0.23.0.md release record.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already shows the v0.23.0 performance guide, the activity tracing contract, and the tracing and benchmark evidence surfaces are landed; this ticket should be treated as docs consolidation, not as a wait-state for fresh product-code work.
- Treat docs/architecture/dvault-v1-activity-tracing-contract.md as the authoritative source for exact ActivitySource, span-name, event-name, and redaction wording; public docs should summarize and cross-link it, not redefine it.
- Treat docs/performance-profiles.md as the already-landed detailed v0.23.0 performance guide; this ticket should align public docs to that guide and the checked-in benchmark artifacts rather than invent new performance prose or tables.
- Treat v0.23.0 as the new coordinated public baseline and keep v0.22.0, v0.21.0, and v0.20.0 as historical references for earlier feature introductions.
- Benchmark-documentation changes are conditional: benchmark docs already link to docs/performance-profiles.md, so only stale links or wording need updates.
- Use the README local validation command set as the repository baseline; no dedicated markdown or link checker is visible in the repo, so touched link and anchor verification is a manual docs-review step alongside the existing gates.

Scope In
- Update README.md current-baseline sections and version-aligned public examples so v0.23.0 becomes the current coordinated documentation baseline.
- Update docs/production-adoption-checklist.md so the current release baseline, activity tracing posture, redaction boundary, telemetry relationship, and performance-profile guidance match the v0.23.0 story.
- Create docs/releases/v0.23.0.md as the coordinated seven-package release record with package scope, compatibility posture, validation evidence, benchmark evidence references, and explicit non-goals.
- Cross-link public docs to the activity tracing contract, the performance profile guide, the benchmark artifact contract, and the root benchmark summary triplet.
- Record the repository validation commands and the focused tracing and performance evidence sources already present in the completed child-ticket outputs.

Scope Out
- No product-code, benchmark harness, or public API changes.
- No new tracing contract, span vocabulary, redaction decision, or telemetry semantics.
- No new benchmark runs, artifact rewrites, or changed timing claims unless a completed child ticket already landed new evidence.
- No claim that packages were published, optional external providers were measured, or non-SQLite provider wins were validated when the checked-in artifacts show skipped rows.
- No exporter, collector, dashboard, alerting, scheduler, container, database, deployment, or release-automation setup guidance.
- No reopening of carried-forward compatibility boundaries for explicit save and read services, PIT and bridge maintenance, provider registration, package-family membership, or manual publication separation.

Open questions
- none

Follow-up questions
- Should a later docs-quality ticket add a dedicated markdown or link validation step, since the current repository-visible automation exposes formatting, build, and test gates but no standalone link checker?

Risks
- README.md and docs/production-adoption-checklist.md still reference v0.22.0 as the current baseline, so partial edits could leave public-versioning guidance inconsistent if the update is not completed across all touched docs.
- Activity tracing docs must reuse the closed contract vocabulary exactly where names matter; paraphrased span names, redaction lists, or telemetry relationships would create public inconsistencies with the landed contract.
- Performance claims are evidence-bound to the root benchmark artifacts and current run context; optional external-provider rows are skipped, so broad provider-specific performance wording would overstate the checked-in evidence.
- Because no dedicated link checker is visible, broken anchors or stale cross-links remain a manual-review risk unless each touched link is checked during the docs pass.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment