[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded compile-time analyzer slice: add high-confidence EF Core misuse diagnostics in `DCoding.Data.DVault.Analyzers`, keep the existing epic/docs relations unchanged, and exclude runtime/preflight/query-shape work already owned by sibling tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified local ticket evidence: the story remains under epic `06F492A3MPSGP3KXDNZECN01QM` and still blocks documentation task `06F492BNDPWS9P4EDSV0W7G6VM`; no relation cleanup was needed.
- Verified repository baseline: `DCoding.Data.DVault.Analyzers` currently ships only Code-First and mapping diagnostics (`DMV1901`, `DMV1902`, `DMV1950`-`DMV1955`), so this story is the first bounded analyzer slice for EF Core misuse diagnostics.
- Verified public DVault boundary: `IDataVaultSaveService` is the default write lane, `UseDataVaultSaveChangesMetadataInterceptor(...)` is explicit opt-in and metadata-only, and direct `Set<Dictionary<string, object>>(...)` queries are documented read patterns.
- No child tickets, description updates, attachments, or planning documents were materialized in this refinement pass; the live ticket contract is kept consistent with the existing persisted relations.

Scope In
- Add compile-time diagnostics in `DCoding.Data.DVault.Analyzers` for statically obvious consumer-side EF Core misuse that violates documented DVault model or write boundaries.
- Cover the bounded misuse families already named by the story when they are high-confidence in source: unsupported/generated-table `DbSet` exposure, obviously unsafe direct generated-table write patterns, statically obvious missing DVault metadata registration, and obvious bypasses of DVault technical metadata conventions.
- Add analyzer tests in `tests/DCoding.Data.DVault.Tests/Analyzers` for both positive findings and documented safe patterns.
- Add a bounded code fix only where the remediation is mechanical and low-risk; otherwise rely on precise diagnostic and remediation text.

Scope Out
- Runtime interception, runtime blocking, or runtime warn-only guard behavior for `SaveChanges`; that is already carved into `06F492AYE4A3PKA2D20DDPQ37C`.
- Preflight aggregation, drift, migration, provider-capability, or query-shape diagnostics beyond analyzer-local misuse detection; those are already covered by sibling tickets in the same epic.
- Whole-application DI inference or cross-project proof that arbitrary `DbContext` construction paths call `UseDataVaultMetadata(...)`; this story only needs statically obvious cases.
- Flagging documented read-only generated-table query patterns such as `Set<Dictionary<string, object>>(...)` plus LINQ or compiled-query reads.
- Broad release-note or documentation rollout beyond the analyzer inputs needed by downstream task `06F492BNDPWS9P4EDSV0W7G6VM`.

Open questions
- none

Follow-up questions
- After the high-confidence v1 rule set lands, do we want a later analyzer phase for broader DI or `DbContext` composition patterns that require multi-file inference rather than local source certainty?
- Once runtime guard mode exists, should a later story align analyzer suppressions and guard-mode messaging for advanced generated-row tracking scenarios that deliberately bypass `IDataVaultSaveService`?

Risks
- False positives will be the main failure mode if rules try to infer arbitrary app composition instead of staying on statically obvious misuse.
- Advanced consumer flows that intentionally track generated DVault rows through EF can resemble unsafe direct writes; diagnostics must distinguish the documented opt-in metadata-interceptor lane from clearly unsupported patterns.
- String-only table-name detection is brittle because DVault supports provider-aware produced names and documented direct read access to shared-type tables.

Split recommendations
- No additional child-ticket split is recommended at PO refinement time; the existing sibling tickets already separate runtime guard, preflight, drift, query-shape, and documentation work, so this story can stay a single compile-time analyzer slice.

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