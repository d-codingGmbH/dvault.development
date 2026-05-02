[gicket-bot] PO-critic review contract

Summary
- Ticket is sufficiently refined for developer handoff; scope, execution surface, and exclusions are concrete, with only non-blocking suggestions around dataset specificity.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 10 comments, all bot claim/lease/refinement/runtime entries; no human or downstream comments add conflicting scope or unresolved questions.
- `git -C /mnt/c/Projects/DVault log --oneline -n 5 ticket/06EXB7SP77MW1HVW7KT4ZFV6G8-task-implement-normal-ef-baseline-for-order-prod` shows only orchestration commits `0257c0f7`, `<redacted>`, `defd4f77`, and `e8b9e8ff` after `develop`, so there is no competing implementation history to reinterpret the ticket scope.
- `examples/` currently contains only `.gitkeep`, while `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj` targets `net10.0` and references `Microsoft.EntityFrameworkCore.Sqlite` 10.0.0; `DVault.slnx` includes that integration project on the root validation path.
- `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault/DataVaultSaveService.cs` define public `ApplyDataVaultMetadata`, `UseDataVault`, `AddDVault`, and `IDataVaultSaveService`, so the contract's DVault-API exclusions are concrete and locally verifiable.
- `rg` across `/mnt/c/Projects/DVault/src`, `/mnt/c/Projects/DVault/tests`, and `/mnt/c/Projects/DVault/docs` showed current `Order` references only in existing DVault metadata/naming/tests and no `Product` or `OrderLine` matches, confirming this ticket fills an uncovered conventional scenario.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not require a concrete multi-line sample, so a one-line order could technically pass while giving a weak comparison baseline.
- The contract does not explicitly require a reused product across multiple lines or orders, which would better expose relationship shape for later comparison.

Risky assumptions
- Assumes the developer will choose a line payload and seed dataset that the blocked follow-up ticket 06EXB7SY3J6160R9Q35CFN6Q1W can mirror, because the ticket leaves the exact payload field open.
- Assumes a test-only artifact is sufficient for stakeholders because `README.md` and `examples/.gitkeep` still position runnable examples as future work.

AC / test suggestions
- Prefer a deterministic sample with at least two `OrderLine` rows and a concrete payload such as `Quantity` so later DVault and benchmark tickets can mirror exact facts.
- Assert both relationship directions in the proof: order-to-lines and line-to-product, not just row counts.

Implementation watchouts
- Keep the work in `tests/DCoding.Data.DVault.Tests/Integration` and on the `DVault.slnx` path; `examples/` is still placeholder-only.
- Do not use `ApplyDataVaultMetadata`, `UseDataVault`, `AddDVault`, or `IDataVaultSaveService`; those public DVault APIs already exist in `src/DCoding.Data.DVault` and are explicitly out of scope for this conventional baseline.
- Because current repo search shows no existing `Product` or `OrderLine` scenario, the first committed naming and payload choices here will become the comparison precedent for the blocked downstream ticket.

Non-blocking notes
- The persisted contract already satisfies the hard approval gate because `## Open Questions` is `none`.

Split recommendations
- No split recommended; the current contract is already bounded to one conventional EF Core Sqlite baseline in the existing integration-test surface.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment