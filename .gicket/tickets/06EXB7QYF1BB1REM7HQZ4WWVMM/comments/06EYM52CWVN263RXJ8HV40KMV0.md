[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git log --oneline --grep '06EXB7R6MTJW1PYRN172MW34DM|06EXB7REMY41DF7RE8J3N1RZYC' --all -- README.md src/DCoding.Data.DVault/DCoding.Data.DVault.csproj tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` shows develop commits `4a12d873` and `f0c99064` for the two child tickets.
- `git show --stat --oneline 4a12d873 -- README.md` reports `README.md | 112 +...`; `git show --stat --oneline f0c99064 -- README.md` reports `README.md | 16 +...`, so the README work already landed on `develop` through the child tickets.
- `git diff --stat develop..ticket/06EXB7QYF1BB1REM7HQZ4WWVMM-story-write-getting-started-documentation` shows only `.gicket/tickets/...` comment/event/description/ticket changes and no diffs for `README.md`, `src/DCoding.Data.DVault/*`, or `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.
- `README.md` already contains the source `ProjectReference`, future NuGet wording, `AddDVault()`, `ApplyDataVaultMetadata(...)`, `IDataVaultSaveService`, `DataVaultSaveRequest`, and a `Dictionary<string, object>` query example.
- `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` targets `net10.0` and packs `../../README.md` as the package readme.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`, and `src/DCoding.Data.DVault/DataVaultSaveService.cs` directly expose `AddDVault`, `ApplyDataVaultMetadata`, `IDataVaultSaveService`, and `DataVaultSaveRequest`; `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` mirrors the Customer/Order/CustomerOrder flow named in the contract.
- Comment file `.gicket/tickets/06EXB7QYF1BB1REM7HQZ4WWVMM/comments/06EYM3SEEGVJ2RSH072VT3MVXC.md` records the relation structure `parentOf` child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC, plus blocked story 06EXB8202A88KJJP7WEGBESBYM.

Blocking findings
- none

Required PO actions
- Decide whether 06EXB7QYF1BB1REM7HQZ4WWVMM should now be treated as an umbrella/aggregation story to advance or close from PO instead of handing it to dev.
- If the parent story is still expected to go to dev, add explicit parent-only remaining work that is not already covered by completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC.
- Align the parent ticket's status/labels with the actual post-child-completion workflow state.

Open issues ledger
- critic-item-1 [required-po-action] Decide whether 06EXB7QYF1BB1REM7HQZ4WWVMM should now be treated as an umbrella/aggregation story to advance or close from PO instead of handing it to dev.
- critic-item-2 [required-po-action] If the parent story is still expected to go to dev, add explicit parent-only remaining work that is not already covered by completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC.
- critic-item-3 [required-po-action] Align the parent ticket's status/labels with the actual post-child-completion workflow state.

Missing examples / edge cases
- none

Risky assumptions
- Assuming there is no separate parent-level closure rule outside the persisted ticket/comment data.

AC / test suggestions
- If any parent-level dev work still exists, add an acceptance criterion that names the remaining delta beyond the two completed child tasks; otherwise the current contract already maps to completed child deliverables.

Implementation watchouts
- Do not reopen README implementation on the parent ticket unless PO identifies a new parent-only delta; current repository evidence shows the documented API flow is already present.

Non-blocking notes
- The persisted contract is otherwise well-formed: `## Open Questions` is `none`, the API references are directly backed by source files, and the README-first/package-README rationale is supported by `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`.
- The release/publication follow-up is already separated via blocked story 06EXB8202A88KJJP7WEGBESBYM.

Split recommendations
- No further split is needed; the existing split into 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC is already the implemented decomposition.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment