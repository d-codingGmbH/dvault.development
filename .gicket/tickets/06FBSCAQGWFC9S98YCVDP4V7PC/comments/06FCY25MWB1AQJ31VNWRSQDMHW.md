[gicket-bot] PO-critic review contract

Summary
- Repository and branch evidence show the DB2 baseline is already landed; this ticket still needs PO closure or deliberate re-scope, not developer implementation handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- git rev-parse HEAD and git rev-parse eb1720916db1d8104d4f4671c6cd05faba052a4f^{commit} both returned eb1720916db1d8104d4f4671c6cd05faba052a4f; git diff --name-status eb1720916db1d8104d4f4671c6cd05faba052a4f..HEAD returned no output, so the target branch currently carries no delta beyond the scratch source ref.
- git log --oneline --decorate over the DB2 code/docs/test files includes 1b5820269 (tag: v0.34.0) Complete v0.34.0 DB2 provider support, showing the DB2 baseline is already landed in repository history.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs:15-25 registers AddDVaultDb2() with Db2DataVaultSaveStrategy plus Db2DataVaultReadStrategy for PIT and bridge reads.
- docs/releases/v0.34.0.md:41-43,154 states the DB2 baseline already includes optimized clean-context save plus diagnostics-gated PIT/bridge reads, and explicitly excludes provider-native latest-satellite reads, staged bulk, provider-native chunk execution, and live-schema reading.
- tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs:29-130,318-348 verifies DB2 representative save selection, provider-neutral latest-satellite fallback, and provider-selected Db2DataVaultReadStrategy for PIT/bridge shapes when configured.
- benchmark-summary.md:74,87-89 and benchmark-summary.json:804-818,<redacted> keep DB2 rows as skipped placeholders because DVAULT_TEST_DB2_CONNECTION_STRING is unset; the save row records db2SaveBoundary=clean-context-set-based and stagedBulkBoundary=not-supported.
- docs/plans/provider-optimization-gap-matrix.md:14,55,60,65,70 classifies DB2 latest-satellite as a capability gap and DB2 save/PIT/bridge as evidence gaps only; it says no completed DB2 timing claim exists and stop conditions include any proposed staged bulk or provider-native chunk work.

Blocking findings
- The ticket's current title, status, and labels still represent open implementation work, but the repository and contract evidence say the implementation baseline already landed and this ticket should not imply unfinished staged DB2 bulk capability.
- The Delivery Contract does not leave a concrete developer implementation objective on this ticket; it requires a PO routing choice first: <redacted> as no-work-required or superseded, or explicitly re-scope to a narrow DB2 evidence/documentation follow-up.

Required PO actions
- Choose the routing explicitly at ticket level: close this ticket as no-work-required or superseded, or retitle and re-scope it to a separate DB2 evidence/documentation objective.
- Add an audit note or relation pointing to the landed DB2 baseline in v0.34.0, the DB2 smoke tests, and the benchmark placeholder evidence when ticket relation/history tooling is available.

Open issues ledger
- critic-item-1 [required-po-action] Choose the routing explicitly at ticket level: close this ticket as no-work-required or superseded, or retitle and re-scope it to a separate DB2 evidence/documentation objective.
- critic-item-2 [required-po-action] Add an audit note or relation pointing to the landed DB2 baseline in v0.34.0, the DB2 smoke tests, and the benchmark placeholder evidence when ticket relation/history tooling is available.
- critic-item-3 [blocking-finding] The ticket's current title, status, and labels still represent open implementation work, but the repository and contract evidence say the implementation baseline already landed and this ticket should not imply unfinished staged DB2 bulk capability.
- critic-item-4 [blocking-finding] The Delivery Contract does not leave a concrete developer implementation objective on this ticket; it requires a PO routing choice first: <redacted> as no-work-required or superseded, or explicitly re-scope to a narrow DB2 evidence/documentation follow-up.

Missing examples / edge cases
- If PO chooses an evidence-only follow-up, specify exactly which artifact triplet or documentation update counts as done and whether the scope is limited to clean-context save evidence or also includes separate PIT/bridge timing evidence.
- If PO chooses closure, specify the intended closure classification, such as no-work-required versus superseded, so downstream automation does not reopen implementation scope.

Risky assumptions
- The prompt snapshot matches the latest persisted ticket state and no newer gicket metadata contradicts it.
- No hidden relation or comment history changes the closure versus re-scope recommendation.
- The empty branch diff versus eb1720916db1d8104d4f4671c6cd05faba052a4f means there is no in-progress implementation delta relevant to this ticket.

AC / test suggestions
- If re-scoped to evidence-only, keep an acceptance criterion that DB2 wording preserves db2SaveBoundary=clean-context-set-based and stagedBulkBoundary=not-supported until new repository evidence changes them.
- If re-scoped to evidence-only, require any DB2 benchmark claim to come from a configured benchmark artifact triplet, not from smoke or diagnostics evidence alone.
- If re-scoped, add an explicit non-goal that latest-satellite optimization, staged DB2 bulk, provider-native chunk execution, and live-schema reading remain out of scope.

Implementation watchouts
- Do not reinterpret Db2DataVaultSmokeTests or PIT/bridge strategy registration as proof of completed DB2 timing.
- Do not infer a DB2 latest-satellite optimized read path from PIT/bridge support; repository evidence keeps latest-satellite on provider-neutral fallback.
- Do not send this ticket to development under the current title unless the scope is first narrowed to evidence or documentation work.

Non-blocking notes
- Open Questions is none, so the blocker is not unresolved contract ambiguity; it is the unresolved PO routing and ticket-state update.
- The repository history and current branch state are consistent with the contract's claim that the DB2 baseline is already landed.
- Recent comments and closure evidence amendments are both empty in the persisted snapshot.

Split recommendations
- Do not split this implementation ticket further.
- If DB2 follow-up work is still desired, create a new narrow evidence/documentation ticket instead of keeping it under the current implementation title.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment