[gicket-bot] PO refinement contract

Summary
- Refined the v0.34.0 DB2 documentation task into a bounded docs-only slice: repository evidence already fixes the DB2 package id, the net8.0/net10.0 target line, the IBM.EntityFrameworkCore versions 8.0.0.400 and 10.0.0.100, and the consumer package lines 8.34.0 and 10.34.0. The remaining work is to align the README-adjacent documentation surfaces and add the v0.34.0 DB2 release-note baseline. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Use the existing DB2 provider package `DCoding.Data.DVault.Db2` as the authoritative package id for consumer documentation.
- Use the current package-version lines already present in README: `8.34.0` for `net8.0` and EF Core 8, and `10.34.0` for `net10.0` and EF Core 10. Do not document a consumer-facing `0.34.0` package version.
- Use `src/DCoding.Data.DVault.Db2/DCoding.Data.DVault.Db2.csproj` as the authoritative source for IBM.EntityFrameworkCore dependency versions: `8.0.0.400` on `net8.0` and `10.0.0.100` on `net10.0`.
- Keep external DB2 setup explicitly opt-in and developer-managed; the documentation must not imply that default builds or tests require a DB2 instance, Docker, or Podman.
- No bounded ticket writes were applied during refinement; existing relations, attachments, and ticket description were left unchanged.

Scope In
- README installation and provider-setup guidance updates needed to make DB2 documentation consistent with the current 8.34.0 and 10.34.0 package-line baseline.
- Provider compatibility/support documentation updates for supported DVault-on-DB2 behavior, caveats, and explicit non-goals.
- External DB2 test/setup instructions that explain the opt-in container/Podman assumptions and any required environment or configuration markers.
- Production-adoption checklist updates so it no longer remains on the v0.33.0 / 8.33.0 / 10.33.0 baseline where DB2 support is being introduced.
- A v0.34.0 release-note surface that records the DB2 documentation baseline, package outputs, caveats, and manual-publication separation.

Scope Out
- Any runtime or provider code changes, new DB2 capabilities, or new EF Core behavior.
- NuGet publication, package hashes, or final publication-link recording.
- New CI automation, default DB2 test infrastructure, or mandatory local or CI DB2 dependencies.
- Broader provider-matrix rewrites beyond the DB2 documentation changes required for v0.34.0.

Open questions
- none

Follow-up questions
- none

Risks
- If README, production-adoption guidance, provider-support docs, external-test instructions, and v0.34.0 release notes are not updated together, the repository will keep conflicting `8.33.0` / `10.33.0` versus `8.34.0` / `10.34.0` guidance.
- DB2 external-test documentation must stay explicit about developer-managed opt-in setup; otherwise readers may infer unsupported default CI or runtime requirements.
- DB2 behavior claims must stay bounded to the documented support and caveat surface; overclaiming provider-native optimization, migration, or validation guarantees would exceed the current evidence baseline.

Split recommendations
- No split recommended; the remaining work is one coordinated documentation slice across README, provider-support guidance, external-test guidance, production-adoption notes, and v0.34.0 release notes.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment