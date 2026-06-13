[gicket-bot] PO refinement contract

Summary
- Refined the v0.37 documentation ticket around the already-settled target-matched dependency policy and analyzer compatibility outcome. Current repo evidence supports keeping `8.36.0` / `10.36.0` as the visible consumer package lines in v0.37 guidance unless a separate packaging change lands, and existing downstream checklist ticket `06FBSBWW414TE19KZT14CB7Y3R` remains the only active dependent.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already settles the dependency-line policy: `net8.0` stays on the EF Core 8 line and `net10.0` stays on the EF Core 10 line, with the exact visible baseline `8.0.28` / `8.0.28` / `8.0.2` plus DB2 `8.0.0.400`, SQLite `8.0.28`, MySQL `8.0.26`, PostgreSQL `8.0.11`, Oracle `8.<redacted>`, SQL Server `8.0.28` for `net8.0`, and `10.0.9` / `10.0.9` / `10.0.9` plus DB2 `10.0.0.100`, SQLite `10.0.9`, MySQL `10.0.7`, PostgreSQL `10.0.2`, Oracle `<redacted>`, SQL Server `10.0.9` for `net10.0`.
- Repository evidence already settles the analyzer outcome: `DCoding.Data.DVault.Analyzers` remains one `net10.0` analyzer asset with a `.NET 10 SDK` build-host requirement for both coordinated consumer lines; current evidence does not prove pure `.NET 8 SDK` analyzer consumption.
- The current repo-visible consumer package lines are still `8.36.0` and `10.36.0`; no visible pack-script, verifier, README, or release-input evidence introduces `8.37.0` / `10.37.0`, so this ticket should document the visible baseline rather than invent a new consumer package version.
- No new child tickets, attachments, planning documents, or relation writes are justified in this refinement. Existing done tickets `06FBSBN23A20NX2K0YAXZ40ZGR`, `06FBSBW6HDT15D1KGVD7XBQXM8`, and `06FBSBWH9F415E12VRHRYQ2JJM` are prerequisite evidence, and live downstream ticket `06FBSBWW414TE19KZT14CB7Y3R` stays blocked on this baseline work.

Scope In
- Update `README.md` so the current-baseline navigation and install/publication guidance point to `docs/releases/v0.37.0.md` for the dependency-line and analyzer-compatibility record, without leaving `v0.36.0` labeled as the current baseline where v0.37 guidance is expected.
- Update `CHANGELOG.md` so `v0.37.0 - Dependency Line and Analyzer Compatibility` becomes the current top-level release summary and `v0.36.0` becomes historical trail context.
- Update `docs/manual-nuget-publication.md` so the current manual publication baseline matches the settled `8.36.0` / `10.36.0` package lines, the exact target-specific dependency matrix, the analyzer `.NET 10 SDK` build-host boundary, and the current validation evidence story.
- Create `docs/releases/v0.37.0.md` as the authoritative current release record for the settled dependency-line policy, exact package matrix, analyzer compatibility outcome, carried-forward validation commands/evidence, and explicit non-goals.
- Keep the four in-scope current-baseline surfaces consistent with the already-landed project, test, verifier, and analyzer-audit evidence so no stale v0.36 dependency matrix remains where v0.37 guidance is expected.

Scope Out
- Changing project `PackageReference` values, pack-script version lines, `PackageVerifier` logic, unit/integration tests, or analyzer asset targeting unless a direct contradiction is found in the named documentation surfaces.
- Reopening the dependency-line policy or analyzer compatibility decision already settled by done tickets `06FBSBN23A20NX2K0YAXZ40ZGR`, `06FBSBW6HDT15D1KGVD7XBQXM8`, and `06FBSBWH9F415E12VRHRYQ2JJM`.
- Updating `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/plans/shared-implementation-standards.md`, or downstream release-checklist ticket `06FBSBWW414TE19KZT14CB7Y3R` as primary delivery surfaces for this ticket.
- Inventing consumer package versions `8.37.0`, `10.37.0`, or `0.37.0` without separate repo-visible packaging evidence.
- Changing hash-key storage behavior, release automation, publication approval mechanics, or other product/runtime scope unrelated to the bounded current-baseline documentation pass.

Open questions
- none

Follow-up questions
- If release management later wants consumer package lines `8.37.0` / `10.37.0`, should that be a separate packaging/release ticket that updates pack-script, verifier, and install guidance together instead of being inferred here from the planning label alone?

Risks
- `README.md` currently labels `v0.36.0` as the current baseline and uses v0.36-specific section wording, so a partial update could leave competing current-baseline signals between README and the new v0.37 release record.
- Because the planning label is `v0.37.0` but current repo-visible consumer lines are still `8.36.0` / `10.36.0`, careless documentation could wrongly invent `8.37.0` / `10.37.0` or a consumer-facing `0.37.0` package version.
- If the v0.37 docs omit the explicit `.NET 10 SDK` analyzer build-host boundary, they will overstate compatibility beyond what the repository actually proves for net8-target consumers.
- The downstream release-checklist ticket `06FBSBWW414TE19KZT14CB7Y3R` remains blocked until this current-baseline documentation work lands.

Split recommendations
- No new split. Keep existing done tickets as prerequisite evidence and keep `06FBSBWW414TE19KZT14CB7Y3R` as the downstream checklist follow-up that consumes this ticket's finalized baseline.

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