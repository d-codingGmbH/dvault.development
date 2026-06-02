<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence is sufficient to ratify this as the contract ticket for request-bound provider tuning diagnostics built on the existing diagnostics and tracing vocabulary; no new child tickets or planning-document writes are needed because related implementation stories already exist.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The v1 surface is request-bound provider tuning diagnostics layered onto the existing DataVaultDiagnosticsResult strategy and read-shape model, not a new telemetry-only API and not an automatic optimizer.
- Save-path tuning should ratify the currently visible DataVaultSaveStrategyDiagnostics facts: selected strategy name and priority, candidate eligibility, finite gate requirements, fallback causes, and staged-provider bulk caveats.
- Read-path tuning should ratify the currently visible DataVaultReadStrategyDiagnostics plus ReadShape.Provider facts for the supported read kinds LatestSatellite, PitAsOf, and Bridge; v1 does not invent numeric read thresholds where the repository shows none.
- Benchmark-profile vocabulary should default to the four checked-in docs/performance-profiles.md start profiles: Small app-local vault, Medium chunked ingestion, Staged provider ingestion, and Read-model heavy.
- Recommendation output is bounded guidance that points callers to one of the checked-in profiles or to provider-neutral fallback; exact enum member names are implementation detail, but the category set is closed for v1 and non-applicable fields are omitted instead of filled with placeholders.
- Redaction and omission rules should inherit the v0.25 read-plan baseline already landed by done ticket 06F7Y0HZKHBHMYX9EYDYFRYXZ0: no raw keys, raw timestamps, SQL text, query plans, credentials, connection strings, provider exception text, stack traces, or workload values.

### Scope In
- Define the request-bound provider tuning diagnostics contract for single, bulk, and chunked save paths plus the currently supported read paths.
- Ratify finite status, fallback, eligibility, threshold, selected-strategy, and provider-profile facts using existing diagnostics and tracing vocabulary where the repository already proves them.
- Define bounded benchmark-profile and recommendation categories, including omission rules when no evidence-backed profile or recommendation applies.
- Specify the redaction boundary and the separation between diagnostics output and any automatic optimization, deployment, or DBA behavior.

### Scope Out
- No automatic batch-size tuning, strategy switching, deployment, migration, or DBA workflow automation.
- No raw SQL, provider plan capture, credentials, connection strings, provider error text, exception messages, stack traces, or workload data serialization.
- No new save or read strategy implementations, benchmark reruns, or benchmark artifact schema changes in this ticket.
- No expansion beyond the supported read kinds and the current checked-in performance-profile categories.
- No dashboard, exporter, alerting, or support-bundle transport automation work.

## Acceptance Criteria
- The refined contract states that provider tuning diagnostics are request-bound and additive to the existing diagnostics surface, with separate save and read strategy facts and no automatic behavior changes.
- Save diagnostics cover selected strategy, selected priority, candidate eligibility, finite fallback causes, and currently evidenced gate requirements, including dirty-context, multi-active, provider-name mismatch, staged-provider bulk caveats, SQL Server minimum and maximum gates, MySQL minimum gate, and Oracle minimum and maximum gates.
- Read diagnostics cover selected strategy, candidate eligibility, finite fallback causes, provider and read-shape facts, and the supported read kinds LatestSatellite, PitAsOf, and Bridge; numeric read thresholds are omitted unless later repository evidence adds them.
- Benchmark-profile references use only the current checked-in profile categories Small app-local vault, Medium chunked ingestion, Staged provider ingestion, and Read-model heavy, and the profile field is omitted when no evidence-backed mapping applies.
- Recommendation output uses a closed machine-readable category set with bounded human messages, and consumers are not required to parse prose to determine fallback or tuning guidance.
- Serialized or documented examples omit raw keys, raw timestamps, SQL text, query plans, credentials, provider exception messages, stack traces, and workload data values, while also omitting non-applicable optional fields such as selectedStrategyName, threshold facts, benchmark profile, or recommendation when they do not apply.
- The contract explicitly distinguishes diagnostics from automatic optimization, deployment posture, or benchmark publication claims.

## Definition of Done
- An authoritative ticket-level contract ratifies the existing diagnostics and activity vocabulary instead of inventing parallel naming for provider, strategy status, fallback, and read mode.
- The contract leaves no blocking ambiguity for related implementation tickets 06F7Y0JZKTVBGGQ9Q4EBC2PCDG and 06F7Y0K95VW0PX21F6R2YGP8DM about supported read kinds, save thresholds, benchmark-profile categories, or redaction rules.
- The contract preserves the v0.25 omission rule that non-applicable optional fields stay absent rather than using placeholder strings or sentinel text.
- The contract keeps performance-profile mapping anchored to checked-in documentation and does not overstate unsupported provider read or write behavior.

## Implementation Notes
- Use DataVaultDiagnosticsResult as the canonical top-level diagnostics payload; add provider-tuning details as additive structured sections rather than a separate free-form blob.
- Reuse DataVaultSaveStrategyDiagnostics, DataVaultReadStrategyDiagnostics, DataVaultReadShapeDiagnostics, and DataVaultActivityTracing terminology so diagnostics, tracing, and docs stay aligned.
- Treat current checked-in save thresholds as the v1 baseline: SQL Server minimum 50 total operations and maximum 500 satellite operations, MySQL minimum total-operation gate, Oracle minimum 50 total operations and maximum 10000 satellite operations, plus dirty-context, multi-active, provider mismatch, and staged-provider bulk decline causes.
- Treat read tuning as profile-and-fallback guidance over the existing supported read kinds, not as raw query-plan or numeric-threshold analysis; if no evidence-backed benchmark profile or recommendation applies, omit those fields.
- Map benchmark-profile guidance to the four current checked-in performance profiles and keep copied timing claims tied to the existing benchmark artifact contract rather than embedding raw benchmark rows in diagnostics.
- Keep recommendation categories closed and deterministic, but let the developer choose exact enum or member names as long as each category maps 1:1 to an already documented profile or provider-neutral fallback posture.
- The done documentation ticket 06F7Y0HZKHBHMYX9EYDYFRYXZ0 is landed redaction and read-plan baseline evidence; this ticket extends that bounded disclosure posture rather than reopening it.
- Keep this ticket contract-only; related implementation and evidence-verifier work already belongs in 06F7Y0JZKTVBGGQ9Q4EBC2PCDG and 06F7Y0K95VW0PX21F6R2YGP8DM.

## Open Questions
- none

## Follow-Up Questions
- After implementation lands, should a later ticket add explicit cross-links from each recommendation category to the matching section in docs/performance-profiles.md and the verified benchmark artifacts?
- When dedicated read-path benchmark evidence exists, should a follow-up ticket add finite numeric read-threshold guidance for provider-specific read recommendations instead of today's profile-only mapping?

## Risks
- Recommendation prose can drift from checked-in evidence unless the implementation keeps the machine-readable category set anchored to the four current performance profiles.
- Read tuning could overpromise provider-specific behavior if developers infer query-plan or numeric-threshold claims that the current repository does not prove.
- Redaction can regress if provider exception text or workload values leak into example payloads instead of staying behind finite fallback messages and omitted optional fields.

## Split Recommendations
- Keep the current split: this ticket defines the contract, 06F7Y0JZKTVBGGQ9Q4EBC2PCDG implements eligibility, threshold, and recommendation diagnostics, and 06F7Y0K95VW0PX21F6R2YGP8DM owns benchmark-artifact verification.
- If the team later wants new benchmark profiles, provider-specific read thresholds, or transport and reporting surfaces, create separate follow-up stories rather than widening this contract ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Define the bounded contract for provider tuning diagnostics before implementation.

# Scope In
- Define eligibility, threshold, selected strategy, fallback, benchmark profile, and recommendation vocabulary for save/read provider paths.
- Reuse existing telemetry and performance-profile concepts where possible.
- Define redaction and omission rules.

# Acceptance Criteria
- Contract avoids raw SQL, query plans, credentials, provider exception messages, and workload data values.
- Contract distinguishes diagnostics from automatic optimization or deployment behavior.