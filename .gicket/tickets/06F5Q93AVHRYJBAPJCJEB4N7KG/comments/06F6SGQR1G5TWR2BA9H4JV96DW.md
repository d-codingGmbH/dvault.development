[gicket-bot] PO-critic review contract

Summary
- Approve for dev handoff: the delivery contract is specific, the current hashing boundary is directly evidenced in repo docs/source, and ## Open Questions is explicitly closed.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F5Q93AVHRYJBAPJCJEB4N7KG/description.md:29-50 defines 4 acceptance criteria, 4 DoD items, and ## Open Questions = none; follow-up questions are separate at lines 52-54.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:1318,1360,<redacted> computes hub and link hash keys by _stableHashNormalizer.NormalizeFields(...) followed by _stableHashService.ComputeHash(...).
- rg -n ComputeHash\(|StableHash under src/DCoding.Data.DVault* found the same .NET-side hashing pattern in provider-specific save paths: Sqlite (DVaultSqliteServiceCollectionExtensions.cs:119,165,481-487), MySql (MySqlDataVaultSaveStrategy.cs:357,393,949-955), Postgres (PostgresDataVaultSaveStrategy.cs:262,299,<redacted>), SqlServer (SqlServerDataVaultSaveStrategy.cs:670,706,<redacted>), and Oracle (OracleDataVaultSaveStrategy.cs:155,191,<redacted>).
- src/DCoding.Data.DVault/IDataVaultProviderBehavior.cs:4-6 says provider behavior overrides may adapt provider-specific physical behavior without changing DVault naming, hashing, record-source, or timestamp semantics.
- docs/plans/stable-hashing-contract.md:15-22,37-43,91-100, docs/plans/dvault-v1-default-persistence-convention-policy.md:120-145, docs/plans/optional-advanced-configuration-hooks.md:120-137, and docs/plans/performance-evidence-benchmark-artifact-contract.md:12-29 already define the current sha256-v1/sha-256 contracts, reserve provider-accelerated hashing for separate versioned contracts, and require matched-input benchmark artifact triplets.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Future provider-side proposals should show one explicit safe-decline/fallback example for a provider that cannot prove exact canonicalization/vector parity.
- Future provider-side proposals should show matched-input benchmark evidence examples for both completed and skipped optional-provider rows using the shared benchmark-summary triplet.

Risky assumptions
- The contract assumes the implementer can choose the authoritative documentation target without more PO routing; it defines required content but not a specific destination file.
- The contract assumes release-facing wording can remain deferred to downstream ticket 06F5Q93H60W6X8FJ88PWTR6NG4 without reopening this ticket's scope.

AC / test suggestions
- Keep the acceptance wording explicit that any provider-side path may preserve semantics only; it may not silently replace today's .NET-side hashing path.
- When documentation examples are added, reuse the published stable-hash vectors and the matched-input benchmark artifact contract instead of inventing ticket-specific formats.
- If the doc includes examples, include one parity-proof example and one fallback example.

Implementation watchouts
- The branch currently contains ticket metadata only; the developer still needs to land the authoritative documentation update before downstream ticket 06F5Q93H60W6X8FJ88PWTR6NG4 can consume this boundary.
- Do not let provider-optimization wording imply current database-side hashing exists; current provider-neutral and provider-specific save paths still hash on the .NET side.

Non-blocking notes
- benchmark-summary.json:2-42 already demonstrates the shared artifact context shape, including optional-provider status and skip reasons, which fits the future evidence gate this ticket asks providers to reuse.

Split recommendations
- Keep this ticket as shared documentation/governance only; if provider-side hashing is pursued later, open separate provider-specific evidence or implementation tickets.
- Do not bundle multi-provider runtime hashing work into this ticket; at most, split future work into one shared contract ticket plus one provider-specific ticket per provider.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment