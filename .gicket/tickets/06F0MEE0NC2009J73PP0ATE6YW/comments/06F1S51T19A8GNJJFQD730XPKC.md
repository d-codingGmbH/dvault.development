[gicket-bot] PO refinement contract

Summary
- Refined the model-first import story against the existing schema contract, repository evidence, and completed child-ticket evidence. The story is ready for PO-critic review with no blocking product questions and no new ticket, relation, attachment, or planning-document writes materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 artifact baseline is JSON-first with required schemaVersion exactly dvault.model.v1, strict unknown-field rejection, default naming.policy = default, optional declaration arrays defaulting to empty, and ordinal string comparison semantics.
- Already-created child work covers the split: schema contract 06F0MEE8T9PKPKQH8EPWNQ2CRW is done, parser/diagnostics 06F0MEEGJE9QCHC8YN4FEXYX10 is done, YAML boundary 06F0MEERJ7D5Q4WYBQAJD3GFVC is done, and import/projection 06F0MEF08AJ1K52STF42T74B04 is done.
- Governance documentation ticket 06F0MEGAGJCEHQ8QRHGH8W7804 remains a separate todo consumer and does not reopen the import story scope.
- The v1 YAML decision is external pre-conversion only; DVault v1 should not add direct YAML parsing, YAML-only semantics, or a core YAML dependency.
- Projection should reuse the existing registry and EF metadata pipeline rather than create a parallel model-first projection stack.

Scope In
- Story-level delivery of dvault.model.v1 import across schema contract, strict JSON parser and validation diagnostics, YAML boundary decision, and import-to-registry/import-to-EF projection.
- Model-first declarations for hubs, links, hub-parent and link-parent satellites, multi-active driving keys, PIT tables, bridges, naming policy, and load timestamp storage choices as defined by the v1 contract.
- Structured diagnostics for version, shape, unknown field, reference, duplicate, naming collision, provider-choice, capability, and recursive participant binding failures.
- Parity between imported model artifacts and metadata-first or Code-First semantics where those repository surfaces currently overlap.

Scope Out
- Runtime model mutation after import.
- Code generation beyond import/projection.
- Direct YAML ingestion or YAML-specific schema semantics.
- Export tooling, drift reporting, CLI/build integration, and governance docs beyond linking to their existing tickets.
- Provider-specific DDL, SQL, migrations, or read optimizations outside the existing provider capability profile mechanism.
- Expanding the public Code-First API to cover link-parent satellites, PITs, bridges, or role-bearing recursive links.

Open questions
- none

Follow-up questions
- Governance ticket 06F0MEGAGJCEHQ8QRHGH8W7804 should document the recommended choice between model-first, metadata-first, and Code-First flows after the import surface is ready for users.
- Future export and drift tickets should consider consuming the same import result surface so artifact normalization and comparison behavior stay centralized.
- A later release can revisit optional YAML tooling or richer naming/provider extension sections as versioned additions, not implicit v1 behavior.

Risks
- If unknown fields are ignored, misspelled artifacts can silently drift from intended metadata.
- If loadTimestampStorage is not propagated into provider capability profiles, imported projection can diverge from metadata-first and Code-First behavior.
- If post-parse failures collapse to generic metadata exceptions, users will lose the source-path diagnostics promised by the story.
- Recursive-role and hierarchy bridge cases remain sensitive because current public link metadata may not carry enough role information without a narrow model-first adapter.

Split recommendations
- No new split is recommended. The existing child-ticket set already covers schema, parser/diagnostics, YAML boundary, import/projection, and downstream governance documentation.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment