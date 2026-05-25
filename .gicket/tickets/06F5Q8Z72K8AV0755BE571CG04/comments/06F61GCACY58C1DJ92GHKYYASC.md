[gicket-bot] PO refinement contract

Summary
- Refined the SQL Server staged bulk save story around the existing SQL Server provider-strategy boundary, provider-neutral fallback semantics, and opt-in SQL Server validation/benchmark lanes; no child tickets, relation edits, description updates, attachments, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Visible repository evidence already fixes the implementation lane as the existing SQL Server provider package and strategy path, so this story should refine AddDVaultSqlServer()/SqlServerDataVaultSaveStrategy behavior rather than introduce a new public save entry point.
- The current explicit-save and streaming contracts keep provider-specific chunk execution outside scope, so this story targets eligible ordered bulk batches behind the existing IDataVaultSaveService contract rather than new chunk-only behavior.
- SQL Server live validation and benchmark evidence remain opt-in through DVAULT_TEST_SQLSERVER_CONNECTION_STRING; default local validation stays at non-live smoke or contract coverage.
- Current ticket relations were verified and left unchanged: this story remains a child of 06F5Q8YBVRS2EZVMJK5EATV9AR, is blocked by 06F5Q8YKR31DXGRXVPJ9031BQW, and blocks 06F5Q900FC0P3HBZP81CVK7264.
- No bounded planning writes were applied because the current evidence supports a single bounded refinement contract without an immediate split or authoritative description rewrite.

Scope In
- Implement a staged SQL Server bulk save path inside src/DCoding.Data.DVault.SqlServer as the provider-specific optimization for eligible ordered DVault save batches.
- Use SQL Server staging plus SqlBulkCopy or an equivalent SQL Server-native transfer mechanism for eligible hub, link, and ordinary satellite persistence work accepted by the SQL Server strategy.
- Preserve deterministic caller order, idempotent hub and link reuse, latest-state satellite hash-diff behavior, caller-owned transaction participation, cancellation propagation, and staging cleanup within the optimized path.
- Keep SQL Server eligibility gating and fallback aligned with the documented provider-strategy boundary so declined or unsupported batches continue through the provider-neutral writer.
- Add SQL Server-gated tests and benchmark or evidence rows consistent with the existing optional external-provider lane.

Scope Out
- New public save-service overloads, streaming or chunked API changes, or provider-specific chunk execution claims.
- Changes to non-SQL Server provider packages except for minimal shared contract work required to preserve existing dispatcher semantics.
- Making live SQL Server infrastructure a required local prerequisite for all contributors.
- Background ingestion, CDC or file ingestion, implicit SaveChanges interception, or release-governance work.

Open questions
- none

Follow-up questions
- After the SQL Server ordered-bulk staged path lands, decide whether a separate story should add provider-specific chunk execution instead of continuing to route chunked saves through the existing ordered-bulk or provider-neutral behavior.
- After benchmark evidence is collected, decide whether the current documented SQL Server operation thresholds or satellite batch limit should be tuned in a follow-up performance story.

Risks
- Because live SQL Server execution is opt-in, the staged path can be under-validated in unattended environments unless a configured SQL Server lane runs during CI or release validation.
- The staged path crosses temporary tables and bulk-transfer boundaries while still needing exact latest-state and hash-diff correctness; regressions here would be data-correctness issues, not only performance issues.
- Cleanup behavior under cancellation or mid-batch failure is a concentrated risk area because staging artifacts and caller-owned transaction behavior must remain consistent.

Split recommendations
- No split is recommended from the current evidence; provider-specific ordered-bulk staging, SQL Server-gated tests, and benchmark-lane evidence still fit one bounded story.

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