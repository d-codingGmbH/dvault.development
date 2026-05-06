[gicket-bot] PO refinement contract

Summary
- Refined the PIT EF mapping task against the existing PIT story split and current hub/link/satellite translator baseline; no new child tickets, relation changes, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live ticket relations already place this task under PIT story 06EZ0NSXY2Y1JZ8SSCX177C770 through an incoming parentOf relation; no relation cleanup or new relation writes were needed in this pass.
- Sibling task 06EZ0NT4FDPC7XTQH40PQS942M owns the PIT metadata model and builder API, and sibling task 06EZ0NTJZEMVA5RPR01V0KNVMR owns PIT docs/example scope; this task stays focused on EF Core projection of that metadata.
- Current repository evidence shows DataVaultEfMetadataTranslator and ApplyDataVaultMetadata() only project hub, link, and satellite shared-type EF metadata today, so PIT work is an additive opt-in extension and must leave existing mappings unchanged when no PIT metadata is configured.
- docs/plans/deferred-data-vault-capabilities.md already fixes PIT as an opt-in deferred capability that must not change ordinary hub/link/satellite setup; no planning document or attachment was materialized because that architecture record and the existing ticket tree already cover the split.

Scope In
- Translate the validated PIT metadata from task 06EZ0NT4FDPC7XTQH40PQS942M into provider-neutral EF Core shared-type metadata through the existing ApplyDataVaultMetadata() path.
- Project the baseline PIT table shape for one hub and one or more satellites attached to that hub, including the PIT key columns, the hub reference column, one satellite snapshot reference column per included satellite, and PIT load-timestamp metadata.
- Add unit and SQLite-baseline integration coverage that verifies deterministic names, column order, key metadata, and basic PIT queryability without putting SQLite-specific SQL into core translation logic.
- Surface deterministic validation or translation failures for PIT shapes that fall outside the agreed baseline instead of silently emitting partial mappings.

Scope Out
- Defining the PIT metadata model, builder API, or its first-pass validation rules beyond what sibling task 06EZ0NT4FDPC7XTQH40PQS942M owns.
- PIT refresh or population orchestration, query helpers, materialization jobs, migrations, or provider-specific SQL optimization work.
- README or docs/example authoring, which stays in sibling task 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Expanding PIT v0.5 scope to link-based PIT tables, multi-active satellite PIT behavior, or broader deferred-capability work.

Open questions
- none

Follow-up questions
- If later PIT work needs link-based PIT tables or multi-active satellite snapshots, should those land as separate follow-up tickets instead of widening this baseline task?
- After PIT mapping lands, does v0.5 want convenience query helpers for PIT consumption, or is ordinary EF shared-type access sufficient until a later read-API ticket?

Risks
- If PIT naming or key semantics drift from sibling task 06EZ0NT4FDPC7XTQH40PQS942M, translator tests and public snapshots can diverge across the same story.
- PIT work can accidentally expand into refresh/materialization or provider-specific optimization scope unless the additive EF-mapping boundary stays enforced.
- New public PIT-facing enums, annotations, or table-shape surface may ripple into existing snapshot and compatibility checks beyond the immediate translator code.

Split recommendations
- No additional split is recommended. The existing PIT story 06EZ0NSXY2Y1JZ8SSCX177C770 already has bounded child tasks for metadata API (06EZ0NT4FDPC7XTQH40PQS942M), EF mapping (06EZ0NTB26CCYQ7FCN2REEGDGW), and docs/examples (06EZ0NTJZEMVA5RPR01V0KNVMR).

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