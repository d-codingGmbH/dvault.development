[gicket-bot] PO refinement contract

Summary
- Refined the DB2 promotion-guardrail task against the live benchmark, evidence, smoke-test, and relation baseline; no persistent writes were needed because the existing split and repository docs already define a conservative DB2 evidence contract.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The only current ticket comments are automation claim and lease comments; no human comment changes the scope.
- Live relations already match the intended split: story 06FE4QNWP9606HTB92MTVQMYDG both blocks and relates to this ticket, and this ticket blocks 06FE4QR3DD7EFZ4F35SBTFGWSR.
- The root benchmark-summary.json already exposes the DB2 optimized save, latest-satellite, PIT, and bridge rows as skipped placeholders when DVAULT_TEST_DB2_CONNECTION_STRING is unset, with iterations 0, null metrics, and persistedOutcome 'not executed'.
- docs/plans/provider-optimization-evidence-matrix.md and docs/plans/provider-optimization-gap-matrix.md already fix the DB2 guardrail baseline: completed timing requires a provider-configured artifact triplet and run context; diagnostics-only, smoke-only, and skipped-placeholder rows are not measured timing evidence.
- AddDVaultDb2() and Db2DataVaultSmokeTests prove strategy registration and diagnostics or smoke candidate behavior for clean-context save and latest-satellite, PIT, and bridge reads, but they do not by themselves prove completed DB2 timing.

Scope In
- Lock the DB2 promotion rules for provider-native save, latest-satellite read, PIT as-of read, and bridge traversal read evidence.
- Define how completed, skipped, failed, diagnostics-only, and smoke-only DB2 evidence may be represented without overstating support.
- Preserve the existing provider-neutral fallback conditions for unset DB2 configuration, dirty save context, provider mismatch, unsupported or incomplete read shapes, stale PIT or bridge maintenance, or diagnostics that do not select the DB2 strategy.
- Carry the current dependency boundary where this guardrail ticket precedes 06FE4QR3DD7EFZ4F35SBTFGWSR for provider-configured DB2 tuning and evidence collection.

Scope Out
- Collecting new provider-configured DB2 benchmark evidence or retuning DB2 execution paths.
- Claiming staged DB2 bulk, provider-native chunk execution, completed DB2 latest-satellite, PIT, or bridge timing, or DB2 live-schema reading without new completed evidence.
- Widening DB2 latest-satellite beyond the supported hub-parent, non-multi-active shapes or adding automatic PIT or bridge maintenance behavior.
- Changing ticket relations, creating child tickets, or writing planning documents in this pass; the existing split already covers the downstream work.

Open questions
- none

Follow-up questions
- After 06FE4QR3DD7EFZ4F35SBTFGWSR lands a provider-configured DB2 artifact triplet, which current DB2 rows should move from skipped-placeholder or diagnostics-only posture to completed-timing and which should remain non-promoted historical guidance?

Risks
- The root quick benchmark triplet currently contains only skipped DB2 external-provider rows, so downstream docs can easily overread planned executionPath or selectedStrategy tokens as completed timing evidence.
- DB2 support stays narrow and conditional on clean context, explicit PIT or bridge maintenance, supported read shapes, diagnostics selection, and a reachable DVAULT_TEST_DB2_CONNECTION_STRING.
- The repository already documents DB2 live-schema reading as out of scope; inconsistent follow-on wording could accidentally imply support that the current guardrail contract forbids.
- This ticket only fixes the promotion boundary; measurable DB2 evidence still depends on downstream delivery in 06FE4QR3DD7EFZ4F35SBTFGWSR.

Split recommendations
- No additional split recommended. The live blocks relation to 06FE4QR3DD7EFZ4F35SBTFGWSR already captures the downstream DB2 tuning and provider-configured evidence work.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment