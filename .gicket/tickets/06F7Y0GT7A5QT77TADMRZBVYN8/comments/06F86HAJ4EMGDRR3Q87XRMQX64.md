[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract closes open questions, matches the current satellite-only generator baseline, and is grounded in existing PIT/bridge runtime and support-bundle evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F7Y0GT7A5QT77TADMRZBVYN8/description.md` contains the authoritative Delivery Contract, and its `## Open Questions` section is `none`.
- `docs/releases/v0.22.0.md` and `src/DCoding.Data.DVault.Analyzers/README.md` both state the current generated-helper baseline is support-bundle-driven and satellite-only; PIT and bridge helpers are explicitly not emitted in v0.22.0.
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs` covers the current satellite-only generator boundary and asserts PIT/bridge shapes are skipped or diagnosed instead of generating helpers (`DMV1963`, `DMV1964`, `DMV1969`).
- `docs/architecture/dvault-v1-pit-bridge-boundary.md`, `src/DCoding.Data.DVault/DataVaultBridgeTraversalEndpoint.cs`, and `src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs` enumerate the supported runtime PIT/bridge shapes, the closed bridge endpoint vocabulary (`From`, `To`, `Ancestor`, `Descendant`), and the required bounded hierarchy `maximumDepth` rule.
- `src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs` and `src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs` expose existing provider-neutral PIT and bridge helper surfaces over `IDataVaultReadService`, which supports the ticket's additive helper framing.
- `src/DCoding.Data.DVault/DataVaultPitProjectionRow.cs`, `src/DCoding.Data.DVault/DataVaultBridgeProjectionRow.cs`, and `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs` provide direct source evidence for the technical/member vocabulary the ticket references (`ParentHashKey`, `LoadTimestamp`, `TraversalDepth`, `SnapshotReference`, `BridgeDepth`).
- `git diff --name-status develop...HEAD` shows only `.gicket/tickets/06F7Y0GT7A5QT77TADMRZBVYN8/` metadata files changed on this branch, which matches a pre-development contract-definition story rather than missing implementation work.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A worked example of generated bridge helper names for each endpoint family (`From`/`To`, `Ancestor`/`Descendant`) is still absent; not a blocker, but approval tests should lock the chosen names immediately.
- A mixed support-bundle example showing valid satellite helpers still emit while one PIT or bridge entity is skipped on per-entity diagnostics would make acceptance-test scoping clearer.

Risky assumptions
- Implementation must assume the authoritative `dvault.support-bundle.v1` actually includes request-bound `readShape.pit` and `readShape.bridge` facts for the reviewed entities; the v2 explain contract makes `readShape` request-bound rather than universally present.

AC / test suggestions
- Add approval tests that prove PIT/bridge helper generation is additive to the v0.22.0 satellite-only baseline and does not regress existing satellite helper emission.
- Add per-entity diagnostic tests for missing PIT/bridge read-shape evidence, stale fingerprint, link-parent multi-active PIT rejection, and hierarchy bridge cases without bounded `maximumDepth` evidence.
- Add public API snapshot coverage for generated record constants and direction-specific bridge helper names so naming cannot drift once implementation starts.

Implementation watchouts
- Bridge helpers should ride the existing `IDataVaultReadService` bridge extension surface in `src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs`; do not widen the public read-service contract just to generate helpers.
- PIT generated read models must stay PIT-table-only as the contract says; current runtime PIT projection helpers can materialize satellite payload segments, so generator implementation must avoid reintroducing payload joins or latest-satellite fallback semantics.
- Keep generator inputs bounded to reviewed support-bundle metadata plus fingerprint gating; `docs/releases/v0.22.0.md` and `src/DCoding.Data.DVault.Analyzers/README.md` explicitly exclude raw `dvault.model.v1` and source-visible metadata from the generator boundary.

Non-blocking notes
- The clarification text in `.gicket/tickets/06F7Y0GT7A5QT77TADMRZBVYN8/description.md` says no description update was applied during refinement, but `git diff --name-status develop...HEAD` shows `description.md` changed and comment `06F86F6VK3RETN1W4G0YQ510ER.md` reports the durable refinement contract was updated. This is editorial, not a dev-handoff blocker.
- The branch is still pre-development: `git diff --name-status develop...HEAD` shows only ticket metadata files under `.gicket/tickets/06F7Y0GT7A5QT77TADMRZBVYN8/` changed.

Split recommendations
- No additional PO split is needed on this contract ticket. Downstream implementation already exists as separate PIT and bridge stories (`06F7Y0H83H29E1D9K5RK3K7Y9W` and `06F7Y0HJ1ZPY7ND9N8RVS92H4C`), which matches the contract's own split guidance.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment