[gicket-bot] PO-critic review contract

Summary
- Delivery contract is now internally consistent, has no open questions, and matches the checked-in conservative DB2 evidence boundary; the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4QPEZW97YR6YT7MQD1MXTG/description.md now has PO Handoff=ready_for_po_critic, Open Questions=- none, and Implementation Notes stating that the durable change in this refinement pass was the description rewrite for contract accuracy.
- Comment .gicket/tickets/06FE4QPEZW97YR6YT7MQD1MXTG/comments/06FE7VEX7RZ5TQDDGH95JARQ1W.md marks prior critic-item-1, critic-item-2, and critic-item-3 as answered and says the authoritative Delivery Contract was rewritten to match the actual description update and current automation comment set.
- Branch history confirms the cleanup: git show --stat --oneline --summary 109d72933 -- .gicket/tickets/06FE4QPEZW97YR6YT7MQD1MXTG shows handoff commit 109d72933 changed .gicket/tickets/06FE4QPEZW97YR6YT7MQD1MXTG/description.md and added the PO refinement contract comment; current HEAD is d239f3d2a, and git diff --name-only HEAD^..HEAD touches only the po-critic lease-claim ticket metadata/comments/events.
- benchmark-summary.json lists DB2 external provider with DVAULT_TEST_DB2_CONNECTION_STRING unset; the DB2 provider-native-bulk-ingestion, latest-satellite-read, pit-as-of-read, and bridge-traversal-read rows all show executionStatus=skipped, iterations=0, null metrics, and persistedOutcome=not executed.
- docs/plans/provider-optimization-evidence-matrix.md defines completed-timing as the only posture that may support measured timing claims with a preserved provider-configured artifact triplet and run context, keeps skipped-placeholder/diagnostics-only/smoke-only out of measured timing claims, and enumerates DB2 save/latest/PIT/bridge rows as skipped-placeholder or diagnostics/smoke-only guidance only.
- docs/plans/provider-optimization-gap-matrix.md keeps DB2 latest-satellite-read, provider-native-bulk-ingestion, pit-as-of-read, and bridge-traversal-read as evidence-gap rows under the narrower v0.34.0 boundary; no completed DB2 timing, staged DB2 bulk, or provider-native chunk execution is claimed.
- src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs registers Db2DataVaultSaveStrategy plus Db2DataVaultReadStrategy for latest/PIT/bridge reads, while tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs provides configured smoke coverage only; tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs enforces that skipped or failed rows keep iterations=0, a non-empty reason, blank metrics, and persistedOutcome=not executed.
- .gicket/relations/TG/SR/06FE4QPEZW97YR6YT7MQD1MXTG--06FE4QR3DD7EFZ4F35SBTFGWSR--blocks.json preserves the live downstream blocks relation; .gicket/tickets/06FE4QR3DD7EFZ4F35SBTFGWSR/ticket.json is still todo, while .gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/ticket.json is done, matching the current contract's dependency narrative.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Optional: add one worked example of a failed DB2 latest/PIT/bridge row so the failed-vs-skipped non-timing rule is mechanically obvious to downstream doc authors.
- Optional: add one concise example that distinguishes diagnostics-only from smoke-only DB2 citation posture when both kinds of non-timing evidence exist.

Risky assumptions
- Downstream docs and manifests will continue to treat executionPath, selectedStrategy, and plannedReadStrategy tokens on skipped DB2 rows as non-timing guidance only.
- Completed DB2 timing promotion will remain deferred to 06FE4QR3DD7EFZ4F35SBTFGWSR until a provider-configured artifact triplet is actually checked in and cited.

AC / test suggestions
- Keep an explicit acceptance/test check that skipped and failed DB2 save/latest/PIT/bridge rows preserve iterations=0, null metrics, a non-empty reason, and persistedOutcome=not executed.
- When 06FE4QR3DD7EFZ4F35SBTFGWSR lands, enumerate exactly which DB2 rows may move from skipped-placeholder or diagnostics-only to completed-timing and which remain historical guidance.

Implementation watchouts
- Do not treat selectedStrategy, plannedReadStrategy, or executionPath tokens in skipped DB2 rows as measured timing evidence.
- DB2 save promotion remains limited to clean-context set-based save; staged bulk and provider-native chunk execution remain out of scope.
- DB2 read promotion remains limited to diagnostics-gated supported latest-satellite/PIT/bridge shapes with explicit PIT/bridge maintenance and provider-neutral fallback otherwise.

Non-blocking notes
- The prior PO-critic blocker about inaccurate ticket narration appears resolved by the current description rewrite plus the answering checklist comment 06FE7VEX7RZ5TQDDGH95JARQ1W.md.

Split recommendations
- No additional split is recommended; the existing blocks relation to 06FE4QR3DD7EFZ4F35SBTFGWSR already captures the downstream provider-configured DB2 tuning and evidence work.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment