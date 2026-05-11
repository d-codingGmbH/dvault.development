[gicket-bot] PO refinement contract

Summary
- Verified the ticket store, relation events, completed child tickets, README/release docs, diagnostics source/tests, and runnable quickstart examples. The parent story is an aggregation/closure refinement: all scoped work is already represented by persisted done child tickets, so no new child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Diagnostics scope is delivered by child 06F0MED4P7HMBDZVMPWQZ5A7PC: IDataVaultDiagnosticsService exposes structured validation/explain output for metadata models, registries, Code-First declarations, and DbContexts, with save-strategy diagnostics request-bound and NotEvaluated when no save request is supplied.
- Starter examples scope is delivered by child 06F0MEDBFZ25YA1M7RJ71Z7ZCM: examples/ contains SQLite and PostgreSQL quickstarts using one shared registry-backed DataVaultMetadataModel, explicit IDataVaultSaveService writes, and typed latest/as-of reads; PostgreSQL uses DVAULT_TEST_POSTGRES_CONNECTION_STRING and exits successfully with the documented skip message when absent.
- README.md and docs/releases/v0.6.0.md are delivered by child 06F0MEDJC732GDD77H60R259P0 and now document the v0.6.0 usability flow, including Code-First as the recommended bounded happy path and metadata-first/registry APIs as compatible advanced/shared-metadata paths.
- The repository already fixes the v1 defaults for this story: Code-First covers hubs, hub-parent satellites, multi-active driving keys, and ordered hub links; registry-backed metadata remains the authoritative quickstart/shared-metadata path; no public Code-First-to-registry bridge is part of this release.

Scope In
- Parent-level aggregation of completed diagnostics, examples, and documentation work for the v0.6.0 developer usability story.
- Diagnostics/explain output that lets callers inspect generated tables/entities, columns/properties, indexes, constraints, primary keys, parent references, metadata source, provider capability/behavior profile, load-timestamp storage, and request-bound save-strategy selection/fallback.
- Runnable SQLite local quickstart and PostgreSQL quickstart with environment-variable configuration, no committed secrets, explicit saves, and typed latest/as-of reads over a small history flow.
- README and release documentation that compare the recommended bounded Code-First path with metadata-first/registry APIs and explain when to use low-level/raw APIs versus convenience typed helpers.

Scope Out
- New parent-level product-code implementation; the story is already split into completed child implementation/documentation tickets.
- CI-driven package publishing automation, final NuGet publication, release tag creation, and release-operator approval.
- Full database provisioning automation for every provider or mandatory external PostgreSQL infrastructure.
- Public Code-First-to-registry conversion API, model-first import/export specs, PIT-backed read APIs, bridge traversal helpers, PIT/bridge row maintenance, provider-specific read optimizations, and additional provider quickstarts.
- CLI diagnostics command implementation or provider optimization behavior changes beyond the delivered structured diagnostics payload.

Open questions
- none

Follow-up questions
- After the v0.6.0 tag exists, the release operator should rerun the manual NuGet publication checklist from the tagged checkout and record final audited 0.6.0 package evidence before publication.
- Should a future CLI wrapper expose the same structured diagnostics payload directly, or should CLI shaping remain outside the core library?
- Should a later release add a public Code-First-to-DataVaultMetadataModel/DataVaultMetadataRegistry bridge for examples or shared metadata scenarios?
- Should optional CI or a documented local harness later exercise the PostgreSQL quickstart automatically when a developer-managed connection string is available?
- Should later tickets add additional provider quickstarts after the SQLite/PostgreSQL baseline is proven useful?

Risks
- Final package publication remains outside this story and still depends on release-operator validation and approval.
- Diagnostics fallback-cause reporting can drift from runtime dispatch behavior if provider strategy gates change without updating shared tests and docs.
- Reviewers may confuse README v0.6.0 install guidance with pre-tag MinVer artifact evidence; the completed docs child separates those concerns, but the distinction should remain visible through release validation.
- Provider capability auto-registration and provider-specific save-strategy registration are separate surfaces; future provider docs should keep that distinction explicit.

Split recommendations
- No new split is recommended. The story already has the needed persisted child split and all three children are done: diagnostics 06F0MED4P7HMBDZVMPWQZ5A7PC, quickstart examples 06F0MEDBFZ25YA1M7RJ71Z7ZCM, and README/release docs 06F0MEDJC732GDD77H60R259P0.
- Future work should be split only for concrete new scope such as CLI diagnostics, Code-First-to-registry bridging, additional provider quickstarts, or post-tag package publication defects; none blocks this PO-critic handoff.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment