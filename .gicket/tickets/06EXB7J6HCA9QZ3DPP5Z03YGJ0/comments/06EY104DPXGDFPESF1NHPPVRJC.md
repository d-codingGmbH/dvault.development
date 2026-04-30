[gicket-bot] PO-critic review contract

Summary
- Current refinement resolves the prior PO-critic gaps: it names a concrete first consumer, bounds Sqlite v1 to explicit none/unsupported function and concurrency capabilities, and anchors type mappings and failure behavior to repository-backed evidence.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7J6HCA9QZ3DPP5Z03YGJ0/description.md` now names `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` as the explicit first consumer, says `ApplyProperty` is the required capability reader, and has `## Open Questions` -> `- none`.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` currently routes property projection through `ApplyProperty`, where `LoadTimestamp` uses `IndexerProperty<DateTimeOffset>` and all other current properties use `IndexerProperty<string>`; `rg -n` reported this at lines `253-259`.
- `docs/architecture/mvp-data-vault-concepts.md` states SQLite examples use ISO 8601 text for load timestamps (`line 68`) and keeps hub/link/satellite business-key, hash, load-timestamp, and record-source fields in the current SQLite-friendly baseline (`lines 26-27, 38, 49-50, 72`).
- `docs/plans/optional-advanced-configuration-hooks.md` defines provider behavior as an optional additive extension and says persisted timestamps should use ISO 8601 UTC text (`lines 36-37`), while `docs/plans/dvault-v1-default-persistence-convention-policy.md` still avoids provider-specific physical schema details (`line 21`).
- Earlier blocking findings were recorded in `.gicket/tickets/06EXB7J6HCA9QZ3DPP5Z03YGJ0/comments/06EY0RV1JCSTVD5ATTX2T13GS0.md`; the latest PO refinement comment `.gicket/tickets/06EXB7J6HCA9QZ3DPP5Z03YGJ0/comments/06EY0XMKGRBC4MTSGKACBFWSVR.md` marks `critic-item-1` through `critic-item-5` as `answered`.
- `git show --stat --summary --format=fuller 7fb6b6621426` shows the latest PO handoff commit updated `.gicket/tickets/06EXB7J6HCA9QZ3DPP5Z03YGJ0/description.md`, `ticket.json`, and added the refinement comment responding to the prior critic block.
- Persisted relation files still match the refined contract context: parent `.gicket/relations/H8/J0/06EXB7HYG17X73GH0K535GYJH8--06EXB7J6HCA9QZ3DPP5Z03YGJ0--parentOf.json`, blocker `.gicket/relations/1R/J0/06EXB7FYXNBPMH8VGQCGP2R41R--06EXB7J6HCA9QZ3DPP5Z03YGJ0--blocks.json`, and downstream blocked tickets `.gicket/relations/J0/2R/06EXB7J6HCA9QZ3DPP5Z03YGJ0--06EXB7JEF55Y007XK28DAD1E2R--blocks.json` plus `.gicket/relations/J0/34/06EXB7J6HCA9QZ3DPP5Z03YGJ0--06EXB817Q8RAXCQH5QQR5RFY34--blocks.json`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The implementation will keep provider-capability selection internal for v1; the current docs permit provider behavior as an additive hook but scope public configuration API design out of this ticket.
- SQLite persistence verification may need careful test-surface choices, because current repository evidence shows a raw SQLite helper in `tests/DCoding.Data.DVault.Tests/Shared/SqliteTestDatabase.cs` but no existing `Microsoft.EntityFrameworkCore.Sqlite` usage.

AC / test suggestions
- Use the required unsupported-capability test to assert that the `NotSupportedException` message includes both the provider profile name and the missing capability identifier.
- Keep translator-path coverage centered on `DataVaultEfMetadataTranslator.ApplyProperty`, not only on standalone profile-object tests.

Implementation watchouts
- Do not leak provider-native terms into the shared metadata surface already guarded by `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs`, which rejects provider tokens in current metadata abstractions.
- Do not widen SQL-function or concurrency scope beyond the explicit `none in v1 / unsupported` baseline; `docs/plans/dvault-v1-default-persistence-convention-policy.md` still defers broader SQL physical-schema and mutable-concurrency behavior.
- Keep the first-consumer work anchored to the existing `ApplyProperty` `DateTimeOffset` vs `string` branch instead of inventing a broader provider matrix.

Non-blocking notes
- The current review was taken on branch `ticket/06EXB7J6HCA9QZ3DPP5Z03YGJ0-task-define-provider-capability-abstraction`; `git rev-parse HEAD` returned `da305442da0db513e84faa83b203bd5cd11699b6`, and `git show --no-patch --format=fuller HEAD` shows this is the PO-critic lease-claim commit over the refined ticket state.

Split recommendations
- No split recommended; the ticket is now bounded to one concrete consumer path, one Sqlite profile, explicit none/unsupported capability baselines, and one negative-capability test surface.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment