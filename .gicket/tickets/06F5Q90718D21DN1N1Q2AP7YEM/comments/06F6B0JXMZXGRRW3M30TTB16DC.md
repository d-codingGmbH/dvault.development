[gicket-bot] PO refinement contract

Summary
- Authoritative ticket contract already reflects the required description update, corrected relation/risk text, and provider-specific v0.20.0 documentation boundary, so the ticket is ready to return to PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract no longer says the ticket is currently relation-blocked by `06F5Q8Z0Y0ADE5H37DAPA1ADQM` or `06F5Q900FC0P3HBZP81CVK7264`; it now records those persisted incoming `blocks` links as historical landed-context references from done stories rather than active blockers unless later evidence reopens them.
- critic-item-2: `answered` - The refinement audit is corrected: this run materialized an authoritative ticket-description update only; it did not create child tickets, relation changes, attachments, or planning documents.
- critic-item-3: `answered` - The v0.20.0 hierarchy is now explicit and provider-specific: `IDataVaultSaveService` remains the public provider-neutral explicit-save boundary, `DataVaultBulkSaveRequest` remains the compatibility baseline, `DataVaultChunkedSaveRequest` remains the provider-neutral bounded streaming path, staged bulk is preferred only where repository evidence already shows staged behavior, SQL Server stays on native-bulk wording, Oracle remains the retained direct optimized exception with `stagedOracleBulk=not-selected-no-measured-win`, and stored procedures stay non-default escape-hatch guidance only.
- critic-item-4: `answered` - The contract now names the intended benchmark-facing deliverables as `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/releases/v0.20.0.md`; both must reuse the root `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet plus the shared artifact contract instead of inventing new evidence files.
- critic-item-5: `answered` - The authoritative contract is now internally consistent about refinement output because it explicitly acknowledges the persisted description update instead of claiming no description edits were materialized.
- critic-item-6: `answered` - The v0.20.0 narrative no longer overgeneralizes staged bulk; it preserves the provider-specific exceptions and keeps Oracle on the retained direct optimized path until measured staged Oracle evidence exists.

Clarifications
- This refinement has already materialized one authoritative ticket-description update only; no child tickets, relation changes, attachments, or planning documents were created.
- v0.19.0 remains the current public baseline and keeps staged provider bulk ingestion outside that release's claim set, so this ticket documents the v0.20.0 boundary rather than reopening v0.19.0.
- The public write baseline remains provider-neutral explicit save through `IDataVaultSaveService`; `DataVaultBulkSaveRequest` stays the compatibility baseline for already-materialized ordered saves, and `DataVaultChunkedSaveRequest` stays the provider-neutral bounded streaming path rather than a provider-native bulk default.
- The v0.20.0 optimized-path narrative is provider-specific: staged bulk is the preferred optimized path only where repository evidence already shows supported or measured staged behavior, SQL Server stays on native-bulk wording, Oracle keeps the retained direct optimized path, and stored procedures remain non-default escape-hatch guidance only.

Scope In
- Update `README.md` and `docs/production-adoption-checklist.md` to present the v0.20.0 write-path hierarchy as provider-neutral explicit save baseline plus provider-specific optimized paths, without presenting stored procedures as the default recommendation.
- Update `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` and `docs/releases/v0.20.0.md` to explain the benchmark-visible provider boundaries, named artifact files, and shared evidence contract for staged, direct, and native-bulk claims.
- Document provider-specific optimized-path wording precisely: staged bulk where the repository already shows supported or measured staged behavior, SQL Server native-bulk wording, and Oracle direct optimized exception until a measured staged Oracle path exists.
- Document stored procedures only as an explicit design-time or provider-specific escape hatch that requires confirmed provider evidence and migration-synchronization guidance.

Scope Out
- Implementing staged bulk ingestion, provider-native chunk execution, or automatic stored-procedure generation behavior in product code.
- Introducing new benchmark artifact schemas, new performance harnesses, or release automation changes.
- Designing generic stored-procedure scaffolding or migration helpers beyond documenting the boundary and caveats.

Open questions
- none

Follow-up questions
- After provider evidence is stable, does the roadmap want a future provider-by-provider decision matrix covering staged bulk, retained direct or multi-row paths, chunked explicit save, and explicit stored-procedure escape hatches?
- If a later Oracle benchmark proves a staged win over the retained direct path, should that change land as a separate Oracle comparison follow-up rather than widening this ticket beyond the current repository-evidenced boundary?

Risks
- The persisted relation graph still carries incoming `blocks` links from done stories `06F5Q8Z0Y0ADE5H37DAPA1ADQM` and `06F5Q900FC0P3HBZP81CVK7264`; treat them as historical rather than active blockers, but reopened implementation or evidence changes would still require documentation wording updates before release.
- Because three downstream tickets are currently blocked by this documentation ticket, ambiguity in the provider-specific write-path hierarchy or stored-procedure caveats will propagate quickly.
- If provider evidence or migration-synchronization rules are incomplete at doc-authoring time, the stored-procedure section can overclaim unsupported automation.
- If v0.20.0 release prose generalizes staged bulk beyond measured or supported provider lanes, adopter guidance can overstate SQL Server or Oracle behavior relative to the current repository evidence.

Split recommendations
- none

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