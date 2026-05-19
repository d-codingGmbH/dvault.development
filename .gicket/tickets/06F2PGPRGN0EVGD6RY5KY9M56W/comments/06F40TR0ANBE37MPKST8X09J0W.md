[gicket-bot] PO refinement contract

Summary
- Repository evidence shows PIT-backed and bridge reads already exist as provider-neutral read-service helpers over maintained PIT and bridge tables; this refinement bounds the story to internal provider-aware dispatch and evidence updates, with SQLite as the required local optimized proof point. No child tickets, relations, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The target branch still matches scratch source ref `49ba94dcf686da0e9b6fd8bb205809eabffd24d2`, so refinement assumes no partial PIT/bridge optimization implementation is already present on the ticket branch.
- Repository evidence already delivers provider-neutral PIT reads through `IDataVaultReadService.ReadPitRowsAsync(...)` and `ReadPitAsync(...)`, provider-neutral bridge reads through `ReadBridgeRowsAsync(...)` and `ReadBridgeAsync(...)`, explicit bridge maintenance, and PIT maintenance semantics; this story optimizes those existing read paths rather than introducing them.
- Current provider-specific read dispatch is limited to latest/as-of satellite reads, and SQLite is the only visible repository provider package that currently registers a read strategy.
- README, benchmark guidance, production-adoption guidance, and release notes currently state that PIT and bridge reads are provider-neutral and should not be described as provider-optimized until benchmarkable repository evidence exists.

Scope In
- Internal/provider-package dispatch that can choose optimized PIT-backed read execution for supported provider and request-shape combinations behind the existing public read APIs.
- Internal/provider-package dispatch that can choose optimized bridge read execution for supported provider and request-shape combinations behind the existing public read APIs.
- Provider-neutral fallback preservation for unsupported providers, unsupported request shapes, and existing maintained PIT/bridge usage.
- Tests, diagnostics, benchmark coverage, and documentation updates needed to prove optimized-path selection and unchanged semantics.

Scope Out
- New public PIT or bridge request shapes, new caller-visible maintenance APIs, or changes to `IDataVaultReadService` request contracts.
- PIT row refresh, implicit PIT maintenance, scheduler or background behavior, or bridge maintenance automation.
- Bridge delete-aware rebuild policy changes, broader graph traversal APIs, effectivity or path-payload features, or closure-state modeling.
- Link-based PITs, multi-active PIT semantics, PIT/bridge interaction redesign, or unrelated provider-save-strategy refactoring.

Open questions
- none

Follow-up questions
- After the SQLite-backed optimized baseline is proven locally, which external provider package should receive the next PIT/bridge optimized implementation first: <redacted>, SQL Server, MySQL, or Oracle?
- Should optional external-provider benchmark runs later emit provider-specific PIT/bridge optimized rows in the default archived artifact set, or stay as opt-in evidence only?
- If implementation effort grows beyond one story, should provider-aware dispatch plumbing be split from provider-specific PIT and bridge strategy tickets rather than expanding this story further?

Risks
- Because current provider-read strategy and diagnostic surfaces are latest-satellite-specific, PIT/bridge optimization may require cross-cutting internal refactoring before provider packages can plug in cleanly.
- PIT and bridge reads consume maintained tables with correctness-sensitive ordering and snapshot semantics; provider-specific SQL that changes tie-breaking or filtering would create hard-to-detect read regressions.
- Repository benchmark guidance currently supports provider-specific read evidence only for latest-satellite reads, so documentation must not over-claim PIT/bridge optimization until artifact-backed evidence exists.
- External-provider proof beyond SQLite is opt-in and consumer-managed, which can slow validation of non-SQLite optimized implementations.

Split recommendations
- If this story becomes too large during implementation, split first into a common provider-aware PIT/bridge dispatch-plus-diagnostics slice and two execution slices: PIT optimized reads and bridge optimized reads.
- If non-SQLite provider-specific SQL is desired in the same release, track each external provider package in its own child ticket so fallback-safe SQLite and local proof are not blocked by external database setup.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment