[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the ticket is a clear documentation-only recommendation, the persisted contract has no unresolved Open Questions, and the repository baseline supports 'use existing satellite patterns, not new STS/RTS core semantics'.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4RBA6WXPTV321ZT9M0XPV4/description.md sets PO handoff to ready_for_po_critic, states the deliverable is documentation/architecture guidance only, and its Open Questions section is '- none'.
- The same description file says entity-local privacy state should use hub-parent satellites, relationship/consent/effectivity state should use link-parent satellites, and concurrent series should use existing DrivingKey(...) multi-active semantics.
- src/DCoding.Data.DVault/Modeling/DataVaultTableKind.cs lists only Hub, Link, Satellite, PointInTime, Pit, and Bridge; there is no STS or RTS table kind.
- src/DCoding.Data.DVault/Modeling/DataVaultSatelliteMetadata.cs exposes ordinary and multi-active satellite construction only; the only optional extension is drivingKeyNames, with no STS/RTS-specific metadata kind or technical-column family.
- docs/releases/v0.13.0.md documents effectivity as caller-owned link-parent satellite state and explicitly says v0.13 does not add an effectivity-specific fluent API, metadata kind, entity family, validation layer, or technical column family.
- Repository grep over docs/model-first-governance.md and docs/production-adoption-checklist.md found the same public baseline at line 262 and line 28 respectively: effectivity remains ordinary caller-owned link-parent satellite state with optional DrivingKey(...), not a special builder.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md keeps privacy behavior inside an opt-in add-on boundary over existing save/read/provider seams rather than a core semantic change.
- git diff --name-only develop...HEAD shows this branch differs from develop only under .gicket/tickets/06FE4RBA6WXPTV321ZT9M0XPV4/**, and git show --stat --oneline --no-patch HEAD shows HEAD 801070291d1d443a42315aa39bfd9b52018a9d58 is the po-critic lease-claim commit, so there is no unreviewed product-code widening on this branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A small worked example of relationship or consent effectivity modeled as link-parent satellite state would make downstream documentation more concrete, but the ticket already captures this as a follow-up question rather than a blocker.
- If later documentation uses STS/RTS terminology, it should explicitly map that terminology back to ordinary hub-parent or link-parent satellites plus optional DrivingKey(...) so readers do not infer new core semantics.

Risky assumptions
- Downstream authors may still over-read the STS/RTS wording unless the implementation repeats the 'no new core semantics' rule near any example or decision note.
- Future privacy follow-on work could drift into provider-specific behavior or compliance guarantees unless it stays anchored to docs/architecture/dvault-v1-optional-privacy-extension-boundary.md and the done boundary ticket 06FE4R9PP99G6Q1PTPK4TKD460.

AC / test suggestions
- When the documentation change is executed, verify it cites the current shipped baseline from src/DCoding.Data.DVault/Modeling/DataVaultTableKind.cs, src/DCoding.Data.DVault/Modeling/DataVaultSatelliteMetadata.cs, and docs/releases/v0.13.0.md as direct evidence for 'no first-class STS/RTS semantics'.
- Add a doc-review check that the final wording covers the three bounded cases already named in the contract: hub-parent privacy state, link-parent consent/effectivity state, and concurrent series via DrivingKey(...).

Implementation watchouts
- Keep this ticket documentation-only. The current branch diff against develop is ticket metadata only; widening the task into core library or provider code would exceed the accepted scope.
- Do not introduce new DataVaultTableKind values, satellite metadata kinds, builders, or privacy-specific technical columns under this ticket; any future privacy-specific behavior must remain additive on existing provider-neutral abstractions.

Non-blocking notes
- The ticket already includes sensible follow-up and split guidance, so developer handoff can proceed without another PO refinement pass.

Split recommendations
- No split is needed for the current documentation/recommendation lane.
- If later evidence shows a real gap beyond ordinary satellites, link-parent satellites, and multi-active driving keys, split that work into one additive architecture/helper contract ticket and separate implementation/provider tickets instead of reopening core STS/RTS semantics inside this task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment