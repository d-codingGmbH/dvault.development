[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is detailed, but it assumes delete-aware bridge maintenance is already a completed baseline even though current repo and related-ticket closure evidence show bridge maintenance is still non-delete-aware.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Current ticket contract at .gicket/tickets/06F5Q91DR1555RSBQT7KDST684/description.md says upstream story 06F5Q916BXE2N372SWMH1X776G is a completed baseline and scopes in 'delete-aware bridge evidence' plus an 'explicit shrink-safe maintenance path'; its ## Open Questions section is 'none'.
- src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs shows MaintainBridgeAsync(...) inserts missing rows, only lowers hierarchy TraversalDepth when a shorter path appears, and returns rowsDeleted: 0; README.md and docs/production-adoption-checklist.md both still describe bridge maintenance as explicit and not delete-aware.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs covers many-to-many insert-only maintenance and hierarchy convergence via RebuildBridgeAsync(...); it does not prove delete-aware incremental reconciliation.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs exposes IDataVaultReadDiagnosticsService overloads for explicit PIT reads plus registry-backed latest-satellite and bridge reads, and an rg check returned 'NOT_FOUND: DataVaultRegistryPitAsOfReadRequest'; that matches the current ticket's PIT clarification boundary.
- benchmark-summary.csv lines 19-22 contain only ordinary pit-as-of-read and bridge-traversal-read rows, and git diff --name-only develop..HEAD returned only .gicket/tickets/06F5Q91DR1555RSBQT7KDST684/... files, so no implementation or evidence changes have landed on this owner branch yet.

Blocking findings
- The bridge baseline is wrong: the contract treats delete-aware bridge maintenance as already completed, but current repo code, docs, and the closure evidence on 06F5Q916BXE2N372SWMH1X776G all say bridge maintenance is still non-delete-aware.
- Because that baseline is false, the story is ambiguous about scope: developers cannot tell whether this is evidence-only work over an existing capability or whether they must first add the missing delete-aware maintenance behavior/API.

Required PO actions
- Revise the delivery contract to stop assuming delete-aware bridge maintenance is already delivered. Either rewrite this story around the actual baseline (append-only MaintainBridgeAsync(...) plus RebuildBridgeAsync(...) for shrink) or make this ticket explicitly depend on a real delete-aware bridge implementation ticket/commit.
- Update the bridge acceptance criteria and scope-in language so they clearly say whether dev work here is limited to diagnostics/benchmark evidence over the current non-delete-aware contract or includes adding a new delete-aware maintenance path.

Open issues ledger
- critic-item-1 [required-po-action] Revise the delivery contract to stop assuming delete-aware bridge maintenance is already delivered. Either rewrite this story around the actual baseline (append-only MaintainBridgeAsync(...) plus RebuildBridgeAsync(...) for shrink) or make this ticket explicitly depend on a real delete-aware bridge implementation ticket/commit.
- critic-item-2 [required-po-action] Update the bridge acceptance criteria and scope-in language so they clearly say whether dev work here is limited to diagnostics/benchmark evidence over the current non-delete-aware contract or includes adding a new delete-aware maintenance path.
- critic-item-3 [blocking-finding] The bridge baseline is wrong: the contract treats delete-aware bridge maintenance as already completed, but current repo code, docs, and the closure evidence on 06F5Q916BXE2N372SWMH1X776G all say bridge maintenance is still non-delete-aware.
- critic-item-4 [blocking-finding] Because that baseline is false, the story is ambiguous about scope: developers cannot tell whether this is evidence-only work over an existing capability or whether they must first add the missing delete-aware maintenance behavior/API.

Missing examples / edge cases
- If bridge shrink behavior stays in scope, state whether evidence must cover many-to-many pair deletion, hierarchy row deletion, and increased TraversalDepth after topology shrink; current repo evidence only covers insert-only maintenance and rebuild convergence.
- If this remains evidence-only, give one explicit example of the acceptable bridge-maintenance baseline to validate against: append-only MaintainBridgeAsync(...) versus full RebuildBridgeAsync(...) after shrink.

Risky assumptions
- Assuming an 'explicit shrink-safe maintenance path' already exists in code somewhere other than RebuildBridgeAsync(...); current source and docs do not support that.
- Assuming downstream documentation task 06F5Q91M0PM17RP43ZQRPBDXP0 can absorb this wording mismatch later even though the current developer contract already depends on the incorrect bridge baseline.

AC / test suggestions
- Keep the current PIT-boundary wording that forbids inventing a new DataVaultRegistryPitAsOfReadRequest and requires registry-backed PIT evidence through metadata resolution plus explicit DataVaultPitAsOfReadRequest diagnostics.
- Once bridge scope is corrected, make one acceptance criterion explicitly state whether benchmark evidence is ever required for bridge-maintenance reconciliation itself or whether correctness tests and diagnostics are sufficient unless a performance claim is added.

Implementation watchouts
- Any future delete-aware bridge feature would need additive updates across IDataVaultBridgeMaintenanceService, DataVaultBridgeMaintenanceServiceRegistryExtensions, tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt, and bridge-maintenance test doubles.
- New measured claims still need either new root benchmark-summary.* rows or a checked-in artifacts/benchmarks/<label>/before|after bundle that follows docs/plans/performance-evidence-benchmark-artifact-contract.md.
- Link-parent and multi-active PIT scenarios already rely on diagnostics-backed fallback visibility in some shapes; new evidence must not imply SQLite/provider-specific optimization where the strategy actually declines the request.

Non-blocking notes
- Aside from the bridge-baseline contradiction, the ticket is otherwise well bounded: the current description has ## Open Questions = none and the registry-backed PIT limitation is explicitly called out against the visible public API.

Split recommendations
- If stakeholders still want both capability work and evidence work, split delete-aware bridge implementation from this evidence-focused follow-up instead of treating the implementation as a completed baseline.
- Keep any future public registry-backed PIT read request as a separate additive API ticket, consistent with the current contract's follow-up questions and the absence of DataVaultRegistryPitAsOfReadRequest in the repo.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment