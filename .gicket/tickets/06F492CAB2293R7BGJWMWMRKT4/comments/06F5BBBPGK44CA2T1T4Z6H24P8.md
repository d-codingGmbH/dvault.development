[gicket-bot] PO-critic review contract

Summary
- Delivery contract is clear, open questions are resolved, and repository evidence confirms both the required benchmark baseline and the existing provider-neutral read surface; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F492CAB2293R7BGJWMWMRKT4/description.md contains Open Questions -> none, a ready_for_po_critic handoff, and bounded scope/AC/DoD for latest-satellite, PIT as-of, and bridge traversal read tuning.
- docs/plans/performance-evidence-benchmark-artifact-contract.md requires before/after benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json, the same provider filter/load-timestamp/iteration context, and explicitly lists latest satellite read, PIT as-of read, and bridge traversal read in the minimum SQLite baseline.
- benchmark-summary.md on the ticket branch includes the three target rows with the same allocation numbers cited in the contract: latest-satellite <redacted> vs SQLite optimized <redacted>, PIT as-of <redacted> vs <redacted>, and bridge traversal <redacted> vs <redacted>; the same file also records Iterations 1, Warmup iterations 0, Load timestamp storage ProviderDefault, and Provider filter all.
- src/DCoding.Data.DVault/IDataVaultReadService.cs directly exposes provider-neutral raw read methods for latest-satellite and PIT reads, while src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs and src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs expose the existing bridge raw/projection helper surface anchored on IDataVaultReadService.
- git diff --name-only 28b3b04209ffa6fc4b9fcf7c382ba71ef4e97072..HEAD returned no files, and git log --oneline -n 5 shows only PO handoff / lease commits b42ed1dfc, 511fc30e4, and 28b3b0420, which is consistent with a pre-development handoff rather than a partially implemented branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking edge-case gaps remain at ticket level; the contract already captures the split-later-if-needed contingency for one read family requiring a materially larger refactor.

Risky assumptions
- The contract should be read against source, not release-note prose: bridge traversal reads are currently provided through DataVaultReadServiceBridgeExtensions and registry extensions anchored on IDataVaultReadService, not as interface members in IDataVaultReadService.cs.
- The root benchmark-summary.md and benchmark-summary.json snapshot is only a seed baseline; developers still need to archive ticket-labeled before/after artifacts under artifacts/benchmarks/<label>/before and after with matched run context.

AC / test suggestions
- Call out the exact scenario row names latest-satellite-read, pit-as-of-read, and bridge-traversal-read in final evidence so each claimed win maps to a concrete benchmark row.
- Keep acceptance verification explicit that skipped optional-provider rows remain visible in markdown, CSV, and JSON artifacts even though this story only tunes provider-neutral SQLite-backed baselines.
- If any claimed win depends on changed SQL shape or materialization behavior rather than pure allocation reduction, require the archived evidence set to include representative SQL alongside the benchmark trio.

Implementation watchouts
- Do not widen the public read surface just to optimize bridge traversal; direct source evidence shows the existing bridge API is the extension-based helper surface around IDataVaultReadService.
- Treat the lack of current code diff as normal pre-dev state, not as proof that the benchmark baseline is missing; the checked-in benchmark summary already provides the seed evidence and target rows.
- Keep provider-specific optimized rows as comparison baselines only; this ticket's scope is limited to provider-neutral read-service, query, and materialization tuning.

Non-blocking notes
- The ticket has no assignee yet in .gicket/tickets/06F492CAB2293R7BGJWMWMRKT4/ticket.json; if dev routing depends on assignees, that should be handled by workflow automation after approval rather than PO refinement.
- Outgoing blocks relations to 06F492CTREZEDXVKJ839YGCPWW and 06F492D05THPGQVT3B3K7853A0 reflect downstream regression-baseline and documentation work and do not make this pre-development tuning story unclear.

Split recommendations
- Keep this as one dev ticket unless profiling shows one of the three read families needs a materially larger architectural refactor than the others; that is the only split trigger preserved in the current contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment