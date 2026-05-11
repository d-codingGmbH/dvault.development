[gicket-bot] PO-critic review contract

Summary
- PO handoff is ready for developer work: the ticket has a bounded JSON-first YAML boundary, no unresolved Open Questions, and direct repository evidence for the authoritative dvault.model.v1 contract it depends on.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEERJ7D5Q4WYBQAJD3GFVC/description.md contains PO Handoff decision ready_for_po_critic and ## Open Questions - none.
- .gicket/tickets/06F0MEERJ7D5Q4WYBQAJD3GFVC/description.md scopes in documenting YAML as JSON-first conversion for v1, scopes out direct YAML parsing and YAML-specific semantics, and requires no new YAML parser dependency.
- Comment .gicket/tickets/06F0MEERJ7D5Q4WYBQAJD3GFVC/comments/06F1FZAPCX03M3EPH92E3SCDRG.md records the same PO refinement contract, including external conversion to canonical JSON and no YAML-only fields, merge semantics, anchors, tags, comments, duplicate-key behavior, or YAML-specific diagnostics.
- Comment .gicket/tickets/06F0MEERJ7D5Q4WYBQAJD3GFVC/comments/06F1FZCKQ2Y5HD6Z8MT35AM7ER.md reports outcome po-refinement-ready and handoff to role po-critic.
- git rev-parse shows the repository on branch ticket/06F0MEERJ7D5Q4WYBQAJD3GFVC-task-decide-and-implement-yaml-ingestion-boundar at a345a5ffc4d84d7d96c0c49f109cab0d7ef5cbb5.
- git log --max-count=12 shows this ticket branch has PO claim/handoff and PO-critic claim commits after develop; git diff --name-status develop..HEAD lists only .gicket ticket metadata, comments, and events for this ticket.
- docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md states the durable dvault.model.v1 artifact contract is JSON-first, says no YAML dependency is defined, and defines the artifact as a JSON object with required schemaVersion.
- The same schema contract defines exact schemaVersion dvault.model.v1, strict version compatibility, ordinal token/declaration-name comparisons, optional arrays defaulting to empty arrays, and unknown fields as validation errors at every object level.
- Package/dependency search across csproj/props/targets files found PackageReference entries for EF/Core/test dependencies and no YamlDotNet or other YAML parser package reference in the core DVault package family.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No PO-blocking gaps. Developer-facing examples should still cover direct YAML input rejection or unsupported-format wording, externally converted JSON acceptance, and a docs-only YAML snippet only if paired with the resulting JSON boundary.

Risky assumptions
- The ticket assumes documentation and/or focused tests are enough to establish the v1 boundary unless implementation work touches a parser path; if code is touched, the existing JSON validation path must be directly evidenced.
- The phrase YAML support can still be misread, so implementation should consistently use pre-conversion or authoring convenience wording rather than direct ingestion wording.

AC / test suggestions
- Add a public docs section stating YAML may be used only as caller-owned authoring input that is converted before DVault ingestion.
- Assert or document that converted artifacts must preserve the same JSON object shape, schemaVersion dvault.model.v1, supported tokens, defaults, unknown-field behavior, and ordinal string comparison behavior from the schema contract.
- Verify package references for the six packable DVault packages do not add a YAML parser dependency.
- If parser code exists or is added, include tests that direct YAML text is not accepted as a dvault.model.v1 artifact and that converted JSON follows the same validation diagnostics as hand-authored JSON.

Implementation watchouts
- Do not add CLI, build integration, importer/exporter workflow, code generation, drift tooling, runtime mutation, or provider-specific behavior under this ticket.
- Keep examples centered on JSON fixtures; any YAML snippet must be explicitly labeled pre-conversion authoring input and not authoritative contract data.
- Do not introduce YAML-only semantics such as anchors, tags, comments, merge behavior, duplicate-key behavior, or YAML-specific diagnostic categories.
- Preserve the existing JSON-first schema contract in docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md as the architectural source of truth.

Non-blocking notes
- none

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment