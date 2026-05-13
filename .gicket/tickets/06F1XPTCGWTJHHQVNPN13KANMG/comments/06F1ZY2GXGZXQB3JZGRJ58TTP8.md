[gicket-bot] PO refinement contract

Summary
- Refinement ratifies the current branch baseline: build on the existing internal DVM2001-DVM2006 migration-operation diagnostics, productize them into a public/reportable API, extend coverage to PIT and bridge generated tables, and keep docs to one minimal pre-apply example.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- PIT coverage in this story targets the current DataVaultPitMetadata/DataVaultTableKind.Pit generated-table baseline, not the older DataVaultPointInTimeMetadata surface called out as separate in docs/plans/deferred-data-vault-capabilities.md.
- CLI/CI-friendly report path means a public structured result plus deterministic human-readable rendering that consumer-owned console, test, or build steps can call; this story does not require a first-party standalone CLI executable or runtime-owned CI workflow.
- Use the existing repo baseline DVM2001-DVM2006 meanings where the same invariant still applies, and add new DVM2xxx codes only when PIT- or bridge-specific invariants need distinct stable semantics.

Scope In
- Analyze generated EF Core MigrationOperation collections against DVault-produced schema baselines derived from existing diagnostics explain metadata rather than SQL text.
- Cover generated hub, link, satellite, PIT, and bridge tables represented by the current DataVaultTableKind and DataVaultPropertyRole baselines.
- Detect risky table drops, risky add/drop/alter/rename column operations, and broken primary-key/index/uniqueness contracts on DVault-owned structures.
- Report insert-only violations for hub and link tables, including payload-column additions that belong in satellites instead of insert-only core tables.
- Return stable DVM diagnostic codes, deterministic issue paths/messages, and remediation guidance through a reusable consumer-facing report surface.
- Add one concise pre-apply usage example for local scripts or CI/build/test integration without requiring a live database.

Scope Out
- Automatic migration execution, rollback, or DDL rewrite.
- SQL string parsing, provider-specific DDL diffing, or a full live-database schema diff engine.
- A standalone first-party CLI/tool package or repository-owned CI workflow definitions.
- PIT row refresh, bridge population/maintenance, or any change to save-service/write-path behavior.
- Reconciliation, renaming, or deprecation work for the older DataVaultPointInTimeMetadata surface.
- A broad adoption-guide or checklist rewrite beyond the minimal usage snippet needed to make the guardrail discoverable.

Open questions
- none

Follow-up questions
- After the public API exists, does DVault want a standalone dotnet tool or should CLI ownership stay with consuming repositories and their build/test scripts?
- Should a later docs-focused ticket add repository-agnostic CI snippets for GitHub Actions/Azure Pipelines once the broader adoption guide lands?
- Should a later tooling ticket expose diagnostic definition metadata as a first-class public lookup API for richer IDE/build integrations?

Risks
- EF Core providers can express equivalent schema changes through different MigrationOperation sequences, so guardrail coverage must stay high-confidence without creating noisy false positives or false negatives.
- PIT and bridge baselines are narrower and more opt-in than hubs/links/satellites; incorrect mapping of snapshot-reference columns, TraversalDepth, or bridge traversal indexes will create misleading findings.
- The current public diagnostics issue shape does not obviously carry remediation text, so exposing guidance for automation may require a careful API extension or adjacent report surface.
- The repository still contains older point-in-time terminology, so docs/examples must clearly distinguish DataVaultPitMetadata from legacy DataVaultPointInTimeMetadata to avoid adoption confusion.

Split recommendations
- Keep this story limited to the guardrail API/report contract, diagnostic taxonomy, PIT/bridge baseline coverage, and one minimal pre-apply usage snippet.
- Route broader README/example/checklist work to existing docs story 06F1XQ2MB5Y9JW25W2CWVZZ9G4 and checklist task 06F1XQ3006JYSJT5EHT05GV1HG instead of growing this ticket.
- If a standalone CLI package is still desired after the reusable API exists, split it as separate adoption tooling work rather than expanding this story.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment