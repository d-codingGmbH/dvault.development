[gicket-bot] PO-critic review contract

Summary
- Clear PO contract with no open questions and direct repo evidence for the bulk-ingestion baseline; approve for developer handoff, with the caveat that the branch already reflects an implemented v0.14.0 baseline rather than pending feature work.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGMFWSEC95ATBCGZ6HYT5W/description.md:31-36 defines five acceptance criteria, and description.md:50-51 says Open Questions -> none, so the contract is eligible for approve_for_dev under the handoff rule.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:12-35 exposes IDataVaultSaveService.SaveAsync(DbContext, DataVaultBulkSaveRequest), and DataVaultSaveService.cs:76-110 resolves DataVaultRegistryBulkSaveRequest into the same ordered bulk pipeline.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted> defines provider-save gates for clean contexts, multi-active exclusion, provider-name matching, SQL Server >=50 total operations and <=500 satellite operations, MySQL >=50 total operations, and Oracle >=50 total operations.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs:59-196 covers provider-neutral fallback and provider strategy selection behavior, and tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs:21-50 plus :142-167 define the ordered bulk-save scenario and diagnostics assertions reused by the Postgres, SQL Server, Oracle, and MySQL integration suites.
- docs/releases/v0.14.0.md:22-64 and README.md:204-205, README.md:503-508, README.md:571-601, README.md:609-637 document the same public bulk-save contract, provider-native boundaries, benchmark artifact surface, and opt-in external-provider test lanes.
- git diff --stat c9fb3de6e..491ec0b59 -- . ':(exclude).gicket/**' returned no output, while git diff --stat c9fb3de6e..491ec0b59 -- showed only .gicket/tickets/06F2PGMFWSEC95ATBCGZ6HYT5W metadata/comment changes; the described baseline is already in repository rather than pending on this ticket branch.
- PO comments comments/06F3QDAEY6PZPJ2EWKNNN1S59G.md:3-59 and comments/06F3QDD6HYHG8HCXJCFQ78CES0.md:13-18 explicitly say the refinement grounded the ticket in repository-visible v0.14.0 evidence and only updated description/labels for handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developer handoff assumes the intent is to ratify or verify an already-landed v0.14.0 baseline, because the ticket branch carries no non-.gicket changes relative to develop.
- The opt-in external-provider lanes remain environment-dependent; without the documented DVAULT_TEST_*_CONNECTION_STRING variables, default local or CI validation will not exercise PostgreSQL, SQL Server, Oracle, and MySQL native bulk paths.

AC / test suggestions
- Use DataVaultSaveStrategySelectionTests plus ExternalProviderBulkSaveAssertions and the provider-specific callers as the explicit AC-to-test mapping during dev/test handoff.
- Keep release and benchmark evidence tied to docs/releases/v0.14.0.md and the benchmark-summary.md/.csv/.json artifact set so performance claims stay contextualized.

Implementation watchouts
- Do not reopen the public write boundary, provider list, or multi-active native-batch exclusion; those decisions are already fixed in the contract and corroborated in DataVaultSaveService.cs, DataVaultDiagnostics.cs, and docs/releases/v0.14.0.md.
- Treat this epic as scope coordination around an implemented baseline, not as a blank prompt for new provider work.

Non-blocking notes
- I found no conflicting human review comments; the current ticket history is PO refinement and automation metadata only.

Split recommendations
- No mandatory split is needed for developer handoff.
- If finer execution tracking is later required, split along the three boundaries already named in the contract: core bulk-save contract and fallback behavior, provider-native strategy coverage and diagnostics, and documentation/benchmark evidence.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment