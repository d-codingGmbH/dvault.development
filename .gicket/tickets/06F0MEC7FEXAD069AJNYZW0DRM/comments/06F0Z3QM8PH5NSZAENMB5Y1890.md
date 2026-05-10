[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract is generally well-bounded and aligned with the existing registry-backed save surface, but it leaves same-hub/self-link link mappings ambiguous and does not define who owns the promised pre-orchestration missing-value validation.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- HEAD is `ticket/06F0MEC7FEXAD069AJNYZW0DRM-task-define-typed-hub-link-and-satellite-mapper` at `ae8a12507bd15d3c269ad643d79fcf91292e5fec`; `git diff --stat develop...HEAD` shows only `.gicket/tickets/06F0MEC7FEXAD069AJNYZW0DRM/*` changes, so this branch is ticket-contract-only.
- The persisted contract at `.gicket/tickets/06F0MEC7FEXAD069AJNYZW0DRM/description.md:42-47` requires `IDataVaultHubMapper<TSource>`, `IDataVaultLinkMapper<TSource>`, and `IDataVaultSatelliteMapper<TSource>`, says link mappers key participant hash keys by exact participant hub metadata names, and requires missing-required-value failures before persistence orchestration starts; `description.md:67-68` records `## Open Questions` as `- none`.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:296-302` defines `DataVaultRegistryLinkSaveOperation(string linkName, IEnumerable<KeyValuePair<string,string>> participantHashKeyValues)` and routes those values through `DataVaultHubSaveOperation.RequireValues(...)`; `DataVaultSaveService.cs:540-557` rejects duplicate names, so one operation cannot carry two separate entries for the same participant hub name.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:624-639` shows the current metadata model accepts a same-hub self-link: `EmployeeReportsTo` is constructed from `[employee.ToReference(), employee.ToReference()]` and the model accepts it.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:<redacted>` builds link save plans by reading participant values back by `participant.HubReference.Name` in declaration order, so repeated same-hub participants collapse onto the same dictionary key instead of identifying distinct roles.
- Early reusable validation today is limited to null/duplicate-name checks (`src/DCoding.Data.DVault/DataVaultSaveService.cs:540-557`) and exact multi-active driving-key checks (`DataVaultSaveService.cs:661-684`); missing required hub/link/payload names are enforced later by `GetRequiredValue(...)` at `DataVaultSaveService.cs:<redacted>`.
- The repository already documents the same collision risk in related contract work: `.gicket/tickets/06F0MEA1FF743S14XQW02H4A3W/description.md:48-59` says repeated same-hub participants should be rejected in v1 when participant hash-key names would collide.
- Several positive compatibility assumptions in this ticket are directly supported by source: `DataVaultRegistrySaveRequest` keeps `LoadTimestamp` and `RecordSource` explicit at `src/DCoding.Data.DVault/DataVaultSaveService.cs:155-186`; `DataVaultMetadataReference.Hub(...)` and `.Link(...)` exist at `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:59-75`; link-parent satellites are retained at `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:160-169`; and `DataVaultMetadataRegistry.Create(metadataModel)` does not require CLR mappings at `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs:101-130`.

Blocking findings
- The link-mapper contract is under-specified for same-hub/self-link links. Existing metadata models accept self-links, but the chosen registry-backed link operation shape is keyed only by participant hub metadata name and rejects duplicate keys, so v1 currently cannot represent distinct same-hub participants unless the ticket explicitly rejects that shape or adds a disambiguation mechanism.
- The contract promises missing-required-value failures before persistence orchestration starts, but the current reusable validation surface only catches null/duplicate names early; required-name enforcement happens later inside `DefaultDataVaultSaveService.GetRequiredValue(...)`. With typed helper/orchestration work explicitly scoped out, the ticket does not define what public abstraction owns that earlier validation.

Required PO actions
- Clarify v1 link-mapper support for repeated same-hub/self-link links: either mark them out of scope with an explicit rejection contract and tests, or add a role/ordinal/alias-based participant identity that can represent valid self-links.
- Clarify validation ownership for missing required hub/link/payload values: either relax the acceptance criterion to match the existing save-pipeline boundary, or explicitly scope in a thin registry-aware validating abstraction/factory and define its diagnostics.

Open issues ledger
- critic-item-1 [required-po-action] Clarify v1 link-mapper support for repeated same-hub/self-link links: either mark them out of scope with an explicit rejection contract and tests, or add a role/ordinal/alias-based participant identity that can represent valid self-links.
- critic-item-2 [required-po-action] Clarify validation ownership for missing required hub/link/payload values: either relax the acceptance criterion to match the existing save-pipeline boundary, or explicitly scope in a thin registry-aware validating abstraction/factory and define its diagnostics.
- critic-item-3 [blocking-finding] The link-mapper contract is under-specified for same-hub/self-link links. Existing metadata models accept self-links, but the chosen registry-backed link operation shape is keyed only by participant hub metadata name and rejects duplicate keys, so v1 currently cannot represent distinct same-hub participants unless the ticket explicitly rejects that shape or adds a disambiguation mechanism.
- critic-item-4 [blocking-finding] The contract promises missing-required-value failures before persistence orchestration starts, but the current reusable validation surface only catches null/duplicate names early; required-name enforcement happens later inside `DefaultDataVaultSaveService.GetRequiredValue(...)`. With typed helper/orchestration work explicitly scoped out, the ticket does not define what public abstraction owns that earlier validation.

Missing examples / edge cases
- Add one concrete self-link example such as `EmployeeReportsTo` or `SalesRegionParentChild` showing either the supported mapping shape or the mandated rejection behavior.
- Add one explicit example for null `source` handling on `Map(TSource source)` for reference types and state whether `TSource` is unconstrained, `notnull`, or expected to throw `ArgumentNullException`.
- Add one example that shows where missing business-key, participant, or payload names are detected in the contract-only flow.

Risky assumptions
- Assumes every link participant can be uniquely identified by hub metadata name alone.
- Assumes the promised pre-orchestration missing-value validation can be delivered without expanding into the scoped-out helper/orchestration layer.
- Assumes null-source behavior can be made consistent across reference and value `TSource` without an explicit nullability constraint.

AC / test suggestions
- Add an explicit failing test for repeated same-hub/self-link participant mapping if v1 rejects it, or a passing test that proves the chosen disambiguation survives registry-backed save planning.
- Add a contract test that distinguishes duplicate-name failures from missing-required-value failures so diagnostics are pinned separately.
- Add a representative manual link-parent-satellite mapping flow in addition to the hub-plus-satellite example already called out by the contract.
- Add API snapshot assertions for the nullable annotations of `Map(TSource source)`.

Implementation watchouts
- Do not rely on participant enumeration order alone to identify repeated same-hub link participants; the current registry-backed link operation shape is name-keyed.
- Do not hide `LoadTimestamp` or `RecordSource` inside mapper implementations; the explicit request boundary remains `DataVaultRegistrySaveRequest`.
- If thin supporting abstractions are added for validation, keep them additive and avoid changing `IDataVaultSaveService`, `DataVaultSaveRequest`, `DataVaultRegistrySaveRequest`, or the existing metadata-based APIs.

Non-blocking notes
- Outside the two blockers above, the ticket is well-bounded: `## Open Questions` is `none`, helper/read/source-generation work is explicitly scoped out, and the repository already supports registry-backed outputs, link-parent satellite identity, exact-name registry lookup, and optional CLR mappings.
- The existing split to 06F0MECFNF42NK9PND9DWVW9VW and 06F0MECPFAVBFBNC5XMVDZRQ6M still looks appropriate once this contract is clarified.

Split recommendations
- No new split is needed, but the current ticket should not move to developer handoff until the same-hub link boundary and validation-ownership boundary are made explicit in this contract.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment