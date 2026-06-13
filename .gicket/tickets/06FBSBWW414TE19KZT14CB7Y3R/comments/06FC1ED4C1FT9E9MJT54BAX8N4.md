[gicket-bot] PO refinement contract

Summary
- Repository evidence already satisfies refinement for the v0.37 release checklist and validation note: the current docs ratify the dual package lines, exact dependency matrix, net10 analyzer host boundary, and required validation commands, so the ticket is ready for PO critic without additional planning writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already ratifies `docs/releases/v0.37.0.md` as the authoritative v0.37 release-note and validation-note surface for this ticket.
- `docs/manual-nuget-publication.md`, `docs/local-validation.md`, `README.md`, and `CHANGELOG.md` align with that release record on package-line separation, analyzer guidance, and validation evidence.
- No bounded planning writes were applied in this run: no child tickets, relation changes, description updates, attachments, or planning documents were materialized.

Scope In
- Document the coordinated eight-package v0.37.0 documentation baseline for `DCoding.Data.DVault`, `DCoding.Data.DVault.Analyzers`, `DCoding.Data.DVault.Db2`, `DCoding.Data.DVault.MySql`, `DCoding.Data.DVault.Oracle`, `DCoding.Data.DVault.Postgres`, `DCoding.Data.DVault.Sqlite`, and `DCoding.Data.DVault.SqlServer`.
- Ratify the two visible consumer package-version lines: `8.36.0` for `net8.0` / EF Core 8 and `10.36.0` for `net10.0` / EF Core 10.
- Record the accepted analyzer compatibility boundary, required validation commands, publication checklist expectations, and known limitations that future release closure must reuse.

Scope Out
- Publishing packages, recording final publish approval, package hashes, signing evidence, or release automation changes.
- Changing runtime behavior, project `PackageReference` values, provider pins, package verifier logic, tests, or pack-script package versions.
- Claiming pure `.NET 8 SDK` analyzer consumption or retargeting the analyzer package away from its current `net10.0` build-host baseline.

Open questions
- none

Follow-up questions
- When release closure happens, which package-version line is approved first: <redacted> / `net8.0` / EF Core 8 or `10.36.0` / `net10.0` / EF Core 10? The checklist already requires separate approvals.
- If product requirements later expand to `net8.0` consumer projects compiling analyzers on a pure `.NET 8 SDK` host, should that ship as a separate compatibility ticket with analyzer retargeting and a new verification lane?
- At delivery closure, confirm whether the current `blocks` chain with `06FBSBWPN112S4CGP0239K0ZT8` and `06FBSBZRR9DP7YTR1ZZA3N6ANG` still reflects execution order or needs cleanup.

Risks
- Manual release closure still depends on rerunning the five required validation commands against the selected package line and recording the final approval record before any package push.
- Because `DCoding.Data.DVault.Analyzers` stays on a single `net10.0` asset, any downstream assumption of pure `.NET 8 SDK` analyzer support would overstate what the repository currently validates.

Split recommendations
- No split recommended; current scope is already bounded and evidenced by the existing repository release documentation.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment