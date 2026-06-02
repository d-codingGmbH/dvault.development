[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the refined ticket closes open questions, matches the existing diagnostics/read-shape/profile/redaction surfaces in the repository, and only non-blocking watchouts remain around preserving existing gate distinctions and omission rules.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F7Y0JQ2FZQZVTNFX2T25DAS4/description.md` contains `## Open Questions` followed by `- none`, so the persisted delivery contract has no unresolved open questions.
- `git log --oneline --decorate -n 8` in `/mnt/c/Projects/DVault` shows branch `ticket/06F7Y0JQ2FZQZVTNFX2T25DAS4-story-define-provider-performance-tuning-diagnos` at `04cbc803c` with only PO and PO-critic handoff commits since `develop`; `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F7Y0JQ2FZQZVTNFX2T25DAS4/*`.
- `.gicket/tickets/06F7Y0JQ2FZQZVTNFX2T25DAS4/events/06F7Y0Z7ASJK8Y1T22WX0984WR.json` and `.../06F7Y0Z97TH2JE609B3KRQ1RJ8.json` record `blocks` relations from this contract ticket to implementation tickets `06F7Y0JZKTVBGGQ9Q4EBC2PCDG` and `06F7Y0K95VW0PX21F6R2YGP8DM`.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` defines `DataVaultDiagnosticsResult` with additive `SaveStrategy`, init-only `ReadStrategy`, and nullable `ReadShape`; the same file closes `DataVaultReadShapeKind` to `LatestSatellite`, `PitAsOf`, and `Bridge`.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` also declares save gate constants `50/500` for SQL Server, `50` direct plus `60` staged for MySQL, and `<redacted>` for Oracle, plus fallback kinds for dirty context, provider mismatch, multi-active operations, and staged-provider bulk causes.
- `docs/performance-profiles.md` `## Profile Selection` lists exactly four checked-in starting profiles: `Small app-local vault`, `Medium chunked ingestion`, `Staged provider ingestion`, and `Read-model heavy`; the same document states SQLite is the only repository-proven optimized latest-satellite, PIT, or bridge read path in the root benchmark triplet.
- `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md` states that read diagnostics reuse `DataVaultDiagnosticsResult` `ReadStrategy` and `ReadShape`, omit non-applicable optional fields, and must not emit raw keys, timestamps, SQL text, query plans, credentials, connection strings, provider error text, or stack traces.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Consider adding one explicit no-match example where `benchmarkProfile` and `recommendation` are both omitted because no evidence-backed mapping applies.
- Consider adding one candidate-level example that shows MySQL direct save eligibility at 50 total operations versus staged MySQL eligibility at 60 total operations.
- Consider adding one fallback example where `selectedStrategyName` is absent and only finite fallback causes plus redacted `readShape.provider` facts are serialized.

Risky assumptions
- Treating MySQL as having one undifferentiated minimum gate would be unsafe; `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` has `MinimumMySqlOptimizedBatchOperationCount = 50` and `MinimumMySqlStagedBatchOperationCount = 60`.
- Treating non-SQLite provider read optimization as repository-proven would be unsafe; `docs/performance-profiles.md` says SQLite is the only repository-proven optimized latest-satellite, PIT, or bridge read path in the checked-in artifact set.
- Treating the historical `06F7Y0HZKHBHMYX9EYDYFRYXZ0 -> 06F7Y0JQ2FZQZVTNFX2T25DAS4` `blocks` relation as active would be unsafe; the related ticket is `done`, the current ticket has `isBlocked: false`, and `git log --all --grep '06F7Y0HZKHBHMYX9EYDYFRYXZ0' -n 5` shows integration commit `f089c643a`.

AC / test suggestions
- Verify request-bound outputs omit non-applicable optional fields such as `selectedStrategyName`, threshold facts, `benchmarkProfile`, and `recommendation` rather than emitting placeholders.
- Verify the closed recommendation category set maps 1:1 to the four checked-in performance profiles plus provider-neutral fallback and does not require prose parsing.
- Verify `LatestSatellite`, `PitAsOf`, and `Bridge` read diagnostics stay value-free and do not introduce numeric read thresholds until new repository evidence exists.

Implementation watchouts
- Keep provider-tuning diagnostics additive to `DataVaultDiagnosticsResult` using the existing `SaveStrategy`, `ReadStrategy`, and `ReadShape` structures instead of inventing a parallel blob.
- Preserve the redaction boundary from `docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md`: no raw keys, timestamps, SQL, query plans, connection strings, provider error text, stack traces, or workload values.
- Preserve candidate-level save gate distinctions and staged-provider bulk decline causes instead of collapsing them into a single generic provider recommendation.
- Anchor profile and recommendation outputs to `docs/performance-profiles.md` rather than copied benchmark prose or unsupported provider claims.

Non-blocking notes
- All inspected comment files under `.gicket/tickets/06F7Y0JQ2FZQZVTNFX2T25DAS4/comments` are workflow or automation comments; no newer discussion was found that reopens scope or adds open questions.
- `git diff --name-only develop..HEAD` shows only `.gicket/tickets/06F7Y0JQ2FZQZVTNFX2T25DAS4/*`, which matches the contract-only nature of this pre-development story.
- Related implementation tickets `06F7Y0JZKTVBGGQ9Q4EBC2PCDG` and `06F7Y0K95VW0PX21F6R2YGP8DM` are still `todo`; this ticket now carries the shared contract boundary they should consume.

Split recommendations
- Keep this ticket contract-only; implementation of provider eligibility, threshold, and recommendation diagnostics stays in `06F7Y0JZKTVBGGQ9Q4EBC2PCDG` and benchmark-artifact verification stays in `06F7Y0K95VW0PX21F6R2YGP8DM`.
- If the team later wants new benchmark profiles, provider-specific read thresholds, or transport, reporting, or exporter surfaces, open separate follow-up tickets instead of widening this v1 contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment