[gicket-bot] PO refinement contract

Summary
- Refined the story to the provider-native bulk strategy implementation boundary already evidenced on branch, confirmed the existing child/downstream split is sufficient, and made no persistent planning changes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository-local .gicket artifacts were used to verify ticket, comment, and relation state in this pass; this ticket currently has only bot claim/lease comments, no persisted attachments, and existing relations from done SPI story 06F2PGMSQ4D4FV8W5ZERD4GS8C, done fallback task 06F2PGN4GPQCGC5WHZQBGP4SD0, parent epic 06F2PGMFWSEC95ATBCGZ6HYT5W, done child 06F2PGNT7DF4DVNKYWDFZC8DEM, and downstream tickets 06F2PGP2B2RZGGK3CVKK5WRRP8 and 06F2PGNZBRNCQ1SV2KKP6F3BA8.
- Repository evidence already fixes the v1 ownership boundary: the explicit bulk SPI is owned by 06F2PGMSQ4D4FV8W5ZERD4GS8C, the provider-neutral fallback baseline is owned by 06F2PGN4GPQCGC5WHZQBGP4SD0, and this story owns provider-package native strategy implementation and supported eligibility gates rather than a second bulk API.
- Current source and tests show four native strategy owners for this story: Postgres, SQL Server, MySQL, and Oracle provider packages each register IDataVaultProviderSaveStrategy through their AddDVault* extensions; the existing SQLite optimized path remains baseline and is not reopened here.
- Gate behavior is already repository-visible in DataVaultDiagnostics.cs: all native strategies decline dirty DbContexts and multi-active satellite batches; SQL Server also requires at least 50 total operations and at most 500 satellite operations; MySQL and Oracle require at least 50 total operations; Postgres has no minimum-operation threshold.
- No persistent planning action was materialized in this pass: no child ticket was created, no relation was added or removed, no attachment was added, and no planning document was written.

Scope In
- Implement and register provider-native ordered bulk save strategies in src/DCoding.Data.DVault.Postgres, src/DCoding.Data.DVault.SqlServer, src/DCoding.Data.DVault.MySql, and src/DCoding.Data.DVault.Oracle.
- Keep provider-specific bulk execution behind the existing DataVaultBulkSaveRequest and IDataVaultProviderSaveStrategy boundary, including unique-row reuse and latest-state satellite filtering for eligible native batches.
- Keep diagnostics and strategy dispatch aligned with the implemented provider gates so eligible batches select a native strategy and ineligible batches report material fallback causes before using the provider-neutral writer.
- Maintain repository-visible provider strategy tests that prove ordered hub, link, and ordinary satellite batch correctness for eligible native paths without reopening the public SPI boundary.

Scope Out
- Redesigning or renaming the public bulk SPI, registry bulk adapters, or typed bulk helper boundary already ratified by 06F2PGMSQ4D4FV8W5ZERD4GS8C.
- Changing the provider-neutral fallback writer or its core batch-correctness ownership already carried by 06F2PGN4GPQCGC5WHZQBGP4SD0.
- External opt-in live provider coverage, which is already split to done child 06F2PGNT7DF4DVNKYWDFZC8DEM.
- Benchmark runs, crossover analysis, or comparative performance reporting, which stay with 06F2PGNZBRNCQ1SV2KKP6F3BA8.
- Broader README, release-note, and adoption-document closure beyond narrow strategy-touching updates, which stay with 06F2PGP2B2RZGGK3CVKK5WRRP8.
- Multi-active satellite native support, dirty-context optimization, streaming ingestion, or any implicit SaveChanges-based ingestion mode.

Open questions
- none

Follow-up questions
- Should benchmark story 06F2PGNZBRNCQ1SV2KKP6F3BA8 publish crossover guidance using the same eligible batch shapes and thresholds as these native strategies before stronger performance claims are made?
- Should docs task 06F2PGP2B2RZGGK3CVKK5WRRP8 add a concise provider matrix that names the native-bulk eligibility gates without overcommitting to SQL internals?
- If future work wants native support for multi-active satellite batches or dirty tracked contexts, should that be separate follow-on work instead of widening this story?

Risks
- Provider-specific behavior can drift from the documented gates if strategy CanSave logic, diagnostics fallback-cause reporting, and provider tests stop evolving together.
- SQL Server, MySQL, and Oracle are intentionally shape-gated; undersized or dirty batches will correctly fall back, but consumers may misread that as missing optimization unless the docs task explains the gates clearly.
- Oracle support is still bounded to eligible ordinary satellite batches and excludes multi-active satellite shapes; widening that boundary later will require dedicated proof rather than silent expansion.

Split recommendations
- No additional split is recommended; the current relation graph already separates SPI, fallback, native strategy implementation, live provider coverage, benchmarks, and docs.
- If future work adds streaming ingestion, multi-active native support, or broader provider-decline observability beyond the current diagnostics model, create separate follow-on tickets instead of widening this story.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment