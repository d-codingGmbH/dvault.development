[gicket-bot] PO refinement contract

Summary
- Refined the developer adoption tooling epic against the provided ticket snapshot, local relation state, attachment metadata, child-ticket contracts, and repository evidence. The three direct child stories are already materialized and done; no new tickets, relation changes, attachments, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The attached v0.10.0-adoption-tooling-plan.md establishes the intended sequence: analyzer package foundation, Testcontainers/helper examples, then production adoption checklist and examples refresh.
- Current repository evidence ratifies the analyzer boundary at src/DCoding.Data.DVault.Analyzers with package id DCoding.Data.DVault.Analyzers, packable analyzer assets, README installation/suppression guidance, and DMV1901/DMV1902 coverage.
- The Testcontainers-oriented v1 scope is the opt-in PostgreSQL local container fixture and documentation using DVAULT_TEST_POSTGRES_CONNECTION_STRING; a published reusable DotNet.Testcontainers helper library is not required for this epic.
- The adoption documentation baseline is README.md, examples/README.md, examples/DCoding.Data.DVault.PostgresQuickstart/README.md, docs/production-adoption-checklist.md, and the v0.8 design-time workflow, using the coordinated 0.9.0 package examples and current API names.

Scope In
- Treat the epic as the adoption-tooling umbrella for the three completed child stories: analyzer package foundation, opt-in provider-container example guidance, and production adoption documentation refresh.
- Preserve the analyzer package boundary, DMV1901/DMV1902 first-rule baseline, package metadata, analyzer asset packing, tests, and installation/suppression documentation.
- Preserve the opt-in PostgreSQL container fixture pattern with explicit image/tag, connection-string handoff, targeted external-provider test command, skip/failure behavior, and no default external database dependency.
- Keep adopter-facing docs aligned around the provider-neutral package, the five provider packages, NuGet installation, AddDVault()/AddDVaultProvider registration, Code-First/metadata-first/model-first declaration paths, explicit save boundaries, read helpers, migration guardrails, and drift limits.
- Keep known limitations visible, especially SQLite-first live-schema drift support and external opt-in evidence for PostgreSQL, SQL Server, Oracle, and MySQL.

Scope Out
- No IDE-specific extension beyond normal Roslyn analyzer distribution.
- No hosted service, SaaS dependency, bundled database image, checked-in secret, or mandatory Docker/Podman dependency for default tests.
- No full provider fixture matrix, CI provider matrix expansion, or reusable Testcontainers helper package unless later tickets explicitly add them.
- No new core DVault semantics, provider implementation, migration automation, EF CLI shim, or runtime behavior changes under this epic.
- No provider support promise without runnable evidence or an explicit documented limitation.

Open questions
- none

Follow-up questions
- Should a future release add more analyzer rule families beyond DMV1901/DMV1902, such as missing business keys, suspicious satellite names, metadata-first/model-first artifact checks, or provider governance diagnostics?
- Should MySQL, SQL Server, or Oracle get separate provider-container fixture tickets after the PostgreSQL baseline?
- Should a later ticket add reusable DotNet.Testcontainers helper code, or keep provider startup as documented local Podman/Docker commands?
- Should provider-specific production operations get separate deep-dive docs after the general checklist stabilizes?

Risks
- The epic wording can be read as requiring a full Testcontainers library or provider matrix; acceptance should stay bounded to the completed PostgreSQL opt-in fixture unless new tickets expand it.
- Analyzer messaging can overpromise if presented as complete DVault modeling validation; keep it scoped to the high-confidence DMV1901/DMV1902 baseline.
- External provider examples can leak into default-test expectations unless docs continue to emphasize opt-in configuration, placeholder credentials, and conditional package restore.

Split recommendations
- No additional split is recommended now; the existing three direct child stories cover the epic scope and are done.
- Future provider fixture expansion or broader analyzer coverage should be split into separate provider-specific or rule-family tickets rather than expanding this epic.

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