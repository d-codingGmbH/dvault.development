[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The persisted contract is concrete, bounded to the existing benchmark harness and artifact contract, and has no unresolved open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9GF66B10J4K7RBDTJ9NQRQC/description.md contains the durable Delivery Contract with Open Questions set to none, 5 acceptance criteria, 4 Definition of Done items, and a bounded four-variant scope.
- .gicket/tickets/06F9GF66B10J4K7RBDTJ9NQRQC/comments/06FBM6CH0JTEQ860Q1BTAQ1GJW.md records PO handoff ready_for_po_critic and states the work must extend the existing benchmark harness and artifact contract rather than create a second harness.
- src/DCoding.Data.DVault/DataVaultHashKeyStorageProfile.cs defines only HexString and Binary, and src/DCoding.Data.DVault/BuiltInStableHashService.cs exposes sha256-v1, sha1-v1, sha256-128-v1, and sha256-160-v1, matching the ticket's bounded algorithm and storage inventory.
- benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkOptions.cs and benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs currently expose iterations, warmup, output, --scale, --latest-indexes, load-timestamp storage, and provider filter only; there is no hash-storage or stable-hash comparison option surface yet, so the requested harness generalization is directly evidenced.
- benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs still hard-codes IsLowercaseSha256(...), and benchmarks/DCoding.Data.DVault.Benchmarks/CustomerProfileDataVaultBenchmark.cs plus benchmarks/DCoding.Data.DVault.Benchmarks/OrderProductDataVaultBenchmark.cs still assert SHA-256-shaped hash keys, matching the contract's stated generalization need.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs already document and verify the existing benchmark artifact triplet, optional-provider skipped-row semantics, and --latest-indexes mode that this ticket reuses.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs already projects sha256-128-v1 for both HexString and Binary store types across the built-in provider matrix, supporting the ticket's choice of sha256-128-v1 as the bounded shorter baseline.
- git diff --name-only develop...HEAD lists only .gicket/tickets/06F9GF66B10J4K7RBDTJ9NQRQC/**, and git log develop..HEAD shows only PO and PO-critic workflow commits cb7902bfd, 00351e850, 383ae418a, and <redacted>, so no implementation work has started yet; this is a pure pre-development handoff review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Supplemental storage-footprint or SQL evidence can be captured as same-label sidecars without needing a separate contract ticket, because the ticket bounds them to the existing artifact bundle rather than a new row schema.
- Optional external-provider comparison rows will continue to use the existing configured-versus-skipped provider model, so the required deliverable remains the SQLite-local four-variant baseline.

AC / test suggestions
- Add benchmark tests that verify variant routing covers all four required combinations: sha256-v1 hex, sha256-v1 binary, sha256-128-v1 hex, and sha256-128-v1 binary.
- Add verifier coverage that fails on algorithm or storage drift by checking the active algorithm id and digest or storage shape instead of a fixed 64-character SHA-256 assertion.
- Preserve artifact tests for skipped optional providers so new variant rows still emit visible skipped rows with deterministic executionDetail and persistedOutcome=not executed when providers are not configured.

Implementation watchouts
- The current save benchmarks verify hash shape through DataVaultBenchmarkHelpers.IsLowercaseSha256(...); that helper and all callers must be generalized before shorter-digest runs can pass for the right reason.
- The ticket should stay inside the existing benchmarks/DCoding.Data.DVault.Benchmarks executable and current artifact triplet; creating a parallel harness or ad hoc report format would violate the contract.
- The existing --latest-indexes surface is the concrete reuse point for repeated-write lookup and index-shape evidence; widening beyond that bounded mode would be scope creep.

Non-blocking notes
- none

Split recommendations
- No split recommended while the work stays within the existing benchmark harness, current artifact contract, and bounded four-variant comparison baseline.
- If stakeholders later want a broader algorithm matrix or mandatory external-provider execution, open a follow-up ticket instead of widening this task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment