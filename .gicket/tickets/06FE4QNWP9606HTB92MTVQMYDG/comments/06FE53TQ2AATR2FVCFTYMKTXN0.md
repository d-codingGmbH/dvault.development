[gicket-bot] PO refinement contract

Summary
- Verified the blocking PO-critic findings against persisted ticket state and repository evidence, explicitly brought docs/production-adoption-checklist.md into the coordinated v0.42 documentation surface, corrected the stale implementation-note interpretation around the earlier description update, and confirmed that no new child-ticket or relation write is needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - docs/production-adoption-checklist.md is explicitly in scope for the coordinated v0.42 documentation move. It is a current adopter-facing baseline document, and because it still carries v0.41 package and evidence guidance, the contract must require it to move together with performance profiles, evidence and gap matrices, release notes and changelog, README, package compatibility, manual publication, and local validation so adopter guidance cannot stay stale.
- critic-item-2: `answered` - The prior implementation note was historically inaccurate. Repository-backed ticket evidence shows the earlier PO pass already refreshed the durable ticket description, so the authoritative clarification is that this refinement response applies no new description write, but it must not describe the prior pass as if no description update happened.
- critic-item-3: `answered` - The coordinated v0.42 documentation surface is now explicitly bounded to include docs/production-adoption-checklist.md, and the contract now requires the final v0.42 package and evidence move to clear stale non-historical 8.41.0 and 10.41.0 adopter guidance from that surface and the other coordinated docs. That closes the downstream gap identified by the blocking finding.

Clarifications
- The active release metadata already fixes the planning release mapping: v0.42.0 - Provider Performance Evidence and Tuning maps to consumer package lines 8.42.0 and 10.42.0, and this story ratifies that baseline instead of reopening it.
- The root benchmark-summary.json preserves selectedStrategy and plannedReadStrategy tokens for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite, PIT, and bridge rows, but those optional-provider rows remain executionStatus=skipped with persistedOutcome=not executed when the matching DVAULT_TEST_* connection string is unset; they are guidance, not completed timing evidence.
- docs/production-adoption-checklist.md is part of the coordinated v0.42 documentation surface because it is current adopter-facing package and evidence guidance, not only historical background.
- Current repository tooling and current-doc surfaces are still pinned to the v0.41 consumer package lines across the pack script, package verifier, README, package compatibility, manual publication, local validation, changelog, and production adoption checklist; the v0.42 move is still downstream delivery work rather than already-landed repository content.
- Live relation state already materializes the downstream split. Local .gicket relation files show parentOf links for the downstream task set, with historical blocks and relates links still present as workflow context, so no new child-ticket or relation write is needed for this refinement.
- The earlier contract note claiming that no ticket-description update occurred in the prior PO pass is superseded by the persisted ticket comments and diff evidence that the durable description refresh already happened.

Scope In
- Define the v0.42 promotion rules for provider-configured benchmark evidence and the line between completed timing claims versus skipped, diagnostics-only, smoke-only, or other non-timing posture.
- Ratify the current starting thresholds and stop conditions for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 tuning work.
- Bound latest-satellite tuning to the visible provider set and supported shapes already present in the repository diagnostics, gap-matrix, and benchmark baselines.
- Define the coordinated v0.42 documentation and release surface as docs/performance-profiles.md, docs/plans/provider-optimization-evidence-matrix.md, docs/plans/provider-optimization-gap-matrix.md, docs/releases/v0.42.0.md, CHANGELOG.md, README.md, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, and docs/production-adoption-checklist.md.
- Define the v0.42 package-line expectations as 8.42.0 for net8.0 and EF Core 8 and 10.42.0 for net10.0 and EF Core 10, including synchronized pack, verify, install, and adopter-guidance updates.

Scope Out
- Adding a new provider baseline, widening supported latest-satellite, PIT, or bridge shapes, or introducing a new public save or read API.
- Promoting skipped-placeholder, diagnostics-only, or smoke-only rows into measured external-provider timing claims without a provider-configured benchmark artifact triplet and preserved run context.
- Automatic provider routing, automatic PIT or bridge maintenance, dashboards, SLOs, release publication or signing automation, or database provisioning.
- Deployable provider-specific SQL artifact generation or runtime dispatch; the done provider-specific SQL artifact contract remains historical context only.
- Changing stable-hash defaults, the public hash-key contract, or analyzer and runtime asset boundaries outside the coordinated package-line move.

Open questions
- none

Follow-up questions
- After downstream delivery closes, should the redundant historical blocks and relates links be cleaned up now that parentOf relations already exist for the same task set?
- If v0.42 evidence materially changes a threshold, how much comparator detail should remain in archival performance and release history beyond keeping v0.32 as historical evidence rather than current adopter guidance?

Risks
- Most external-provider root rows are still skipped when the matching DVAULT_TEST_* environment variable is unset; without strict wording, downstream work could overstate strategy-registration rows as measured timing evidence.
- Provider-specific wins are sensitive to workload shape, operation counts, maintenance freshness, and clean-context prerequisites; threshold changes without preserved benchmark artifacts risk misleading tuning claims or regressions.
- DB2 remains especially narrow: completed timing, staged bulk, provider-native chunk execution, and live-schema-reading claims stay out of scope unless a new provider-configured artifact bundle lands.
- The 8.42.0 and 10.42.0 package-line move spans tooling and multiple adopter-facing docs, including production adoption guidance; partial updates would leave stale install, validation, or evidence wording behind.

Split recommendations
- Already materialized: 06FE4QP6FB892E7TJMB47A3MSR and 06FE4QPEZW97YR6YT7MQD1MXTG separate latest-satellite lane normalization from DB2 promotion guardrails before downstream tuning claims are widened.
- Already materialized: 06FE4QPR8TF8R6PXNM3RMXN8JG, 06FE4QQ0YTHD7624MGVPKKK1C0, 06FE4QQ9VF7B74E60CXEHSS5XW, 06FE4QQJCJH7J9AWQTPDR5DSSG, and 06FE4QQTS5NFAYN39KP4QW2424 cover provider-specific latest-satellite and Oracle hotspot tuning against the normalized evidence contract.
- Already materialized: 06FE4QR3DD7EFZ4F35SBTFGWSR, 06FE4QRC7D55RS8ZZ37ZAEJ98M, and 06FE4QRMXVGJVA65ZR5MZ817K8 cover DB2 hotspot evidence, SQL Server bulk-threshold retuning, and coordinated v0.42 documentation and release updates; no further PO split is needed now.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 2

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment