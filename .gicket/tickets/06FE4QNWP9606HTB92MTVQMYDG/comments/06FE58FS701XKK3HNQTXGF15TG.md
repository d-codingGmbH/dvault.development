[gicket-bot] PO refinement contract

Summary
- Corrected the refinement contract to match live relation state: the downstream split is currently represented by active blocks and relates links, not live parentOf files, and no new child-ticket or relation write is required in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is corrected to describe the observed live split exactly as it exists now: active blocks and relates links represent the downstream work, while parentOf survives only as historical removal events. This pass does not claim restored live parentOf relations.
- critic-item-2: `answered` - Repository inspection confirms the stale live-parentOf claim was wrong. The source ticket currently has 3 active blocks files, 10 active relates files, 0 active parentOf files under .gicket/relations, and 10 historical TicketRelationRemoved events for parentOf in its event history.
- critic-item-3: `answered` - The authoritative ticket-level split model is now explicit: blocks remain active from this story to 06FE4QP6FB892E7TJMB47A3MSR, 06FE4QPEZW97YR6YT7MQD1MXTG, and 06FE4QRC7D55RS8ZZ37ZAEJ98M; active relates links cover all ten downstream tickets. No new relation write is needed because the contract now matches the live relation state instead of asserting a nonexistent hierarchy.

Clarifications
- The active release metadata already fixes the planning release baseline: v0.42.0 - Provider Performance Evidence and Tuning maps to consumer package lines 8.42.0 and 10.42.0.
- The root benchmark-summary.json keeps PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite, PIT, and bridge guidance rows visible even when the matching DVAULT_TEST_* connection string is unset, but those rows remain executionStatus=skipped with persistedOutcome=not executed and are not completed timing evidence.
- docs/production-adoption-checklist.md is part of the coordinated v0.42 documentation surface because it is a current adopter-facing baseline that still carries 8.41.0 / 10.41.0 package guidance and historical evidence-routing language.
- Current repository tooling and adopter-facing docs are still pinned to the v0.41 consumer lines across tools/pack-release-packages.sh, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, docs/production-adoption-checklist.md, and the current documentation baseline, so the v0.42 package/doc move remains downstream delivery work.
- The live downstream split is not represented by active parentOf relations. It is currently represented by active blocks links to 06FE4QP6FB892E7TJMB47A3MSR, 06FE4QPEZW97YR6YT7MQD1MXTG, and 06FE4QRC7D55RS8ZZ37ZAEJ98M, plus active relates links across all ten downstream tickets, with historical parentOf removal events preserved in ticket history.

Scope In
- Define the v0.42 promotion rules for provider-configured benchmark evidence and the line between completed timing claims versus skipped, diagnostics-only, smoke-only, or other non-timing posture.
- Ratify the current starting thresholds and stop conditions for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 tuning work.
- Bound latest-satellite tuning to the visible provider set and supported shapes already present in repository diagnostics, the evidence matrix, the gap matrix, and the checked-in benchmark baseline.
- Define the coordinated v0.42 documentation and release surface as docs/performance-profiles.md, docs/plans/provider-optimization-evidence-matrix.md, docs/plans/provider-optimization-gap-matrix.md, docs/releases/v0.42.0.md, CHANGELOG.md, README.md, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, and docs/production-adoption-checklist.md.
- Define the v0.42 consumer package-line expectations as 8.42.0 for net8.0 and EF Core 8 and 10.42.0 for net10.0 and EF Core 10, including synchronized pack, verify, install, validation, and adopter-guidance updates.

Scope Out
- Adding a new provider baseline, widening supported latest-satellite, PIT, or bridge shapes, or introducing a new public save or read API.
- Promoting skipped-placeholder, diagnostics-only, or smoke-only rows into measured external-provider timing claims without a provider-configured benchmark artifact triplet and preserved run context.
- Automatic provider routing, automatic PIT or bridge maintenance, dashboards, SLOs, release publication or signing automation, or database provisioning.
- Deployable provider-specific SQL artifact generation or runtime dispatch; the done provider-specific SQL artifact contract remains historical context only.
- Changing stable-hash defaults, the public hash-key contract, or analyzer/runtime asset boundaries outside the coordinated package-line move.

Open questions
- none

Follow-up questions
- After downstream delivery closes, should the current story-to-task relation mix be normalized into a single intended relation model, or is the live blocks-plus-relates workflow semantics intentional?
- If v0.42 evidence materially changes a threshold, how much superseded v0.32 comparator detail should remain in adopter-facing documentation versus historical release history only?

Risks
- Most external-provider root rows are still skipped when the matching DVAULT_TEST_* environment variable is unset; without strict wording, downstream work could overstate strategy-registration rows as measured timing evidence.
- Provider-specific wins are sensitive to workload shape, operation counts, maintenance freshness, and clean-context prerequisites; threshold changes without preserved benchmark artifacts risk misleading tuning claims or regressions.
- DB2 remains especially narrow: completed timing, staged bulk, provider-native chunk execution, and live-schema-reading claims stay out of scope unless a new provider-configured artifact bundle lands.
- The 8.42.0 and 10.42.0 package-line move spans tooling and multiple adopter-facing docs, including production adoption guidance; partial updates would leave stale install, validation, or evidence wording behind.
- The current live split uses workflow links rather than an active hierarchy; later automation that assumes parentOf semantics would misread the ticket graph until a deliberate relation migration is performed.

Split recommendations
- Already materialized as active blocks follow-up work from this story: 06FE4QP6FB892E7TJMB47A3MSR, 06FE4QPEZW97YR6YT7MQD1MXTG, and 06FE4QRC7D55RS8ZZ37ZAEJ98M.
- Already materialized as active relates follow-up work from this story: 06FE4QP6FB892E7TJMB47A3MSR, 06FE4QPEZW97YR6YT7MQD1MXTG, 06FE4QPR8TF8R6PXNM3RMXN8JG, 06FE4QQ0YTHD7624MGVPKKK1C0, 06FE4QQ9VF7B74E60CXEHSS5XW, 06FE4QQJCJH7J9AWQTPDR5DSSG, 06FE4QQTS5NFAYN39KP4QW2424, 06FE4QR3DD7EFZ4F35SBTFGWSR, 06FE4QRC7D55RS8ZZ37ZAEJ98M, and 06FE4QRMXVGJVA65ZR5MZ817K8.
- No additional PO split or relation write is recommended now; if a later workflow needs hierarchical parent-child semantics, handle that as explicit relation-normalization work rather than inferring live parentOf state from the current graph.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment