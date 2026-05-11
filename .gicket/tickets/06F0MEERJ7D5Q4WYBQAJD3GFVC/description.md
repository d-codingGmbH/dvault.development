<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a bounded JSON-first YAML boundary: DVault v1 should not add direct YAML parsing or a YAML dependency; YAML input is supported only by documented external conversion into the existing dvault.model.v1 JSON artifact before normal validation.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 decision is to defer direct YAML ingestion and keep the repository's existing dvault.model.v1 contract JSON-first.
- Any YAML authoring flow must convert outside DVault into canonical JSON that exactly follows schemaVersion dvault.model.v1, then use the same parser and validator path as hand-authored JSON.
- No YAML-only fields, merge semantics, anchors, tags, comments, duplicate-key behavior, or YAML-specific diagnostics are part of this ticket.

### Scope In
- Document the YAML ingestion boundary as JSON-first conversion for v1.
- Keep direct parser behavior focused on the existing dvault.model.v1 JSON object contract.
- Add or update tests or documentation proving the selected boundary is explicit and discoverable.
- Ensure the package dependency surface does not gain an unbounded YAML parsing dependency.

### Scope Out
- Direct YAML parsing in DVault packages for v1.
- YAML-specific schema semantics, validation categories, examples that imply YAML is an authoritative artifact format, or parallel YAML fixtures as contract sources.
- CLI, build integration, code generation, drift tooling, importer/exporter workflows, or runtime model mutation.
- Provider-specific behavior or metadata projection changes beyond what the existing JSON model contract already requires.

## Acceptance Criteria
- A repository document or public docs section explicitly states that dvault.model.v1 ingestion is JSON-first and that YAML authoring requires external conversion to JSON before DVault validation.
- The documented conversion boundary says the converted artifact must be the same JSON object shape, schemaVersion, token values, defaults, unknown-field behavior, and ordinal string comparison behavior defined by the dvault.model.v1 contract.
- No new YAML parser dependency is added to the core DVault package family for this ticket.
- Tests or documentation cover that the selected path preserves the same validated model semantics as JSON and does not introduce YAML-only behavior.
- User-facing wording makes the limitation clear without implying YAML is unsupported forever.

## Definition of Done
- The YAML boundary decision is recorded in the ticket implementation artifacts and aligns with docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md.
- Any model-first parser or ingestion docs continue to present JSON as the authoritative v1 artifact format.
- Automated checks relevant to the touched docs or tests pass, or any unavailable checks are called out by the implementer.
- Dependency changes, if any, are justified and show no direct YAML parsing package added for this ticket.

## Implementation Notes
- Use the existing schema contract as the architectural source of truth: its non-goals already exclude a YAML dependency and define a JSON object envelope.
- Prefer wording such as 'YAML may be used as an authoring convenience only when converted before DVault ingestion' over adding a YAML ingestion API.
- If code is touched, route all accepted content through the existing JSON model validation path and return a clear non-JSON or unsupported-format diagnostic for direct YAML input.
- Keep examples centered on JSON fixtures; an optional docs-only YAML snippet is acceptable only if it is explicitly labeled pre-conversion authoring input and paired with the resulting JSON boundary.

## Open Questions
- none

## Follow-Up Questions
- Should a later release consider an optional companion package for direct YAML parsing if user demand justifies the maintenance and dependency cost?
- Should future tooling provide a first-party CLI conversion command, or should conversion remain entirely caller-owned?

## Risks
- Documentation that casually says 'YAML support' could be misread as direct DVault YAML ingestion unless it consistently states the pre-conversion boundary.
- A future implementation could accidentally add YAML-only semantics during conversion examples; review should keep JSON as the only authoritative contract.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Decide whether YAML is supported directly or through documented conversion, then implement the chosen bounded path without destabilizing the package dependency surface.

## Scope In

- Dependency and maintenance assessment for YAML support.
- Either direct YAML parsing or a documented JSON-first conversion boundary.
- Tests or documentation for the selected path.

## Scope Out

- Parallel YAML-only semantics.
- Unbounded dependency additions.

## Acceptance Criteria

- The decision is explicit and documented.
- If YAML is implemented, it maps to the same validated model as JSON.
- If YAML is deferred, the JSON-first path remains complete and the limitation is clear.