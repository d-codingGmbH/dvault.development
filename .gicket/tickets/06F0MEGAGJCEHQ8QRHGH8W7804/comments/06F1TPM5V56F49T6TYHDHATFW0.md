[gicket-bot] PO refinement contract

Summary
- Verified the ticket snapshot, bot-only comments, attachment state, live relation state, README baseline, and dvault.model.v1 planning contract. No child tickets, relation changes, attachments, or planning documents were needed; the refinement is ready for PO critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The ticket has no human clarification comments and no existing attachments.
- Live relations were inspected and left unchanged; no child tickets or follow-up tickets were created during this PO pass.
- The model-first baseline is fixed by docs/plans/06F0MEE8T9PKPKQH8EPWNQ2CRW-dvault-model-v1-schema-contract.md: canonical JSON artifact, exact schemaVersion dvault.model.v1, default naming policy, strict unknown-field validation, loadTimestampStorage tokens, and stable diagnostic categories/codes.
- README currently makes Code-First the v0.6.0 happy path where the implemented hub, hub-parent satellite, driving-key, and ordered hub-link surface fits, while registry-backed metadata remains the shared authoritative metadata path.
- Model-first import/export specs are explicitly deferred in v0.6.0, so this ticket should document governance workflow and limitations without implying shipped runtime tooling.

Scope In
- Update README or add a linked docs guide for model-first governance usage.
- Document which profile should use Code-First, metadata-first registry-backed metadata, or model-first governed artifacts.
- Describe the review workflow for model-first artifacts, exported models, imports, and drift reports as governance evidence alongside Code-First usage.
- Document artifact versioning rules using the dvault.model.v1 contract, including strict schemaVersion handling, canonical JSON, unknown-field errors, and safe handling of future schema versions.
- Make current limitations explicit, including JSON-first ingestion, YAML as external authoring-only conversion, no public Code-First-to-registry bridge, and deferred model-first import/export tooling.

Scope Out
- Implementing parser, exporter, importer, drift reporting, CLI commands, build integration, or CI gates.
- Publishing packages or changing NuGet publication workflow.
- Documenting unimplemented graph semantics as supported runtime behavior.
- Changing product code, package metadata, provider behavior, or verification scripts.

Open questions
- none

Follow-up questions
- After import/export/drift tooling is implemented, decide whether to add concrete command examples, expected report formats, and CI gating snippets.
- If first-party YAML ingestion becomes a product goal, handle it as a separate additive contract instead of expanding this documentation ticket.
- After model-first tooling is real, decide whether the manual NuGet publication checklist should require model artifact or drift-report evidence for releases.

Risks
- The main risk is overstating model-first support while v0.6.0 release notes still defer import/export specs; the docs should separate governance workflow from shipped tooling.
- README is packaged with NuGet, so long governance detail could obscure the quickstart; a concise README entry with a linked guide is the safer documentation shape.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment