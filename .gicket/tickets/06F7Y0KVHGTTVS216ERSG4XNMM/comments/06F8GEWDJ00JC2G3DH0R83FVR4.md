[gicket-bot] PO refinement contract

Summary
- Refined the story against existing preflight, live-schema drift, diagnostics, and relation evidence; no blocking PO questions remain and no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Verified local ticket state: the current ticket has only bot lease/claim comments, no attachments, an incoming `parentOf` relation from epic `06F7Y0J8PRFRSSWZ3GGT91S0TW`, and an outgoing `blocks` relation to documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4`; no relation cleanup was justified.
- The current owner branch is still at the PO lease-claim commit, so refinement is based on checked-in repository evidence rather than in-flight feature code on this ticket branch.
- `DataVaultPreflight.Run(...)` already aggregates caller-supplied validation/provider, artifact drift, snapshot drift, migration guardrail, and representative diagnostics lanes, and the README explicitly keeps omitted lanes skipped and live-database access opt-in.
- `DataVaultLiveSchemaReader.ReadAsync(...)` and `DataVaultLiveSchemaDriftReporter.Compare(...)` already provide bounded provider-aware live-schema comparison over DVault-owned tables, ordered columns, named primary-key constraints, and secondary indexes for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Provider capability and diagnostics surfaces already encode the physical differences this ticket must honor, including redundant-index suppression when a provider disallows secondary indexes covered by the primary key and include-column fallback behavior when native included indexes are unavailable.

Scope In
- Add one additive provider-aware idempotency preflight/report surface, or one new optional `DataVaultPreflight` lane backed by such a surface, that validates only idempotency-critical constraints and indexes instead of reopening general model drift.
- Validate hub business-key uniqueness, link participant access-path indexes, satellite latest-state parent/load/hash-diff structures including multi-active driving-key variants, PIT primary-key and traversal indexes, and bridge primary-key/traversal indexes when the metadata model projects those tables.
- Use the provider-selected translated DVault schema as the authoritative expected baseline so the check respects current identifier naming, uniqueness, included-column handling, descending-order handling, and provider-specific effective index shape.
- Keep live-schema reading explicit and consumer-owned: omitted live input or live lane selection remains skipped, while requested live checks evaluate against the existing classified `Succeeded`, `UnsupportedProvider`, and `Unavailable` outcomes.
- Return deterministic redacted findings that identify the affected DVault table and operation family and explain schema-level remediation without raw database values, SQL text, connection strings, or unbounded provider error details.

Scope Out
- No automatic schema repair, migration application, destructive DDL, repository scanning, or default live-database connection behavior.
- No replacement of the existing full model/artifact/live drift surfaces; non-idempotency table, column, or store-type drift stays on the current drift reports instead of being pulled into this ticket's contract.
- No foreign-key graph validation, arbitrary non-DVault catalog inspection, general physical-design advisory, or raw database-data capture.
- No new standalone CLI/platform surface; any command-line or support-bundle exposure should remain a thin consumer-owned wrapper or a separate follow-up if later justified.
- No public documentation rollout inside this story; the existing blocked documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4` remains the place for README, guide, and release-note updates after implementation lands.

Open questions
- none

Follow-up questions
- When `06F7Y0NBHXQ6CK8R3AH4DEP9V4` documents this behavior, should it present the new check primarily as an additive `DataVaultPreflight` lane, as a companion helper, or both?
- After this story lands, should support-bundle or consumer-owned design-time command flows preserve the idempotency report as an additive section, or keep it as a separate runtime/library check until adopter demand is clearer?
- If later work wants broader live-schema guardrails beyond idempotency-critical constraints and indexes, should that remain a separate follow-up story instead of widening this contract?

Risks
- If the comparison logic ignores provider-capability normalization, providers with redundant-index suppression or non-native included-index behavior will false-fail even when their generated schema is correct.
- If implementation reuses general drift reporting without filtering, the preflight output will become noisy and blur the ticket's idempotency-specific purpose.
- If live schema is opened implicitly on ordinary preflight runs, the implementation will violate the documented consumer-owned design-time boundary and make default local/CI execution brittle.
- If remediation output includes raw provider messages, connection details, or database values, it will break the repository's redaction contract.

Split recommendations
- No child-ticket split is required for PO-critic readiness; the repository already has the necessary preflight, live-schema, diagnostics, and provider-capability building blocks, so this remains one bounded additive story.
- Keep README, architecture-guide, checklist, and release-note rollout on the existing blocked documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4` instead of broadening this story's delivery contract.
- If future work wants non-idempotency live-schema drift aggregation, automatic repository discovery, or first-class support-bundle/command integration, track those as separate follow-up tickets rather than widening this ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment