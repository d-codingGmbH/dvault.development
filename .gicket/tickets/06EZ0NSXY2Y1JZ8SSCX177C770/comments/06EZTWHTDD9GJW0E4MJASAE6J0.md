[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the delivery contract now explicitly makes `DataVaultPitMetadata` / `DataVaultMetadataModel.Pits` the canonical story surface, scopes the older `PointInTime` API out, and matches the current repository PIT translator and test baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NSXY2Y1JZ8SSCX177C770/description.md now states that the canonical acceptance surface is `DataVaultPitMetadata`, ordered `DataVaultPitSatelliteReferenceMetadata`, and `DataVaultMetadataModel.Pits`; it also explicitly scopes `DataVaultPointInTimeMetadata` / `DataVaultModelBuilder.PointInTime(...)` out of this story and sets `## Open Questions` to `none`.
- .gicket/tickets/06EZ0NSXY2Y1JZ8SSCX177C770/comments/06EZT82Y91F0ZT1PJSPBW2YMX8.md explicitly answers prior critic items 1-5 and requires docs/examples for this story to use `[<Hub>HashKey, LoadTimestamp, <Satellite>LoadTimestamp...]` while treating the older `PitLoadTimestamp` naming as separate and unchanged.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs defines both `DataVaultPointInTimeMetadata` and `DataVaultPitMetadata`; src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs exposes both `PointInTimeTables` and `Pits`; tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt exposes both public surfaces. The refined contract therefore matches the actual dual-API repository state.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs creates PIT entities from `metadataModel.Pits`, uses `LoadTimestamp` plus ordered `<Satellite>LoadTimestamp` snapshot columns, and emits no secondary indexes; tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs asserts `PitCustomerProfileStatus` with `CustomerHashKey`, `LoadTimestamp`, `ProfileLoadTimestamp`, and `StatusLoadTimestamp`.
- tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs still asserts the older `PointInTime` modeling path with `PitLoadTimestamp`, which is consistent with the ticket's explicit scope boundary that this older public surface is not being reconciled here.
- tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs contains the SQLite baseline PIT create/read proof for `PitCustomerProfileStatus`, matching the contract's required integration coverage.
- git show --stat --oneline e8207e2c08e5 shows the PO handoff commit updated the durable ticket contract, and git diff 726f97ce9e53dcccf55133c3100855d7a3dd653e..e8207e2c08e5 -- .gicket/tickets/06EZ0NSXY2Y1JZ8SSCX177C770/description.md shows the exact additions that resolved the earlier canonical-surface ambiguity.
- git rev-parse HEAD returned `ae4ee6545db61011c3e48fb926ade119f39effa6`, matching the prompt's `scratch-source-ref`, and git diff --name-only develop...HEAD -- . ':(exclude).gicket' returned no output, so this review context contains ticket-metadata refinement only and no new non-.gicket code drift.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Keep at least one acceptance/documentation check that the minimal example uses `DataVaultPitMetadata` / `DataVaultMetadataModel.Pits` and explicitly notes that `DataVaultPointInTimeMetadata` / `PointInTime(...)` remain separate and out of scope.
- Use the existing PIT assertions in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` and the SQLite create/read proof in `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` as the ticket-level verification baseline.

Implementation watchouts
- Do not let story docs or examples drift back to `DataVaultModelBuilder.PointInTime(...)`; this contract is intentionally anchored on the translator-facing `Pits` path only.
- Do not mix `LoadTimestamp` and `PitLoadTimestamp` when describing this story's canonical PIT output.
- Keep link-based PITs, link-attached satellites, multi-active satellites, and PIT population/refresh behavior out of this baseline story.

Non-blocking notes
- The current branch has no non-.gicket diff versus `develop`, so this approval is about contract readiness and already-visible PIT behavior rather than new story-branch product code.

Split recommendations
- Keep PIT metadata projection, canonical `DataVaultPitMetadata` examples, and documentation in this story; keep PIT row population or refresh orchestration in a follow-up ticket.
- Keep any future consolidation, deprecation, or formal coexistence cleanup for `DataVaultPointInTimeMetadata` / `PointInTime(...)` versus `DataVaultPitMetadata` / `DataVaultMetadataModel.Pits` as a separate public-surface ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment