[gicket-bot] PO-critic review contract

Summary
- Approve for dev. The live ticket contract is specific, current, and evidence-anchored; it has no open questions and already defines the current DB2 baseline, IBM-only validation lane, candidate architecture seam, rollback and fallback cases, and split follow-up boundaries.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF43E0JCE7BSBFBWB49HGB4G/description.md keeps Open Questions as none and explicitly scopes the work to evaluation only, IBM.EntityFrameworkCore only, ordinary-hub-parent-first recommendation, fallback classification, rollback-clean caller-transaction analysis, and a separate DB2 binary-storage caveat.
- git log on branch ticket/06FF43E0JCE7BSBFBWB49HGB4G-task-evaluate-db2-pit-full-rebuild-push-down-fea shows head 97f7ffa102 after handoff commit 09e58497ca, so this is still at the pre-development ticket gate rather than an implementation branch.
- src/DCoding.Data.DVault/IDataVaultProviderPitMaintenanceStrategy.cs defines the provider PIT-maintenance seam, and src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs routes RebuildAsync(...) through registered provider strategies before provider-neutral fallback.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers DB2 provider behavior plus save/read/PIT-read/bridge-read strategies only, while src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs still registers DefaultDataVaultPitMaintenanceService; the live source matches the contract statement that DB2 has no PIT maintenance push-down today.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs registers IDataVaultProviderPitMaintenanceStrategy, and src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs replaces IDataVaultPitMaintenanceService; the ticket's compare-against-Postgres and SQL Server guidance matches live source.
- src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs only has known-strategy evaluation for PostgresDataVaultPitMaintenanceStrategy, which makes the ticket's diagnostics and fallback vocabulary requirement concrete rather than speculative.
- tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs uses provider IBM.EntityFrameworkCore and manually inserts PitCustomerContact rows before PIT reads, confirming that the current DB2 proof is read behavior over maintained PIT rows, not PIT maintenance push-down.
- docs/local-validation.md, docs/performance-profiles.md, docs/plans/provider-optimization-gap-matrix.md, and artifacts/benchmarks/06FE4PMQ8GNKY6X54F8D16AVGC-db2-host-podman-validation-<redacted>/benchmark-summary.md all point to the IBM-only opt-in DB2 live lane via DVAULT_TEST_DB2_CONNECTION_STRING and the existing DB2 save/latest/PIT/bridge evidence bundles.
- artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted>/db2-rowcap-1000/benchmark-summary.md records completed DB2 save/latest-satellite/PIT/bridge rows only, and artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-<redacted>/benchmark-summary.md records DB2 binary-hash CLI0109E String data right truncation failures on save/latest/PIT rows, which supports the contract boundary of provider-default hex-style DB2 now with binary compatibility handled separately.
- Current ticket comments after PO refinement are automation and handoff records only, including 06FFDY7AN8TXV3X1QP07AWJYAW.md, 06FFDYH1833MDSPB5JRQ8468J8.md, 06FFDYJJJCYZPWEVP6RKN4PJR0.md, and 06FFE1BWZ35MHHYV6YNKM7FM90.md; no later comment reopens scope or adds unresolved PO questions.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- None at PO level; the delivery contract already names ordinary hub-parent, shared-driving-key multi-active hub-parent, link-parent non-multi-active, MaintainParentsAsync(...), incompatible driving-key-family, dirty-context, provider mismatch, incomplete maintenance-shape evidence, and caller-transaction cases.

Risky assumptions
- The developer will choose or reuse an authoritative evaluation-note surface similar to the existing MySQL feasibility precedent even though this ticket does not pin one exact output file path.
- IBM DB2 ambient transaction and savepoint behavior may fail the rollback-clean requirement; the ticket is still dev-ready because the contract already allows a defer or fallback recommendation when that proof is missing.
- The compatible baseline remains provider-default hex-style DB2; widening the same ticket into binary hash-key compatibility would conflict with the recorded DB2 truncation evidence.

AC / test suggestions
- Require the final evaluation artifact to cite the exact DB2 evidence bundles it used: the 2026-06-23 closure bundle, the 2026-06-21 host-to-Podman validation bundle, and the 06FE4R1N2ADN77NDFDP4GR7020 binary-matrix bundle when discussing the compatibility caveat.
- Require the outcome to explicitly separate maintained-PIT read proof from maintenance push-down proof by referencing the current DB2 smoke path that seeds PIT rows before reading.
- Require an explicit accepted, deferred, and fallback table for ordinary hub-parent, shared-driving-key multi-active hub-parent, link-parent non-multi-active, MaintainParentsAsync(...), incompatible driving-key-family, dirty-context, provider mismatch, incomplete-shape, and caller-transaction cases.

Implementation watchouts
- If the recommendation is to implement later, keep the candidate architecture on IDataVaultProviderPitMaintenanceStrategy rather than drifting into a SQL Server-style service replacement; the ticket intentionally asks for the seam already present in live source.
- Treat caller-owned transactions and savepoints as a go or no-go boundary for the initial ordinary hub-parent lane, not as a post-implementation cleanup item.
- Do not widen the compatible baseline from provider-default hex-style DB2 to binary hash-key storage in the same follow-up.

Non-blocking notes
- The contract does not pin one exact documentation file for the final evaluation artifact, but the repository already uses docs/plans/provider-optimization-gap-matrix.md and docs/performance-profiles.md as authoritative surfaces for the done MySQL feasibility ticket 06FF43CJ9CJMG7J917RW22QKJC.

Split recommendations
- If the evaluation recommends implementation, keep one follow-up limited to IBM.EntityFrameworkCore ordinary hub-parent full-rebuild push-down through the provider-strategy seam.
- Keep multi-active hub-parent expansion, link-parent expansion, and any benchmark-backed DB2 PIT maintenance timing claim as separate later tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment