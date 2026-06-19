[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff: the contract is bounded, the Delivery Contract Open Questions section is none, and repository evidence already proves the Oracle PIT/bridge strategy boundary and fallback rules; remaining work is canonical evidence/doc alignment rather than PO clarification.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- git rev-parse HEAD and git merge-base HEAD c669fc274795802d575699220f4fa776a2891c66 both returned c669fc274795802d575699220f4fa776a2891c66, and git diff --stat c669fc274795802d575699220f4fa776a2891c66..HEAD -- docs/plans/provider-optimization-evidence-matrix.md docs/plans/provider-optimization-gap-matrix.md benchmark-summary.md benchmark-summary.csv benchmark-summary.json tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs docs/architecture/dvault-v1-pit-bridge-boundary.md returned no changes.
- benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json still record Oracle external provider rows for pit-as-of-read and bridge-traversal-read under baseline dvault-adddvaultoracle-optimized as executionStatus=skipped with skip reason not configured: DVAULT_TEST_ORACLE_CONNECTION_STRING is not set or empty, while executionDetail still names plannedReadStrategy=OracleDataVaultReadStrategy.
- docs/plans/provider-optimization-evidence-matrix.md still classifies Oracle rows latest-satellite-read, pit-as-of-read, and bridge-traversal-read for baseline dvault-adddvaultoracle-optimized as skipped-placeholder guidance rows, and its Global Claim Rules say not to cite smoke-only rows as measured provider performance.
- docs/plans/provider-optimization-gap-matrix.md entries P2.04 and P3.04 still list Oracle external provider pit-as-of-read and bridge-traversal-read as evidence gaps tied to the skipped root triplet rows.
- artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.csv already contains completed Oracle rows for pit-as-of-read (475.258 ms) and bridge-traversal-read (7.388 ms), but docs/releases/v0.32.0.md describes that bundle as a smoke read baseline and docs/performance-profiles.md groups it under threshold evidence bundles instead of the canonical root completed-timing matrix.
- src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs registers OracleDataVaultReadStrategy for IDataVaultProviderPitReadStrategy and IDataVaultProviderBridgeReadStrategy, and src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs gates CanReadPitRows and CanReadBridgeRows through DataVaultProviderReadStrategyGateEvaluator.EvaluateOracle(...).
- tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs ExpectedProviderReadRows requires Oracle pit-as-of-read and bridge-traversal-read guidance rows with selectedStrategy=OracleDataVaultReadStrategy and plannedReadStrategy=OracleDataVaultReadStrategy, while tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs covers Oracle supported PIT/bridge shapes plus provider mismatch, unsupported shape, incomplete read-shape evidence, and stale read-model maintenance fallbacks.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give an explicit example of the exact evidence-matrix/source-artifact citation format expected when Oracle PIT/bridge move off the root skipped placeholders.
- The contract does not explicitly say whether reusing artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted> after posture changes is acceptable, or whether a fresh non-smoke bundle is required.

Risky assumptions
- Assumes the existing smoke-read Oracle bundle can only satisfy this ticket if documentation and evidence posture are updated together; otherwise a new provider-configured bundle will be needed.
- Assumes Oracle latest-satellite remains out of scope even if the smoke-read bundle contains a completed latest-satellite-read row, because providerSpecificReadStrategy=not registered for latest satellite reads is still the documented boundary in benchmark details and the evidence matrix.
- Assumes the live blocks chain 06FBSCGBG8CJ0QNRX4JZJA638G -> 06FBSCH0M358R5J3RGFB6GRDM4 -> 06FBSCHBJEYYERDPA7JN34Y8PG is housekeeping rather than a dev-start gate, because the delivery contract says live relations remain unchanged and Follow-Up Questions treat revalidation as later housekeeping.

AC / test suggestions
- Keep acceptance tied to both Oracle rows together: scenario pit-as-of-read and scenario bridge-traversal-read for provider Oracle external provider and baseline dvault-adddvaultoracle-optimized.
- Add verifier coverage that the canonical evidence matrix Oracle PIT/bridge rows cite a provider-configured artifact bundle path instead of benchmark-summary.md/csv/json root placeholder rows.
- Preserve explicit verifier tokens for readShape=PitAsOf or Bridge, selectedStrategy=OracleDataVaultReadStrategy, plannedReadStrategy=OracleDataVaultReadStrategy, and latest-satellite providerSpecificReadStrategy=not registered for latest satellite reads.

Implementation watchouts
- The root quick benchmark triplet must remain a skipped-placeholder surface when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset; closure needs canonical alternate evidence, not silent mutation of the root contract.
- Do not let docs promote a smoke-only or smoke-labeled source as measured provider performance unless the evidence matrix claim posture and source-artifact rules are updated coherently.
- Preserve fallback behavior for provider mismatch, unsupported shape, incomplete read-shape evidence, and stale read-model maintenance; the current Oracle gate tests already enforce those boundaries.
- Keep scope off Oracle latest-satellite optimization and automatic PIT/bridge maintenance behavior.

Non-blocking notes
- No ticket-specific delivery diff is present on the owner branch yet; that is normal for a pre-development handoff because HEAD still matches scratch-source-ref c669fc274795802d575699220f4fa776a2891c66.
- Repository evidence already includes an Oracle read benchmark bundle with completed PIT/bridge rows; the unresolved work is reconciling canonical evidence posture, docs, and verifier alignment.
- Follow-Up Questions about Oracle latest-satellite backlog handling and blocks-chain revalidation are not blockers because the authoritative Delivery Contract Open Questions section is none.

Split recommendations
- No split recommended; the ticket is already tightly bounded to Oracle PIT/bridge evidence closure plus doc and verifier alignment.
- Do not separate doc-only work from verifier/evidence alignment, because the repository currently has conflicting signals between the canonical skipped root matrix and the existing smoke-read Oracle bundle.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment