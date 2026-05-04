<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to add Oracle capability-profile support to the shared provider contract, introduce an Oracle-bound optimized save-strategy path with deterministic fallback, and keep the current explicit save-service baseline intact.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The shared provider optimization contract is already fixed in core: provider capability profiles, IDataVaultProviderSaveStrategy, and fallback dispatch stay in src/DCoding.Data.DVault; Oracle-specific SQL and registration stay in src/DCoding.Data.DVault.Oracle.
- SQLite remains the current default translation and profile baseline; Oracle work is additive and must not break the existing no-argument AddDVault(), UseDataVault(), or ApplyDataVaultMetadata() path.
- Fallback behavior means the existing provider-neutral IDataVaultSaveService writer handles any request batch the Oracle strategy does not accept.

### Scope In
- Expose an Oracle v1 capability profile through the shared provider-capability contract, covering the existing logical property kinds and explicit unsupported SQL-function and concurrency baselines.
- Add a provider-aware model-configuration path so Oracle consumers can project metadata with Oracle profile annotations instead of the hardcoded SQLite profile while preserving the current default path.
- Register an Oracle-owned optimized save strategy through AddDVaultOracle() and the shared IDataVaultProviderSaveStrategy contract.
- Gate the Oracle strategy by Oracle provider identity and supported request or context shape, with deterministic fallback to the built-in writer for anything unsupported.
- Add or update automated coverage for Oracle profile contents, dispatch selection, fallback behavior, API snapshots, and package dependency isolation.

### Scope Out
- Mandatory local or CI-backed Oracle database infrastructure.
- Provider-specific DDL or index tuning, merge or upsert semantics, multi-writer concurrency guarantees, or non-MVP SQL-function expansion beyond the declared Oracle capability profile.
- Cross-provider redesign of the explicit save-service contract or SaveChanges interception.
- Oracle benchmark baselines.
- Deferred capabilities such as PIT, bridge, or multi-active satellite automation.

## Acceptance Criteria
- The shared capability-profile surface exposes an Oracle profile that declares mappings for HashKey, HashDiff, LoadTimestamp, RecordSource, ParticipantReference, BusinessKey, and PayloadText, plus explicit unsupported SQL-function and concurrency baselines.
- There is a supported Oracle model-configuration path that results in Oracle profile annotations and Oracle-native storage metadata on translated properties, while the existing default path still emits the current SQLite baseline.
- AddDVaultOracle() wires Oracle provider capability registration through the shared contract, and the Oracle package does not introduce a dependency on any non-Oracle DVault provider package or non-Oracle database provider package.
- When the current DbContext or ordered request batch falls outside the Oracle strategy's supported shape, the strategy declines selection and the dispatcher completes the save through the existing provider-neutral IDataVaultSaveService path.
- Automated coverage proves Oracle profile contents, Oracle registration and selection behavior, fallback behavior, and package or API verification expectations.

## Definition of Done
- Relevant unit, smoke, or contract tests are added or updated for core and Oracle projects, including the existing assertions that currently treat Oracle as compatibility-only.
- Any new public core API surface required for provider selection has approved API snapshot updates and XML documentation.
- Package verification still passes for DCoding.Data.DVault.Oracle, including README, XML documentation, symbol-package, and core-version alignment expectations.
- Existing SQLite optimized-path behavior and provider-neutral fallback behavior remain unchanged and covered by passing tests.

## Implementation Notes
- Keep the Oracle capability profile in the shared DataVaultProviderCapabilityProfiles surface beside Sqlite so profile selection remains provider-neutral at the contract level.
- Use the existing logical-property vocabulary and annotation keys rather than introducing Oracle-only metadata concepts.
- Preserve current strategy dispatch semantics: descending Priority, dependency-injection registration order as the tie-break, and whole-batch CanSave evaluation before dispatch.
- Keep the Oracle package boundary narrow: core project reference plus only shared abstractions needed for strategy execution; avoid references to Sqlite, Postgres, MySql, or SqlServer packages.
- If the Oracle optimized path supports only a subset of explicit save batches, reject the entire batch up front in CanSave so the fallback writer handles it deterministically.
- Update provider smoke tests, save-strategy selection tests, capability-profile tests, and public API snapshots to reflect Oracle no longer being compatibility-only once the new strategy path is added.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add external opt-in Oracle integration coverage once the first real Oracle SQL path ships?
- Should the provider-aware model-configuration surface added for Oracle be rolled across Postgres, MySql, and SQL Server in follow-up work instead of leaving them on the default SQLite profile?
- After the first Oracle optimized path lands, do we want Oracle-specific benchmark scenarios similar to the current SQLite benchmark baseline?

## Risks
- Without Oracle-backed integration infrastructure in this ticket, provider-specific SQL correctness will rely mostly on unit and smoke coverage and may leave provider-runtime edge cases for later validation.
- Any additive core model-configuration API introduced for provider selection becomes a long-term public compatibility commitment.
- Whole-batch fallback keeps behavior safe but can reduce performance when a batch mixes shapes the Oracle strategy can and cannot optimize.

## Split Recommendations
- If the ticket grows, separate the shared Oracle capability-profile and model-selection work in src/DCoding.Data.DVault from the Oracle save-strategy implementation in src/DCoding.Data.DVault.Oracle.
- If provider-specific SQL needs real Oracle runtime proof, schedule Oracle integration harness and contract coverage as follow-up validation work instead of inflating this refinement ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: add Oracle provider capability registration and the optimized writer boundary needed for provider-specific SQL.

Acceptance Criteria:
- Oracle capabilities are registered through the shared provider contract.
- The provider package contains no accidental dependency on another database provider package.
- Unsupported write shapes route to fallback behavior.