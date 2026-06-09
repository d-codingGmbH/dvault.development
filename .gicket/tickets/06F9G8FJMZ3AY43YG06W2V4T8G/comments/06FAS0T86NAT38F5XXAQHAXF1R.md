[gicket-bot] PO-critic review contract

Summary
- Repository-backed contract is specific, bounded, and has no unresolved PO questions; ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- repository-list-directory on `docs/releases` returned 29 entries and no `docs/releases/v0.33.0.md`, confirming the release-note file is a real remaining work item on this branch.
- repository-read-text on `docs/manual-nuget-publication.md` already defines the v0.33 dual-line contract: seven unchanged package ids, `8.33.0` for `net8.0`/EF Core 8, `10.33.0` for `net10.0`/EF Core 10, no consumer-facing `0.33.0`, and no mixed-line install/publish approval.
- repository-read-text on `docs/plans/shared-implementation-standards.md` repeats the same v0.33 compatibility matrix and explicitly calls `MySql.EntityFrameworkCore 10.0.7` the required evidence exception across both targets.
- repository-read-text on `tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs` verifies the finite provider/version matrix for `net8.0` and `net10.0`, including `Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11` vs `10.0.2`, `Oracle.EntityFrameworkCore 8.<redacted>` vs `<redacted>`, and the `DVAULT_TEST_*` opt-in gates.
- repository-read-text on `docs/production-adoption-checklist.md` still says to treat `releases/v0.32.0.md` as the current public baseline, which matches the ticket's bounded rollover scope.
- gicket-read-ticket-comments returned 10 comments for `06F9G8FJMZ3AY43YG06W2V4T8G`, and they are bot claim/lease/handoff automation; no human scope-comment evidence was present.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The bounded README/release-note/checklist rollover is assumed sufficient for the current v0.33 baseline, with any broader cross-document normalization deferred to later follow-up work.
- Downstream readers are assumed to treat related done ticket `06F9G8FBQTAPXXS1Y4NR5QKVG8` as completed prerequisite context despite the historical blocking relation remaining visible.

AC / test suggestions
- Keep acceptance checks explicit that documentation must separate package-tested/default-local evidence from external-provider opt-in lanes behind `DVAULT_TEST_*` connection-string gates.
- Keep acceptance checks explicit that `0.33.0` is a planning release label only and that consumer-facing package guidance must use either `8.33.0` or `10.33.0`, never both in one example.

Implementation watchouts
- Do not blur planning release `v0.33.0` with consumer package versions `8.33.0` and `10.33.0`.
- Do not imply the MySQL `10.0.7` pin is general mixed-line permission; the shared standards and matrix tests frame it as a bounded evidence exception.
- Keep this ticket inside its stated scope; helper-project retargeting, publish automation, provider provisioning, and runtime changes are already excluded.

Non-blocking notes
- The ticket is well-grounded in repository sources: the manual publication checklist, shared standards, provider-version matrix tests, and the related done verifier/guidance ticket align.
- The lack of human scope comments is not a problem here because the persisted delivery contract is already detailed and internally consistent.

Split recommendations
- No split recommended; the remaining work is a coherent documentation-baseline rollover bounded to README, the new `docs/releases/v0.33.0.md`, the production checklist, and closely linked compatibility/limitations prose.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment