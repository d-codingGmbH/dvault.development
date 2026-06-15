<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the DB2 benchmark/test-lane task around adding DB2 as a first-class optional benchmark provider and aligning docs/tests with the existing provider-evidence contract.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence shows the benchmark harness currently supports SQLite plus four optional external providers (PostgreSQL, SQL Server, MySQL, Oracle); DB2 is not yet part of `--provider`, `optionalProviders`, benchmark project conditional references, or benchmark provider definitions.
- Repository evidence also shows DB2 runtime support already exists through `AddDVaultDb2()`, `Db2DataVaultSaveStrategy`, `Db2DataVaultReadStrategy`, and opt-in smoke/integration lanes gated by `DVAULT_TEST_DB2_CONNECTION_STRING`.
- The bounded default for this ticket is to add DB2 to the existing artifact triplet and provider-matrix shape rather than inventing a DB2-specific benchmark format, artifact name, or separate evidence schema.
- The ticket has no substantive human comments or attachments beyond bot claim/lease metadata, so repository docs and code are the authoritative refinement inputs.
- Live relation inspection found an incoming `blocks` link from done story `06FBSC3N7ZFVQW3AV2JJ8T7Q7W` and an outgoing `blocks` link to todo task `06FBSC4BEBGSVVTJSQXM1Z74CC`; the done-story link is historical and non-blocking under the current ticket state.

### Scope In
- Add DB2 as an optional benchmark provider in the existing benchmark harness, provider-filter surface, optional-provider discovery context, and root artifact triplet row generation.
- Document the DB2 benchmark lane and the existing DB2 external test lane using the established `DVAULT_TEST_DB2_CONNECTION_STRING` opt-in posture and developer-managed database/container boundary.
- Update benchmark/verifier tests so DB2 appears in the same skipped-or-completed optional-provider matrix shape as the other external providers.
- Update canonical provider-evidence/performance documentation that currently states no DB2 benchmark lane exists, so repository guidance stays consistent after the harness change.

### Scope Out
- Adding a provider-specific DB2 latest-satellite read strategy; DB2 latest/as-of latest-satellite reads remain provider-neutral.
- Adding DB2-specific artifact filenames, schema changes, or a DB2-only evidence manifest.
- Provisioning Docker/Podman containers, DB2 licenses, users, schemas, CI infrastructure, or checked-in machine-specific credentials.
- Claiming new DB2 timing wins without configured local execution; unconfigured runs may still emit skipped placeholder rows.

## Acceptance Criteria
- The benchmark command accepts DB2 as part of the existing optional-provider flow, including provider discovery from `DVAULT_TEST_DB2_CONNECTION_STRING`, the normal provider-filter surface, and the root artifact triplet context.
- When DB2 is not configured or unreachable, `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` still include deterministic DB2 skipped placeholder rows and optional-provider context entries with normalized skip reasons.
- When DB2 is configured, the benchmark matrix can record DB2 provider-neutral fallback plus DB2 provider-specific evidence rows for the bounded scenarios already supported by repository code: clean-context save and PIT/bridge read guidance under the existing contract.
- Benchmark/verifier tests are updated so the persisted matrix shape, optional-provider count, expected rows, and planned/selected execution-detail tokens cover DB2 consistently.
- Documentation tells a developer exactly how to opt into the DB2 benchmark lane and DB2 external test lane, while preserving the current non-goals around repo-managed database/container provisioning.
- Canonical docs no longer contradict each other about whether a DB2 benchmark lane exists.

## Definition of Done
- Benchmark project references, provider definitions, provider filters, helper mappings, and temporary external-database coverage needed for DB2 execution are in place or explicitly skipped under the established optional-provider contract.
- Root benchmark artifacts and their verifier tests reflect the DB2 optional-provider surface without changing the shared artifact schema.
- Benchmark README and related validation/performance docs describe the DB2 opt-in workflow, connection-string gate, and matrix behavior using current repository terminology.
- Any documentation that previously said DB2 has no benchmark lane is updated to the new bounded posture and stays aligned with the root artifact triplet.

## Implementation Notes
- Use the existing benchmark contract as-is: extend `BenchmarkExternalProviderDefinitions`, `BenchmarkDatabaseProviders`, provider filters/options/usage text, strategy/baseline helpers, and row expectations instead of creating a parallel DB2 path.
- Mirror the other optional providers' shape: provider discovery should land in `context.optionalProviders`, and unconfigured/unreachable DB2 runs should preserve row identity with `executionStatus=skipped`, `iterations=0`, blank/null metrics, deterministic `executionDetail`, and `persistedOutcome=not executed`.
- Ratify the visible DB2 capability boundary from repository code and docs: `AddDVaultDb2()` supports optimized clean-context save plus PIT/bridge read dispatch; latest-satellite stays provider-neutral; live-schema reading remains unsupported.
- Re-use the existing DB2 integration posture and example connection string from `docs/local-validation.md` and `docs/releases/v0.34.0.md` rather than inventing a new provisioning contract.
- The benchmark project currently lacks DB2 conditional package references and a DB2 temp database implementation; this ticket should add the missing benchmark-lane plumbing rather than revisiting provider runtime behavior.
- No persistent planning document, child ticket, or relation mutation was materialized during this refinement run.

## Open Questions
- none

## Follow-Up Questions
- If DB2 benchmark runs later produce stable completed timing evidence, decide whether to check in a dedicated artifact bundle comparable to the existing v0.32 external-provider bundles.

## Risks
- Several canonical docs currently state that no DB2 benchmark lane exists; landing only part of the documentation sweep would leave conflicting guidance.
- DB2 remains an external opt-in dependency, so local or CI environments without a reachable DB2 instance can only validate skipped placeholder behavior, not completed DB2 timing evidence.
- DB2 benchmark execution needs new benchmark-project conditional package restore and a DB2 temp database path; if either is missed, the lane will document support without actually producing matrix rows.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Document and, where useful, wire the DB2 optional connection/container lane into provider benchmark instructions. Acceptance: DB2 evidence can be collected with the same provider matrix shape as other providers.