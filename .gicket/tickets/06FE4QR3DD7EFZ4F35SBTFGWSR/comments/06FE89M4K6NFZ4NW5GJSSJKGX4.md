[gicket-bot] PO refinement contract

Summary
- Verified that 06FE4QR3DD7EFZ4F35SBTFGWSR is the materialized downstream owner for provider-configured DB2 hotspot evidence, confirmed the supported DB2 save/read boundaries from the repository and live relation state, and persisted a ticket-bound planning note at docs/plans/db2-hotspot-evidence-refinement-06FE4QR3DD7EFZ4F35SBTFGWSR.md.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The incoming blocks relation from 06FE4QPEZW97YR6YT7MQD1MXTG is historical upstream guardrail context, not an active blocker, because that ticket is done and this ticket's is-blocked flag is false.
- The live downstream dependency remains the blocks relation from this ticket to 06FE4QRMXVGJVA65ZR5MZ817K8, so this ticket stays the upstream owner for DB2 evidence before v0.42 documentation promotion.
- The repository already proves AddDVaultDb2 strategy registration, DB2 smoke execution for save/latest-satellite/PIT/bridge, skipped root benchmark row identity when DVAULT_TEST_DB2_CONNECTION_STRING is unset, and explicit DB2 live-schema unsupported status.
- A planning document was materialized at docs/plans/db2-hotspot-evidence-refinement-06FE4QR3DD7EFZ4F35SBTFGWSR.md; no ticket-description write, child-ticket creation, relation mutation, or attachment write was applied in this pass.

Scope In
- Collect provider-configured DB2 benchmark evidence for the already supported provider-specific paths: clean-context optimized save, latest-satellite read on supported hub-parent non-multi-active shapes, PIT as-of read on supported maintained PIT shapes, and bridge traversal read on supported maintained bridge shapes.
- Keep diagnostics-selected strategy evidence aligned with the benchmark artifact triplet so DB2 selected paths, fallback causes, and supported-shape limits are explicit.
- Define which DB2 benchmark-backed rows may move from skipped-placeholder to completed-timing once a provider-configured artifact triplet lands.
- Preserve the existing upstream/downstream dependency boundary where this ticket follows the done DB2 promotion-guardrail task and precedes the v0.42 documentation-update task.

Scope Out
- Adding new DB2 provider capabilities beyond the currently implemented strategy surface.
- Staged DB2 bulk, provider-native chunk execution, DB2 live-schema reading, automatic PIT or bridge maintenance, or broader latest-satellite shape support.
- Promoting diagnostics-only, smoke-only, or skipped-placeholder DB2 evidence into measured timing claims without a provider-configured artifact triplet and preserved run context.
- Coordinated v0.42 documentation and release-note updates already owned by 06FE4QRMXVGJVA65ZR5MZ817K8, except where that downstream ticket cites evidence produced here.

Open questions
- none

Follow-up questions
- none

Risks
- Without strict evidence-posture wording, downstream work could overread selectedStrategy or plannedReadStrategy tokens, or smoke-test success, as measured timing evidence.
- DB2 support remains configuration-sensitive: missing DVAULT_TEST_DB2_CONNECTION_STRING, dirty save contexts, unsupported read shapes, incomplete read-shape evidence, or stale PIT/bridge maintenance all force provider-neutral fallback.
- This ticket must stay within the already implemented DB2 provider boundary; widening runtime capability and collecting timing evidence are separate concerns and should not be conflated.
- The downstream docs-update ticket depends on this evidence handoff; partial evidence or ambiguous promotion rules would propagate inconsistent v0.42 documentation claims.

Split recommendations
- No additional PO split is recommended. 06FE4QR3DD7EFZ4F35SBTFGWSR already owns the provider-configured DB2 hotspot evidence slice left open by the done guardrail task 06FE4QPEZW97YR6YT7MQD1MXTG.
- Keep the existing downstream blocks relation to 06FE4QRMXVGJVA65ZR5MZ817K8; documentation and release-surface promotion should remain a separate follow-up that consumes this ticket's evidence output rather than being merged back into this ticket.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment