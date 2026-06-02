[gicket-bot] PO refinement contract

Summary
- Repository evidence is sufficient to ratify this as the contract ticket for request-bound provider tuning diagnostics built on the existing diagnostics and tracing vocabulary; no new child tickets or planning-document writes are needed because related implementation stories already exist.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 surface is request-bound provider tuning diagnostics layered onto the existing DataVaultDiagnosticsResult strategy and read-shape model, not a new telemetry-only API and not an automatic optimizer.
- Save-path tuning should ratify the currently visible DataVaultSaveStrategyDiagnostics facts: selected strategy name and priority, candidate eligibility, finite gate requirements, fallback causes, and staged-provider bulk caveats.
- Read-path tuning should ratify the currently visible DataVaultReadStrategyDiagnostics plus ReadShape.Provider facts for the supported read kinds LatestSatellite, PitAsOf, and Bridge; v1 does not invent numeric read thresholds where the repository shows none.
- Benchmark-profile vocabulary should default to the four checked-in docs/performance-profiles.md start profiles: Small app-local vault, Medium chunked ingestion, Staged provider ingestion, and Read-model heavy.
- Recommendation output is bounded guidance that points callers to one of the checked-in profiles or to provider-neutral fallback; exact enum member names are implementation detail, but the category set is closed for v1 and non-applicable fields are omitted instead of filled with placeholders.
- Redaction and omission rules should inherit the v0.25 read-plan baseline already landed by done ticket 06F7Y0HZKHBHMYX9EYDYFRYXZ0: no raw keys, raw timestamps, SQL text, query plans, credentials, connection strings, provider exception text, stack traces, or workload values.

Scope In
- Define the request-bound provider tuning diagnostics contract for single, bulk, and chunked save paths plus the currently supported read paths.
- Ratify finite status, fallback, eligibility, threshold, selected-strategy, and provider-profile facts using existing diagnostics and tracing vocabulary where the repository already proves them.
- Define bounded benchmark-profile and recommendation categories, including omission rules when no evidence-backed profile or recommendation applies.
- Specify the redaction boundary and the separation between diagnostics output and any automatic optimization, deployment, or DBA behavior.

Scope Out
- No automatic batch-size tuning, strategy switching, deployment, migration, or DBA workflow automation.
- No raw SQL, provider plan capture, credentials, connection strings, provider error text, exception messages, stack traces, or workload data serialization.
- No new save or read strategy implementations, benchmark reruns, or benchmark artifact schema changes in this ticket.
- No expansion beyond the supported read kinds and the current checked-in performance-profile categories.
- No dashboard, exporter, alerting, or support-bundle transport automation work.

Open questions
- none

Follow-up questions
- After implementation lands, should a later ticket add explicit cross-links from each recommendation category to the matching section in docs/performance-profiles.md and the verified benchmark artifacts?
- When dedicated read-path benchmark evidence exists, should a follow-up ticket add finite numeric read-threshold guidance for provider-specific read recommendations instead of today's profile-only mapping?

Risks
- Recommendation prose can drift from checked-in evidence unless the implementation keeps the machine-readable category set anchored to the four current performance profiles.
- Read tuning could overpromise provider-specific behavior if developers infer query-plan or numeric-threshold claims that the current repository does not prove.
- Redaction can regress if provider exception text or workload values leak into example payloads instead of staying behind finite fallback messages and omitted optional fields.

Split recommendations
- Keep the current split: this ticket defines the contract, 06F7Y0JZKTVBGGQ9Q4EBC2PCDG implements eligibility, threshold, and recommendation diagnostics, and 06F7Y0K95VW0PX21F6R2YGP8DM owns benchmark-artifact verification.
- If the team later wants new benchmark profiles, provider-specific read thresholds, or transport and reporting surfaces, create separate follow-up stories rather than widening this contract ticket.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment