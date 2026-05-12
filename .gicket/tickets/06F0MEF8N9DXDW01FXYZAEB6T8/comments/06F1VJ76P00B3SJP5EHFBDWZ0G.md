[gicket-bot] PO refinement contract

Summary
- Refined the story around the existing dvault.model.v1 governance baseline, deterministic export, manual drift reporting, and docs updates.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 export target is the existing canonical JSON-first dvault.model.v1 artifact contract documented in docs/model-first-governance.md and docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md.
- The v1 default naming policy is the repository default policy with naming.policy set to default; alternate naming policies are future advanced configuration work and do not block this ticket.
- Drift comparison is manual tooling/library functionality for review evidence and does not include database migration execution, release publishing, or CI automation.
- The comparison baseline is the expected dvault.model.v1 artifact versus generated/current EF metadata and produced table metadata visible through DVault annotations, produced names, declaration ordering, and provider-neutral roles.

Scope In
- Add deterministic export from DVault Code-First declarations and registry-backed metadata into canonical dvault.model.v1 JSON artifacts.
- Preserve the existing v1 artifact envelope, declaration categories, strict schemaVersion value, default values, stable declaration ordering, and unknown-field behavior when exporting.
- Add drift tooling that compares an expected dvault.model.v1 model against generated/current EF/table metadata for provider-neutral Data Vault structures.
- Report added, removed, renamed, and incompatible drift across relevant tables, columns, indexes, constraints, entity kinds, metadata names, parent/participant references, ordering, and provider-neutral property roles where that metadata is available.
- Update model governance documentation so teams can export artifacts, run drift comparison manually, and use the report as pre-release review evidence without release credentials.

Scope Out
- Executing or generating database migrations.
- CI publishing automation or release-gate wiring.
- Direct YAML ingestion, YAML fixture contracts, or a core YAML dependency.
- Provider-specific DDL diffing beyond the provider-neutral EF/table metadata available in the current branch.
- Changing the dvault.model.v1 schema contract or introducing v2 artifact compatibility.
- Advanced custom naming, hashing, timestamp, record-source, or provider hook implementation beyond honoring the existing v1 defaults.

Open questions
- none

Follow-up questions
- Should a later ticket wire drift comparison into CI or repository release gates once the manual workflow is proven?
- Should future schema versions add provider-specific DDL drift or migration-plan generation beyond provider-neutral metadata comparison?
- Should future advanced configuration tickets expose custom naming/hash/timestamp policies in exported model artifacts once those hooks exist?

Risks
- Rename detection may be limited when metadata lacks a stable identity that survives produced-name changes; report unmatched items as added and removed rather than guessing.
- Provider-specific EF metadata can vary by provider, so this story should keep v1 drift semantics grounded in DVault-owned provider-neutral annotations and documented logical metadata.
- PIT and bridge support depends on the current branch's available metadata surfaces; tests should pin the supported v1 shapes and report unsupported comparison gaps explicitly.

Split recommendations
- If implementation size grows, split into exporter implementation, drift report implementation, and documentation/examples as separate delivery slices while keeping this story's v1 contract unchanged.

Persisted contract coverage
- acceptance-criteria items: 10
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment