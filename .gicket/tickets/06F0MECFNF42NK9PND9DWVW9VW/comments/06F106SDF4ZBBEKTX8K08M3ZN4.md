[gicket-bot] PO-critic review contract

Summary
- Refined contract is source-backed, bounded to the thin save-helper layer, and ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MECFNF42NK9PND9DWVW9VW/description.md:7-9 records PO handoff decision `ready_for_po_critic`, and :58-59 records `## Open Questions` as `- none`.
- .gicket/tickets/06F0MECFNF42NK9PND9DWVW9VW/comments/06F101MC2E14GM2J6KGFXV687W.md records the persisted PO refinement comment, including that save helpers stay on this ticket while sibling task `06F0MECPFAVBFBNC5XMVDZRQ6M` retains typed read projections.
- `git log --oneline --decorate -n 12` on the target branch shows commit `5a44a70e3 [06F0MEC7FEXAD069AJNYZW0DRM] AUTO-INTEGRATION squash into develop` in branch history before this ticket's PO handoff commit `39314ffed`, so the mapper-contract prerequisite is already on the branch baseline.
- src/DCoding.Data.DVault/IDataVaultLinkMapper.cs:8-13 directly documents the current typed-link boundary: only unique participant hub metadata names are supported; same-hub and self-link typed mappings remain unsupported in v1.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:39-110 already exposes registry-backed `IDataVaultSaveService` extension overloads for `DataVaultRegistrySaveRequest` and `DataVaultRegistryBulkSaveRequest`, and :147-244 defines those request types including caller-ordered bulk requests.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs:13-69 shows the existing SQLite baseline where manual typed mappers already feed `DataVaultRegistrySaveRequest` through `IDataVaultSaveService` with explicit `loadTimestamp` and `recordSource`.
- tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:14-24 proves `AddDVault()` registers no `ISaveChangesInterceptor`, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:60-138 proves the current fallback and SQLite strategy-dispatch behavior that helper regressions are expected to preserve.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give a concrete bulk-failure example where a second-or-later source fails and the surfaced diagnostic must include the zero-based batch index.
- The contract bounds link helpers correctly, but it does not include a concrete positive example of a valid unique-participant link helper call; this is non-blocking because the source baseline already defines the boundary.

Risky assumptions
- `prepared source batches` will be interpreted as caller-ordered source sequences mapped one row at a time, not as composite graph saves; that is implied by Scope Out and the thin-helper notes rather than by an explicit signature.
- Helper diagnostics can wrap mapper or request-assembly failures while preserving the existing inner validation reason without fixing one exact outer exception type; the contract requires preserved reason plus stable context, not a specific wrapper type.

AC / test suggestions
- Add one explicit regression that uses the helper layer for an ordered bulk save and asserts preserved `DataVaultSaveResult` ordering as well as caller batch order.
- Add one explicit negative regression for a link helper failure that asserts the surfaced message includes the logical link target, source CLR type, and zero-based batch index for bulk paths.

Implementation watchouts
- Keep the surface additive on `IDataVaultSaveService` and reuse the existing registry-backed `SaveAsync` overloads; do not widen this ticket into a new orchestration service.
- Do not broaden the ticket into multi-active or link-parent satellite convenience, same-hub/self-link link support, hidden hash-key derivation, or SaveChanges interception; those boundaries are already explicit in the persisted contract.

Non-blocking notes
- The live `blocks` relation to quickstart ticket `06F0MEDBFZ25YA1M7RJ71Z7ZCM` is already persisted in `.gicket/relations/VW/CM/06F0MECFNF42NK9PND9DWVW9VW--06F0MEDBFZ25YA1M7RJ71Z7ZCM--blocks.json`; dependency hygiene does not require another split.

Split recommendations
- No additional split recommended; future composite hub-plus-satellite convenience, multi-active/link-parent helper coverage, and same-hub/self-link typed-link support should remain separate follow-up tickets as already recorded in the contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment