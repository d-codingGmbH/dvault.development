<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already bounds provider read parity acceptance: SQLite-only latest-satellite optimization, diagnostics-gated PIT/bridge candidate lanes, finite fallback causes, completed-timing benchmark posture, and no raw-SQL or platform-behavior promises. No new child tickets or relation writes were needed; the live graph already has one PIT/bridge audit task and five per-provider latest-satellite gap tasks, while two incoming blocks from done documentation tickets remain housekeeping.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Latest-satellite parity is currently closable only on the SQLite optimized lane; PostgreSQL, SQL Server, MySQL, Oracle, and DB2 still surface `providerSpecificReadStrategy=not registered for latest satellite reads`, so those tickets may close as `no-work-required` unless a new strategy, diagnostics selection, and completed timing evidence are all added.
- PIT and bridge provider work is a separate lane: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 already have provider-specific PIT/bridge candidate registrations or parity coverage, but external-provider timing claims remain evidence-gated.
- Row/projection parity, diagnostics selection, and benchmark timing are distinct evidence layers; parity tests or diagnostics-only evidence do not by themselves justify an external-provider performance claim.
- The fallback vocabulary is finite and enum-backed, including provider mismatch or unregistered provider, missing provider-specific registration, unsupported satellite, PIT, or bridge shape, incomplete read-shape evidence, stale maintenance, and bounded strategy decline reasons.
- The live graph already contains downstream tickets `06FBSCGBG8CJ0QNRX4JZJA638G`, `06FBSCFDFFYQXBK17RT3E8W4CM`, `06FBSCFKWGQMBEF5Q96AZ5Q0X0`, `06FBSCFVT3SBHKMDGNEXWVWFXG`, `06FBSCG18KBRT1FTHDRX073EF4`, and `06FBSCG6C40X9CV3FFEHHKS6G0`; no further split, relation change, description write, attachment write, or planning-document write was materialized in this pass.

### Scope In
- Define the acceptance boundary for provider-specific latest-satellite tickets, including when `no-work-required` is the correct outcome.
- Define the acceptance boundary for PIT and bridge provider tickets across the visible SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 lanes.
- Ratify supported-shape, fallback, diagnostics, and benchmark-evidence rules using the current repository architecture, tests, and evidence matrices.
- Ratify the no-platform-behavior boundary: no implicit maintenance, raw SQL contract, query-plan promise, or physical-design promise.

### Scope Out
- Adding new provider read strategy implementations or widening provider service registrations.
- Rerunning benchmarks, generating new artifact triplets, or promoting skipped, diagnostics, or smoke evidence into completed timing claims.
- Changing PIT or bridge maintenance behavior, automatic scheduling, or read-time refresh behavior.
- Claiming raw SQL capture, provider plan inspection, automatic index advice, or provider-specific physical-design guarantees.

## Acceptance Criteria
- The contract states that SQLite is the only currently acceptable optimized latest-satellite provider path; non-SQLite latest-satellite work remains provider-neutral or `no-work-required` unless a provider-specific strategy is registered, diagnostics select it, and completed benchmark evidence proves it against fallback.
- The contract states that provider-specific PIT and bridge work is valid only over explicitly maintained read-model rows and only for the supported maintained shapes already bounded in the PIT/bridge architecture note.
- Any provider-specific read lane must fail closed to provider-neutral reads with finite existing fallback causes for provider mismatch, unregistered strategy, unsupported shape, incomplete read-shape evidence, stale maintenance, or other bounded strategy decline.
- Closure evidence must include request-bound `IDataVaultReadDiagnosticsService` output for read-strategy status, selected strategy or fallback causes, and read-shape provider facts, without exposing raw hash keys, as-of values, SQL text, query plans, credentials, or automatic design advice.
- Performance claims may rely only on `completed-timing` evidence with the preserved artifact triplet and run context; `skipped-placeholder`, `diagnostics-only`, `smoke-only`, and `storage-footprint` postures do not satisfy timing-claim closure and allow `no-work-required` or defer outcomes instead.
- When a ticket introduces or updates measured benchmark rows, it must reuse the visible regression-budget rules: the targeted metric improves or holds, required SQLite non-target regressions above 5% fail by default, and configured optional-provider regressions above 10% require explicit justification.

## Definition of Done
- A downstream provider-read ticket can be marked implement, `no-work-required`, or defer without reopening provider names, shape vocabulary, or evidence-posture semantics.
- Any close-as-implemented record cites the authoritative architecture and evidence surfaces plus the exact diagnostic and benchmark posture it relied on.
- Any close-as-`no-work-required` record explains which bounded gate failed, such as no strategy registration, unsupported shape, incomplete read-shape evidence, stale maintenance, or missing completed timing evidence.
- Accepted closure text keeps latest-satellite capability gaps distinct from PIT and bridge evidence gaps and does not treat parity-only evidence as timing proof.
- No accepted closure implies automatic PIT or bridge maintenance, raw SQL or public plan surfaces, or provider-specific platform behavior beyond the current contract.

## Implementation Notes
- Use `docs/architecture/dvault-v1-pit-bridge-boundary.md` and `docs/releases/v0.28.0.md` as the core boundary sources for supported read-model behavior, fallback, and explicit non-goals.
- Use `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` for gate coverage and finite fallback-cause evidence.
- Use `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs` for PIT and bridge row and projection parity against provider-neutral fallback.
- Use `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs`, `docs/plans/provider-optimization-evidence-matrix.md`, and `docs/plans/provider-optimization-gap-matrix.md` for benchmark posture, guidance-row identity, and capability-versus-evidence gap classification.
- Latest-satellite guidance rows currently keep `selectedStrategy=<none>` and `providerSpecificReadStrategy=not registered for latest satellite reads` for PostgreSQL, SQL Server, MySQL, Oracle, and DB2.
- External-provider PIT and bridge timing remains gated: SQLite has completed timing rows; PostgreSQL, SQL Server, MySQL, Oracle, and DB2 root rows remain skipped placeholders unless provider-configured benchmark evidence is checked in; DB2 also keeps the narrower v0.34 evidence boundary.
- Incoming `blocks` relations from done tickets `06FBSC4QXYQ0SWB1DPMGJJ5XX0` and `06FBSCAX98ZFQZWBYEQMB8WF18` look like relation-housekeeping rather than PO blockers, but no relation cleanup was materialized in this pass.

## Open Questions
- none

## Follow-Up Questions
- After this criteria story is accepted, should `06FBSCGBG8CJ0QNRX4JZJA638G` apply the same gates to classify PostgreSQL, SQL Server, MySQL, Oracle, and DB2 PIT and bridge rows as implement, `no-work-required`, or defer?
- If product later wants non-SQLite latest-satellite optimization, should priority remain the current gap-matrix order PostgreSQL, SQL Server, MySQL, Oracle, then DB2?

## Risks
- Downstream tickets can overclaim external-provider performance if they treat parity-only, `skipped-placeholder`, `diagnostics-only`, or `smoke-only` evidence as completed timing.
- DB2 remains intentionally narrower than the other provider lanes: PIT and bridge candidate behavior may be cited, but DB2 latest-satellite optimization and completed DB2 timing still require deliberate scope expansion.

## Split Recommendations
- No new split recommended; the live graph already covers the next bounded work as PIT and bridge audit ticket `06FBSCGBG8CJ0QNRX4JZJA638G` plus latest-satellite gap tickets `06FBSCFDFFYQXBK17RT3E8W4CM`, `06FBSCFKWGQMBEF5Q96AZ5Q0X0`, `06FBSCFVT3SBHKMDGNEXWVWFXG`, `06FBSCG18KBRT1FTHDRX073EF4`, and `06FBSCG6C40X9CV3FFEHHKS6G0`.
- Do not pre-split PIT and bridge implementation tickets before `06FBSCGBG8CJ0QNRX4JZJA638G` applies the refined criteria and classifies each provider lane.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Define criteria for provider-specific latest-satellite, PIT, and bridge read strategies: supported shapes, SQL boundaries, fallback, diagnostics, benchmark threshold, and no platform behavior. Acceptance: provider read tickets can be closed as no-work if evidence does not meet criteria.