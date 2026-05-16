[gicket-bot] PO refinement contract

Summary
- Refined the ticket against local repository and ticket-store evidence: the analyzer package already has a packaged README for installation and suppression guidance, so this task is bounded and ready for PO-critic without new splits or relation writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Local ticket-store evidence shows this task is a `parentOf` child of story `06F2PGHQ2GATEM13M5QK1MSX1G` and has one incoming `blocks` relation from epic `06F2PGFT8Z406HFBJGQSY7YRJ0`; that epic is already `done`, so the relation is treated as satisfied historical context rather than a live blocker.
- The current branch already exposes the primary consumer guidance in `src/DCoding.Data.DVault.Analyzers/README.md`: install `DCoding.Data.DVault.Analyzers` in the project that owns DVault Code-First declarations and normally keep it local with `PrivateAssets=all`.
- Repository evidence ratifies the configuration and suppression baseline as standard Roslyn behavior, not a DVault-specific API: use local `#pragma warning`, path or project severity policy in `.editorconfig`, or MSBuild `NoWarn`.
- Current source and tests bound the visible analyzer surface to warning-enabled diagnostics `DMV1901` and `DMV1902`; this ticket should document only implemented ids and behavior and should not speculate about future diagnostics or code fixes.
- The ticket sits in active release `v0.12.0 - Analyzer and Generator Ergonomics`, but repository release-note files currently stop at `docs/releases/v0.11.0.md`; broader `v0.12.0` release-note closure belongs to `06F2PGJYY6S97B4Z8044D34K5C` rather than widening this task.
- Current ticket comments in the local store are bot claim and lease comments only; no human clarification, child-ticket creation, relation change, attachment, or planning-document write was materialized in this refinement pass.

Scope In
- Consumer-facing documentation for installing and configuring `DCoding.Data.DVault.Analyzers` in projects that own DVault Code-First declarations.
- Suppression and severity guidance for the implemented analyzer diagnostics, with concrete examples for local pragma, `.editorconfig`, and `NoWarn` usage.
- Alignment between documentation, analyzer descriptor metadata, package README packaging, and analyzer tests so published guidance matches shipped behavior.
- Only the minimal supporting mentions in broader docs that are needed to keep installation guidance consistent with the packaged analyzer README.

Scope Out
- Adding new analyzer diagnostics, changing descriptor semantics, or implementing code fixes; those stay in sibling or follow-on analyzer tickets such as `06F2PGHWEWYJZSRQ9QPT4NJ0QM` and `06F2PGJBRXFCP038CN6XVAYSZM`.
- Repository-wide `v0.12.0` release-note closure or a broad documentation sweep; that belongs to `06F2PGJYY6S97B4Z8044D34K5C`.
- Any custom DVault suppression API, runtime toggle, MSBuild target, or non-Roslyn configuration mechanism.
- Source-generator contracts, generator documentation, or package-shape changes outside the existing analyzer README and package-metadata path.

Open questions
- none

Follow-up questions
- When `06F2PGHWEWYJZSRQ9QPT4NJ0QM` lands additional diagnostics, should the broader `v0.12.0` documentation ticket `06F2PGJYY6S97B4Z8044D34K5C` surface a short diagnostic catalog summary outside the package README, or keep detailed suppression guidance package-local only?
- Should the follow-on code-fix story `06F2PGJBRXFCP038CN6XVAYSZM` add documentation that prefers automatic fixes over manual suppression where a mechanical correction exists?

Risks
- If documentation names diagnostics or suppression paths that are not actually present in `CodeFirstDiagnosticCatalog`, consumer guidance will diverge from shipped analyzer behavior.
- If this ticket expands into a repo-wide `v0.12.0` documentation or version sweep, it will overlap the separate release-closure ticket and increase merge churn without clarifying analyzer usage.
- If suppression guidance is framed too broadly, consumers may silence analyzer coverage that should remain enabled by default instead of treating suppression as an intentional exception path.

Split recommendations
- No additional split is recommended; the current ticket is already a bounded documentation slice under story `06F2PGHQ2GATEM13M5QK1MSX1G`.
- Keep implementation of new analyzer rules in `06F2PGHWEWYJZSRQ9QPT4NJ0QM`, broader `v0.12.0` documentation and release-note wrap-up in `06F2PGJYY6S97B4Z8044D34K5C`, and later code-fix ergonomics in `06F2PGJBRXFCP038CN6XVAYSZM`.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment