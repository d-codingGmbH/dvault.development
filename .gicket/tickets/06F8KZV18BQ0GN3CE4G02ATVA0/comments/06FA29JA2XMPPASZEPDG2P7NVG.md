[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff as a bounded dry-run SQL Server manifest prototype; the persisted contract is specific and has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- '.gicket/tickets/06F8KZV18BQ0GN3CE4G02ATVA0/description.md' contains the persisted Delivery Contract, sets PO Handoff to 'ready_for_po_critic', narrows scope to the SQL Server 'provider-native-bulk-ingestion' prototype, and has '## Open Questions' = 'none'.
- '.gicket/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/ticket.json' is 'done'; '.gicket/relations/HG/A0/06F8KZTNG44XDPMVTVCV4WJSHG--06F8KZV18BQ0GN3CE4G02ATVA0--blocks.json' and '.gicket/relations/8M/A0/06F8KZTCEMNNFBFTVMFXEN268M--06F8KZV18BQ0GN3CE4G02ATVA0--parentOf.json' match the stated parent/epic context.
- 'docs/plans/provider-specific-sql-artifact-contract.md' fixes the lane to one provider and one workload, one deterministic 'dvault.sql-artifact.v1' manifest, design-time consumer ownership, and explicit non-goals against runtime dispatch, deployment, or automatic migration synchronization.
- 'docs/performance-profiles.md', 'benchmark-summary.md', and 'benchmark-summary.csv' keep the SQL Server 'provider-native-bulk-ingestion' row visible as skipped with 'DVAULT_TEST_SQLSERVER_CONNECTION_STRING' unset and record 'selectedStrategy=SqlServerDataVaultSaveStrategy', 'transfer=SqlBulkCopy', 'nativeBulkBoundary=50-plus-operations', and 'cleanupBoundary=temporary-staging-table'; the same doc states the workload includes 20 order-product pairs, 20 order-product links, and 3 ordered fulfillment satellite operations with one unchanged replay.
- 'src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs' and 'src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs' already expose the consumer-owned design-time/output-path boundary, while 'src/DCoding.Data.DVault/DataVaultAnnotationNames.cs' and 'src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs' expose 'MetadataSourceKind', 'MetadataSourceFingerprint', and 'sqlserver-v1'.
- 'src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs' confirms provider name 'Microsoft.EntityFrameworkCore.SqlServer', the 50-operation native bulk gate, the 500-satellite cap, and 'SqlBulkCopy'-based staging behavior that the ticket wants referenced.
- 'git log --oneline' on the ticket branch shows PO or PO-critic workflow commits such as '88ee539af' and '98f346d85', and 'git diff --name-only develop...HEAD' changes only '.gicket/tickets/06F8KZV18BQ0GN3CE4G02ATVA0/**'; 'dvault.sql-artifact.v1' is not yet present, which is expected pre-development.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No checked-in manifest example exists yet; review proof should cover both the zero-sidecar dry-run case and an optional manifest-relative sidecar hash case.
- The unchanged-replay slice should be shown explicitly so reviewers can see why the workload has 3 satellite operations but may persist fewer changed fulfillment rows.
- The reject or skip path for non-SQL-Server providers, out-of-slice workloads, or missing request-bound diagnostics is not exemplified yet.

Risky assumptions
- Skipped SQL Server benchmark rows will be used only as traceability anchors, not misread as completed performance evidence.
- Implementation will source provider and workload facts from request-bound diagnostics or authoritative metadata, not infer them from ambient provider registration or transient SQL.
- Field naming and reference-shape details can still be settled during implementation without widening the prototype scope.

AC / test suggestions
- Prove identical inputs produce byte-identical manifest JSON with no wall-clock timestamps, random ids, machine-specific paths, raw diagnostics text, or secrets.
- Add a contract test that the manifest records 'Microsoft.EntityFrameworkCore.SqlServer', 'sqlserver-v1', 'provider-native-bulk-ingestion', 'MetadataSourceKind', and 'MetadataSourceFingerprint' exactly.
- Add boundary tests for provider mismatch, workload drift, missing diagnostics, and optional sidecars remaining manifest-relative and content-hashed only.

Implementation watchouts
- Do not leak transient '#dvault_stage_<guid>' staging identifiers from 'SqlServerDataVaultSaveStrategy' into the manifest; the manifest must stay deterministic.
- Keep generation inside the existing consumer-owned design-time command and host boundary with caller-supplied output paths; no runtime dispatch, deployment hooks, or standalone DVault CLI.
- Keep explicit dry-run status and consumer-owned operational ownership visible so the prototype cannot be mistaken for a deployable artifact lane.

Non-blocking notes
- The ticket branch currently changes only '.gicket/tickets/06F8KZV18BQ0GN3CE4G02ATVA0/**'; there is no product-file scope creep before dev handoff.
- The ticket is still 'todo' with 'critic-needed' and no assignee, which is workflow state rather than a contract blocker.
- The epic '06F8KZTCEMNNFBFTVMFXEN268M' remains open, but the direct architecture parent is already 'done' and the child scope is independently bounded.

Split recommendations
- No additional split is needed now; keep architecture contract, prototype, downstream evidence, and documentation as separate tickets.
- If later work wants provider-matrix manifests, deployable SQL sidecars, runtime invocation helpers, or validators, create follow-up tickets instead of widening this story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment