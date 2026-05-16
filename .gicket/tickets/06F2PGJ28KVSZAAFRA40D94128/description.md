<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket against local repository and ticket-store evidence: the analyzer package already has a packaged README for installation and suppression guidance, so this task is bounded and ready for PO-critic without new splits or relation writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Local ticket-store evidence shows this task is a `parentOf` child of story `06F2PGHQ2GATEM13M5QK1MSX1G` and has one incoming `blocks` relation from epic `06F2PGFT8Z406HFBJGQSY7YRJ0`; that epic is already `done`, so the relation is treated as satisfied historical context rather than a live blocker.
- The current branch already exposes the primary consumer guidance in `src/DCoding.Data.DVault.Analyzers/README.md`: install `DCoding.Data.DVault.Analyzers` in the project that owns DVault Code-First declarations and normally keep it local with `PrivateAssets=all`.
- Repository evidence ratifies the configuration and suppression baseline as standard Roslyn behavior, not a DVault-specific API: use local `#pragma warning`, path or project severity policy in `.editorconfig`, or MSBuild `NoWarn`.
- Current source and tests bound the visible analyzer surface to warning-enabled diagnostics `DMV1901` and `DMV1902`; this ticket should document only implemented ids and behavior and should not speculate about future diagnostics or code fixes.
- The ticket sits in active release `v0.12.0 - Analyzer and Generator Ergonomics`, but repository release-note files currently stop at `docs/releases/v0.11.0.md`; broader `v0.12.0` release-note closure belongs to `06F2PGJYY6S97B4Z8044D34K5C` rather than widening this task.
- Current ticket comments in the local store are bot claim and lease comments only; no human clarification, child-ticket creation, relation change, attachment, or planning-document write was materialized in this refinement pass.

### Scope In
- Consumer-facing documentation for installing and configuring `DCoding.Data.DVault.Analyzers` in projects that own DVault Code-First declarations.
- Suppression and severity guidance for the implemented analyzer diagnostics, with concrete examples for local pragma, `.editorconfig`, and `NoWarn` usage.
- Alignment between documentation, analyzer descriptor metadata, package README packaging, and analyzer tests so published guidance matches shipped behavior.
- Only the minimal supporting mentions in broader docs that are needed to keep installation guidance consistent with the packaged analyzer README.

### Scope Out
- Adding new analyzer diagnostics, changing descriptor semantics, or implementing code fixes; those stay in sibling or follow-on analyzer tickets such as `06F2PGHWEWYJZSRQ9QPT4NJ0QM` and `06F2PGJBRXFCP038CN6XVAYSZM`.
- Repository-wide `v0.12.0` release-note closure or a broad documentation sweep; that belongs to `06F2PGJYY6S97B4Z8044D34K5C`.
- Any custom DVault suppression API, runtime toggle, MSBuild target, or non-Roslyn configuration mechanism.
- Source-generator contracts, generator documentation, or package-shape changes outside the existing analyzer README and package-metadata path.

## Acceptance Criteria
- The consumer-facing analyzer docs state where to install `DCoding.Data.DVault.Analyzers`, that it is optional developer tooling, and that consuming projects should normally keep it local with `PrivateAssets=all`.
- The documentation describes the implemented analyzer rule slice by real diagnostic id and intent, using the branch source as authority and making no claims beyond the implemented diagnostics.
- The documentation explains supported suppression and configuration paths with concrete examples for `#pragma warning`, `.editorconfig` severity configuration, and MSBuild `NoWarn`.
- The authoritative suppression and configuration guidance ships with the analyzer package README and remains consistent with `CodeFirstDiagnosticCatalog`, `CodeFirstAnalyzerDiagnosticMetadata`, and `DataVaultCodeFirstAnalyzerTests`.
- If broader docs are touched for consistency, they stay concise and point back to the packaged analyzer guidance instead of creating a second conflicting suppression contract.

