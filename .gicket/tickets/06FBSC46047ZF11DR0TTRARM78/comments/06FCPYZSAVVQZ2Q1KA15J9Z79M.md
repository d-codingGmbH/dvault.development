[gicket-bot] PO refinement contract

Summary
- Refined the DB2 benchmark/test-lane task around adding DB2 as a first-class optional benchmark provider and aligning docs/tests with the existing provider-evidence contract.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows the benchmark harness currently supports SQLite plus four optional external providers (PostgreSQL, SQL Server, MySQL, Oracle); DB2 is not yet part of `--provider`, `optionalProviders`, benchmark project conditional references, or benchmark provider definitions.
- Repository evidence also shows DB2 runtime support already exists through `AddDVaultDb2()`, `Db2DataVaultSaveStrategy`, `Db2DataVaultReadStrategy`, and opt-in smoke/integration lanes gated by `DVAULT_TEST_DB2_CONNECTION_STRING`.
- The bounded default for this ticket is to add DB2 to the existing artifact triplet and provider-matrix shape rather than inventing a DB2-specific benchmark format, artifact name, or separate evidence schema.
- The ticket has no substantive human comments or attachments beyond bot claim/lease metadata, so repository docs and code are the authoritative refinement inputs.
- Live relation inspection found an incoming `blocks` link from done story `06FBSC3N7ZFVQW3AV2JJ8T7Q7W` and an outgoing `blocks` link to todo task `06FBSC4BEBGSVVTJSQXM1Z74CC`; the done-story link is historical and non-blocking under the current ticket state.

Scope In
- Add DB2 as an optional benchmark provider in the existing benchmark harness, provider-filter surface, optional-provider discovery context, and root artifact triplet row generation.
- Document the DB2 benchmark lane and the existing DB2 external test lane using the established `DVAULT_TEST_DB2_CONNECTION_STRING` opt-in posture and developer-managed database/container boundary.
- Update benchmark/verifier tests so DB2 appears in the same skipped-or-completed optional-provider matrix shape as the other external providers.
- Update canonical provider-evidence/performance documentation that currently states no DB2 benchmark lane exists, so repository guidance stays consistent after the harness change.

Scope Out
- Adding a provider-specific DB2 latest-satellite read strategy; DB2 latest/as-of latest-satellite reads remain provider-neutral.
- Adding DB2-specific artifact filenames, schema changes, or a DB2-only evidence manifest.
- Provisioning Docker/Podman containers, DB2 licenses, users, schemas, CI infrastructure, or checked-in machine-specific credentials.
- Claiming new DB2 timing wins without configured local execution; unconfigured runs may still emit skipped placeholder rows.

Open questions
- none

Follow-up questions
- If DB2 benchmark runs later produce stable completed timing evidence, decide whether to check in a dedicated artifact bundle comparable to the existing v0.32 external-provider bundles.

Risks
- Several canonical docs currently state that no DB2 benchmark lane exists; landing only part of the documentation sweep would leave conflicting guidance.
- DB2 remains an external opt-in dependency, so local or CI environments without a reachable DB2 instance can only validate skipped placeholder behavior, not completed DB2 timing evidence.
- DB2 benchmark execution needs new benchmark-project conditional package restore and a DB2 temp database path; if either is missed, the lane will document support without actually producing matrix rows.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment