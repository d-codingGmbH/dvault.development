<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refinement ratifies the current branch baseline: build on the existing internal DVM2001-DVM2006 migration-operation diagnostics, productize them into a public/reportable API, extend coverage to PIT and bridge generated tables, and keep docs to one minimal pre-apply example.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- PIT coverage in this story targets the current DataVaultPitMetadata/DataVaultTableKind.Pit generated-table baseline, not the older DataVaultPointInTimeMetadata surface called out as separate in docs/plans/deferred-data-vault-capabilities.md.
- CLI/CI-friendly report path means a public structured result plus deterministic human-readable rendering that consumer-owned console, test, or build steps can call; this story does not require a first-party standalone CLI executable or runtime-owned CI workflow.
- Use the existing repo baseline DVM2001-DVM2006 meanings where the same invariant still applies, and add new DVM2xxx codes only when PIT- or bridge-specific invariants need distinct stable semantics.

### Scope In
- Analyze generated EF Core MigrationOperation collections against DVault-produced schema baselines derived from existing diagnostics explain metadata rather than SQL text.
- Cover generated hub, link, satellite, PIT, and bridge tables represented by the current DataVaultTableKind and DataVaultPropertyRole baselines.
- Detect risky table drops, risky add/drop/alter/rename column operations, and broken primary-key/index/uniqueness contracts on DVault-owned structures.
- Report insert-only violations for hub and link tables, including payload-column additions that belong in satellites instead of insert-only core tables.
- Return stable DVM diagnostic codes, deterministic issue paths/messages, and remediation guidance through a reusable consumer-facing report surface.
- Add one concise pre-apply usage example for local scripts or CI/build/test integration without requiring a live database.

### Scope Out
- Automatic migration execution, rollback, or DDL rewrite.
- SQL string parsing, provider-specific DDL diffing, or a full live-database schema diff engine.
- A standalone first-party CLI/tool package or repository-owned CI workflow definitions.
- PIT row refresh, bridge population/maintenance, or any change to save-service/write-path behavior.
- Reconciliation, renaming, or deprecation work for the older DataVaultPointInTimeMetadata surface.
- A broad adoption-guide or checklist rewrite beyond the minimal usage snippet needed to make the guardrail discoverable.

## Acceptance Criteria
- Consumers can run the guardrail against generated MigrationOperation input and a DVault metadata baseline from a metadata model, registry, Code-First declaration, or configured DbContext, and the analysis does not require a live database connection.
- Safe changes remain quiet: non-DVault tables are ignored, and safe satellite payload evolution does not emit findings.
- Risky changes to DVault-owned hub, link, satellite, PIT, or bridge tables emit stable DVM diagnostics with deterministic severity, code, path, message, and remediation guidance.
- Guardrails cover required technical columns, stable key/parent/participant/driving columns, PIT snapshot-reference columns, hierarchy bridge TraversalDepth, DVault-owned table drops, and missing or mismatched DVault primary-key/index/uniqueness contracts.
- Hub and link payload-column additions are reported as insert-only violations instead of being treated as safe schema growth.
- Documentation includes one pre-integration example that shows how to surface the structured result and fail a local script or CI/build step before applying a migration.

## Definition of Done
- The chosen reusable guardrail API is public, covered by API snapshot updates if needed, and returns a stable diagnostics/report contract suitable for automation.
- Unit tests cover quiet and finding cases across hub, link, satellite, PIT, and bridge baselines with representative EF migration operation types.
- Any new migration guardrail catalog entries define code, severity, category, summary, explanation, and remediation in the central diagnostics catalog pattern.
- Integration coverage proves the guardrail can run from a configured DbContext without applying a migration or requiring a live database round-trip.
- A minimal doc/example is added and kept consistent with current package names, current branch limitations, and the no-SQL-parsing design.

## Implementation Notes
- Reuse DataVaultDiagnosticsResult.Explain.Entities plus DataVaultEntityExplain properties, indexes, and constraints as the schema baseline so migration checks compare against the same provider-neutral metadata already exposed by diagnostics.
- Prefer EF Core migration-operation metadata over SQL parsing.
- Use the current branch PIT and bridge baselines as-is: Pit tables come from DataVaultPitMetadata, bridge tables come from DataVaultBridgeMetadata, and this story should not introduce foreign keys, navigations, or provider-specific DDL promises.
- Keep issue ordering and path rendering deterministic; the current branch already uses migration/<Operation>/<Target>/<Member> style paths and that is a good bounded default for automation.
- Surface remediation guidance from the stable diagnostics catalog rather than ad hoc test-only strings, and reuse existing DVM2001-DVM2006 semantics when the invariant meaning is unchanged.

## Open Questions
- none

## Follow-Up Questions
- After the public API exists, does DVault want a standalone dotnet tool or should CLI ownership stay with consuming repositories and their build/test scripts?
- Should a later docs-focused ticket add repository-agnostic CI snippets for GitHub Actions/Azure Pipelines once the broader adoption guide lands?
- Should a later tooling ticket expose diagnostic definition metadata as a first-class public lookup API for richer IDE/build integrations?

## Risks
- EF Core providers can express equivalent schema changes through different MigrationOperation sequences, so guardrail coverage must stay high-confidence without creating noisy false positives or false negatives.
- PIT and bridge baselines are narrower and more opt-in than hubs/links/satellites; incorrect mapping of snapshot-reference columns, TraversalDepth, or bridge traversal indexes will create misleading findings.
- The current public diagnostics issue shape does not obviously carry remediation text, so exposing guidance for automation may require a careful API extension or adjacent report surface.
- The repository still contains older point-in-time terminology, so docs/examples must clearly distinguish DataVaultPitMetadata from legacy DataVaultPointInTimeMetadata to avoid adoption confusion.

## Split Recommendations
- Keep this story limited to the guardrail API/report contract, diagnostic taxonomy, PIT/bridge baseline coverage, and one minimal pre-apply usage snippet.
- Route broader README/example/checklist work to existing docs story 06F1XQ2MB5Y9JW25W2CWVZZ9G4 and checklist task 06F1XQ3006JYSJT5EHT05GV1HG instead of growing this ticket.
- If a standalone CLI package is still desired after the reusable API exists, split it as separate adoption tooling work rather than expanding this story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Detect risky or invalid EF migration operations before teams apply schema changes to Data Vault tables.

## Scope In

- Inspect generated EF migration operations for hubs, links, satellites, PITs, and bridges.
- Report dangerous drops, lossy renames, missing technical columns, missing uniqueness/index contracts, and insert-only violations.
- Expose reusable API plus CLI/CI-friendly report path.
- Use stable DVault diagnostic codes.

## Scope Out

- No automatic migration execution or rollback.
- No provider-specific DDL rewrite engine.
- No full database diff implementation.

## Acceptance Criteria

- Safe migrations pass without findings.
- Dangerous operations produce stable diagnostics and remediation guidance.
- The guardrail can run without a live database.
- Docs show pre-integration usage.

## Implementation Notes

- Prefer EF Core migration operation metadata over SQL string parsing.

## Open Questions

- none