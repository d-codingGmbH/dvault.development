<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Corrected the refinement contract to match live relation state: the downstream split is currently represented by active blocks and relates links, not live parentOf files, and no new child-ticket or relation write is required in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The active release metadata already fixes the planning release baseline: v0.42.0 - Provider Performance Evidence and Tuning maps to consumer package lines 8.42.0 and 10.42.0.
- The root benchmark-summary.json keeps PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite, PIT, and bridge guidance rows visible even when the matching DVAULT_TEST_* connection string is unset, but those rows remain executionStatus=skipped with persistedOutcome=not executed and are not completed timing evidence.
- docs/production-adoption-checklist.md is part of the coordinated v0.42 documentation surface because it is a current adopter-facing baseline that still carries 8.41.0 / 10.41.0 package guidance and historical evidence-routing language.
- Current repository tooling and adopter-facing docs are still pinned to the v0.41 consumer lines across tools/pack-release-packages.sh, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, docs/production-adoption-checklist.md, and the current documentation baseline, so the v0.42 package/doc move remains downstream delivery work.
- The live downstream split is not represented by active parentOf relations. It is currently represented by active blocks links to 06FE4QP6FB892E7TJMB47A3MSR, 06FE4QPEZW97YR6YT7MQD1MXTG, and 06FE4QRC7D55RS8ZZ37ZAEJ98M, plus active relates links across all ten downstream tickets, with historical parentOf removal events preserved in ticket history.

### Scope In
- Define the v0.42 promotion rules for provider-configured benchmark evidence and the line between completed timing claims versus skipped, diagnostics-only, smoke-only, or other non-timing posture.
- Ratify the current starting thresholds and stop conditions for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 tuning work.
- Bound latest-satellite tuning to the visible provider set and supported shapes already present in repository diagnostics, the evidence matrix, the gap matrix, and the checked-in benchmark baseline.
- Define the coordinated v0.42 documentation and release surface as docs/performance-profiles.md, docs/plans/provider-optimization-evidence-matrix.md, docs/plans/provider-optimization-gap-matrix.md, docs/releases/v0.42.0.md, CHANGELOG.md, README.md, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, and docs/production-adoption-checklist.md.
- Define the v0.42 consumer package-line expectations as 8.42.0 for net8.0 and EF Core 8 and 10.42.0 for net10.0 and EF Core 10, including synchronized pack, verify, install, validation, and adopter-guidance updates.

### Scope Out
- Adding a new provider baseline, widening supported latest-satellite, PIT, or bridge shapes, or introducing a new public save or read API.
- Promoting skipped-placeholder, diagnostics-only, or smoke-only rows into measured external-provider timing claims without a provider-configured benchmark artifact triplet and preserved run context.
- Automatic provider routing, automatic PIT or bridge maintenance, dashboards, SLOs, release publication or signing automation, or database provisioning.
- Deployable provider-specific SQL artifact generation or runtime dispatch; the done provider-specific SQL artifact contract remains historical context only.
- Changing stable-hash defaults, the public hash-key contract, or analyzer/runtime asset boundaries outside the coordinated package-line move.

## Acceptance Criteria
- Provider evidence claims cite the canonical matrix row identity of scenario, provider, baseline, and posture, and a row counts as measured timing only when a provider-configured benchmark artifact triplet with preserved run context completed that row.
- The current provider tuning gates are fixed as starting boundaries, not universal promises: PostgreSQL keeps direct or UNNEST below 60 operations and staged COPY at 60-plus; SQL Server keeps native bulk at 50-plus total operations and no more than 500 satellite operations; MySQL keeps retained multi-row behavior for smaller eligible batches and staged bulk for larger eligible batches with tiny satellite-history fallback remaining provider-neutral; Oracle keeps direct optimized batching at 50-plus total operations and no more than 10000 satellite operations and makes no staged Oracle claim without new evidence; DB2 stays limited to clean-context set-based save with no staged bulk or provider-native chunk-execution claim.
- Latest-satellite tuning remains limited to the visible provider set of PostgreSQL, SQL Server, MySQL, Oracle, and DB2 and the already documented supported hub-parent, non-multi-active shapes; no new provider or shape baseline is implied by this ticket.
- Fallback behavior is explicit: unset DVAULT_TEST_* connection strings, provider mismatch, dirty contexts, unsupported shapes, incomplete diagnostics or read-shape evidence, stale PIT or bridge maintenance, or diagnostics that do not select the provider strategy must keep the provider-neutral save and read path as the public fallback.
- The coordinated v0.42 documentation surface explicitly includes docs/performance-profiles.md, docs/plans/provider-optimization-evidence-matrix.md, docs/plans/provider-optimization-gap-matrix.md, docs/releases/v0.42.0.md, CHANGELOG.md, README.md, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, and docs/production-adoption-checklist.md, and each surface must distinguish measured improvements, deferred gaps, provider-specific caveats, and historical baselines without backporting unsupported timing claims.
- Planning release v0.42.0 maps to consumer package lines 8.42.0 and 10.42.0, never to a consumer-facing 0.42.0 package version, and any package-line move must update pack, verify, install, validation, and adopter guidance together while clearing stale non-historical 8.41.0 and 10.41.0 guidance from the coordinated v0.42 surfaces.

## Definition of Done
- Downstream tasks can evaluate pass and fail using one shared rule set for completed timing, skipped guidance rows, diagnostics-only posture, smoke-only posture, and provider-neutral fallback.
- Every promoted v0.42 provider claim either cites completed provider-configured benchmark evidence with preserved run context or remains explicitly documented as skipped, deferred, diagnostics-only, smoke-only, or fallback-only.
- Threshold, stop-condition, and fallback wording is consistent across docs/performance-profiles.md, docs/plans/provider-optimization-evidence-matrix.md, docs/plans/provider-optimization-gap-matrix.md, docs/releases/v0.42.0.md, CHANGELOG.md, README.md, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, and docs/production-adoption-checklist.md.
- The v0.42 package-line story is consistent across tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, release notes and changelog, README, package compatibility, manual publication, local validation, and production adoption guidance, and no non-historical coordinated surface still presents stale 8.41.0 or 10.41.0 guidance or a consumer-facing 0.42.0 package version as current install guidance.
- No finished story text claims new provider support, automatic routing or maintenance, provider-specific SQL runtime dispatch, or package publication evidence.

## Implementation Notes
- Live split evidence for source ticket 06FE4QNWP9606HTB92MTVQMYDG is 3 active blocks files, 10 active relates files, 0 active parentOf files under .gicket/relations, and 10 parentOf removal events under .gicket/tickets/06FE4QNWP9606HTB92MTVQMYDG/events; this contract therefore treats the split as workflow links plus historical parentOf removals, not as an active hierarchy.
- When v0.42 moves package lines, keep tools/pack-release-packages.sh, tools/DCoding.Data.DVault.PackageVerification/PackageVerifier.cs, README.md, docs/package-compatibility.md, docs/manual-nuget-publication.md, docs/local-validation.md, CHANGELOG.md, and docs/production-adoption-checklist.md aligned with 8.42.0 and 10.42.0.
- docs/releases/v0.42.0.md is still missing in the current branch snapshot, so downstream documentation work must add it instead of treating release-note alignment as already landed.

## Open Questions
- none

## Follow-Up Questions
- After downstream delivery closes, should the current story-to-task relation mix be normalized into a single intended relation model, or is the live blocks-plus-relates workflow semantics intentional?
- If v0.42 evidence materially changes a threshold, how much superseded v0.32 comparator detail should remain in adopter-facing documentation versus historical release history only?

## Risks
- Most external-provider root rows are still skipped when the matching DVAULT_TEST_* environment variable is unset; without strict wording, downstream work could overstate strategy-registration rows as measured timing evidence.
- Provider-specific wins are sensitive to workload shape, operation counts, maintenance freshness, and clean-context prerequisites; threshold changes without preserved benchmark artifacts risk misleading tuning claims or regressions.
- DB2 remains especially narrow: completed timing, staged bulk, provider-native chunk execution, and live-schema-reading claims stay out of scope unless a new provider-configured artifact bundle lands.
- The 8.42.0 and 10.42.0 package-line move spans tooling and multiple adopter-facing docs, including production adoption guidance; partial updates would leave stale install, validation, or evidence wording behind.
- The current live split uses workflow links rather than an active hierarchy; later automation that assumes parentOf semantics would misread the ticket graph until a deliberate relation migration is performed.

## Split Recommendations
- Already materialized as active blocks follow-up work from this story: 06FE4QP6FB892E7TJMB47A3MSR, 06FE4QPEZW97YR6YT7MQD1MXTG, and 06FE4QRC7D55RS8ZZ37ZAEJ98M.
- Already materialized as active relates follow-up work from this story: 06FE4QP6FB892E7TJMB47A3MSR, 06FE4QPEZW97YR6YT7MQD1MXTG, 06FE4QPR8TF8R6PXNM3RMXN8JG, 06FE4QQ0YTHD7624MGVPKKK1C0, 06FE4QQ9VF7B74E60CXEHSS5XW, 06FE4QQJCJH7J9AWQTPDR5DSSG, 06FE4QQTS5NFAYN39KP4QW2424, 06FE4QR3DD7EFZ4F35SBTFGWSR, 06FE4QRC7D55RS8ZZ37ZAEJ98M, and 06FE4QRMXVGJVA65ZR5MZ817K8.
- No additional PO split or relation write is recommended now; if a later workflow needs hierarchical parent-child semantics, handle that as explicit relation-normalization work rather than inferring live parentOf state from the current graph.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: define v0.42 acceptance gates for provider-configured benchmark evidence, tuning thresholds, fallback behavior, documentation, and package-version expectations. Acceptance: downstream tasks have clear pass/fail criteria and non-goals.