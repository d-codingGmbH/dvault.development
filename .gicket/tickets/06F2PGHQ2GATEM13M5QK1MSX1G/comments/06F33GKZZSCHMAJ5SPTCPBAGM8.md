[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The persisted contract is bounded, matches the current repo state, and has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGHQ2GATEM13M5QK1MSX1G/description.md:33-54` defines bounded acceptance criteria and implementation notes, and `## Open Questions` is explicitly `none` at lines 53-54.
- Relation files `.gicket/relations/1G/QM/06F2PGHQ2GATEM13M5QK1MSX1G--06F2PGHWEWYJZSRQ9QPT4NJ0QM--parentOf.json`, `.gicket/relations/1G/28/06F2PGHQ2GATEM13M5QK1MSX1G--06F2PGJ28KVSZAAFRA40D94128--parentOf.json`, `.gicket/relations/1G/ZM/06F2PGHQ2GATEM13M5QK1MSX1G--06F2PGJBRXFCP038CN6XVAYSZM--blocks.json`, `.gicket/relations/1G/5C/06F2PGHQ2GATEM13M5QK1MSX1G--06F2PGJYY6S97B4Z8044D34K5C--blocks.json`, and `.gicket/relations/J0/1G/06F2PGFT8Z406HFBJGQSY7YRJ0--06F2PGHQ2GATEM13M5QK1MSX1G--blocks.json` confirm done child tickets, downstream blocked tickets, and a done upstream epic.
- `src/DCoding.Data.DVault.Analyzers/CodeFirstDiagnosticCatalog.cs:8-22` exposes only `DMV1901` and `DMV1902`; `src/DCoding.Data.DVault.Analyzers/DataVaultCodeFirstAnalyzer.cs:23-117,142-155` limits analysis to `BusinessKey(...)` plus satellite `Payload(...)` and `DrivingKey(...)`, with duplicate checks inside one builder scope.
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultCodeFirstAnalyzerTests.cs:31-121` covers unsupported selector shapes, duplicate members, valid direct readable scalar selectors, separate satellite scopes, and selector variables outside the first direct-lambda slice.
- `src/DCoding.Data.DVault.Analyzers/README.md:8-50` documents installation and suppression paths, `src/DCoding.Data.DVault.Analyzers/DCoding.Data.DVault.Analyzers.csproj:16-40` packs the README and analyzer assets, and `docs/releases/v0.10.0.md:22-43` records the narrow DMV1901/DMV1902 baseline while `docs/releases/v0.11.0.md:88-92` still treats the analyzer package as optional developer tooling.
- `git log --oneline --graph --grep '06F2PGHQ2GATEM13M5QK1MSX1G|06F2PGHWEWYJZSRQ9QPT4NJ0QM|06F2PGJ28KVSZAAFRA40D94128'` shows the story branch sits on top of `develop` after the child ticket integrations, and `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06F2PGHQ2GATEM13M5QK1MSX1G/*`, so the current story branch carries ticket metadata rather than new code.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give a concrete ticket-level example for reusing the same logical member across `Payload(...)` and `DrivingKey(...)` in one satellite scope; current behavior is only evidenced by the analyzer implementation.
- The contract does not spell out field-selector or nullable-scalar examples, even though `DataVaultCodeFirstAnalyzer.cs:203-214` accepts readable scalar properties and fields.

Risky assumptions
- Approval assumes this story is intentionally a ratification or umbrella ticket and not expected to carry fresh implementation on its own branch, because `git diff --name-only develop...HEAD` shows only ticket metadata changes.
- Approval assumes historical release-note coverage in `docs/releases/v0.10.0.md` is sufficient for this story even though the active release is `v0.12.0 - Analyzer and Generator Ergonomics` and `docs/releases/v0.12.0.md` does not exist in the repo.

AC / test suggestions
- If the story is revisited later, keep explicit ticket-level examples for the intentional non-reporting guards already covered by tests: separate satellite scopes and selector variables outside the first direct-lambda slice.
- If downstream work needs stronger contract precision, add one explicit example that states whether duplicate detection is verb-local or cross-verb within a satellite scope.

Implementation watchouts
- Do not reopen analyzer scope on this story branch; the current branch delta over `develop` is ticket metadata only, and functional expansion should happen only in downstream tickets.
- Keep any code-fix behavior in `06F2PGJBRXFCP038CN6XVAYSZM`, not in this umbrella story.
- Keep v0.12 versioned documentation and release-note alignment in `06F2PGJYY6S97B4Z8044D34K5C`; current README snippets still show `0.11.0`.

Non-blocking notes
- The prompt seed said recent comments were none, but the persisted ticket now contains PO refinement and lease comments under `.gicket/tickets/06F2PGHQ2GATEM13M5QK1MSX1G/comments/`; repo state was the source of truth for this review.
- Release `06F2PH99NN9B0S4RZW0NPST1CR` is `v0.12.0 - Analyzer and Generator Ergonomics`, while the current root README and analyzer README still advertise `0.11.0` package versions; the ticket already records this as downstream documentation risk rather than a PO blocker.

Split recommendations
- No additional split recommended. The current graph already separates rule implementation, analyzer configuration docs, code fixes, and v0.12 documentation and release-note closure.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment