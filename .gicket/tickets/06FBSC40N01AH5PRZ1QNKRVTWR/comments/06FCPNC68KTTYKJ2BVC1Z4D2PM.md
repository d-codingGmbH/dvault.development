[gicket-bot] PO refinement contract

Summary
- Refined the ticket to the existing four-variant hash-key benchmark matrix baseline so SQLite plus configured PostgreSQL, SQL Server, MySQL, and Oracle lanes can emit comparable binary-vs-hex rows without reopening DB2 or consumer-runtime scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The bounded variant set is already visible in the repository: sha256-v1-hex, sha256-v1-binary, sha256-128-v1-hex, and sha256-128-v1-binary.
- Comparable evidence should reuse the existing benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json schema plus deterministic executionDetail hashKeyVariant metadata; hash-key-footprint sidecars remain supplemental SQLite-local storage evidence.
- Configured external benchmark providers for this ticket are the existing PostgreSQL, SQL Server, MySQL, and Oracle lanes. DB2 stays out of scope because the benchmark filter set and evidence matrix still treat DB2 as diagnostics-only or smoke-only with no benchmark lane.
- The safe v1 baseline is one matrix run that keeps the required SQLite rows present and adds configured optional-provider rows, rather than inventing standalone provider-only footprint artifacts.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement.

Scope In
- Extend or verify the benchmark harness, tests, and guidance so the bounded hash-key storage matrix can run against the required SQLite baseline and any configured PostgreSQL, SQL Server, MySQL, or Oracle benchmark lane.
- Preserve comparable scenario and baseline identities for provider-native bulk ingestion, latest-satellite read, PIT as-of read, bridge traversal read, and the existing SQLite save, streaming-save, read, and latest-index rows under each hash-key variant.
- Preserve run-context reporting of hash-key variants, provider filter, optional-provider availability, skip reasons, runtime environment, and iteration or warmup settings in the shared artifact triplet.
- Keep optional-provider rows visible as skipped placeholders when connection strings, provider packages, or live connectivity are unavailable.
- Update benchmark-facing docs or release/planning guidance only as needed to make the dimension discoverable without making benchmark execution a consumer prerequisite.

Scope Out
- Adding a DB2 benchmark lane, DB2 provider filter, or DB2 timing claims.
- Replacing the shared benchmark artifact schema or inventing provider-specific benchmark file names.
- Claiming cross-provider physical footprint evidence; hash-key-footprint sidecars remain SQLite-local unless a separate provider-specific bundle is added.
- Changing public hash-key value boundaries, default stable-hash behavior, binary-first migration posture, or persisted-key repair behavior.
- Collecting and checking in new external-provider evidence bundles; downstream evidence population remains separate work under ticket 06FBSC4BEBGSVVTJSQXM1Z74CC.

Open questions
- none

Follow-up questions
- After this dimension is in place, should ticket 06FBSC4BEBGSVVTJSQXM1Z74CC check in one external-provider evidence bundle so the matrix includes at least one measured non-SQLite binary-vs-hex comparison instead of only placeholder rows when providers are absent?
- If later work needs provider-specific storage-footprint sidecars, which provider should be first rather than broadening this ticket beyond the current SQLite-local footprint baseline?
- If DB2 eventually gains a benchmark lane, should it reuse the same four-variant matrix labels and shared artifact contract rather than introducing a separate DB2-only format?

Risks
- If docs broaden SQLite-local hash-key-footprint sidecars into cross-provider proof, downstream tickets may overstate storage evidence that the repository does not yet measure.
- If matrix mode drops skipped optional-provider rows or their strategy metadata, artifact readers will lose comparability when external providers are not configured.
- If non-SQLite latest-satellite rows are presented as optimized binary-vs-hex timing evidence, the ticket contract will contradict the current read-strategy baseline.
- If this ticket grows from harness and dimension work into checked-in external evidence collection, it will overlap the separate provider-evidence population ticket.

Split recommendations
- No split recommended inside this ticket; harness and dimension work is already cleanly separated from downstream evidence population in ticket 06FBSC4BEBGSVVTJSQXM1Z74CC.
- If provider-specific footprint bundles are needed later, create a separate follow-up per provider or one explicit cross-provider storage-evidence story instead of enlarging this ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment