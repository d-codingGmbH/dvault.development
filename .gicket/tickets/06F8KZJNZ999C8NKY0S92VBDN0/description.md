<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around bounded MySQL and Oracle PIT/bridge read candidate work on the existing provider-neutral read boundary; no persistent planning writes were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Related done stories 06F8KZHZ27SDTNCFNMFDQRVCKM and 06F8KZJAKN7Q2QXXP9PRK2V94G already fix the provider read evidence contract and the PostgreSQL/SQL Server candidate pattern, so this ticket should extend that baseline rather than reopen PIT/bridge API shape.
- Current repository evidence already fixes the v1 boundary: IDataVaultReadService remains the PIT/bridge read surface over caller-maintained read-model tables, and this ticket does not add automatic PIT/bridge maintenance, SaveChanges refresh, or background orchestration.
- Current source and docs show only AddDVaultSqlite(), AddDVaultPostgres(), and AddDVaultSqlServer() register PIT/bridge read strategies today; AddDVaultMySql() and AddDVaultOracle() currently register save strategies only, so this story is additive provider-package work or an explicit provider-local decline.
- MySQL provider matching must stay dual-provider (Pomelo.EntityFrameworkCore.MySql and MySql.EntityFrameworkCore) consistent with existing capability-profile and save-strategy registration, while Oracle matching stays Oracle.EntityFrameworkCore.
- Benchmark row work and broad release, README, and performance-profile wording are already split into sibling tasks 06F8KZK2MSFQP9G2DBM61ZVGD4 and 06F8KZKFTCC0YXAPRTXA53DNEC; no child tickets, relation changes, description updates, attachments, or planning documents were materialized in this PO pass.

### Scope In
- MySQL PIT read candidate evaluation and outcome within the existing maintained PIT boundary.
- MySQL bridge read candidate evaluation and outcome within the existing maintained bridge boundary.
- Oracle PIT read candidate evaluation and outcome within the existing maintained PIT boundary.
- Oracle bridge read candidate evaluation and outcome within the existing maintained bridge boundary.
- Read-strategy registration, gate evaluation, diagnostics, parity tests, and fail-closed fallback behavior for any implemented MySQL or Oracle candidate paths.

### Scope Out
- Latest-satellite read optimization for MySQL or Oracle.
- New PIT or bridge metadata shapes, request semantics, public IDataVaultReadService APIs, or typed helper contract changes.
- Automatic PIT or bridge maintenance, read-time refresh, SaveChanges orchestration, or provider-specific maintenance strategies.
- Benchmark artifact row expansion or verifier changes owned by task 06F8KZK2MSFQP9G2DBM61ZVGD4.
- Broad documentation and publication work owned by task 06F8KZKFTCC0YXAPRTXA53DNEC.
- Provider-specific save-strategy or bulk-write changes.

## Acceptance Criteria
- For MySQL, the repository outcome is explicit: either AddDVaultMySql() registers PIT and bridge read strategy candidates for supported maintained shapes, or the package stays provider-neutral fallback-only with evidence-backed limitation notes explaining the deliberate decline.
- For Oracle, the repository outcome is explicit: either AddDVaultOracle() registers PIT and bridge read strategy candidates for supported maintained shapes, or the package stays provider-neutral fallback-only with evidence-backed limitation notes explaining the deliberate decline.
- Any implemented MySQL or Oracle PIT candidate selects only when provider identity, supported PIT shape, complete read-shape evidence, and clean-context stale-maintenance checks all pass, and otherwise falls back through the existing provider-neutral PIT path.
- Any implemented MySQL or Oracle bridge candidate selects only when provider identity, supported bridge shape, complete read-shape evidence, and clean-context stale-maintenance checks all pass, and otherwise falls back through the existing provider-neutral bridge path.
- Any implemented candidate path returns the same PIT or bridge rows and typed projection results as the provider-neutral fallback for the same supported inputs.
- Registration and diagnostic coverage keep selected strategy names, supported provider names, gate requirements, and finite fallback causes visible through the existing read diagnostics and telemetry surfaces.

## Definition of Done
- MySQL and Oracle provider packages either gain bounded PIT and bridge read strategy registrations plus tests, or ship an explicit evidence-backed decline that keeps live provider posture honest.
- DataVaultProviderReadStrategyGateEvaluator and known-strategy diagnostics coverage are updated consistently for any newly added MySQL or Oracle read strategies.
- Unit coverage proves provider-name gating, supported-shape selection, unsupported-shape fallback, incomplete-evidence fallback, and stale-maintenance fallback for each implemented provider path.
- Result-parity coverage exercises implemented MySQL or Oracle candidate paths against provider-neutral fallback for raw PIT or bridge rows and typed projections.
- Any provider-matrix or limitation change required by the implementation is handed off to the existing benchmark and documentation sibling tickets instead of widening this story.

## Implementation Notes
- Reuse the existing relational PIT and bridge pattern already used by PostgresDataVaultReadStrategy and SqlServerDataVaultReadStrategy; this ticket should extend provider-package registration and gate evaluation rather than invent a new read pipeline.
- MySQL candidate evaluation should align with existing MySQL capability selection by accepting both Pomelo.EntityFrameworkCore.MySql and MySql.EntityFrameworkCore.
- Oracle candidate evaluation should align with the existing Oracle.EntityFrameworkCore exact provider-name gate and reuse the same fail-closed diagnostics model as the other PIT and bridge read strategies.
- Existing read diagnostics already define ProviderNeutralFallback, UnsupportedPitShape, UnsupportedBridgeShape, IncompleteReadShapeEvidence, and StaleReadModelMaintenance; reuse that vocabulary unless a repository-backed provider limitation truly requires an additive contract change.
- Extend the same known-strategy surfaces that currently enumerate SQLite, PostgreSQL, and SQL Server so MySQL or Oracle candidates expose supported provider names and gate requirements through diagnostics tests and provider registration tests.
- If live provider proof is added, keep it opt-in behind the existing DVAULT_TEST_MYSQL_CONNECTION_STRING and DVAULT_TEST_ORACLE_CONNECTION_STRING lanes rather than introducing new mandatory local prerequisites.

## Open Questions
- none

## Follow-Up Questions
- If one provider is deliberately declined, should task 06F8KZKFTCC0YXAPRTXA53DNEC publish it as a current non-goal or as future-candidate wording in the v0.28.0 docs?
- After the provider matrix is finalized here, should task 06F8KZK2MSFQP9G2DBM61ZVGD4 emit skipped read rows for unmeasured providers or only measured rows for implemented candidates?
- If only one of MySQL or Oracle ships a candidate now, should the remaining provider stay in this epic or move to a later follow-up story?

## Risks
- MySQL and Oracle currently have no PIT or bridge read strategy registrations and no read-focused opt-in integration classes, so scope can sprawl if benchmark or documentation follow-through leaks into this story.
- MySQL dual-provider identity and Oracle-specific parameter or identifier behavior can drift from provider-neutral parity unless raw-row and typed-projection parity coverage stays first-class.
- A deliberate decline without explicit tests, diagnostics, and handoff notes would leave the public provider matrix easier to overstate than the visible source proves.
- This story currently blocks benchmark task 06F8KZK2MSFQP9G2DBM61ZVGD4, so unresolved provider outcome here will cascade into downstream evidence work.

## Split Recommendations
- Keep the story whole if implementation stays limited to candidate evaluation, provider-package registration, gate coverage, and explicit decline evidence inside the existing PIT and bridge architecture.
- Split by provider only if MySQL and Oracle diverge enough that one ships a candidate path while the other needs a decline-only outcome or materially different live-provider validation work.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Add or deliberately decline MySQL and Oracle optimized read strategy candidates with explicit evidence, diagnostics, provider limitations, and safe fallback gates.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Implemented MySQL and Oracle PIT/bridge read strategy candidates on the existing provider-neutral read boundary.
- `AddDVaultMySql()` now registers `MySqlDataVaultReadStrategy` for maintained PIT and bridge reads across both `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`.
- `AddDVaultOracle()` now registers `OracleDataVaultReadStrategy` for maintained PIT and bridge reads under `Oracle.EntityFrameworkCore`.

### Verification
- `dotnet build DVault.slnx --nologo --no-restore` passed with 0 errors. Existing warning noise remained, including NU1900 from the sandbox read-only NuGet vulnerability cache.
- `dotnet test DVault.slnx --nologo --no-build --no-restore` passed. External provider integration lanes were skipped where their connection-string environment variables were absent.
- `bash tools/check-format.sh` passed.

### Downstream Handoff
- Provider-matrix, benchmark-row, and broad documentation wording remain scoped to downstream tickets `06F8KZK2MSFQP9G2DBM61ZVGD4` and `06F8KZKFTCC0YXAPRTXA53DNEC`.
<!-- gicket-bot:developer-delivery:v1:end -->