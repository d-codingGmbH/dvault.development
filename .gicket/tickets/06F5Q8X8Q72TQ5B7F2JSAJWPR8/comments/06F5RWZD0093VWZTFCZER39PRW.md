[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The ticket now has a concrete additive API target, explicit scope boundaries, direct repository-backed compatibility evidence, and no unresolved `## Open Questions`; the remaining gaps are edge-case examples worth tightening but not PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q8X8Q72TQ5B7F2JSAJWPR8/description.md:18-50` defines scope in/out, acceptance criteria, DoD, and implementation notes for `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, and a new `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...)` overload; `description.md:52-53` says `## Open Questions` is `none`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:13-36` currently exposes only `SaveAsync(DbContext, DataVaultSaveRequest, ...)` and `SaveAsync(DbContext, DataVaultBulkSaveRequest, ...)`; `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:<redacted>` matches that public API baseline and contains no chunked overload yet.
- `docs/architecture/dvault-v1-streaming-explicit-save-contract.md:8-21,25-27,45-66` pins the additive chunked contract, ordered/no-op behavior, caller-owned transaction and cancellation semantics, and the bounded-state/non-goal split that this story is supposed to implement against.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:850,908,948,1006,1059` already names five compatibility scenarios for ordering, cancellation, transaction participation, repeated hub/link reuse, and satellite continuity across chunks.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:<redacted>` shows current chunked coverage is still a private harness that loops `new DataVaultBulkSaveRequest(chunk.Requests)` per chunk, which matches the ticket clarification that production chunked API/types do not yet exist.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:23-33,47-99` confirms provider strategies currently accept flat ordered `IReadOnlyList<DataVaultSaveRequest>` / `ResolvedRequests`, matching the ticket note that this story should stay provider-neutral and avoid provider-package API churn.
- `git diff --name-only develop..HEAD` listed only `.gicket/tickets/06F5Q8X8Q72TQ5B7F2JSAJWPR8/...` metadata/comment files plus one related closure-evidence amendment; no `src/`, `tests/`, or `docs/` product files changed. `git log --oneline --max-count=8` showed PO/po-critic workflow commits (`f77948eca`, `99366d449`, `e98e21744`), so this is still a pre-development ticket-quality handoff, not an implementation branch review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The ticket text requires empty chunk sequence / empty chunk no-op behavior (`description.md:34`), but there is no named real-API compatibility scenario for that case yet; adding one would reduce interpretation drift during dev/test handoff.
- The contract requires cross-chunk satellite continuity by parent/driving-key identity (`description.md:35` and `description.md:50`), but the named chunked compatibility scenarios do not explicitly call out a multi-active satellite example; current direct repo evidence for canonical driving-key behavior is only the non-chunked SQLite test at `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:1780`.

Risky assumptions
- The story assumes the provider-neutral implementation can preserve cross-chunk continuity while leaving the flat provider-strategy batch contract intact; that assumption is consistent with `description.md:49` and `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:23-33,47-99`, but it is still the key execution risk.
- The story assumes retained latest-state growth is acceptable for this handoff as long as user-visible remediation stays in follow-up ticket `06F5Q8XPXEQPJTKGJ7BQGCY438`; `description.md:56-63` and `docs/architecture/dvault-v1-streaming-explicit-save-contract.md:64-68` support that split, but developers will still need to avoid silently unbounded memory behavior.

AC / test suggestions
- Add one explicit acceptance-test bullet for the production chunked API proving empty chunk sequence and empty chunk no-op behavior, since that rule is already contractual but not named in the current five-scenario SQLite list.
- Add one explicit production-API scenario for multi-active satellite continuity across chunk boundaries using canonical driving-key ordering, mirroring the existing non-chunked coverage already present in `ExplicitDataVaultSaveServiceSqliteTests.cs`.
- If the team wants a sixth non-blocking compatibility proof, call out resolver-hook propagation across chunks so `IDataVaultLoadTimestampResolver` / `IDataVaultRecordSourceResolver` behavior is explicitly preserved on the new boundary, not only inferred from existing single/bulk tests.

Implementation watchouts
- Do not accept an implementation that merely externalizes the current private helper pattern from `ExplicitDataVaultSaveServiceSqliteTests.cs:<redacted>`; the ticket explicitly requires one production chunked service boundary that owns cross-chunk continuity and cancellation semantics.
- Do not let this story force provider-package API churn; the current flat ordered `DataVaultProviderSaveStrategyContext.Requests` / `ResolvedRequests` contract in `src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs:47-99` is part of the compatibility baseline.
- Do not fold fallback/remediation guidance or benchmark evidence into this ticket; those are already split to `06F5Q8XPXEQPJTKGJ7BQGCY438` and `06F5Q8XXSBGW1B8RDRMGVF557W`.

Non-blocking notes
- ` .gicket/tickets/06F5Q8X8Q72TQ5B7F2JSAJWPR8/ticket.json:7-19` shows the ticket is still `todo`, unassigned, and not blocked, with `critic-needed` present; that is consistent with a pre-dev gate awaiting this review.
- The delivery contract is already explicit that the authoritative content is the refined contract block, not the legacy draft (`description.md:70-79`), which reduces ambiguity for developer handoff.

Split recommendations
- No further split recommended. The current graph already separates the landed contract ticket `06F5Q8X261DQHG7N1445NGXB5W`, this execution story, fallback/remediation ticket `06F5Q8XPXEQPJTKGJ7BQGCY438`, and benchmark ticket `06F5Q8XXSBGW1B8RDRMGVF557W`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment