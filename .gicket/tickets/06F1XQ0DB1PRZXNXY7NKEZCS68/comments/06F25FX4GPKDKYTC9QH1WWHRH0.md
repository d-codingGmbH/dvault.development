[gicket-bot] PO-critic review contract

Summary
- Ticket 06F1XQ0DB1PRZXNXY7NKEZCS68 is ready for developer handoff; the persisted contract has no open questions and matches the current provider-neutral bulk/strategy baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XQ0DB1PRZXNXY7NKEZCS68/description.md contains the authoritative Delivery Contract and ## Open Questions lists only 'none'.
- Comment 06F25BP1BHVF45HTB7A9QSJWHM.md records the PO refinement contract; comments 06F25BPRVTXE3KXH5P7AV2PQMW.md and 06F25BQSYZVWNB29MWD30KAMAG.md record handoff to po-critic on branch ticket/06F1XQ0DB1PRZXNXY7NKEZCS68-task-define-bulk-strategy-contract-and-fallback at e65487124455.
- src/DCoding.Data.DVault/DataVaultSaveService.cs lines 12-35 expose IDataVaultSaveService single-save and DataVaultBulkSaveRequest overloads; lines 479-496 define ordered DataVaultBulkSaveRequest.Requests.
- src/DCoding.Data.DVault/DataVaultProviderSaveStrategy.cs lines 10-33 expose IDataVaultProviderSaveStrategy with Priority, CanSave(DbContext, IReadOnlyList<DataVaultSaveRequest>), and SaveAsync(context); lines 68-109 show context carries DbContext, Requests, ResolvedRequests, StableHashService, and StableHashNormalizer.
- src/DCoding.Data.DVault/DataVaultSaveService.cs lines 834-876 show dispatcher ordering by descending Priority, calling CanSave, and passing DataVaultProviderSaveStrategyContext; lines 879-910 show fallback writer behavior after no strategy accepts.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs covers no-strategy fallback (lines 59-84), unsupported/unknown strategy fallback (231-256), priority selection (286-330), and equal-priority DI order (332-367).
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs lines 153-178 verify ordered bulk request diagnostics, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs lines 533-635 verify provider-neutral bulk fallback RowsWritten/hash-diff behavior.
- docs/architecture/dvault-v1-explicit-save-service.md lines 31-35 document the provider-neutral dispatcher, priority/tie rule, ordered batch context, and fallback semantics; README.md lines 355-407 document request-bound diagnostics and provider package strategy posture.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The handoff assumes developers will extend or verify the existing public save-strategy surface rather than introduce a parallel bulk strategy API; direct source evidence shows that surface already exists.
- The contract relies on request-bound diagnostics rather than provider-specific implementations for this task; source and docs support that boundary, but implementation should keep assertions deterministic.

AC / test suggestions
- Keep the existing acceptance matrix explicit in dev verification: no registered strategy, registered but declining strategy, selected compatible strategy, descending priority, equal-priority registration order, ordered bulk preservation, RowsWritten, and diagnostics status.
- Use fake/test strategies for selection behavior and keep provider-specific SQL out of scope, consistent with the persisted Scope Out.

Implementation watchouts
- Do not add provider package dependencies to src/DCoding.Data.DVault.
- Preserve the explicit IDataVaultSaveService boundary and request ordering for hub/link saved records, satellite results, and RowsWritten.
- Do not treat validation-only diagnostics as proof of save-strategy selection; README and ticket both scope strategy diagnostics to supplied save requests.

Non-blocking notes
- The branch currently contains ticket metadata/refinement changes only relative to develop; that is appropriate for PO handoff.
- git status reported unrelated local modifications in .gicket-bot/.gitignore, .gicket/.gitignore, .gicket/project.json, and .gicket/types.json; they were not needed for this ticket-level assessment.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment