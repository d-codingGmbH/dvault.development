[gicket-bot] PO refinement contract

Summary
- Re-scoped this ticket toward closure: the repo already ships the six-package API snapshot guardrail, so this ticket no longer owns standalone developer work and instead records that future deferred-capability snapshot diffs belong to the specific owning capability story that first exports a public API.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This ticket should be re-scoped toward closure as already-covered snapshot infrastructure, not kept as a standalone developer task. The repository already contains the approval test, six package baselines, and the workflow documentation, so the remaining PO work is to record ownership and audit rules rather than ask developers to build new guardrail infrastructure here.
- critic-item-2: `answered` - Future snapshot changes belong to the owning deferred-capability story, not this shared ticket. PIT public API changes belong in 06EZ0NSXY2Y1JZ8SSCX177C770, bridge public API changes belong in 06EZ0NTV4SVAKV98C418T8A3CC, and multi-active public API changes belong in 06EZ0NVN71BN0QWJDCWGVZ2PYG. This ticket should stop serving as the blocker for those stories because no deferred-capability public API is visible in the current approved snapshots.
- critic-item-3: `answered` - For an internal-only deferred-capability change, the auditable per-change artifacts are: 1) the implementing story's final delivery summary or change description explicitly stating that no public contract was introduced, and 2) the absence of any diff under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/. The standing policy artifact for reviewers is docs/quality/api-surface-snapshots.md.
- critic-item-4: `answered` - The independent deliverable problem is resolved by removing standalone developer scope from this ticket. No new repository code or snapshot-file change is required here because the existing approval test and package baselines already provide the shared guardrail; concrete snapshot edits now belong only to the owning story that introduces a real exported API.
- critic-item-5: `answered` - Scope ownership is no longer ambiguous in the contract: each deferred-capability story owns snapshot updates only when that story's implementation exports a public API in its own package. This shared ticket no longer owns a future API change and should not be treated as the continuing blocker for PIT, bridge, or multi-active implementation work.
- critic-item-6: `answered` - Acceptance Criterion 2 is now objectively checkable against named artifacts: reviewers inspect the implementing story's explicit final delivery note or change description for the internal-only statement, and they confirm that no approved snapshot file changed under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.

Clarifications
- The shared API snapshot guardrail already exists in tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs and in the six approved files under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.
- DVault.slnx shows the bounded packable-package baseline for this guardrail: DCoding.Data.DVault, DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer.
- docs/quality/api-surface-snapshots.md is the standing repository policy artifact for the validation path and snapshot approval workflow.
- No current approved snapshot exposes PIT, bridge, multi-active-satellite, or advanced-hook-specific public types or members, so this ticket must not invent placeholder public contracts.
- The deferred capability owner stories already exist as 06EZ0NSXY2Y1JZ8SSCX177C770 for PIT, 06EZ0NTV4SVAKV98C418T8A3CC for bridge, and 06EZ0NVN71BN0QWJDCWGVZ2PYG for multi-active work.

Scope In
- Record that the existing repository snapshot infrastructure already satisfies the shared guardrail baseline for the six packable packages.
- State that future deferred-capability snapshot updates belong to the specific owning capability story that first exports a real public API.
- Name the auditable internal-only review rule: an explicit no-public-contract note in the implementing story's final delivery summary or change description plus no diff in the approved snapshot directory.
- Remove this ticket from standalone developer scope so it no longer asks for code or snapshot work that already exists.

Scope Out
- Implementing new snapshot test infrastructure or replacing the existing approval mechanism.
- Creating placeholder public types, members, or names only to exercise the snapshot gate.
- Keeping PIT, bridge, or multi-active delivery blocked on this shared ticket when those stories still have no declared public API export.
- Designing deferred-capability runtime behavior, provider behavior, DDL, or release-governance work.

Open questions
- none

Follow-up questions
- When relation-removal support is available, should the current blocks links from 06EZ0NSQFCD3W4CDCJ44GFSKA0 to the PIT, bridge, and multi-active stories be deleted outright or downgraded to relates for traceability?
- If a future advanced-hook story introduces a public API, should it copy this same internal-only audit rule directly into its own delivery contract or reference the shared snapshot policy note?

Risks
- If the legacy blocks relations remain untouched, automation or humans may continue to misread this ticket as a prerequisite for PIT, bridge, or multi-active delivery.
- A future owning story may forget to include the explicit internal-only note even when snapshots remain unchanged.
- Developers may still try to add placeholder public APIs to force snapshot activity despite the contract forbidding that approach.

Split recommendations
- Do not split this into new development subtasks. Treat the ticket as closure/re-scope and mirror snapshot ownership into the concrete deferred-capability story that actually exports a public contract.
- If backlog hygiene needs separate tracking, create a small planning/admin follow-up to remove or downgrade the three stale blocks relations because that cleanup could not be materialized through the declared tool surface in this run.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment