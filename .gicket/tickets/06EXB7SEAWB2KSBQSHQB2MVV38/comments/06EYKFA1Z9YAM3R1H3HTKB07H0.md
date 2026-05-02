[gicket-bot] PO-critic review contract

Summary
- Return to PO: the story is mostly well-bounded, but the parent acceptance criteria and definition-of-done overclaim that the existing two-task split already covers explicit HubOrder/HubProduct schema proof.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7SEAWB2KSBQSHQB2MVV38/description.md` has `## Open Questions` = `none`, so approval is not blocked by open questions.
- Child comments `.gicket/tickets/06EXB7SP77MW1HVW7KT4ZFV6G8/comments/06EYJGG6KKP26134V656DWCJ6G.md` and `.gicket/tickets/06EXB7SY3J6160R9Q35CFN6Q1W/comments/06EYKCKPKPJE0Y6J7FWY4MG8H0.md` report tester verification of all acceptance criteria and handoff to integrator; `.gicket/tickets/06EXB7SP77MW1HVW7KT4ZFV6G8/comments/06EYJGZ34B044EBCXGBH4E310W.md` and `.gicket/tickets/06EXB7SY3J6160R9Q35CFN6Q1W/comments/06EYKCNC4TDACTFZ6QQ41JP4R4.md` show integrator `ACCEPT` decisions.
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:11-105` contains the conventional SQLite Order/Product/OrderLine scenario, and `:110-298` contains the DVault Order/Product/OrderProduct/Fulfillment scenario using `services.AddDVault()` and `IDataVaultSaveService`.
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:280-298` explicitly asserts table names plus full `AssertTable(...)` checks for `LinkOrderProduct` and `SatOrderProductFulfillment`, but there is no equivalent `AssertTable(...)` call for `HubOrder` or `HubProduct`.
- `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:240-258` reads `HubOrder` and `HubProduct` rows and checks business-key/hash-key values, but it does not explicitly assert hub `LoadTimestamp`/`RecordSource` technical metadata columns.
- `docs/architecture/mvp-data-vault-concepts.md:25-28` says each hub row stores a hash key, business key values, `LoadTimestamp`, and `RecordSource`.
- The public API surfaces named in scope are directly present in source: `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:29-38` exposes `ApplyDataVaultMetadata`, `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-25` exposes `AddDVault`, and `src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21` exposes `IDataVaultSaveService`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:529-558` suppresses a satellite insert when the latest hash diff matches, and `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs:206-226` asserts the unchanged replay writes `0` rows.
- `git diff --name-only 804d0036..9b92ac3a` lists only `.gicket/...` ticket metadata files, so the current story branch adds no implementation files beyond the already integrated child work.

Blocking findings
- The parent story acceptance criterion requires schema/table assertions for `HubOrder`, `HubProduct`, `LinkOrderProduct`, and `SatOrderProductFulfillment`, including technical metadata columns, but direct repository evidence only shows full schema assertions for the link and satellite tables. The story definition-of-done still claims both child tasks already satisfy the story-level criteria without reopening the split.
- The DVault child task contract (`.gicket/tickets/06EXB7SY3J6160R9Q35CFN6Q1W/description.md`) scopes visible schema proof to the relationship link and its satellite, not explicit HubOrder/HubProduct technical-metadata assertions, so the parent story currently assigns more than the child split clearly owns.

Required PO actions
- Clarify whether the story truly requires explicit HubOrder/HubProduct schema or technical-metadata assertions, or whether the current hub table-name and row-shape evidence is sufficient.
- If explicit hub-schema proof is required, update ownership so that remainder is explicitly assigned or the split is reopened instead of stating that the existing two-task split already fully satisfies the story-level acceptance criteria.

Open issues ledger
- critic-item-1 [required-po-action] Clarify whether the story truly requires explicit HubOrder/HubProduct schema or technical-metadata assertions, or whether the current hub table-name and row-shape evidence is sufficient.
- critic-item-2 [required-po-action] If explicit hub-schema proof is required, update ownership so that remainder is explicitly assigned or the split is reopened instead of stating that the existing two-task split already fully satisfies the story-level acceptance criteria.
- critic-item-3 [blocking-finding] The parent story acceptance criterion requires schema/table assertions for `HubOrder`, `HubProduct`, `LinkOrderProduct`, and `SatOrderProductFulfillment`, including technical metadata columns, but direct repository evidence only shows full schema assertions for the link and satellite tables. The story definition-of-done still claims both child tasks already satisfy the story-level criteria without reopening the split.
- critic-item-4 [blocking-finding] The DVault child task contract (`.gicket/tickets/06EXB7SY3J6160R9Q35CFN6Q1W/description.md`) scopes visible schema proof to the relationship link and its satellite, not explicit HubOrder/HubProduct technical-metadata assertions, so the parent story currently assigns more than the child split clearly owns.

Missing examples / edge cases
- The contract does not clearly say what counts as acceptable hub-table evidence: full schema assertions, queried technical-column values, or simple table existence.
- Hub-level technical metadata visibility (`LoadTimestamp` and `RecordSource` on `HubOrder` and `HubProduct`) is the only repository-backed example gap relative to the parent story wording.

Risky assumptions
- The current contract assumes the hub row checks in `NormalEfOrderProductSqliteTests.cs` are enough to satisfy the parent story's stronger `schema or table assertions` wording for HubOrder/HubProduct.
- The contract assumes the persisted `blocks` relation from done story `06EXB7G6YE4X0GA0CT7EPEFMPR` into this story is operationally harmless for later workflow steps.

AC / test suggestions
- State the exact acceptable evidence form for HubOrder/HubProduct in the parent story: full `AssertTable(...)`-style schema proof versus table-name existence plus queried technical columns.
- Mirror that clarified evidence requirement into the owning child ticket if the parent story is meant to remain fully satisfied by the existing split.

Implementation watchouts
- Any remaining scope should stay on the established integration-test surface `tests/DCoding.Data.DVault.Tests/Integration/NormalEfOrderProductSqliteTests.cs` and the root `DVault.slnx` path, not `examples/`.
- Any clarified remainder must preserve the already verified public API boundary around `ApplyDataVaultMetadata`, `AddDVault`, and `IDataVaultSaveService`.

Non-blocking notes
- The ticket is otherwise well-bounded: `examples/` still contains only `examples/.gitkeep`, while `tests/DCoding.Data.DVault.Tests/Integration/DCoding.Data.DVault.Tests.Integration.csproj:15-18` confirms the existing SQLite integration-test surface.
- Main ticket comments are all bot/runtime comments; no human comment history introduced extra business-scope changes beyond the persisted delivery contract.

Split recommendations
- Keep the current two-task split only if the parent story acceptance criteria are aligned down to what those child tickets already own and verify.
- If explicit HubOrder/HubProduct schema proof remains a story requirement, assign that remainder explicitly instead of leaving it implied while claiming no split reopen is needed.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment