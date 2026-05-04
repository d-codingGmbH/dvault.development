[gicket-bot] PO refinement contract

Summary
- Refined the ticket to add Oracle capability-profile support to the shared provider contract, introduce an Oracle-bound optimized save-strategy path with deterministic fallback, and keep the current explicit save-service baseline intact.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The shared provider optimization contract is already fixed in core: provider capability profiles, IDataVaultProviderSaveStrategy, and fallback dispatch stay in src/DCoding.Data.DVault; Oracle-specific SQL and registration stay in src/DCoding.Data.DVault.Oracle.
- SQLite remains the current default translation and profile baseline; Oracle work is additive and must not break the existing no-argument AddDVault(), UseDataVault(), or ApplyDataVaultMetadata() path.
- Fallback behavior means the existing provider-neutral IDataVaultSaveService writer handles any request batch the Oracle strategy does not accept.

Scope In
- Expose an Oracle v1 capability profile through the shared provider-capability contract, covering the existing logical property kinds and explicit unsupported SQL-function and concurrency baselines.
- Add a provider-aware model-configuration path so Oracle consumers can project metadata with Oracle profile annotations instead of the hardcoded SQLite profile while preserving the current default path.
- Register an Oracle-owned optimized save strategy through AddDVaultOracle() and the shared IDataVaultProviderSaveStrategy contract.
- Gate the Oracle strategy by Oracle provider identity and supported request or context shape, with deterministic fallback to the built-in writer for anything unsupported.
- Add or update automated coverage for Oracle profile contents, dispatch selection, fallback behavior, API snapshots, and package dependency isolation.

Scope Out
- Mandatory local or CI-backed Oracle database infrastructure.
- Provider-specific DDL or index tuning, merge or upsert semantics, multi-writer concurrency guarantees, or non-MVP SQL-function expansion beyond the declared Oracle capability profile.
- Cross-provider redesign of the explicit save-service contract or SaveChanges interception.
- Oracle benchmark baselines.
- Deferred capabilities such as PIT, bridge, or multi-active satellite automation.

Open questions
- none

Follow-up questions
- Should a later ticket add external opt-in Oracle integration coverage once the first real Oracle SQL path ships?
- Should the provider-aware model-configuration surface added for Oracle be rolled across Postgres, MySql, and SQL Server in follow-up work instead of leaving them on the default SQLite profile?
- After the first Oracle optimized path lands, do we want Oracle-specific benchmark scenarios similar to the current SQLite benchmark baseline?

Risks
- Without Oracle-backed integration infrastructure in this ticket, provider-specific SQL correctness will rely mostly on unit and smoke coverage and may leave provider-runtime edge cases for later validation.
- Any additive core model-configuration API introduced for provider selection becomes a long-term public compatibility commitment.
- Whole-batch fallback keeps behavior safe but can reduce performance when a batch mixes shapes the Oracle strategy can and cannot optimize.

Split recommendations
- If the ticket grows, separate the shared Oracle capability-profile and model-selection work in src/DCoding.Data.DVault from the Oracle save-strategy implementation in src/DCoding.Data.DVault.Oracle.
- If provider-specific SQL needs real Oracle runtime proof, schedule Oracle integration harness and contract coverage as follow-up validation work instead of inflating this refinement ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment