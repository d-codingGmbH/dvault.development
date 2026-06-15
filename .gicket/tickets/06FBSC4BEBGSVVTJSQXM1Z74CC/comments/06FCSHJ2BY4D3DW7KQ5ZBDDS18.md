[gicket-bot] PO-critic review contract

Summary
- Refined ticket is clear enough for developer handoff: the delivery contract has no open questions, and the repository already contains the matrix, root artifact triplet, external-provider bundle references, skip-reason vocabulary, and verifier/smoke evidence the task depends on.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `git --no-pager diff --name-only develop...HEAD` lists only `.gicket/tickets/06FBSC4BEBGSVVTJSQXM1Z74CC/**`; no repository docs, tests, or source files changed on this branch, so the handoff relies on already-landed repository evidence rather than pending implementation work.
- `docs/plans/provider-optimization-evidence-matrix.md` defines the closed evidence postures (`completed-timing`, `skipped-placeholder`, `diagnostics-only`, `smoke-only`, `storage-footprint`) and its save/read matrices explicitly cover SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- `benchmark-summary.md` records PostgreSQL, SQL Server, MySQL, Oracle, and DB2 as `skipped - not configured`, and rows 63-89 preserve save/read guidance rows with `executionStatus=skipped`, `iterations=0`, and `persistedOutcome=not executed` for those providers.
- `benchmark-summary.json` `optionalProviders[]` includes all five optional external providers with skipped status and normalized `not configured` reasons, and the result rows include DB2 save and read guidance entries with skipped outcomes.
- `benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkSkipReason.cs` exposes the normalized unavailable categories `not configured`, `provider dependency unavailable`, and `connection unreachable` that the ticket contract cites.
- `docs/performance-profiles.md` points to checked-in v0.32 benchmark bundles under `artifacts/benchmarks/...` for PostgreSQL, SQL Server, MySQL, and Oracle, matching the ticket's completed-bundle citation rule.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs` expects skipped external-provider rows for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 plus PIT/bridge read guidance rows, and `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` provides DB2 opt-in smoke evidence.
- Incoming `blocks` relation files still exist under `.gicket/relations/...--06FBSC4BEBGSVVTJSQXM1Z74CC--blocks.json`, but source tickets `06FBSC3V8NQS032B8MK84FMGVC`, `06FBSC40N01AH5PRZ1QNKRVTWR`, and `06FBSC46047ZF11DR0TTRARM78` are `done` in their `ticket.json` files, matching the contract's housekeeping note.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Downstream consumers must cite the checked-in v0.32 external-provider bundles for completed PostgreSQL, SQL Server, MySQL, and Oracle timing claims and must not promote root skipped-placeholder rows into timing evidence.
- This approval assumes the stale incoming `blocks` relations remain workflow housekeeping only, because the current ticket is `is-blocked=false` and the three source tickets are already `done`.
- This approval assumes the baseline for this ticket is the existing checked-in evidence set plus explicit placeholders, not a requirement for a fresh multi-provider rerun on this branch.

AC / test suggestions
- When the dev handoff comment or status update is written, cite the exact artifact source per provider: root triplet for SQLite and skipped placeholders, v0.32 benchmark bundles for completed PostgreSQL/SQL Server/MySQL/Oracle evidence, and DB2 smoke or diagnostics sources when timing rows are absent.
- Keep the acceptance distinction explicit that non-SQLite latest-satellite rows are guidance only; PIT/bridge candidate rows and DB2 smoke evidence must not be restated as completed timing evidence.

Implementation watchouts
- Do not broaden this ticket into fresh benchmark collection, DB2 completed timing claims, or binary-vs-hex provider expansion; the refined scope explicitly keeps those as follow-up work.
- Any downstream handoff text should keep `executionStatus=skipped`, `iterations=0`, blank or null metrics, and `persistedOutcome=not executed` intact for unavailable provider lanes.

Non-blocking notes
- The branch history is refinement-only: `git log --oneline --max-count=8` shows PO and PO-critic lease/handoff commits on top of develop, not repository evidence commits.
- Story `06FBSC4HSXFJ5FM6GWECH2CTGG` remains `todo`; the current ticket is already framed as the baseline evidence owner that the downstream gap-matrix story should consume.
- No PO blocker remains from contract ambiguity because the persisted delivery contract already resolves `## Open Questions` to `none`.

Split recommendations
- No split recommended; the contract is already bounded and downstream publication work is separated into story `06FBSC4HSXFJ5FM6GWECH2CTGG`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment