<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Re-scoped this ticket toward closure: the repo already ships the six-package API snapshot guardrail, so this ticket no longer owns standalone developer work and instead records that future deferred-capability snapshot diffs belong to the specific owning capability story that first exports a public API.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The shared API snapshot guardrail already exists in tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs and in the six approved files under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.
- DVault.slnx shows the bounded packable-package baseline for this guardrail: DCoding.Data.DVault, DCoding.Data.DVault.MySql, DCoding.Data.DVault.Oracle, DCoding.Data.DVault.Postgres, DCoding.Data.DVault.Sqlite, and DCoding.Data.DVault.SqlServer.
- docs/quality/api-surface-snapshots.md is the standing repository policy artifact for the validation path and snapshot approval workflow.
- No current approved snapshot exposes PIT, bridge, multi-active-satellite, or advanced-hook-specific public types or members, so this ticket must not invent placeholder public contracts.
- The deferred capability owner stories already exist as 06EZ0NSXY2Y1JZ8SSCX177C770 for PIT, 06EZ0NTV4SVAKV98C418T8A3CC for bridge, 06EZ0NVN71BN0QWJDCWGVZ2PYG for multi-active work, and 06EZ0NWKC9ZME5BSCJFSQEQ02R for advanced-hook work.

### Scope In
- Record that the existing repository snapshot infrastructure already satisfies the shared guardrail baseline for the six packable packages.
- State that future deferred-capability snapshot updates belong to the specific owning capability story that first exports a real public API.
- Name the auditable internal-only review rule: an explicit no-public-contract note in the implementing story's final delivery summary or change description plus no diff in the approved snapshot directory.
- Remove this ticket from standalone developer scope so it no longer asks for code or snapshot work that already exists.

### Scope Out
- Implementing new snapshot test infrastructure or replacing the existing approval mechanism.
- Creating placeholder public types, members, or names only to exercise the snapshot gate.
- Keeping PIT, bridge, multi-active, or advanced-hook delivery blocked on this shared ticket when those stories still have no declared public API export.
- Designing deferred-capability runtime behavior, provider behavior, DDL, or release-governance work.

## Acceptance Criteria
- The ticket contract explicitly acknowledges that the shared API snapshot guardrail is already implemented by tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, the six approved snapshot files, and docs/quality/api-surface-snapshots.md.
- The contract explicitly states that only the owning deferred-capability story that introduces a real public API export updates the matching approved snapshot file in the same change.
- For internal-only deferred-capability work, reviewers can objectively verify compliance by finding an explicit no-public-contract statement in the implementing story's final delivery summary or change description and by confirming that no file changed under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.
- This ticket no longer defines standalone developer work or serves as the substantive reason to block PIT, bridge, multi-active, or advanced-hook implementation stories.

## Definition of Done
- The ticket refinement text makes the closure/re-scope decision explicit and stops asking developers to add already-existing snapshot infrastructure.
- The contract names the future owners for deferred-capability snapshot diffs: the specific capability story that introduces the public API.
- The contract names the exact internal-only audit artifacts reviewers must inspect.
- No new product-code, test-infrastructure, or approved-snapshot-file deliverable remains on this ticket by itself.

## Implementation Notes
- Use tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs, tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/, DVault.slnx, and docs/quality/api-surface-snapshots.md as the evidence-backed shared baseline.
- Use docs/plans/deferred-data-vault-capabilities.md as the ownership map for deferred capability families; PIT, bridge, and multi-active already have separate owner tickets and should carry their own snapshot diffs if they later export public API.
- Because provider packages share the DCoding.Data.DVault namespace, reviewers should anchor ownership and diffs on the package-specific approved snapshot filenames, not namespace text alone.
- The implementing story's final delivery summary or change description is the per-change note location for the internal-only case; unchanged approved snapshot files provide the matching repository evidence.
- The stale workflow-blocking relations around this shared guardrail were downgraded from `blocks` to `relates`; they are now traceability-only links to the capability owner stories and the completed decision-record task.

## Open Questions
- none

## Follow-Up Questions
- none

## Risks
- A future owning story may forget to include the explicit internal-only note even when snapshots remain unchanged.
- Developers may still try to add placeholder public APIs to force snapshot activity despite the contract forbidding that approach.

## Split Recommendations
- Do not split this into new development subtasks. Treat the ticket as closure/re-scope and mirror snapshot ownership into the concrete deferred-capability story that actually exports a public contract.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: protect new deferred capability contracts from accidental public API drift.

Acceptance Criteria:
- API snapshot coverage includes new public contracts or documents why a contract remains internal.
- Snapshot failures clearly identify deferred capability API changes.
- The test setup remains compatible with the existing package quality gate.