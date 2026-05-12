[gicket-bot] PO-critic review contract

Summary
- Persisted contract, child-ticket state, and repository evidence are aligned; this is now an umbrella/completion story over already-delivered PIT and bridge read helpers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/description.md:32-55 defines bounded PIT/bridge acceptance criteria and shows `## Open Questions` = `none`.
- `git diff --name-only develop...ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers` returned only `.gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/**`, while `git log --oneline -- src/DCoding.Data.DVault/IDataVaultReadService.cs src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` shows `0db0450fa [06F0MEH660Y5QTNR5P8JPS2QXC] AUTO-INTEGRATION squash into develop` and `95cbdef44 [06F0MEHKYTBJEJH2DVZ2CFH9Z0] AUTO-INTEGRATION squash into develop`.
- src/DCoding.Data.DVault/IDataVaultReadService.cs:21-31 exposes `ReadPitRowsAsync`; src/DCoding.Data.DVault/DataVaultReadServicePitExtensions.cs:19-40 exposes `ReadPitAsync`; src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs:17-64 exposes `ReadBridgeRowsAsync` and `ReadBridgeAsync`.
- src/DCoding.Data.DVault/DataVaultPitAsOfReadRequest.cs:15-24 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitReadServiceTests.cs:9-33 confirm PIT request UTC normalization, ordinal dedupe, and null/empty/whitespace parent-key rejection.
- src/DCoding.Data.DVault/DataVaultBridgeReadRequest.cs:97-140 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultBridgeReadServiceTests.cs:9-42 confirm many-to-many requests use only `From`/`To`, reject `maximumDepth`, and hierarchy requests require non-negative bounded `maximumDepth`.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs:103-163 verifies visible PIT rows only, no placeholder rows before the first visible PIT row, and typed PIT projection behavior.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs:29-133 and 234-279 verify deterministic endpoint-column order, `TraversalDepth` only for hierarchy rows, and bounded hierarchy direction/depth behavior.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:34-50,270-275,400-405,647-649 contains the public API snapshot entries for bridge/PIT request and read-service surfaces.
- docs/releases/v0.6.0.md:43-49 still says PIT-backed reads and bridge helpers are not delivered, but .gicket/relations/XC/7W/06F0MEH660Y5QTNR5P8JPS2QXC--06F0MEJPGG7JBFEXD693BHY07W--blocks.json, .gicket/relations/Z0/7W/06F0MEHKYTBJEJH2DVZ2CFH9Z0--06F0MEJPGG7JBFEXD693BHY07W--blocks.json, .gicket/relations/XC/VR/06F0MEH660Y5QTNR5P8JPS2QXC--06F0MEJ0NE80R7CNS982S3PKVR--blocks.json, and .gicket/relations/Z0/VR/06F0MEHKYTBJEJH2DVZ2CFH9Z0--06F0MEJ0NE80R7CNS982S3PKVR--blocks.json plus the downstream ticket files show docs and benchmark follow-up is already tracked outside this parent story.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Consumer-facing README/release-note examples still do not show the required hierarchy `maximumDepth` behavior; that remains downstream docs work.
- No consumer-facing example yet demonstrates the explicit read-only boundary that PIT/bridge helpers do not populate maintenance tables.

Risky assumptions
- Approval assumes the downstream dev workflow can accept a completion/consistency umbrella story even though `git diff develop...ticket/06F0MEGPPETJD4ZDEN5ESGR7JW-story-add-pit-and-bridge-read-query-helpers` shows no source/test delta on the parent branch.
- Approval assumes the stale v0.6.0 limitation text is acceptable temporarily because dedicated downstream docs and benchmark tickets already exist.

AC / test suggestions
- When ticket 06F0MEJPGG7JBFEXD693BHY07W resumes, add a consumer-facing acceptance/example note that hierarchy reads require explicit non-negative `maximumDepth` and that many-to-many reads reject it.
- Keep PIT/bridge contract snapshots and SQLite integration coverage tied to any future expansion so the no-placeholder PIT behavior and exact bridge-column-name projection rules stay stable.

Implementation watchouts
- Do not reopen parent-story implementation scope on this branch; the observed parent-branch diff is ticket metadata only and the PIT/bridge feature commits are already on develop.
- Treat remaining work as downstream docs/release and benchmark follow-up, not PIT refresh, bridge maintenance, provider-specific read optimization, or unbounded hierarchy traversal.

Non-blocking notes
- All inspected comments under .gicket/tickets/06F0MEGPPETJD4ZDEN5ESGR7JW/comments were bot-authored workflow/refinement artifacts; no human scope-change comment was observed.
- docs/releases/v0.6.0.md remains a pre-delivery limitation snapshot, but the parent contract explicitly classifies that as downstream documentation consistency work.

Split recommendations
- No further split recommended; the four child tickets are done and the remaining docs/benchmark work already exists as separate downstream tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment