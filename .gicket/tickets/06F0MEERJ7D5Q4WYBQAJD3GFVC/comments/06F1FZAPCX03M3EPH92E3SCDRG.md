[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded JSON-first YAML boundary: DVault v1 should not add direct YAML parsing or a YAML dependency; YAML input is supported only by documented external conversion into the existing dvault.model.v1 JSON artifact before normal validation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 decision is to defer direct YAML ingestion and keep the repository's existing dvault.model.v1 contract JSON-first.
- Any YAML authoring flow must convert outside DVault into canonical JSON that exactly follows schemaVersion dvault.model.v1, then use the same parser and validator path as hand-authored JSON.
- No YAML-only fields, merge semantics, anchors, tags, comments, duplicate-key behavior, or YAML-specific diagnostics are part of this ticket.

Scope In
- Document the YAML ingestion boundary as JSON-first conversion for v1.
- Keep direct parser behavior focused on the existing dvault.model.v1 JSON object contract.
- Add or update tests or documentation proving the selected boundary is explicit and discoverable.
- Ensure the package dependency surface does not gain an unbounded YAML parsing dependency.

Scope Out
- Direct YAML parsing in DVault packages for v1.
- YAML-specific schema semantics, validation categories, examples that imply YAML is an authoritative artifact format, or parallel YAML fixtures as contract sources.
- CLI, build integration, code generation, drift tooling, importer/exporter workflows, or runtime model mutation.
- Provider-specific behavior or metadata projection changes beyond what the existing JSON model contract already requires.

Open questions
- none

Follow-up questions
- Should a later release consider an optional companion package for direct YAML parsing if user demand justifies the maintenance and dependency cost?
- Should future tooling provide a first-party CLI conversion command, or should conversion remain entirely caller-owned?

Risks
- Documentation that casually says 'YAML support' could be misread as direct DVault YAML ingestion unless it consistently states the pre-conversion boundary.
- A future implementation could accidentally add YAML-only semantics during conversion examples; review should keep JSON as the only authoritative contract.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment