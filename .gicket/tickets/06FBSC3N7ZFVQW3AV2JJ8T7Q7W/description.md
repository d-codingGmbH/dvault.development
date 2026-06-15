<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story into a documentation-level provider optimization evidence matrix that reuses the current benchmark, diagnostics, hash-key storage, and DB2 baselines instead of reopening provider-scope decisions.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The ticket is documentation/contract scope: define one canonical evidence matrix that later tickets cite; it does not add new provider implementations or rerun benchmarks.
- The v1 baseline is already bounded by repository evidence: SQLite is the only optimized latest-satellite path; SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 are PIT/bridge candidates; PostgreSQL, SQL Server, MySQL, Oracle, and DB2 have distinct save evidence postures.
- DB2 is in scope as a documented evidence row, but the current baseline is diagnostics-gated save and PIT/bridge support plus opt-in smoke evidence; no DB2 benchmark lane exists today.
- Binary-vs-hex storage comparisons must point to the checked-in SQLite hash-key storage bundle and footprint sidecars, not be generalized as cross-provider timing claims.

### Scope In
- Create a single comparable matrix that maps provider optimization scenarios to canonical evidence rows and artifacts for save, latest-satellite read, PIT read, bridge read, hash-key storage profile, and DB2.
- Ratify current row identities and artifact sources from benchmark-summary.*, the hash-key storage bundle, architecture docs, release notes, and diagnostics/fallback contracts.
- Classify each matrix row as completed timing evidence, skipped optional-provider placeholder evidence, diagnostics-only evidence, smoke-test evidence, or storage-footprint evidence.
- Record the bounded provider posture and stop/fallback conditions that determine when a matrix row may or may not support a performance claim.

### Scope Out
- Adding new provider save/read strategies, new DB2 latest-satellite optimization, or new PIT/bridge maintenance behavior.
- Adding a DB2 benchmark harness lane or turning smoke/diagnostics evidence into measured timing evidence.
- Changing the benchmark artifact schema, benchmark harness row format, or release/package baselines.
- Introducing provider-specific SQL artifact exporters beyond the already documented SQL Server dry-run lane.
- Changing the default hash-key algorithm or storage profile, or performing persisted-key migration work.

## Acceptance Criteria
- A canonical provider optimization evidence matrix is documented in one repository document using the existing benchmark artifact contract vocabulary so later tickets can cite matrix rows by scenario, baseline, and provider instead of ad hoc benchmark notes.
- The matrix includes save rows for provider-neutral fallback, SQLite optimized save, streaming-save variants, PostgreSQL direct-or-UNNEST and staged COPY, SQL Server native bulk, MySQL multi-row and staged bulk, Oracle direct optimized batching, and the DB2 no-benchmark-lane posture.
- The matrix includes read rows for SQLite latest-satellite fallback and optimized paths, SQLite PIT and bridge fallback and optimized paths, and skipped optional-provider PIT/bridge and latest-satellite guidance rows for PostgreSQL, SQL Server, MySQL, and Oracle with their planned or selected strategy facts.
- The matrix includes a hash-key storage section that points to the checked-in SQLite hash-key storage artifact bundle and footprint sidecars for HexString versus Binary and sha256-v1 versus sha256-128-v1 variants.
- The matrix explicitly states that SQLite is the only repository-proven optimized latest-satellite provider path, while PostgreSQL, SQL Server, MySQL, Oracle, and DB2 are diagnostics-gated PIT/bridge candidates and non-SQLite latest-satellite requests remain provider-neutral.
- The matrix explicitly states that DB2 evidence is limited to diagnostics-gated clean-context save behavior, diagnostics-gated PIT/bridge read behavior, and opt-in live smoke evidence until a DB2 benchmark lane is added.
- Every matrix section captures the finite stop and fallback conditions required before making a provider-specific claim, including skipped optional-provider rows, missing connection strings, provider-name mismatch, unsupported shape, incomplete read-shape evidence, stale read-model maintenance, dirty context, and relevant provider thresholds.
- The document cross-references the authoritative benchmark-summary.*, hash-key-footprint.*, performance-evidence artifact contract, save/read boundary docs, and release notes that already own the detailed evidence.

## Definition of Done
- The matrix document is checked in and references the authoritative row sources without inventing new benchmark fields or duplicate timing tables.
- Referenced docs that already guide provider selection or adoption point to the matrix as the canonical evidence lookup surface.
- Matrix row labels match the checked-in scenario, baseline, and provider identities already used by benchmark-summary.* or the cited artifact bundle.
- Measured, skipped, diagnostics-only, smoke-only, and storage-footprint evidence are visually distinguished so downstream tickets cannot cite them interchangeably.
- No open PO questions remain about the v1 provider set, latest-satellite baseline, DB2 posture, or hash-key storage baseline.

## Implementation Notes
- Use docs/plans as the default home for the canonical matrix, then link it from docs/performance-profiles.md, docs/architecture/dvault-v1-explicit-save-service.md, and docs/architecture/dvault-v1-pit-bridge-boundary.md rather than copying timing values into multiple places.
- Reuse row identities already enforced by tests in tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs, including customer-profile-streaming-save, provider-native-bulk-ingestion, latest-satellite-read, pit-as-of-read, and bridge-traversal-read baselines.
- Treat benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json as the quick baseline for SQLite-required rows and skipped optional PostgreSQL, SQL Server, MySQL, and Oracle rows.
- Treat artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/ plus root hash-key-footprint.md as the authoritative binary-vs-hex storage evidence surface.
- Carry forward save posture from docs/architecture/dvault-v1-explicit-save-service.md and read posture from docs/architecture/dvault-v1-pit-bridge-boundary.md and docs/performance-profiles.md instead of redefining provider capabilities.
- Use the closed fallback vocabularies from src/DCoding.Data.DVault/DataVaultSaveStrategyFallbackCauseKind.cs, src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs, and src/DCoding.Data.DVault/DataVaultChunkedSaveStateFallbackCauseKind.cs when listing stop conditions.
- Ratify DB2 from docs/releases/v0.34.0.md and docs/plans/hash-key-storage-profile-contract.md as: save and PIT/bridge read supported, latest-satellite optimization absent, live-schema reader unsupported, benchmark lane absent.
- Keep storage claims scoped: binary-vs-hex timing and footprint evidence are SQLite-local unless a future provider-specific bundle is added.

## Open Questions
- none

## Follow-Up Questions
- Should a future release add a DB2 benchmark lane so DB2 can move from smoke and diagnostics evidence to timing-row evidence?
- If non-SQLite latest-satellite optimization is added later, which new matrix rows and benchmark labels should extend this baseline?
- Should future provider-specific SQL artifact exporters beyond SQL Server gain their own matrix subsection once implemented?

## Risks
- If the matrix does not distinguish measured rows from skipped placeholders and diagnostics-only evidence, downstream tickets may overstate provider performance.
- If DB2 is presented alongside benchmark-backed providers without its current no-benchmark-lane qualifier, the repository will imply unsupported timing evidence.
- If binary-vs-hex storage rows are generalized beyond the checked-in SQLite bundle, later tickets may claim cross-provider storage wins that the current evidence does not prove.
- If fallback and stop-condition vocabularies are paraphrased loosely instead of using the bounded enums and contracts, later tickets may reopen already-closed gate semantics.

## Split Recommendations
- If documentation-only consolidation grows into new measured evidence work, split future execution into a DB2 benchmark-lane ticket and a separate cross-provider hash-key-storage evidence expansion ticket.
- If the team wants automated consumer-facing matrix generation from benchmark artifacts later, handle that as a separate tooling story rather than enlarge this documentation contract ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define a comparable evidence matrix for provider optimizations across save, latest-satellite read, PIT read, bridge read, binary-vs-hex storage, DB2, fallback reasons, and stop conditions. Acceptance: implementation tickets in later releases can point to matrix rows instead of ad hoc benchmark notes.