[gicket-bot] PO refinement contract

Summary
- Refined this PIT child task against the current provider-neutral modeling surfaces and the existing PIT story split; no child-ticket, relation, attachment, or planning-document writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket remains the metadata-and-builder child under PIT story 06EZ0NSXY2Y1JZ8SSCX177C770; sibling task 06EZ0NTB26CCYQ7FCN2REEGDGW owns provider-neutral EF mapping and sibling task 06EZ0NTJZEMVA5RPR01V0KNVMR owns docs/example work.
- Repository evidence shows no existing PIT implementation surface in src/DCoding.Data.DVault, so this task should introduce a new provider-neutral PIT contract rather than retrofit provider-specific behavior.
- PIT remains an explicit opt-in deferred capability per docs/plans/deferred-data-vault-capabilities.md and must not change default hub/link/satellite modeling when no PIT is declared.
- The PIT contract should mirror the current split between provider-neutral metadata declarations and a convention-first model-generation builder surface, instead of collapsing PIT directly into EF-only mapping code.
- Live relation state remains unchanged: the ticket still has the incoming parentOf relation from 06EZ0NSXY2Y1JZ8SSCX177C770, and no relation writes were materialized in this pass.

Scope In
- Provider-neutral PIT metadata declarations for one PIT table, exactly one hub reference, and one or more satellite references.
- Aggregate-model validation that resolves PIT references against declared hub and satellite metadata.
- A convention-first PIT builder API used by model generation and flowing through the existing naming-policy override surface.
- The provider-neutral PIT generated table and field shape needed for deterministic names and key-field assertions.
- Unit and public-API coverage for PIT metadata, validation, and model-generation output.

Scope Out
- Provider-neutral EF Core mapping and annotation projection for PIT tables; that work already belongs to 06EZ0NTB26CCYQ7FCN2REEGDGW.
- README/docs examples and end-user PIT guidance; that work already belongs to 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Refresh scheduling, late-arriving-data reconciliation, persisted-versus-computed PIT materialization policy, provider-specific SQL, and migrations.
- Any change that makes PIT required for AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), or ordinary hub/link/satellite declarations.
- Bridge, multi-active, hook, or save-service behavior outside the PIT metadata and model-generation contract.

Open questions
- none

Follow-up questions
- When PIT generation moves beyond the metadata and builder contract, which v0.5 refresh strategy should land first: <redacted> recompute, incremental refresh, or a provider-owned materialization path?
- Should a later PIT behavior ticket define persisted-only PIT tables, computed query-time PIT projections, or both?
- How should late-arriving satellite rows be reconciled in PIT refresh logic once materialization behavior is in scope?

Risks
- If the implementation reuses existing satellite technical metadata or key-role abstractions too aggressively, PIT-specific fields may leak into the closed v1 ingest contract and create unnecessary public-API churn.
- If this ticket does not pin one provider-neutral PIT key and reference baseline, sibling mapping work may invent a different field shape and create model-builder versus EF-mapping drift.
- Current branch evidence already contains satellite index-shape differences between pure model generation and EF translation, so PIT tests need explicit cross-surface assertions to prevent the same divergence.

Split recommendations
- No additional split recommended. The parent story is already bounded across 06EZ0NT4FDPC7XTQH40PQS942M for metadata and builder work, 06EZ0NTB26CCYQ7FCN2REEGDGW for EF mapping, and 06EZ0NTJZEMVA5RPR01V0KNVMR for docs/example work.
- No child tickets, relation changes, attachments, or planning documents were materialized in this refinement pass.

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