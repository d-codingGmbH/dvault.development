[gicket-bot] PO refinement contract

Summary
- Refined the story into a documentation-level provider optimization evidence matrix that reuses the current benchmark, diagnostics, hash-key storage, and DB2 baselines instead of reopening provider-scope decisions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The ticket is documentation/contract scope: define one canonical evidence matrix that later tickets cite; it does not add new provider implementations or rerun benchmarks.
- The v1 baseline is already bounded by repository evidence: SQLite is the only optimized latest-satellite path; SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 are PIT/bridge candidates; PostgreSQL, SQL Server, MySQL, Oracle, and DB2 have distinct save evidence postures.
- DB2 is in scope as a documented evidence row, but the current baseline is diagnostics-gated save and PIT/bridge support plus opt-in smoke evidence; no DB2 benchmark lane exists today.
- Binary-vs-hex storage comparisons must point to the checked-in SQLite hash-key storage bundle and footprint sidecars, not be generalized as cross-provider timing claims.

Scope In
- Create a single comparable matrix that maps provider optimization scenarios to canonical evidence rows and artifacts for save, latest-satellite read, PIT read, bridge read, hash-key storage profile, and DB2.
- Ratify current row identities and artifact sources from benchmark-summary.*, the hash-key storage bundle, architecture docs, release notes, and diagnostics/fallback contracts.
- Classify each matrix row as completed timing evidence, skipped optional-provider placeholder evidence, diagnostics-only evidence, smoke-test evidence, or storage-footprint evidence.
- Record the bounded provider posture and stop/fallback conditions that determine when a matrix row may or may not support a performance claim.

Scope Out
- Adding new provider save/read strategies, new DB2 latest-satellite optimization, or new PIT/bridge maintenance behavior.
- Adding a DB2 benchmark harness lane or turning smoke/diagnostics evidence into measured timing evidence.
- Changing the benchmark artifact schema, benchmark harness row format, or release/package baselines.
- Introducing provider-specific SQL artifact exporters beyond the already documented SQL Server dry-run lane.
- Changing the default hash-key algorithm or storage profile, or performing persisted-key migration work.

Open questions
- none

Follow-up questions
- Should a future release add a DB2 benchmark lane so DB2 can move from smoke and diagnostics evidence to timing-row evidence?
- If non-SQLite latest-satellite optimization is added later, which new matrix rows and benchmark labels should extend this baseline?
- Should future provider-specific SQL artifact exporters beyond SQL Server gain their own matrix subsection once implemented?

Risks
- If the matrix does not distinguish measured rows from skipped placeholders and diagnostics-only evidence, downstream tickets may overstate provider performance.
- If DB2 is presented alongside benchmark-backed providers without its current no-benchmark-lane qualifier, the repository will imply unsupported timing evidence.
- If binary-vs-hex storage rows are generalized beyond the checked-in SQLite bundle, later tickets may claim cross-provider storage wins that the current evidence does not prove.
- If fallback and stop-condition vocabularies are paraphrased loosely instead of using the bounded enums and contracts, later tickets may reopen already-closed gate semantics.

Split recommendations
- If documentation-only consolidation grows into new measured evidence work, split future execution into a DB2 benchmark-lane ticket and a separate cross-provider hash-key-storage evidence expansion ticket.
- If the team wants automated consumer-facing matrix generation from benchmark artifacts later, handle that as a separate tooling story rather than enlarge this documentation contract ticket.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment