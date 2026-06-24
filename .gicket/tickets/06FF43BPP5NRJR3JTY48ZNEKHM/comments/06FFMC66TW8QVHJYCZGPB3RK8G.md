[gicket-bot] PO-critic review contract

Summary
- Contract is ready for an ordinary dev handoff, but it fails the supplied closure-only audit because repo evidence still shows PIT maintenance comparator work as unlanded implementation/testing.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF43BPP5NRJR3JTY48ZNEKHM/description.md:29-38 defines future work to normalize comparator rows, emit stable benchmark rows, and add regression coverage; lines 46-47 show Open Questions is none.
- docs/plans/provider-optimization-evidence-matrix.md:344-350 says the provider-neutral, PostgreSQL, and SQL Server pit-full-rebuild-maintenance rows are contract rows only until sibling benchmark artifacts land and are not timing evidence yet.
- docs/plans/performance-evidence-benchmark-artifact-contract.md:78-88 requires completed pit-full-rebuild-maintenance rows to carry maintenanceScope=FullRebuild, selected provider strategy or selectedStrategy=<none>, bounded fallbackCauses, and the benchmark artifact triplet/run context.
- git show --stat --summary c4eb9e27999384b4d209395f33cbd1d13b792d1a and git diff --stat c153fe42fb980137c1c9341c0acd8976d2423ebf..c4eb9e27999384b4d209395f33cbd1d13b792d1a show only .gicket/tickets/06FF43BPP5NRJR3JTY48ZNEKHM description/ticket/comment/event files changed on this branch; no src, tests, or artifacts/benchmarks files changed.
- artifacts/benchmarks/06FF0000000000000000000000-provider-optimization-closure-<redacted> contains only db2-rowcap-1000, mysql-live, oracle-lob-prefetch, postgres-podman-live, and sqlserver-live bundles, and a repo-wide search for pit-full-rebuild-maintenance matched docs contracts only, not benchmark-summary.*, artifacts/benchmarks/**, src/**, or tests/**.
- src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs and src/DCoding.Data.DVault.SqlServer/SqlServerPitMaintenanceFallbackCauseKind.cs confirm the bounded fallback vocabularies named in the contract already exist.

Blocking findings
- For the supplied closure-only review path, the ticket is not yet closeable: its own Definition of Done still requires benchmark-generation/normalization logic and regression coverage, but current branch history and repository artifacts show only ticket-metadata changes and contract-level guidance, not landed PIT maintenance comparator implementation/test evidence.

Required PO actions
- Fix the routing/contract mismatch: either re-route this as a normal pre-development developer handoff ticket, or keep closure-only routing and rewrite the contract so it refers only to already-landed evidence.

Open issues ledger
- critic-item-1 [required-po-action] Fix the routing/contract mismatch: either re-route this as a normal pre-development developer handoff ticket, or keep closure-only routing and rewrite the contract so it refers only to already-landed evidence.
- critic-item-2 [blocking-finding] For the supplied closure-only review path, the ticket is not yet closeable: its own Definition of Done still requires benchmark-generation/normalization logic and regression coverage, but current branch history and repository artifacts show only ticket-metadata changes and contract-level guidance, not landed PIT maintenance comparator implementation/test evidence.

Missing examples / edge cases
- A concrete comparator-row example across markdown, CSV, and JSON would remove ambiguity for both the completed-row case and the non-executed placeholder case.

Risky assumptions
- Assuming the runtime intended a closure-only audit even though the persisted contract reads like a normal pre-development implementation task.
- Assuming sibling pit-full-rebuild-maintenance benchmark artifacts can land without any further contract/schema clarification; current repo docs still treat those rows as unlanded contract-only guidance.

AC / test suggestions
- Add an acceptance/test sentence that the same comparator-row identity and deterministic executionDetail tokens must match across benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json.
- Cover both a completed comparator row and a placeholder/non-executed row so selectedStrategy=<none> and bounded fallbackCauses stay stable in both states.

Implementation watchouts
- Do not let pit-as-of-read or bridge-traversal-read rows satisfy PIT maintenance evidence for this ticket.
- Preserve existing bounded fallback enum names in fallbackCauses instead of inventing provider-specific prose markers.
- SQL Server's PIT maintenance support is narrower than PostgreSQL's, so comparator normalization must not imply identical provider-specific shape coverage.

Non-blocking notes
- The persisted contract is otherwise well-bounded: Open Questions is none, scope is limited to PostgreSQL/SQL Server comparator-row normalization, and the referenced fallback vocabularies already exist in source.
- Existing triplet-parity verification already exists in tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:<redacted>, so the eventual verification surface looks bounded.

Split recommendations
- No split is needed if PO re-routes this as a normal developer-hand-off ticket.
- If the ticket must remain closure-only, split out the future implementation/coverage work and leave this ticket limited to already-landed contract/evidence updates.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment