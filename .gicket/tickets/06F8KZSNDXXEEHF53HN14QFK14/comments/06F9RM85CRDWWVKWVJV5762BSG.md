[gicket-bot] PO-critic review contract

Summary
- Delivery contract is bounded to existing quickstart and diagnostics surfaces with no unresolved Open Questions, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZSNDXXEEHF53HN14QFK14/description.md contains '## Open Questions' with '- none' and a bounded contract with 5 acceptance-criteria items and 4 definition-of-done items.
- .gicket/tickets/06F8KZSNDXXEEHF53HN14QFK14/comments/06F9RHWNBC5KGWYPSTJ1FE1AYM.md records PO handoff decision 'ready_for_po_critic' and ties scope to the existing shared quickstart pair under examples/.
- examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs already contains the shared Customer/CustomerProfile scenario with explicit IDataVaultSaveService.SaveAsync(...) calls and latest/as-of IDataVaultReadService.ReadLatestSatelliteAsync(...) reads.
- examples/DCoding.Data.DVault.SqliteQuickstart/Program.cs uses AddDVaultSqlite() with a temp SQLite file, and examples/DCoding.Data.DVault.PostgresQuickstart/Program.cs exits cleanly with a skip message when DVAULT_TEST_POSTGRES_CONNECTION_STRING is unset.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers IDataVaultDiagnosticsService and IDataVaultReadDiagnosticsService, and src/DCoding.Data.DVault/DataVaultDiagnostics.cs defines both request-bound diagnostics interfaces referenced by the contract.
- git diff --name-status 4a7463f32bdd281810e90fb9c9503e297fe7fce8..HEAD returned no paths, and git diff --name-only develop..HEAD listed only .gicket/tickets/06F8KZSNDXXEEHF53HN14QFK14/**, so there is no partial example or README implementation to reconcile on this branch.
- .gicket/relations/14/18/06F8KZSNDXXEEHF53HN14QFK14--06F8KZSYCVZ21MS983501BZG18--blocks.json shows the downstream release-doc dependency, .gicket/tickets/06F8KZSYCVZ21MS983501BZG18/ticket.json is still 'todo', and docs/releases/v0.31.0.md is currently missing, so release-note follow-through is already tracked separately.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not pin the diagnostics example to save-strategy versus read-strategy diagnostics; either bounded request-level surface appears acceptable from the current text.
- The contract reuses the customer-profile domain but does not require literal parity with docs/plans/customer-profile-comparison-contract.md values such as C-100, crm-import, and crm-change.

Risky assumptions
- Implementation will keep any diagnostics snippet limited to redacted facts such as strategy status, selected strategy name, fallback presence, or read-shape category, without printing raw SQL or provider messages.
- v0.31 guardrail wording can be added in examples/README.md without this ticket also owning the separate release-note file docs/releases/v0.31.0.md.
- SQLite remains the default proof path and PostgreSQL remains optional; developers should not reinterpret the contract as requiring live PostgreSQL validation for normal success.

AC / test suggestions
- Use the existing PostgreSQL skip behavior as an explicit acceptance proof so the optional-provider boundary is observable, not implied.
- Check the chosen diagnostics example against the contract redaction list: no business keys, hash keys, payload values, raw SQL, connection strings, or provider message text.
- If exact parity with the comparison-contract event literals matters, capture that in a follow-up comment or test note during dev handoff; the current contract does not require it.

Implementation watchouts
- Keep work inside the existing examples quickstart pair and named documentation surfaces; do not widen into new runnable sample families or hosted observability assets.
- Do not let the example imply automatic PIT/bridge maintenance, runtime routing, exporters, dashboards, collectors, or schedulers beyond the linked docs contracts.
- Current branch content is ticket metadata only, so developers should plan fresh example/documentation work rather than assume an unfinished implementation already exists.

Non-blocking notes
- Only two live relations were observed for this ticket: parent epic 06F8KZQNH8CCMTJW9P95W1N388 and outgoing blocks to release-doc ticket 06F8KZSYCVZ21MS983501BZG18; no incoming blocker relation was found.
- The current branch head 4a7463f32 is a 'lease claim po-critic' metadata commit, which matches the absence of product-file diffs.
- examples/README.md already documents the shared quickstart pair and telemetry/tracing guardrails, so this ticket is extending an existing documentation surface rather than defining it from scratch.

Split recommendations
- No split recommended; the example plus README work remains bounded to one shared quickstart surface.
- If a later request needs provider-specific or observability-heavy samples, create a follow-up ticket instead of broadening 06F8KZSNDXXEEHF53HN14QFK14.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment