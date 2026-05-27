[gicket-bot] PO refinement contract

Summary
- Verified that the epic split already covers the full staged-provider bulk ingestion scope and that current repository evidence now spans the SPI, diagnostics, provider implementations, benchmark matrix, and v0.20.0 documentation boundary, so no new planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The live relation graph already contains eight parentOf children covering the staging SPI, staged fallback diagnostics, SQL Server, PostgreSQL, Oracle, MySQL, benchmark matrix, and v0.20.0 documentation slices, and each child ticket currently reads done.
- Repository evidence confirms the implemented provider matrix: SQL Server stages through SqlBulkCopy, PostgreSQL stages through COPY, MySQL registers MySqlStagedDataVaultSaveStrategy, and Oracle currently remains on the retained direct optimized path with stagedOracleBulk=not-selected-no-measured-win.
- Core diagnostics and telemetry already carry staged-provider decline and fallback vocabulary, and the root benchmark triplet plus docs/releases/v0.20.0.md and benchmark docs preserve the same provider-specific boundaries and stored-procedure non-default guidance.
- No child tickets, relation mutations, description updates, attachments, or planning documents were applied in this refinement pass; the live graph still carries one incoming blocks edge from done ticket 06F5Q8Y3WW9FFV7HA289VHCEAM, treated here as historical rather than an active blocker because the source ticket is complete.

Scope In
- Keep the epic as the parent contract for the provider-staging SPI, staged fallback diagnostics, provider-specific optimized save paths, benchmark evidence, and v0.20.0 documentation and stored-procedure boundary guidance.
- Preserve IDataVaultSaveService as the public write boundary and EF metadata as the model source while allowing provider strategies to select optimized execution behind diagnostics gates.
- Ratify the current evidence-bound provider matrix: PostgreSQL staged COPY above the 60-operation boundary with retained direct or UNNEST below it, SQL Server staged native bulk via SqlBulkCopy, MySQL staged bulk at 60 plus operations with retained multi-row above 50 and below 60, and Oracle retained direct optimized batching until staged Oracle shows a measured win.
- Preserve provider-neutral fallback or smaller provider-native path selection for dirty contexts, unsupported shapes, multi-active satellites, or provider and schema limits.

Scope Out
- New public save-service APIs, public staging types, or provider-native chunk execution claims.
- Automatic stored-procedure generation or making stored procedures the default architecture.
- Reopening the settled v0.19.0 chunked-save public contract or redefining caller-owned transaction and cancellation behavior.
- Treating Oracle staged bulk as selected before repository-visible evidence shows a deterministic win over the retained direct Oracle path.

Open questions
- none

Follow-up questions
- Should a later roadmap or release document publish one consolidated provider decision matrix covering staged bulk, retained direct or multi-row paths, provider-neutral chunked save, and stored-procedure escape-hatch guidance?
- If Oracle later shows a measured staged win or provider-native chunk execution gains evidence-backed support, should that land as new follow-up tickets rather than reopening this epic's settled boundary?

Risks
- The live relation graph still carries an incoming blocks edge from done ticket 06F5Q8Y3WW9FFV7HA289VHCEAM, which can confuse humans or automation even though its source ticket is complete.
- External-provider benchmark rows are opt-in, so unattended artifacts may continue to preserve skipped evidence rather than live timings unless configured provider lanes rerun.
- Future documentation could overstate Oracle or provider-native chunk behavior if it generalizes beyond the current evidence-backed boundaries.

Split recommendations
- No additional split is recommended; the architecture, diagnostics, provider, benchmark, and documentation slices were already materialized as child tickets, and current evidence does not justify another epic-level decomposition.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment