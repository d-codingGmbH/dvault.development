[gicket-bot] closure-only-ticket-closure-v1

Summary
- Closed closure-only ticket '06EZ0NTJZEMVA5RPR01V0KNVMR' because PO-critic verified that the ticket is already satisfied and no developer or tester execution remains.
- PO-critic closure audit approved that the ticket is satisfied without developer or tester execution.

Evidence
- ticket: `06EZ0NTJZEMVA5RPR01V0KNVMR`
- parentOf child evidence was not required for this closure-only ticket.

PO-critic audit evidence
- `git -C /mnt/c/Projects/DVault diff --name-only develop..ticket/06EZ0NTJZEMVA5RPR01V0KNVMR-task-add-pit-documentation-and-example-scenario` returned only `.gicket/tickets/06EZ0NTJZEMVA5RPR01V0KNVMR/*`, so the branch change is ticket-refinement-only and does not mix in source implementation work.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-28`, `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:17-18,45-73`, and `src/DCoding.Data.DVault/DataVaultSaveService.cs:12-23` directly prove the current public surface `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata(...)`, and `IDataVaultSaveService`.
- `README.md:32-41,61-88,139-156` already documents the same public path and EF shared-type table query pattern that the ticket asks the docs example to reuse.
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md:10-30` fixes the customer profile scenario to `C-100`, `Alice Adams/prospect`, then `Alice Baker/active`.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:753-805,834-853` exercises that exact SQLite customer-profile history and asserts the ordered `SatCustomerProfile` rows.
- `docs/plans/deferred-data-vault-capabilities.md:24-36,58-65` states PIT tables are deferred, points to story `06EZ0NSXY2Y1JZ8SSCX177C770`, and names refresh strategy, temporal grain, persisted-vs-computed shape, and late-arriving data as future decisions.
- `README.md:165-169` and `docs/plans/shared-implementation-standards.md:68` keep `examples/` as future-use, which supports the ticket's scope-out of a new standalone sample app.
- `rg -n "PIT|PointInTime|Point-in-time|PitTable|Pit" /mnt/c/Projects/DVault/src` returned no matches, consistent with the contract's statement that there is no current PIT-specific runtime API surface to document.

PO-critic non-blocking notes
- Branch history shows the PO handoff commit `a93afe63` and current head `ac23b393`; no repository implementation changes are mixed into the refinement handoff.
- `.gicket/relations/70/MR/06EZ0NSXY2Y1JZ8SSCX177C770--06EZ0NTJZEMVA5RPR01V0KNVMR--parentOf.json` preserves the split from this docs task to the broader PIT modeling/generation story.

PO-critic closure watchouts
- Do not introduce PIT-specific metadata, generated tables, refresh jobs, or runtime/query APIs in this ticket.
- Do not place the example in `examples/` or add a standalone sample app; the current layout still treats `examples/` as future-use.
- Keep terminology aligned with the current hub/link/satellite baseline and with the shared-type table naming already shown in `README.md` and the SQLite integration test.
- Keep future-work language explicit so the docs do not accidentally promise provider-specific PIT SQL, indexing, or refresh semantics.