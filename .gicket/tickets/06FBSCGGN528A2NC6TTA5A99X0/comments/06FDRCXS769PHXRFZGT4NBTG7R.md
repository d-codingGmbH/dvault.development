[gicket-bot] PO refinement contract

Summary
- Repository-backed refinement confirms this is a bounded PostgreSQL PIT/bridge evidence-closure task: the runtime candidate already exists, the repo already contains provider-configured smoke-read artifacts for PostgreSQL PIT and bridge rows, and the remaining work is to promote that artifact into the canonical evidence surfaces without widening read-strategy scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Live relations are bounded and consistent with the current workflow: `06FBSCGBG8CJ0QNRX4JZJA638G -> 06FBSCGGN528A2NC6TTA5A99X0` (`blocks`) and `06FBSCGGN528A2NC6TTA5A99X0 -> 06FBSCHBJEYYERDPA7JN34Y8PG` (`blocks`); no relation cleanup is indicated by the verified repository state.
- `AddDVaultPostgres()` already registers `PostgresDataVaultReadStrategy` for PIT and bridge reads, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` already proves incomplete-read-shape and stale-maintenance fail-closed fallback for PostgreSQL PIT/bridge gates.
- The checked-in provider-configured artifact bundle `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.*` already contains completed PostgreSQL `pit-as-of-read` and `bridge-traversal-read` rows selecting `PostgresDataVaultReadStrategy`.
- The root `benchmark-summary.*` triplet remains the quick baseline with PostgreSQL rows intentionally `skipped` when `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is unset; closing this ticket does not require reinterpreting those skipped root rows as completed timing evidence.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Ratify the existing PostgreSQL PIT/bridge runtime candidate boundary as the implementation baseline: no new strategy invention, only evidence-gap closure for the already-registered `PostgresDataVaultReadStrategy`.
- Promote one checked-in provider-configured artifact lane as the canonical completed-evidence source for PostgreSQL `pit-as-of-read` and `bridge-traversal-read` rows.
- Update canonical evidence/planning surfaces that currently classify PostgreSQL P2.01 and P3.01 as open evidence gaps so they cite the verified completed artifact and claim boundary consistently.
- Keep repository diagnostics, benchmark-contract, and test expectations aligned with the evidence reclassification so PostgreSQL PIT/bridge rows are treated as completed supported evidence while unsupported shapes and stale/incomplete maintenance still fall back deterministically.
- Update current guidance surfaces that still describe PostgreSQL PIT/bridge rows as skipped-only or evidence-gap-only when those surfaces are part of the live adopter baseline.

Scope Out
- PostgreSQL latest-satellite optimization remains out of scope; the repository still records that no PostgreSQL provider-specific latest-satellite read strategy is registered.
- SQL Server, MySQL, Oracle, and DB2 PIT/bridge evidence-gap tickets remain separate work items.
- New public read APIs, new read-shape semantics, automatic PIT/bridge maintenance, scheduler behavior, or provider-specific strategy invention are out of scope.
- Do not require the root quick `benchmark-summary.*` triplet to become a completed PostgreSQL timing surface when the checked-in provider-configured artifact bundle already supplies the approved evidence lane.
- Do not widen provider claims into unsupported latest-satellite, unsupported PIT/bridge shapes, or any behavior beyond the current explicit-maintenance and fail-closed fallback boundary.

Open questions
- none

Follow-up questions
- After PostgreSQL PIT/bridge rows are reclassified to completed evidence, should the same artifact-citation pattern be applied immediately to the MySQL and Oracle PIT/bridge gap tickets if the same smoke-read bundle is accepted as authoritative for them?
- Should the downstream documentation ticket `06FBSCHBJEYYERDPA7JN34Y8PG` explicitly restate the PostgreSQL artifact-lane provenance, or should it only reference the canonical evidence matrix once this ticket lands?

Risks
- Current repository guidance is internally inconsistent: the smoke-read artifact bundle already contains completed PostgreSQL PIT/bridge rows, while the evidence matrix, gap matrix, current checklist, and architecture text still describe PostgreSQL PIT/bridge as skipped/evidence-gap posture. Partial updates would preserve contradictory claims.
- If implementers try to close the ticket by converting the root quick baseline into a completed PostgreSQL timing surface instead of citing the existing provider-configured artifact bundle, scope may widen into unnecessary benchmark reruns or artifact-contract churn.
- Because the same smoke-read bundle also contains other provider rows, overly broad documentation edits could accidentally promote MySQL, Oracle, or SQL Server PIT/bridge evidence beyond the exact PostgreSQL rows this ticket owns.

Split recommendations
- No split recommended; the verified repository state keeps this as one bounded evidence-closure task covering artifact adoption, evidence/gap-matrix alignment, and preservation of the existing PostgreSQL PIT/bridge fallback boundary.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment