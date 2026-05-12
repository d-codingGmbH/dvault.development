[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is otherwise bounded and has no persisted open questions, but it depends on a provider read-strategy hook that is not present in the current source despite the related hook ticket being marked done/integrated.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current ticket description lines 12-21 scopes SQLite latest/as-of satellite reads through the provider read-strategy hook and requires before/after benchmark evidence; lines 53-54 record Open Questions as none.
- Relation files show blockers 06F0MEJ7NANHCP64VR1SH3S3G8 -> 06F0MEJE5WC51MFQ3CWDRATCWC and 06F0MEJ0NE80R7CNS982S3PKVR -> 06F0MEJE5WC51MFQ3CWDRATCWC.
- Direct source search `rg -n "ReadStrategy|read-strategy|ProviderReadStrategy|IDataVaultProviderRead|DataVaultProviderRead|IDataVaultReadStrategy|ReadStrategyContext" src tests docs README.md benchmarks` exited 1 with no output.
- src/DCoding.Data.DVault/DefaultDataVaultReadService.cs lines 5-43 routes latest/as-of raw and projection reads directly to DataVaultSatelliteReadPipeline; no dispatcher is present there.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs lines 16-29 registers IDataVaultReadService to DefaultDataVaultReadService but no read strategy collection.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs lines 22-30 calls AddDVault and registers SQLite provider behavior/save strategy only; no SQLite read strategy registration exists.
- Benchmark blocker 06F0MEJ0NE80R7CNS982S3PKVR is done, and its integrator comment lines 5-12 records ACCEPT squash into develop.
- benchmarks/DCoding.Data.DVault.Benchmarks/ReadModelBenchmarks.cs lines 27 and 53-81 define latest-satellite-read over customer profile history and report 100 latest rows from 1000 seeded profile states; README lines 18 and 72 document the latest/PIT/bridge read matrix.

Blocking findings
- The delivery contract depends on an existing completed provider read-strategy hook, but current source has no source-backed hook contract, dispatcher, diagnostics surface, or AddDVaultSqlite registration point to implement against.
- Line 51 asks dev to align to the completed hook contract if local symbols are absent, but the related done ticket's own closure evidence says the hook is absent. That turns this SQLite optimization ticket into an implicit core public API/diagnostics implementation, which is outside its stated bounded scope.

Required PO actions
- Reconcile ticket 06F0MEJ7NANHCP64VR1SH3S3G8: either reopen/replace it so the core read-strategy hook actually lands, or explicitly expand this ticket to own the hook implementation with separate AC/DoD.
- Update this ticket with direct source-backed hook evidence before dev handoff: expected type/interface names, dispatcher entry point, diagnostics contract, and provider registration path.
- Keep this ticket out of dev handoff until the source branch exposes the hook or the contract clearly states that this ticket owns both the core hook and SQLite strategy work.

Open issues ledger
- critic-item-1 [required-po-action] Reconcile ticket 06F0MEJ7NANHCP64VR1SH3S3G8: either reopen/replace it so the core read-strategy hook actually lands, or explicitly expand this ticket to own the hook implementation with separate AC/DoD.
- critic-item-2 [required-po-action] Update this ticket with direct source-backed hook evidence before dev handoff: expected type/interface names, dispatcher entry point, diagnostics contract, and provider registration path.
- critic-item-3 [required-po-action] Keep this ticket out of dev handoff until the source branch exposes the hook or the contract clearly states that this ticket owns both the core hook and SQLite strategy work.
- critic-item-4 [blocking-finding] The delivery contract depends on an existing completed provider read-strategy hook, but current source has no source-backed hook contract, dispatcher, diagnostics surface, or AddDVaultSqlite registration point to implement against.
- critic-item-5 [blocking-finding] Line 51 asks dev to align to the completed hook contract if local symbols are absent, but the related done ticket's own closure evidence says the hook is absent. That turns this SQLite optimization ticket into an implicit core public API/diagnostics implementation, which is outside its stated bounded scope.

Missing examples / edge cases
- Need a concrete supported-shape definition for the SQLite strategy: ordinary hub/link parent satellite, driving-key/multi-active handling, payload column coverage, and timestamp storage modes that must decline.
- Need explicit parity expectations for duplicate load timestamps, since fallback currently replaces the latest row on >= timestamp comparison.
- Need an example of the fallback path for unsupported shapes/providers once the actual read-strategy decline API exists.

Risky assumptions
- The current AddDVaultSqlite benchmark row is labeled sqlite-optimized, but for reads it still resolves the same DefaultDataVaultReadService path until a real read strategy exists.
- Benchmark timing evidence will be machine-specific, so accepting mean-time improvement without attached options/run context would be weak.

AC / test suggestions
- Add or require tests proving AddDVaultSqlite selects the SQLite read strategy through the real hook and AddDVault uses provider-neutral fallback.
- Add tests comparing optimized and fallback rows for latest and as-of reads across metadata/table names, parent hash keys, hash diff, load timestamp, record source, payload values, and ordering.
- Add decline tests for unsupported providers, read families, metadata shapes, and timestamp storage modes once those decline reasons are concrete.
- Add a benchmark artifact/comment requirement that includes command line, --provider sqlite, iterations/warmup, load timestamp storage, machine/runtime context, and before/after mean rows.

Implementation watchouts
- DefaultDataVaultReadService currently calls DataVaultSatelliteReadPipeline directly for raw and projection reads; typed projection parity requires routing the relevant projection path through the same selection/fallback behavior or proving equivalent output.
- DataVaultSatelliteReadPipeline currently queries all rows for requested parent batches and selects latest in memory with >= timestamp; SQLite SQL must match that tie behavior or define a deliberate deterministic tie rule.
- AddDVaultSqlite currently registers save strategy behavior; read strategy registration must not disturb existing write strategy selection or public API snapshots.
- Use existing naming policy and DataVaultLoadTimestampValueConverter behavior; do not concatenate parent hash key values into SQL.

Non-blocking notes
- The persisted Open Questions section is none, so the return is not due to unanswered PO questions.
- The benchmark baseline task is done and integrated; the remaining blocker is the missing source-backed read-strategy hook.
- The downstream docs/release ticket 06F0MEJPGG7JBFEXD693BHY07W is blocked by this ticket and does not block this dev-handoff decision.

Split recommendations
- Prefer reopening or replacing the core read-strategy hook ticket as a blocking predecessor, then keep this ticket scoped to the SQLite latest/as-of read strategy and benchmark proof.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment