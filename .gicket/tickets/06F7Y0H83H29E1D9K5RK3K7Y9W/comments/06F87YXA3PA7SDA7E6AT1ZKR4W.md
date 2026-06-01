[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the persisted contract is additive to the current satellite-only generator baseline, bounded by direct runtime/source evidence, and has no unresolved `## Open Questions` items.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs` currently generates satellite helpers only: non-satellite entities are routed through `ReportUnsupportedSupportBundleReadModelShape(...)`; PITs with missing core PIT facts report `DMV1963`, PITs with driving keys report `DMV1967`, and otherwise valid PIT runtime shapes are still skipped with `DMV1969`.
- `src/DCoding.Data.DVault/IDataVaultReadService.cs` already exposes `ReadPitRowsAsync(DbContext, DataVaultPitAsOfReadRequest, CancellationToken)`, and `src/DCoding.Data.DVault/DataVaultPitAsOfReadRequest.cs` already exists and normalizes `AsOf` to UTC while validating `parentHashKeys` through `DataVaultLatestSatelliteReadRequest.RequireParentHashKeys(...)`.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` directly bounds supported runtime PIT shapes to hub-parent ordinary PITs, hub-parent PITs whose multi-active satellites share one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites on one declared link parent.
- `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md` fixes the additive typed PIT helper surface: one authoritative `dvault.support-bundle.v1`, `Read{ProducedName}AsOfAsync(...)`, PIT-column-only projections, compatibility constants, and bounded diagnostics `DMV1960/<redacted>`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No in-ticket support-bundle example is embedded for a supported link-parent PIT with snapshot-reference columns; implementation/tests should include one concrete positive case.
- No in-ticket example explicitly contrasts the accepted shared-driving-key multi-active PIT case with a rejected mismatched driving-key-family case.
- No in-ticket example explicitly identifies a residual valid PIT shape that should still remain `DMV1969` after supported helper emission is added.

Risky assumptions
- The story assumes the authoritative `dvault.support-bundle.v1` export already carries request-bound `readShape.pit` facts for parent identity, `LoadTimestamp`, snapshot-reference columns, deterministic ordering, and canonical driving keys; if not, supported runtime shapes will still collapse to diagnostics.
- The story assumes the existing PIT runtime API surface is semantically sufficient for bounded link-parent helpers even though `IDataVaultReadService.cs` and `DataVaultPitAsOfReadRequest.cs` summaries still say 'hub' parent hash keys.
- The story assumes any valid-but-intentionally-deferred PIT shapes that keep `DMV1969` can be identified during implementation without reopening product scope.

AC / test suggestions
- Add one helper-emission golden-path test for each supported PIT boundary: ordinary hub-parent PIT, canonical-family multi-active hub-parent PIT, and bounded link-parent PIT.
- Add negative tests that keep `DMV1960`, `DMV1961`, `DMV1963`, `DMV1967`, `DMV1968`, and any intentional residual `DMV1969` on distinct PIT inputs after helper emission is added.
- Add a runtime parity test that generated `Read{ProducedName}AsOfAsync(...)` delegates through `DataVaultPitAsOfReadRequest` and projects only PIT columns plus compatibility constants.
- Add a regression proving satellite helper generation remains unchanged when PIT helpers coexist in the same authoritative support bundle.

Implementation watchouts
- `src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs` is satellite-centric today; PIT emission must preserve existing authoritative-support-bundle counting, fingerprint drift handling, and deterministic name-collision behavior while widening only the PIT path.
- Do not expand beyond the runtime boundary in `docs/architecture/dvault-v1-pit-bridge-boundary.md`: no PIT maintenance, no provider-specific SQL, no tuple filters, and no payload joins.
- `src/DCoding.Data.DVault/DataVaultPitAsOfReadRequest.cs` already normalizes `asOf` to UTC and validates deduplicated `parentHashKeys`; generated helpers should reuse that behavior rather than inventing a parallel request surface.
- Link-parent PIT support should follow the architecture boundary even though current XML summaries in `IDataVaultReadService.cs` and `DataVaultPitAsOfReadRequest.cs` still use hub-oriented wording.

Non-blocking notes
- The persisted contract is specific for a pre-development story: scope, non-goals, diagnostics, acceptance criteria, definition of done, and `## Open Questions: none` are all present.
- Comment history shows current branch evidence is PO transactional handoff metadata (`27a75a4d098e` on `ticket/06F7Y0H83H29E1D9K5RK3K7Y9W-story-generate-typed-pit-read-helpers-from-suppo`); under the stated review policy, that is a developer-handoff watchout rather than a PO blocker.
- The downstream docs task `06F7Y0HZKHBHMYX9EYDYFRYXZ0` remains separate and blocked by this implementation story, which matches the intended implementation-before-docs sequence.

Split recommendations
- No split recommended; PIT implementation is already separated from bridge helper story `06F7Y0HJ1ZPY7ND9N8RVS92H4C` and downstream docs task `06F7Y0HZKHBHMYX9EYDYFRYXZ0`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment