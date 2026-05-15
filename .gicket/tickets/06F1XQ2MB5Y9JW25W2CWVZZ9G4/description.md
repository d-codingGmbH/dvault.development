<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around completing the adopter-facing documentation path that connects the current v0.8 lifecycle guardrails and v0.9 adoption examples/checklist. Existing repository context already establishes the six-package NuGet family, provider-neutral plus provider-specific setup, model declaration paths, migration and drift boundaries, and the production checklist baseline, so no PO-blocking questions remain.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use the current coordinated package family as the documentation baseline: DCoding.Data.DVault plus DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer.
- Do not present src/DCoding.Data as an installable consumer package; it is documented as a non-packable source-root build anchor.
- Keep README and checklist guidance NuGet-based for released consumer setup, with project/source references reserved only for repository development or unpublished local work.
- The supported migration guardrail flow is consumer-owned and preflight-driven; DVault does not ship a dotnet ef shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs.
- Live-schema drift evidence is SQLite-first in the current v1 boundary; other providers should be described as unsupported or external opt-in evidence unless the repository adds first-class readers.
- Analyzer and Testcontainers guidance should appear only if corresponding packages, examples, or test assets are actually present in the repository.
- Examples should stay intentionally small and either build as-is or state exact prerequisites and commands.

### Scope In
- Refresh README adoption guidance so Code-First, metadata-first, and model-first paths are presented as compatible choices for different ownership needs.
- Refresh examples documentation for runnable SQLite and PostgreSQL quickstarts, package installation, provider selection, service registration, migrations, diagnostics, drift checks, read helpers, save boundaries, and interceptors where currently supported.
- Maintain or update the production checklist so it distinguishes required production readiness steps from optional evidence or advanced features.
- Tie v0.8 lifecycle guardrails to the v0.9 adoption story, including design-time diagnostics, migration guardrail validation, model-first drift reports, and documented live-schema drift limits.
- Keep package names, version examples/placeholders, provider extension names, and documented commands consistent across README, examples, and the checklist.
- Keep known limitations visible, especially around provider live-schema support, EF CLI ownership, and non-promised automation.

### Scope Out
- No marketing landing page or product positioning rewrite.
- No new product behavior, provider implementation, analyzer package, Testcontainers package, or release automation work.
- No undocumented feature promises or forward-looking API guarantees.
- No replacement for API reference documentation.
- No new subtickets created by this refinement; larger future documentation expansions should be documented as recommendations only.

## Acceptance Criteria
- README, examples documentation, and the production checklist describe the same package family and installation model using current package IDs and a consistent version value or placeholder.
- The adoption path clearly covers Code-First, metadata-first, and model-first usage and points readers to the appropriate detailed governance or design-time workflow documents.
- Migration guardrails and drift guidance reflect the documented v0.8 boundary, including consumer-owned preflight commands and SQLite-first live-schema drift support.
- Checklist items distinguish required production readiness steps from optional or advanced steps such as PIT, bridge, multi-active satellite, model-first evidence, and live-schema drift evidence.
- Every referenced example either has a runnable command path documented or explicitly states its prerequisites and limitations.
- Analyzer and Testcontainers references are omitted unless backed by available repository packages, examples, or tests.
- Known limitations remain visible and are not softened into implied commitments.

## Definition of Done
- Documentation updates are applied to the adopter-facing README/examples/checklist surfaces without changing product code.
- All package IDs, provider names, service-registration snippets, and command examples are checked for consistency against the current repository baseline.
- Runnable examples referenced by the documentation are verified with their documented build or run commands where feasible, or clearly marked with prerequisites if not executed.
- The production checklist remains a practical production-readiness checklist and links to source documents rather than duplicating full API reference material.
- No new undocumented behavior promises are introduced.
- Relevant documentation validation, formatting, or build checks available in the repository are run or any skipped checks are explicitly noted by the implementer.

## Implementation Notes
- Use README.md, examples/README.md, docs/production-adoption-checklist.md, docs/model-first-governance.md, docs/architecture/dvault-dotnet-ef-design-time-workflow.md, docs/releases/v0.8.0.md, and docs/manual-nuget-publication.md as the main source documents to align terminology and constraints.
- Prefer one adoption narrative: install packages, choose provider, register services, choose one authoritative model declaration path, run diagnostics, manage migrations with guardrails, check drift where supported, then operate writes/reads with documented boundaries.
- Use the six coordinated package IDs already documented in README and publication guidance; keep all examples version-aligned.
- Keep examples compact and consumer-oriented; avoid turning README into API reference material.
- When documenting optional features such as PITs, bridges, multi-active satellites, model-first artifacts, interceptors, or typed/as-of/latest reads, label them according to current support and avoid implying they are required for ordinary hub/link/satellite adoption.
- If repository evidence does not contain analyzer or Testcontainers packages/examples, do not add those references beyond noting that unavailable packages are intentionally not included.

## Open Questions
- none

## Follow-Up Questions
- Should future documentation add a separate deep-dive guide for provider-specific production operations after the general checklist is complete?
- Should a later story add or expand Testcontainers-backed integration examples if the project decides to publish and support that path?
- Should a later release introduce dedicated analyzer package documentation if analyzer packages become part of the coordinated package family?

## Risks
- Documentation can become misleading if it names packages or helper APIs not present in the current repository baseline.
- Provider-specific live drift guidance may overpromise support unless SQLite-first limits are kept explicit.
- A broad adoption document could grow into API reference duplication unless examples stay small and link to detailed source documents.

## Split Recommendations
- No split is required for this story. If implementation grows too large, keep this ticket focused on README/examples/checklist alignment and move future provider-specific deep dives, Testcontainers examples, or analyzer documentation into separate follow-up work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Tie v0.8 and v0.9 features together into a practical adoption path for EF Core teams.

## Scope In

- Update README/examples for Code-First, Model-First, migration guardrails, drift, read helpers, interceptors, and package installation.
- Add a production checklist for diagnostics, migrations, drift checks, provider selection, tests, and publishing/signing notes.
- Keep examples small and runnable.
- Reference analyzer/Testcontainers packages only where available.

## Scope Out

- No marketing landing page.
- No undocumented feature promises.
- No replacement for API reference docs.

## Acceptance Criteria

- Docs use current package names and version placeholders consistently.
- Checklist distinguishes required from optional steps.
- Examples build or have explicit prerequisites.

## Implementation Notes

- Keep known limitations visible.

## Open Questions

- none