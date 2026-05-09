[gicket-bot] PO-critic review contract

Summary
- Ready for dev. The persisted contract is closed, the authoritative addendum is present in the repo, the existing baseline tests and provider-profile API are directly observable, and recent branch history after PO handoff is ticket-metadata-only.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket-comments` returned 10 comments for `06F0MEAD1BAA5QEVM3F9QJA38G`; the thread shown is PO handoff/orchestration history, and no later comment reopens scope or adds unresolved product questions after the `ready_for_po_critic` handoff.
- `docs/plans/06F0MEAD1BAA5QEVM3F9QJA38G-fluent-parity-child-boundary.md` exists and matches the ticket body: hub, link, ordinary satellite, and hub-parent multi-active satellite parity are in scope, while link-parent satellites stay out of scope.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs` already contains translator-level parity coverage for fluent hub/satellite projection and ordered `DrivingKey(...)` projection against metadata-first baselines.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs` already contains translator-level link parity and ordering coverage, including explicit-name two-participant and derived-name multi-participant link cases.
- `tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs` already demonstrates the repository's SQLite schema-inspection pattern with `EnsureCreated()` plus concrete table/column/primary-key/index assertions for hub, link, and satellite shapes, which aligns with this ticket's requested SQLite parity style.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` directly defines the public built-in capability profiles `Sqlite`, `Oracle`, `Postgres`, `SqlServer`, and `MySql` with ids `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, and `mysql-pomelo-v1`, matching the contract's provider matrix.
- Branch-history evidence is consistent with a clean PO handoff: `git rev-parse --verify ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity` resolved to `7130b86ac9978e2269ca918df77c2fedc9abe039`, and `git show --stat -1 7130b86a` plus `git show --stat -1 0c04b0d4ab19` show only `.gicket/...` ticket/comment/event updates, not repo source or test changes after refinement.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Expect dev handoff evidence that all five built-in profiles are exercised independently in the parity matrix: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, and `mysql-pomelo-v1`.
- Expect dev handoff evidence of at least one SQLite-backed schema proof (`EnsureCreated()` or create-script inspection), not only translator-level model comparisons.

Implementation watchouts
- Keep code-first and metadata-first assertions independent; the contract explicitly warns against shared normalization that could hide drift.
- Keep link-parent satellites, PIT, bridge work, external-provider integration runs, and checked-in EF migration artifacts out of scope for this child.

Non-blocking notes
- The title still says `migration`, but the persisted Scope Out and Implementation Notes are explicit that this child uses inspection-style parity tests rather than checked-in EF migration artifacts.

Split recommendations
- No split recommended; the current delivery contract and attached addendum already bound this child to parity coverage on top of the existing fluent baseline.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment