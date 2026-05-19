[gicket-bot] PO-critic review contract

Summary
- The delivery contract now defines a concrete, additive current/as-of convenience overload layer over the existing latest-satellite APIs, explicitly defers PIT-backed historical ergonomics, and leaves no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGPKXWRFXNPFA1JR0X67XC/description.md:16-48 names the exact new overload family (`ReadCurrentSatelliteRowsAsync`, `ReadCurrentSatelliteAsync`, `ReadAsOfSatelliteRowsAsync`, `ReadAsOfSatelliteAsync`), says it must delegate to existing latest request objects, and explicitly scopes PIT helpers out.
- .gicket/tickets/06F2PGPKXWRFXNPFA1JR0X67XC/description.md:51-52 shows `## Open Questions` = `none`, so the persisted delivery contract has no unresolved open-question gate.
- .gicket/tickets/06F2PGPKXWRFXNPFA1JR0X67XC/comments/06F408WZ42S78T5852MV3G57FW.md:10-15 records the PO answers to the prior critic items, including concrete explicit-metadata and `UseDataVaultMetadata()` caller examples and explicit PIT deferral.
- src/DCoding.Data.DVault/DataVaultLatestSatelliteReadRequest.cs:14-35 already exposes latest-without-`asOf` and as-of-with-`DateTimeOffset?` constructors; lines 53-62 deduplicate parent hash keys with `StringComparer.Ordinal`, and lines 33-35 normalize `AsOf` to UTC.
- src/DCoding.Data.DVault/DataVaultRegistryLatestSatelliteReadRequest.cs:15-40 and src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs:26-45,115-136 already provide registry-backed latest/as-of requests and delegate to the explicit latest pipeline.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs:48-72 already provides the typed `ReadLatestSatelliteAsync(...)` projection helper that the new convenience names can reuse unchanged.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs:13-272 and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:<redacted> already cover explicit latest/as-of, registry-backed latest/as-of, link-parent reads, multi-active series, missing-parent empty results, and missing-metadata-before-query behavior that this ticket says must be preserved.
- README.md:222-249 and docs/releases/v0.7.0.md:53-55 still document `ReadLatestSatelliteAsync(...)` / `ReadLatestSatelliteRowsAsync(...)` as the canonical latest/as-of surface and `ReadPitRowsAsync(...)` / `ReadPitAsync(...)` as a separate PIT surface, matching the ticket's non-breaking/latest-vs-PIT boundary.
- The repository currently has no `ReadCurrentSatellite*` or `ReadAsOfSatellite*` symbols: `rg -n "ReadCurrentSatellite|ReadAsOfSatellite" /mnt/c/Projects/DVault/src /mnt/c/Projects/DVault/tests /mnt/c/Projects/DVault/README.md /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt` returned no matches.
- `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD` lists only `.gicket/tickets/06F2PGPKXWRFXNPFA1JR0X67XC/*` artifacts, so this branch is still ticket-refinement-only and has no implementation changes yet.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developers must keep `latest` as the compatibility baseline while adding `current`/`as-of` convenience names; README.md:222-249 and docs/releases/v0.7.0.md:53-55 still treat latest/as-of as the canonical public vocabulary.
- Release-note follow-through may be completed by downstream documentation ticket 06F2PGPXVAYRBC94RQ7X5V4DVG rather than this story; this story should not reopen broader documentation scope while implementing the convenience layer.

AC / test suggestions
- Validate each new convenience overload against the equivalent existing `DataVaultLatestSatelliteReadRequest` or `DataVaultRegistryLatestSatelliteReadRequest` path so the acceptance criteria's parity claim stays objectively checkable.
- Keep the verification set aligned to the contract's preserved baseline: explicit current, explicit as-of, registry current, registry as-of, link-parent, multi-active, missing-parent, and missing-metadata-before-query cases.

Implementation watchouts
- Keep this as an additive convenience layer over the current latest-satellite APIs; do not rename, remove, or semantically change `DataVaultLatestSatelliteReadRequest`, `DataVaultRegistryLatestSatelliteReadRequest`, `ReadLatestSatelliteRowsAsync(...)`, or `ReadLatestSatelliteAsync(...)`.
- Do not blur this story into PIT work; PIT-backed historical reads remain on `DataVaultPitAsOfReadRequest`, `ReadPitRowsAsync(...)`, and `ReadPitAsync(...)`, and PIT maintenance stays with 06F2PGPBRFT48JG57SV57N9TVW / docs/plans/pit-maintenance-service-v1-contract.md.
- Preserve the existing UTC normalization, ordinal parent-hash-key deduplication, deterministic ordering, missing-parent empty results, hub-parent support, link-parent support, and multi-active driving-key behavior that the current latest pipeline already provides.

Non-blocking notes
- none

Split recommendations
- No new split is needed for developer handoff; keep any future PIT-backed or bridge convenience naming work in a separate follow-up ticket rather than expanding 06F2PGPKXWRFXNPFA1JR0X67XC.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment