[gicket-bot] PO refinement contract

Summary
- Refined the v0.42 story as the acceptance-gate contract for provider-configured benchmark evidence, tuning thresholds, fallback behavior, documentation, and package-line movement, and verified that the live downstream split already exists across benchmark-lane, provider-tuning, hotspot, and documentation tasks.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The active v0.42 release metadata already fixes the release/package mapping: `v0.42.0 - Provider Performance Evidence and Tuning` maps to consumer package lines `8.42.0` and `10.42.0`; this story ratifies that baseline instead of reopening it.
- The root `benchmark-summary.json` already preserves latest-satellite `selectedStrategy` and `plannedReadStrategy` tokens for PostgreSQL, SQL Server, MySQL, Oracle, and DB2, but the checked-in external-provider rows are still `executionStatus=skipped` with `persistedOutcome=not executed` when the matching `DVAULT_TEST_*` connection string is unset; those rows are guidance, not completed timing evidence.
- The current starting save gates remain the bounded baseline until new provider-configured evidence supersedes them: PostgreSQL direct/UNNEST below 60 operations and staged COPY at 60-plus, SQL Server native bulk at 50-plus total operations and at most 500 satellite operations, MySQL retained multi-row versus staged bulk with tiny satellite-history fallback remaining provider-neutral, Oracle direct optimized batching at 50-plus total operations and at most 10000 satellite operations, and DB2 clean-context set-based save only with no staged bulk claim.
- Done tickets `06FBSC4QXYQ0SWB1DPMGJJ5XX0`, `06FBSCHBJEYYERDPA7JN34Y8PG`, and `06F8KZTNG44XDPMVTVCV4WJSHG` are historical baseline context only: the v0.39/v0.41 evidence docs and the provider-specific SQL artifact contract bound this story to evidence/tuning/documentation gates rather than new runtime artifact-dispatch scope.
- Live relation state already materializes the downstream split; no new child tickets were needed during this PO pass.

Scope In
- Define the v0.42 promotion rules for provider-configured benchmark evidence and the line between completed timing claims versus skipped, diagnostics-only, smoke-only, or other non-timing posture.
- Ratify the current starting thresholds and stop conditions for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 tuning work.
- Bound latest-satellite tuning to the visible provider set and supported shapes already present in the repository's diagnostics, gap-matrix, and benchmark baselines.
- Define which documentation/release surfaces must move together when tuning claims or caveats change.
- Define the v0.42 package-line expectations as `8.42.0` for `net8.0`/EF Core 8 and `10.42.0` for `net10.0`/EF Core 10, including synchronized verification/install guidance updates.

Scope Out
- Adding a new provider baseline, widening supported latest-satellite/PIT/bridge shapes, or introducing a new public save/read API.
- Promoting `skipped-placeholder`, `diagnostics-only`, or `smoke-only` rows into measured external-provider timing claims without a provider-configured artifact triplet and preserved run context.
- Automatic provider routing, automatic PIT/bridge maintenance, dashboards/SLOs, release publication/signing automation, or database provisioning.
- Deployable provider-specific SQL artifact generation or runtime dispatch; the done provider-specific SQL artifact contract remains review-only context.
- Changing stable-hash defaults, the public hash-key contract, or analyzer/runtime asset boundaries outside the coordinated package-line update.

Open questions
- none

Follow-up questions
- After v0.42 tuning lands, should the current story-to-task relation mix (`blocks` plus `relates`) be normalized to `parentOf` housekeeping, or is the existing workflow semantics intentional?
- If provider-configured v0.42 evidence materially changes a threshold, should adopter-facing docs keep the superseded v0.32 gate visible as historical comparison or replace it entirely with the promoted v0.42 gate?

Risks
- Most external-provider root rows are still skipped when the matching `DVAULT_TEST_*` environment variable is unset; without strict wording, downstream work could overstate strategy-registration rows as measured timing evidence.
- Provider-specific wins are sensitive to workload shape, operation counts, maintenance freshness, and clean-context prerequisites; threshold changes without preserved benchmark artifacts risk misleading tuning claims or regressions.
- DB2 remains especially narrow: completed timing, staged bulk, provider-native chunk execution, and live-schema-reading claims stay out of scope unless a new provider-configured artifact bundle lands.
- The `8.42.0`/`10.42.0` package-line move spans docs and verification tooling; partial updates would leave stale install guidance or verifier mismatches.

Split recommendations
- Already materialized: `06FE4QP6FB892E7TJMB47A3MSR` and `06FE4QPEZW97YR6YT7MQD1MXTG` separate latest-satellite lane normalization from DB2 promotion guardrails before downstream tuning claims are widened.
- Already materialized: `06FE4QPR8TF8R6PXNM3RMXN8JG`, `06FE4QQ0YTHD7624MGVPKKK1C0`, `06FE4QQ9VF7B74E60CXEHSS5XW`, `06FE4QQJCJH7J9AWQTPDR5DSSG`, and `06FE4QQTS5NFAYN39KP4QW2424` cover provider-specific latest-satellite and Oracle hotspot tuning against the normalized evidence contract.
- Already materialized: `06FE4QR3DD7EFZ4F35SBTFGWSR`, `06FE4QRC7D55RS8ZZ37ZAEJ98M`, and `06FE4QRMXVGJVA65ZR5MZ817K8` cover DB2 hotspot evidence, SQL Server bulk-threshold retuning, and the v0.42 documentation/release update; no further PO split is needed now.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment