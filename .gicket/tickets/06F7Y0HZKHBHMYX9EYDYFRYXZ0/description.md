<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence is sufficient: current docs still frame v0.24/satellite-only guidance, while the implemented surface now includes request-bound ReadShape diagnostics plus support-bundle-driven PIT and bridge helpers that delegate through IDataVaultReadService.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- For this ticket, 'read plan' means request-bound IDataVaultReadDiagnosticsService.Analyze(...) output: DataVaultDiagnosticsResult.ReadStrategy plus additive ReadShape, and the same redacted readShape JSON in dvault.support-bundle.v1.
- Treat the current implemented typed-helper boundary as satellite latest/current/as-of helpers, PIT Read...AsOfAsync helpers, and bridge Read...FromAsync/ToAsync plus hierarchy Read...AncestorAsync/DescendantAsync helpers with required maximumDepth.
- Generated helpers are ergonomics over IDataVaultReadService, not a new query API; dynamic IDataVaultReadService requests remain the default for runtime-built shapes and caller-selected projectors, while consumer-owned EF compiled queries remain the stable direct-query option.
- Support-bundle-driven generation still requires exactly one authoritative dvault.support-bundle.v1 additional file and optional DVaultTypedReadModelMetadataSourceFingerprint; raw dvault.model.v1 files and source-visible declarations are not direct generator inputs.

### Scope In
- Refresh the root README, analyzer README, relevant architecture docs, production checklist, and add v0.25.0 release notes so they consistently describe the current read-plan/ReadShape and typed-helper baseline.
- Replace stale satellite-only wording with the implemented support-bundle-driven satellite, PIT, and bounded bridge helper surface.
- Add documentation examples for redacted read-plan/ReadShape output and for generated PIT and bridge helper calls over IDataVaultReadService.
- Explain how representative request-bound diagnostics reach support bundles through DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics.
- Compare generated helpers with dynamic runtime read-service requests and consumer-owned EF compiled queries using the repository's bounded guidance.

### Scope Out
- No runtime, analyzer, source-generator, or test code changes.
- No new helper shapes beyond the implemented bounded satellite/PIT/bridge support.
- No provider-specific SQL, raw query-plan capture, automatic index advice, or LINQ-provider claims.
- No benchmark reruns, package publication, release automation, or support-bundle transport automation.
- No consumer sample-app expansion unless a separate ticket asks for runnable end-to-end examples.

## Acceptance Criteria
- README.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/production-adoption-checklist.md, and the relevant architecture docs no longer describe typed read-model generation as satellite-only and instead match the implemented satellite, PIT, and bounded bridge helper surface.
- The docs describe supported helper shapes exactly: satellite latest/current/as-of; PIT as-of for hub-parent ordinary PITs, hub-parent multi-active PITs with one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites; bridge helpers for many-to-many From/To and hierarchy Ancestor/Descendant with required maximumDepth.
- The docs describe unsupported residual shapes and DMV1963/DMV1964 behavior without implying custom LINQ-provider behavior, provider-specific SQL generation, automatic PIT/bridge maintenance, or unbounded traversal support.
- At least one read-plan example shows request-bound ReadShape output and/or support-bundle JSON using translated table/column facts, read-strategy status, and fallback data while keeping raw request values, timestamps, SQL text, provider plans, and credentials out of the example.
- At least one generated PIT helper example and one generated bridge helper example match the implemented method shapes over IDataVaultReadService.
- The docs explicitly compare when to use generated helpers, dynamic IDataVaultReadService requests, and consumer-owned EF compiled queries.
- A new docs/releases/v0.25.0.md release note becomes the current coordinated documentation baseline and includes compatibility posture, the typed-read generator diagnostic range DMV1960-DMV1969, validation evidence/commands, and explicit non-goals.

## Definition of Done
- The current-baseline docs point to v0.25.0 as the active release note and demote older release notes to historical context where referenced.
- Contradictory statements that PIT/bridge helpers are not emitted or that bridge metadata is always diagnostic-only are removed from current-baseline docs or clearly left only in historical release records.
- The read-plan/ReadShape terminology aligns with DataVaultDiagnosticsResult, IDataVaultReadDiagnosticsService, and dvault.support-bundle.v1 naming already used in the repository.
- The typed-helper docs preserve the bounded API surface and method names already proven by generator tests, including maximumDepth for hierarchy bridge helpers.
- Release-note and checklist evidence sections cite the existing diagnostics and generator test coverage that proves ReadShape export/redaction and generated PIT/bridge helper behavior.

## Implementation Notes
- Use current code and tests as the authority over stale prose: DataVaultTypedReadModelSourceGeneratorTests now prove generated PIT and bridge helpers plus delegation through runtime requests, and DataVaultDiagnosticsTests prove request-bound ReadShape export/redaction.
- Promote docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md from additive/future wording to the current implemented v1 helper contract instead of reopening naming or helper-shape decisions.
- Update docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md so it explains that ReadShape is the bounded diagnostics/support-bundle evidence consumed by helper generation, while preserving the value-free diagnostics and redaction boundary.
- Keep the design-time story explicit: support bundles only include request-bound ReadShape when application code supplies representative diagnostics through DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics; the generic command runner does not invent requests.
- Use the repository's existing bounded comparison: generated helpers for reviewed stable support-bundle-backed shapes, dynamic IDataVaultReadService requests for runtime-built shapes or caller-selected projectors, and compiled EF queries for stable direct shared-type-table expressions.
- Document bridge helper endpoint vocabularies exactly as implemented: many-to-many From/To and hierarchy Ancestor/Descendant with explicit maximumDepth.
- Keep examples redacted and deterministic: show produced table names, column roles, enum/status values, and mapped/produced names, but not raw hash keys, as-of values, SQL text, provider error text, or physical-plan claims.
- Treat the absence of docs/releases/v0.25.0.md as part of this ticket's deliverable, not as a blocker.

## Open Questions
- none

## Follow-Up Questions
- After the narrative docs land, should a separate ticket add an end-to-end consumer sample that exports a support bundle with representative ReadShape evidence and compiles generated PIT/bridge helpers?
- Should historical satellite-only documents or superseded plans get an explicit banner or note to reduce future confusion for readers who land on old release/plan pages first?

## Risks
- Multiple current docs still repeat the old satellite-only story; partial edits will leave contradictory guidance across README, analyzer README, checklist, and architecture pages.
- Read-plan examples can accidentally violate the repository's redaction boundary or imply SQL/plan inspection if they include raw request values or overly concrete provider behavior.
- Bridge helper docs can overstate support unless they keep the bounded endpoint vocabulary and required maximumDepth rule explicit.

## Split Recommendations
- Keep this ticket doc-only. If the team wants runnable consumer samples, benchmark refreshes, or new generator/runtime behavior, handle those as separate follow-up tickets rather than expanding this documentation refinement scope.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Document v0.25 read diagnostics and typed helper additions.

# Scope In
- Update README, generator README, architecture docs, production checklist, and release notes.
- Include examples for read-plan output and generated PIT/bridge helper usage.
- Compare generated helpers with runtime read-service requests and consumer-owned EF compiled queries.

# Acceptance Criteria
- Docs explain supported and unsupported read shapes without implying custom LINQ provider behavior.
- Release notes include compatibility, analyzer IDs, validation, and non-goals.