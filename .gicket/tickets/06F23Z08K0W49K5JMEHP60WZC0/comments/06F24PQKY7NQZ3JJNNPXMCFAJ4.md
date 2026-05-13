[gicket-bot] PO refinement contract

Summary
- Refined the v0.8.0 release-summary ticket against the existing release-note pattern, parent epic and release context, and completed design-time and drift stories; no split or planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository still has no `docs/releases/v0.8.0.md`; this ticket remains the bounded doc-only release-note deliverable for active release `v0.8.0 - EF Core Lifecycle Guardrails`.
- The referenced `docs/releases/v0.x.0.md` pattern is conceptual only because that file is missing; the effective template is `docs/releases/v0.7.0.md` plus the earlier `v0.5.0` and `v0.6.0` release-note structure.
- Release wording must follow completed story `06F1XPVPKVGYKCV04PY98TSS78`: the supported v1 `dotnet ef` path is consumer-owned, single-project, and preflight-driven, not a DVault-owned `IDesignTimeServices` or CLI-shim feature.
- Drift wording must follow completed story `06F1XPWB8DZR4J8EZ00V8DT25G`: metadata or ModelSnapshot comparison is the non-live evidence lane, while live-schema comparison is optional and SQLite-first with explicit unsupported or unavailable outcomes elsewhere.
- Repository evidence already exists for the release-summary claims in `DataVaultMigrationOperationDiagnosticsTests`, `DataVaultModelFirstDesignTimeWorkflowTests`, and `SqliteLiveSchemaDriftTests`.
- No child tickets, relation updates, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Add `docs/releases/v0.8.0.md` for release `v0.8.0 - EF Core Lifecycle Guardrails`.
- Mirror the `docs/releases/v0.7.0.md` section shape closely enough to cover package scope, highlights, compatibility notes, known limitations, and validation evidence.
- Summarize the stable DVault lifecycle-guardrail surface: DMV diagnostics, DVM2001-DVM2006 migration guardrails, and the consumer-owned design-time preflight workflow.
- Summarize the current drift evidence boundary: provider-neutral metadata or ModelSnapshot comparison plus optional SQLite-first live-schema comparison.
- Keep the coordinated six-package-family and manual-publication caveats aligned with prior release-note conventions.

Scope Out
- No product or runtime code changes.
- No DVault-owned `IDesignTimeServices`, no custom `dotnet ef` shim, no CLI interception, and no repo-owned `Microsoft.EntityFrameworkCore.Design` claim.
- No provider-specific online migration runner, automatic migration execution, or repair workflow claims.
- No broader live-schema provider support promise than the current SQLite-first supported lane with explicit unsupported or unavailable outcomes elsewhere.
- No new ticket split, relation cleanup, attachment, or planning document is required for this refinement pass.

Open questions
- none

Follow-up questions
- Should a later docs or examples ticket add one operator-facing end-to-end sample that chains `dotnet ef migrations add`, consumer preflight, and `dotnet ef database update` for the v0.8.0 workflow?
- After v0.8.0, which provider should be the next live-schema implementation after the SQLite-first baseline?
- Should a later guide consolidate artifact review, metadata or ModelSnapshot comparison, and live-schema comparison into one operator workflow document?

Risks
- The parent epic still uses older `design-time services` wording; if the release note repeats that shorthand without the refined boundary, readers may wrongly infer a DVault-owned `IDesignTimeServices` feature.
- If the release note overstates drift support, readers may assume broader multi-provider live-schema coverage or a separate public CLI flow that the repository does not currently prove.
- If migration preflight is described as EF CLI interception or automatic migration execution, the note will over-promise behavior outside the documented workflow.

Split recommendations
- No further split recommended; this ticket is already bounded to one missing release-note artifact, and the underlying design-time and drift implementation work is already covered by completed related stories.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment