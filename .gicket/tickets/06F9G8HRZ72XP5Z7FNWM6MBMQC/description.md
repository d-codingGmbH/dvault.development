<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the v0.34.0 DB2 documentation task into a bounded docs-only slice: repository evidence already fixes the DB2 package id, the net8.0/net10.0 target line, the IBM.EntityFrameworkCore versions 8.0.0.400 and 10.0.0.100, and the consumer package lines 8.34.0 and 10.34.0. The remaining work is to align the README-adjacent documentation surfaces and add the v0.34.0 DB2 release-note baseline. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Use the existing DB2 provider package `DCoding.Data.DVault.Db2` as the authoritative package id for consumer documentation.
- Use the current package-version lines already present in README: `8.34.0` for `net8.0` and EF Core 8, and `10.34.0` for `net10.0` and EF Core 10. Do not document a consumer-facing `0.34.0` package version.
- Use `src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj` as the authoritative source for IBM.EntityFrameworkCore dependency versions: `8.0.0.400` on `net8.0` and `10.0.0.100` on `net10.0`.
- Keep external DB2 setup explicitly opt-in and developer-managed; the documentation must not imply that default builds or tests require a DB2 instance, Docker, or Podman.
- No bounded ticket writes were applied during refinement; existing relations, attachments, and ticket description were left unchanged.

### Scope In
- README installation and provider-setup guidance updates needed to make DB2 documentation consistent with the current 8.34.0 and 10.34.0 package-line baseline.
- Provider compatibility/support documentation updates for supported DVault-on-DB2 behavior, caveats, and explicit non-goals.
- External DB2 test/setup instructions that explain the opt-in container/Podman assumptions and any required environment or configuration markers.
- Production-adoption checklist updates so it no longer remains on the v0.33.0 / 8.33.0 / 10.33.0 baseline where DB2 support is being introduced.
- A v0.34.0 release-note surface that records the DB2 documentation baseline, package outputs, caveats, and manual-publication separation.

### Scope Out
- Any runtime or provider code changes, new DB2 capabilities, or new EF Core behavior.
- NuGet publication, package hashes, or final publication-link recording.
- New CI automation, default DB2 test infrastructure, or mandatory local or CI DB2 dependencies.
- Broader provider-matrix rewrites beyond the DB2 documentation changes required for v0.34.0.

## Acceptance Criteria
- README-facing installation and provider-setup guidance documents the DB2 provider package alongside the existing package family, uses the `8.34.0` and `10.34.0` consumer package lines, and names the IBM.EntityFrameworkCore dependency versions `8.0.0.400` and `10.0.0.100` in the appropriate framework-specific guidance.
- The repository's provider-compatibility or provider-support documentation states the supported DVault-on-DB2 behavior for this release, including caveats and explicit non-goals, without implying undocumented provider-native optimizations or guarantees.
- The external DB2 test instructions describe a developer-managed opt-in fixture path, including container or Podman assumptions and any required configuration markers, and explicitly preserve the default no-external-database build/test posture.
- The production-adoption guidance is updated from the current v0.33.0 / `8.33.0` / `10.33.0` baseline so DB2 is represented consistently in the v0.34.0 package and provider matrix and in adopter caveats.
- The v0.34.0 release notes are added or updated to record the DB2 documentation baseline, the `8.34.0` and `10.34.0` package outputs, caveats, non-goals, and the fact that package publication remains a separate manual activity.

## Definition of Done
- All named documentation surfaces in the ticket are updated consistently in one bounded documentation change.
- Version numbers, provider package ids, and IBM.EntityFrameworkCore dependency versions are internally consistent across README, production-adoption guidance, provider-compatibility guidance, external-test instructions, and release notes.
- DB2 testing guidance clearly separates optional external-provider evidence from the default repository build and test path.
- Historical release-note files remain historical; the new work lands on the v0.34.0 documentation surfaces instead of rewriting prior baselines except where cross-links must point to the new baseline.

## Implementation Notes
- Repository evidence already establishes the DB2 package surface in `src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj`: `PackageId` is `DCoding.Data.DVault.Db2`, target frameworks are `net8.0;net10.0`, and IBM.EntityFrameworkCore is referenced as `8.0.0.400` and `10.0.0.100`.
- README.md already carries the planned consumer guidance for `DCoding.Data.DVault.Db2` and the `8.34.0` and `10.34.0` package lines; the remaining documentation work is alignment of the other docs, not reopening package naming or version-line decisions.
- `docs/production-adoption-checklist.md` still reflects the v0.33.0 baseline and the `8.33.0` and `10.33.0` version line, so this ticket should explicitly bring that document forward rather than leaving a split baseline.
- `docs/releases` currently contains release notes through `v0.33.0.md`; treat this ticket as the bounded place to author the v0.34.0 DB2 documentation notes.
- No child tickets, relation changes, attachments, planning documents, or description rewrites were necessary from the current evidence.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- If README, production-adoption guidance, provider-support docs, external-test instructions, and v0.34.0 release notes are not updated together, the repository will keep conflicting `8.33.0` / `10.33.0` versus `8.34.0` / `10.34.0` guidance.
- DB2 external-test documentation must stay explicit about developer-managed opt-in setup; otherwise readers may infer unsupported default CI or runtime requirements.
- DB2 behavior claims must stay bounded to the documented support and caveat surface; overclaiming provider-native optimization, migration, or validation guarantees would exceed the current evidence baseline.

## Split Recommendations
- No split recommended; the remaining work is one coordinated documentation slice across README, provider-support guidance, external-test guidance, production-adoption notes, and v0.34.0 release notes.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Update README, provider compatibility docs, external DB2 test instructions, production-adoption notes, and release notes for DB2. Document the IBM.EntityFrameworkCore net8.0/net10.0 versions, package outputs 8.34.0 and 10.34.0, Podman/container opt-in assumptions, supported DVault provider behavior, caveats, and non-goals.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

- Implemented the docs-only v0.34.0 DB2 documentation pass across README, production adoption, manual publication, examples, analyzer install guidance, and the new v0.34.0 release notes.
- Documented `DCoding.Data.DVault.Db2`, `8.34.0` / `10.34.0`, `IBM.EntityFrameworkCore` `8.0.0.400` / `10.0.0.100`, `AddDVaultDb2()`, DB2 provider-neutral fallback behavior, DB2 live-schema unsupported status, and opt-in `DVAULT_TEST_DB2_CONNECTION_STRING` external-test setup.
- Validation: `bash tools/check-format.sh` passed. `dotnet build DVault.slnx --nologo --no-restore` was attempted without network restore and failed before compiling because local NuGet restore assets are incomplete (`Microsoft.EntityFrameworkCore.Analyzers` 8.0.27/10.0.8 and `xunit.analyzers` 1.27.0 missing).

<!-- gicket-bot:developer-delivery:v1:end -->