## Definition of Done
- `src/DCoding.Data.DVault.Analyzers/README.md` is the ratified primary artifact for installation, scope, and suppression guidance, and its content matches the diagnostics actually implemented on the branch.
- Any touched versioned snippets or package references stay aligned with the coordinated release version in effect at merge time without turning this ticket into a repo-wide version sweep.
- Analyzer package metadata continues to publish the packaged README, and no new package, attachment, child ticket, or planning document is required to make the guidance consumable.
- No blocking PO questions remain, and the ticket can move to PO-critic with the live relation state left unchanged.

## Implementation Notes
- Use `src/DCoding.Data.DVault.Analyzers/README.md` as the primary documentation surface; it is already wired as the analyzer package readme by `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj`.
- Use `src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs`, `src/DCoding.Data.DVault.Analyzers/CodeFirstAnalyzerDiagnosticMetadata.cs`, and `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs` as the source of truth for diagnostic ids, titles, default warning severity, and supported rule scope.
- Keep the detailed suppression contract package-local. Root README, examples, or adoption-checklist mentions should stay lightweight and avoid inventing a second authoritative suppression guide unless a short consistency pointer is needed.
- Document only standard Roslyn behavior: local pragma suppression, `.editorconfig` severity policy, and `NoWarn`; do not introduce DVault-specific attributes, JSON files, or runtime switches.
- No child tickets, relations, attachments, or planning documents were created during refinement.

## Open Questions
- none

## Follow-Up Questions
- When `06F2PGHWEWYJZSRQ9QPT4NJ0QM` lands additional diagnostics, should the broader `v0.12.0` documentation ticket `06F2PGJYY6S97B4Z8044D34K5C` surface a short diagnostic catalog summary outside the package README, or keep detailed suppression guidance package-local only?
- Should the follow-on code-fix story `06F2PGJBRXFCP038CN6XVAYSZM` add documentation that prefers automatic fixes over manual suppression where a mechanical correction exists?

## Risks
- If documentation names diagnostics or suppression paths that are not actually present in `CodeFirstDiagnosticCatalog`, consumer guidance will diverge from shipped analyzer behavior.
- If this ticket expands into a repo-wide `v0.12.0` documentation or version sweep, it will overlap the separate release-closure ticket and increase merge churn without clarifying analyzer usage.
- If suppression guidance is framed too broadly, consumers may silence analyzer coverage that should remain enabled by default instead of treating suppression as an intentional exception path.

## Split Recommendations
- No additional split is recommended; the current ticket is already a bounded documentation slice under story `06F2PGHQ2GATEM13M5QK1MSX1G`.
- Keep implementation of new analyzer rules in `06F2PGHWEWYJZSRQ9QPT4NJ0QM`, broader `v0.12.0` documentation and release-note wrap-up in `06F2PGJYY6S97B4Z8044D34K5C`, and later code-fix ergonomics in `06F2PGJBRXFCP038CN6XVAYSZM`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Make analyzer adoption predictable for consumers.

## Scope
- Refine and complete the work for "Document analyzer configuration and suppressions" within the boundaries of its parent story, epic, and release.
- Keep the implementation focused on the affected DVault feature area; avoid unrelated refactorings or package shape changes unless they are required by the ticket.
- Update tests, examples, diagnostics, provider behavior, and documentation only where they are relevant to this ticket's observable behavior.

## Acceptance Criteria
- The completed ticket includes clear evidence of the implemented behavior, verification steps, and any intentionally deferred work.
- Relevant unit, integration, provider, analyzer, or documentation checks are added or updated, or the ticket documents why a check is not applicable.
- Public behavior, command output, generated SQL, package contents, examples, README content, and release notes are updated when this ticket changes them.
- The result remains compatible with the release ordering and relations; dependent tickets can start without reworking this ticket's scope.

## Release Notes
- If this ticket changes public behavior, package shape, examples, diagnostics, generated SQL, or provider behavior, update README and the release note document for this release before integration.