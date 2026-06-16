[gicket-bot] PO-critic review contract

Summary
- Return to PO: the Oracle keep-as-is conclusion is technically supported, but the closure-only ticket contract conflicts with observed ticket history and the current canonical `P1.04` backlog row.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Comment `.gicket/tickets/06FBSC9QSAAF0J1Y9K27ZAEPDC/comments/06FCWF63ZWP8E59NABEH1QS42G.md` explicitly says `Updated the durable refinement contract in the ticket description.`
- `docs/plans/provider-optimization-gap-matrix.md:59` still defines `P1.04` as `Evidence gap` and says `Collect provider-configured evidence to validate the direct Oracle optimized batching boundary against provider-neutral fallback.`
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs:84-102` returns `DirectOracleBatching` with `StagedOracleBulkNotSelectedReason = not-selected-no-measured-win`; `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs:143-155` and `:256-264` enforce Oracle provider name plus 50-operation / 10000-satellite gates.
- `tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs:33-82` and `tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs:23-37,102-115` cover retained direct batching, fallback cases, and no staged Oracle selection.
- `artifacts/benchmarks/v0.32.0-06F9XD2TGEYEG6S0AK86YF295M-oracle-high-volume-threshold-<redacted>/benchmark-summary.md:32-34,44,47,50` records the keep-10000 decision, direct Oracle beating provider-neutral fallback at 10000 satellite operations, and fallback with `OracleMaximumSatelliteOperationThreshold` at <redacted>.

Blocking findings
- The persisted delivery contract says `No child tickets, relation changes, description updates... were materialized in this run`, but branch history and current ticket comments contradict that: commit `7c29bd76c` updated `.gicket/.../description.md`, and comment `06FCWF63ZWP8E59NABEH1QS42G.md` says the refinement contract in the ticket description was updated. That is a ticket-level factual inconsistency in a closure-only contract.
- The contract says it ratifies `P1.04` as a documentation/no-op decision, but the canonical repo planning surface at `docs/plans/provider-optimization-gap-matrix.md:59` still marks `P1.04` as an `Evidence gap` and recommends collecting provider-configured evidence. The ticket does not reconcile that mismatch.

Required PO actions
- Reconcile the ticket conclusion with `docs/plans/provider-optimization-gap-matrix.md:59`: either narrow the ticket to acknowledge `P1.04` remains an evidence-gap backlog item, or explicitly explain why closure is valid without changing that canonical backlog surface.
- If closure-only is still intended, make the final expected deliverable explicit: ticket-level recommendation only, or a separate follow-up to align canonical planning surfaces.

Open issues ledger
- critic-item-1 [required-po-action] Reconcile the ticket conclusion with `docs/plans/provider-optimization-gap-matrix.md:59`: either narrow the ticket to acknowledge `P1.04` remains an evidence-gap backlog item, or explicitly explain why closure is valid without changing that canonical backlog surface.
- critic-item-2 [required-po-action] If closure-only is still intended, make the final expected deliverable explicit: ticket-level recommendation only, or a separate follow-up to align canonical planning surfaces.
- critic-item-3 [blocking-finding] The persisted delivery contract says `No child tickets, relation changes, description updates... were materialized in this run`, but branch history and current ticket comments contradict that: commit `7c29bd76c` updated `.gicket/.../description.md`, and comment `06FCWF63ZWP8E59NABEH1QS42G.md` says the refinement contract in the ticket description was updated. That is a ticket-level factual inconsistency in a closure-only contract.
- critic-item-4 [blocking-finding] The contract says it ratifies `P1.04` as a documentation/no-op decision, but the canonical repo planning surface at `docs/plans/provider-optimization-gap-matrix.md:59` still marks `P1.04` as an `Evidence gap` and recommends collecting provider-configured evidence. The ticket does not reconcile that mismatch.

Missing examples / edge cases
- A short ticket-level example contrasting 49 vs 50 total operations and 10000 vs 10001 satellite operations would make the fallback boundary easier to audit without reading repo tests.

Risky assumptions
- The contract assumes the existing v0.32 Oracle artifact bundle is enough to retire `P1.04`, even though the current gap matrix still frames Oracle save as an open evidence gap.
- The contract assumes a closure-only ticket can rely on unchanged repo planning surfaces, but the canonical backlog document still points to follow-up evidence work.

AC / test suggestions
- Reference the exact Oracle artifact path and the exact `P1.04` row/path in the acceptance criteria so the closure evidence is auditable from the ticket alone.
- If the intent is closure-only, add an acceptance statement that no repository planning-surface update is required and explain why that is safe despite the current gap-matrix wording.

Implementation watchouts
- If this ticket eventually turns into delivery work, keep the current evidence boundary: no staged Oracle bulk claim without a measured win over both provider-neutral fallback and retained direct Oracle batching.
- Do not treat the skipped root `benchmark-summary.*` Oracle row as completed external-provider timing evidence; only the provider-configured v0.32 artifact bundle supports measured Oracle timing claims.

Non-blocking notes
- The underlying technical recommendation is repo-backed: Oracle remains direct optimized batching with optional array binding, current thresholds, and staged Oracle left unselected.

Split recommendations
- No implementation split is justified from the current code/tests evidence, but a separate backlog-maintenance or documentation-alignment ticket may be needed if PO wants the canonical gap-matrix surface to reflect the closure recommendation.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment