[gicket-bot] PO-critic review contract

Summary
- Ticket contract is source-backed, has no unresolved PO questions, and is ready for developer handoff as a documentation-only matrix story.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSC3N7ZFVQW3AV2JJ8T7Q7W/description.md:30-58` contains 8 acceptance-criteria items, 5 DoD items, implementation notes naming `docs/plans`, `benchmark-summary.*`, the SQLite hash-key bundle, fallback enums, and `## Open Questions` = `none`.
- `.gicket/tickets/06FBSC3N7ZFVQW3AV2JJ8T7Q7W/comments/06FCNJ6C48SH93N16NXP334VZW.md:1-59` and `.gicket/tickets/06FBSC3N7ZFVQW3AV2JJ8T7Q7W/comments/06FCNJJ9527ZH9K9EK9G2P21CG.md:29-31` show a clean PO handoff to `ready_for_po_critic`; the inspected comment set contains workflow automation only after that handoff and does not add unresolved product questions.
- `benchmark-summary.md:5-14` preserves optional provider skip state; `benchmark-summary.md:42-54` gives completed SQLite streaming and read rows; `benchmark-summary.md:61-82` keeps PostgreSQL/SQL Server/MySQL/Oracle provider save/read guidance visible as skipped rows with planned execution detail.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:193-368` hard-codes the expected provider-native save rows and skipped optional-provider read rows; `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:399-423` hard-codes the guidance fragments for PostgreSQL direct/UNNEST vs COPY, MySQL multi-row vs staged bulk, Oracle direct/no-staged-win, and non-SQLite latest-satellite not registered.
- `hash-key-footprint.md:3-19,23-28` routes to `artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-<redacted>/*` and scopes the binary-vs-hex evidence to SQLite-only variants `sha256-v1` and `sha256-128-v1`.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md:11-13,58-62`, `docs/architecture/dvault-v1-explicit-save-service.md:96-105`, and `docs/releases/v0.34.0.md:41-43,64-70,110-118` already define the bounded provider posture the matrix must reuse: SQLite-only optimized latest-satellite, diagnostics-gated PIT/bridge candidates, DB2 save+PIT/bridge support, no DB2 latest-satellite optimization, no DB2 benchmark lane, and opt-in DB2 smoke evidence.
- `git log --oneline` on the target branch shows `019a46a6c` as the PO handoff commit and `f068b34f0` as the current po-critic lease claim; `git diff --name-only 8b41771f1..019a46a6c` touched only `.gicket/**` metadata, confirming this is still a pre-development handoff rather than delivered repo documentation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not pin the exact `docs/plans/<file>.md` filename or whether shared fallback causes such as `NoProviderSpecificStrategyRegistered` should be shown once globally or repeated per row. That is a minor implementation choice, not a PO blocker.

Risky assumptions
- The implementation will cite mixed-version source baselines by evidence boundary instead of flattening them into one release claim: `docs/performance-profiles.md:1-35` is v0.32.0 guidance, `docs/releases/v0.34.0.md:41-43` carries the DB2 posture, and `hash-key-footprint.md:1-19` routes v0.36.0 SQLite storage evidence.

AC / test suggestions
- Add a review checklist item that the new matrix links directly to the root benchmark triplet, the SQLite hash-key sidecar bundle, and the three named docs (`docs/performance-profiles.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md`) without duplicating raw timing tables.
- When the matrix is reviewed, confirm non-SQLite latest-satellite rows are explicitly marked provider-neutral/no registered latest-satellite strategy and DB2 is explicitly marked no benchmark lane / smoke-or-diagnostics-only.

Implementation watchouts
- Do not turn skipped optional-provider rows into timing claims. `benchmark-summary.md:61-82` keeps PostgreSQL, SQL Server, MySQL, and Oracle guidance rows visible as `skipped` with planned execution detail.
- Do not generalize hash-key storage results beyond SQLite. `hash-key-footprint.md:14-19,23-28` scopes the checked-in storage bundle to SQLite local temporary files only.
- Use the bounded fallback vocabulary directly from `src/DCoding.Data.DVault/DataVaultSaveStrategyFallbackCauseKind.cs:14-98`, `src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs:14-63`, and `src/DCoding.Data.DVault/DataVaultChunkedSaveStateFallbackCauseKind.cs:6-10` rather than paraphrasing stop conditions.

Non-blocking notes
- Branch-history check: `git diff --name-only 8b41771f1..019a46a6c` touched only `.gicket/**` metadata, so no matrix document exists yet. That is expected for this pre-development handoff and is not a PO blocker.

Split recommendations
- If the work expands into new measured evidence, split DB2 benchmark-lane work away from the documentation-only matrix story.
- Keep any future automated matrix generation or provider-specific hash-key expansion as separate tooling/evidence tickets rather than enlarging this handoff story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment