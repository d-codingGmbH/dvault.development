[gicket-bot] PO refinement contract

Summary
- Ratified the existing split of this story into plain EF child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 and DVault child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8 and confirmed the shared two-event contract in docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md; no additional planning writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This story is already decomposed through parentOf relations into child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 for the plain EF baseline and child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8 for the DVault baseline.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md is the authoritative shared scenario and persisted-outcome contract for both children.
- The shared business scenario is fixed to customer business key C-100 with payload fields customer_name and customer_status.
- Event 1 is <redacted>-29T10:15:00Z from crm-import with Alice Adams / prospect; event 2 is <redacted>-29T11:30:00Z from crm-change with Alice Baker / active.
- Repository and relation context already show the SQLite-focused DVault baseline from blocking ticket 06EXB7G6YE4X0GA0CT7EPEFMPR, so no extra prerequisite clarification is needed for this story.
- No new child tickets, relation writes, attachments, or planning documents were created in this refinement run because the split and shared contract were already materialized.

Scope In
- Deliver one SQLite-based comparison scenario that covers both a conventional EF customer profile history baseline and a DVault hub-plus-satellite baseline.
- Keep the scenario limited to the locked two-event customer C-100 history contract and exact persisted-outcome assertions.
- Run the scenario through the existing solution and test layout under tests/DCoding.Data.DVault.Tests.
- Use repository naming, SQLite persistence, and MVP Data Vault conventions already documented in the shared standards and architecture documents.

Scope Out
- Additional entities, relationships, or broader order/link scenarios beyond the single customer profile history comparison.
- Standalone sample applications or a new examples/ surface for v1.
- Deferred Data Vault capabilities such as PIT tables, bridge tables, multi-active satellites, or provider-specific optimizations.
- Alternative timestamps, extra replay cases, deduplication variants, or comparison scenarios beyond the locked two-event contract.

Open questions
- none

Follow-up questions
- After both baselines are integrated, should the same scenario also be promoted from test-only coverage into a runnable example or documentation sample?
- When more comparison scenarios exist, should shared fixtures or assertion helpers be introduced so plain EF and DVault baselines stay synchronized in code as well as in the planning contract?

Risks
- The comparison loses value if either child ticket drifts from the locked two-event contract or adds extra persisted rows not covered by the shared planning document.
- If the underlying SQLite DVault baseline from ticket 06EXB7G6YE4X0GA0CT7EPEFMPR changes its naming or persistence assumptions, the comparison assertions may need coordinated updates.
- Scope can expand unintentionally if example scenario is interpreted as a standalone sample application instead of the current bounded automated comparison baseline.

Split recommendations
- No further split is recommended; the story is already appropriately decomposed into child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- If stakeholders later want a runnable sample, broader relationship demos, or more history variants, create separate follow-up tickets instead of widening this story.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment