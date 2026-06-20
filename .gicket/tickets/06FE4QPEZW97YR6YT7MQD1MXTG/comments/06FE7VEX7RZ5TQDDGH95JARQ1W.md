[gicket-bot] PO refinement contract

Summary
- Verified the already-applied durable description rewrite, confirmed the conservative DB2 guardrail scope and downstream dependency remain unchanged, and found the ticket ready to return to PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The authoritative Delivery Contract was rewritten so it no longer claims that no persistent writes occurred or that the ticket comments are only claim and lease entries. The current contract now acknowledges the broader automation comment set and records that the durable description narrative was rewritten while relations, child tickets, attachments, and planning documents stayed unchanged.
- critic-item-2: `answered` - The normative DB2 guardrail scope was kept intact. The contract still limits measured timing claims to completed-timing rows with a preserved provider-configured artifact triplet, keeps skipped, failed, diagnostics-only, and smoke-only DB2 evidence out of timing claims, and preserves the downstream `blocks` dependency on 06FE4QR3DD7EFZ4F35SBTFGWSR.
- critic-item-3: `answered` - The current-state and process narration now matches the repository evidence. The run report explicitly records that the durable ticket description was updated, the branch diff shows `.gicket/tickets/06FE4QPEZW97YR6YT7MQD1MXTG/description.md` was rewritten on the ticket branch, and the current Implementation Notes now state that the durable description narrative was rewritten instead of claiming that no description update occurred.

Clarifications
- The durable ticket description rewrite is already applied on the owner branch; no additional relation, child-ticket, attachment, or planning-document writes were needed in this pass.
- The active downstream dependency remains this ticket's `blocks` relation to 06FE4QR3DD7EFZ4F35SBTFGWSR; the done story 06FE4QNWP9606HTB92MTVQMYDG remains historical routing context through its existing links and does not make this ticket blocked.
- `benchmark-summary.json` keeps DB2 optimized save, latest-satellite, PIT, and bridge rows as skipped placeholders with `iterations=0`, null metrics, and `persistedOutcome=not executed` when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset.
- `docs/plans/provider-optimization-evidence-matrix.md` and `docs/plans/provider-optimization-gap-matrix.md` remain the canonical lookup surfaces for DB2 row identity, posture, and follow-up gating.

Scope In
- Lock the DB2 promotion rules for provider-native save, latest-satellite read, PIT as-of read, and bridge traversal read evidence.
- Define how completed, skipped, failed, diagnostics-only, and smoke-only DB2 evidence may be represented without overstating support.
- Preserve the existing provider-neutral fallback conditions for unset DB2 configuration, dirty save context, provider mismatch, unsupported or incomplete read shapes, stale PIT or bridge maintenance, or diagnostics that do not select the DB2 strategy.
- Carry the existing downstream dependency boundary where this guardrail ticket precedes 06FE4QR3DD7EFZ4F35SBTFGWSR for provider-configured DB2 tuning and evidence collection.

Scope Out
- Collecting new provider-configured DB2 benchmark evidence or retuning DB2 execution paths.
- Claiming staged DB2 bulk, provider-native chunk execution, completed DB2 latest-satellite, PIT, or bridge timing, or DB2 live-schema reading without new completed evidence.
- Widening DB2 latest-satellite beyond the supported hub-parent, non-multi-active shapes or adding automatic PIT or bridge maintenance behavior.
- Creating new planning artifacts or relation rewrites for this ticket when the existing split already covers the downstream work.

Open questions
- none

Follow-up questions
- After 06FE4QR3DD7EFZ4F35SBTFGWSR lands a provider-configured DB2 artifact triplet, which current DB2 rows should move from skipped-placeholder or diagnostics-only posture to completed-timing and which should remain non-promoted historical guidance?

Risks
- The root quick benchmark triplet currently contains only skipped DB2 external-provider rows, so downstream docs can still overread planned execution-path or selected-strategy tokens as completed timing evidence.
- DB2 support stays narrow and conditional on clean context, explicit PIT or bridge maintenance, supported read shapes, diagnostics selection, and a reachable `DVAULT_TEST_DB2_CONNECTION_STRING`.
- The repository already documents DB2 live-schema reading as out of scope; inconsistent follow-on wording could still imply support that this guardrail contract forbids.
- This ticket only fixes the promotion boundary; measurable DB2 evidence still depends on downstream delivery in 06FE4QR3DD7EFZ4F35SBTFGWSR.

Split recommendations
- No additional split is recommended. The live downstream `blocks` relation to 06FE4QR3DD7EFZ4F35SBTFGWSR already captures the provider-configured DB2 tuning and evidence work.

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