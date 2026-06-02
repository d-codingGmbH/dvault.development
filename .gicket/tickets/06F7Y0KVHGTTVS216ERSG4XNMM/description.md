<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story against existing preflight, live-schema drift, diagnostics, and relation evidence; no blocking PO questions remain and no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Verified local ticket state: the current ticket has only bot lease/claim comments, no attachments, an incoming `parentOf` relation from epic `06F7Y0J8PRFRSSWZ3GGT91S0TW`, and an outgoing `blocks` relation to documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4`; no relation cleanup was justified.
- The current owner branch is still at the PO lease-claim commit, so refinement is based on checked-in repository evidence rather than in-flight feature code on this ticket branch.
- `DataVaultPreflight.Run(...)` already aggregates caller-supplied validation/provider, artifact drift, snapshot drift, migration guardrail, and representative diagnostics lanes, and the README explicitly keeps omitted lanes skipped and live-database access opt-in.
- `DataVaultLiveSchemaReader.ReadAsync(...)` and `DataVaultLiveSchemaDriftReporter.Compare(...)` already provide bounded provider-aware live-schema comparison over DVault-owned tables, ordered columns, named primary-key constraints, and secondary indexes for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- Provider capability and diagnostics surfaces already encode the physical differences this ticket must honor, including redundant-index suppression when a provider disallows secondary indexes covered by the primary key and include-column fallback behavior when native included indexes are unavailable.

### Scope In
- Add one additive provider-aware idempotency preflight/report surface, or one new optional `DataVaultPreflight` lane backed by such a surface, that validates only idempotency-critical constraints and indexes instead of reopening general model drift.
- Validate hub business-key uniqueness, link participant access-path indexes, satellite latest-state parent/load/hash-diff structures including multi-active driving-key variants, PIT primary-key and traversal indexes, and bridge primary-key/traversal indexes when the metadata model projects those tables.
- Use the provider-selected translated DVault schema as the authoritative expected baseline so the check respects current identifier naming, uniqueness, included-column handling, descending-order handling, and provider-specific effective index shape.
- Keep live-schema reading explicit and consumer-owned: omitted live input or live lane selection remains skipped, while requested live checks evaluate against the existing classified `Succeeded`, `UnsupportedProvider`, and `Unavailable` outcomes.
- Return deterministic redacted findings that identify the affected DVault table and operation family and explain schema-level remediation without raw database values, SQL text, connection strings, or unbounded provider error details.

### Scope Out
- No automatic schema repair, migration application, destructive DDL, repository scanning, or default live-database connection behavior.
- No replacement of the existing full model/artifact/live drift surfaces; non-idempotency table, column, or store-type drift stays on the current drift reports instead of being pulled into this ticket's contract.
- No foreign-key graph validation, arbitrary non-DVault catalog inspection, general physical-design advisory, or raw database-data capture.
- No new standalone CLI/platform surface; any command-line or support-bundle exposure should remain a thin consumer-owned wrapper or a separate follow-up if later justified.
- No public documentation rollout inside this story; the existing blocked documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4` remains the place for README, guide, and release-note updates after implementation lands.

## Acceptance Criteria
- Consumers can evaluate an explicit idempotency preflight check against a configured `DbContext` and receive deterministic pass/block/skip results for the relevant DVault operation families without opening a live database unless they opted into that lane.
- For every hub, link, satellite, PIT, and bridge present in the authoritative metadata, the check validates expected named primary-key constraints and idempotency/access-path secondary indexes for presence, column order, uniqueness, and provider-specific effective shape.
- Missing or mismatched structures produce redacted provider-aware findings that identify the translated DVault table and explain the schema-level remediation boundary; valid structures report clean pass state.
- Requested live checks surface `UnsupportedProvider` and `Unavailable` as explicit provider-aware outcomes, while omitted live inputs remain skipped rather than auto-discovered or silently ignored.
- Tests cover valid, missing, mismatched, and provider-caveat scenarios, including at least one default local SQLite path and targeted coverage for provider-specific included-index or redundant-index behavior.

## Definition of Done
- The library exposes the additive idempotency-preflight result surface and wires it into the existing preflight flow only through explicit optional inputs or lane selection, preserving current caller behavior by default.
- Deterministic machine-readable and displayable output exists for pass, block, skip, unsupported-provider, and unavailable-live-schema cases, and the output remains within the existing redaction boundary.
- Unit and integration coverage lock the hub, link, satellite, PIT, and bridge baselines plus provider-shape caveats, and any required API snapshot or public-surface approvals are updated.
- Implementation reuses the current translated naming and provider-capability rules instead of hard-coding SQLite-only expectations or inventing a parallel index vocabulary.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized during this refinement pass, and the current epic/doc-task relation state remains unchanged.

## Implementation Notes
- Build the expected constraint/index catalog from existing translated diagnostics or metadata explain surfaces such as `DataVaultEntityExplain.PrimaryKey`, `DataVaultEntityExplain.Indexes`, `DataVaultEntityExplain.Constraints`, and the provider capability profile selected for the current `DbContext`.
- Do not make full `DataVaultLiveSchemaDriftReporter.Compare(...)` the public result of this story if that would surface unrelated column or store-type drift; filter or map only the idempotency-critical structures required by the ticket.
- Keep the new check consistent with the current read-diagnostics contract: satellite, PIT, and bridge findings should match the same translated index baselines already exposed via `ExpectedIndexBaseline` and `ExpectedTraversalIndexBaseline` rather than creating a conflicting naming or ordering rule.
- Respect existing translator caveats when judging mismatches: a provider may legitimately suppress a secondary index that is fully covered by the primary key, and providers without native included-index support may append included columns to the key or ignore them depending on the selected capability profile.
- Map results back to bounded operation families in the output so developers can tell whether a finding affects hub save idempotency, link save idempotency, satellite latest-state lookup, PIT as-of reads, or bridge traversal/maintenance.
- Keep live-schema invocation explicit and consumer-owned and redact provider exception text, connection-string fragments, SQL, and raw row data from all messages.
- The branch currently contains no feature implementation beyond the PO lease claim commit, so this refinement intentionally materialized no child tickets, relation changes, description updates, attachments, or planning documents.

## Open Questions
- none

## Follow-Up Questions
- When `06F7Y0NBHXQ6CK8R3AH4DEP9V4` documents this behavior, should it present the new check primarily as an additive `DataVaultPreflight` lane, as a companion helper, or both?
- After this story lands, should support-bundle or consumer-owned design-time command flows preserve the idempotency report as an additive section, or keep it as a separate runtime/library check until adopter demand is clearer?
- If later work wants broader live-schema guardrails beyond idempotency-critical constraints and indexes, should that remain a separate follow-up story instead of widening this contract?

## Risks
- If the comparison logic ignores provider-capability normalization, providers with redundant-index suppression or non-native included-index behavior will false-fail even when their generated schema is correct.
- If implementation reuses general drift reporting without filtering, the preflight output will become noisy and blur the ticket's idempotency-specific purpose.
- If live schema is opened implicitly on ordinary preflight runs, the implementation will violate the documented consumer-owned design-time boundary and make default local/CI execution brittle.
- If remediation output includes raw provider messages, connection details, or database values, it will break the repository's redaction contract.

## Split Recommendations
- No child-ticket split is required for PO-critic readiness; the repository already has the necessary preflight, live-schema, diagnostics, and provider-capability building blocks, so this remains one bounded additive story.
- Keep README, architecture-guide, checklist, and release-note rollout on the existing blocked documentation task `06F7Y0NBHXQ6CK8R3AH4DEP9V4` instead of broadening this story's delivery contract.
- If future work wants non-idempotency live-schema drift aggregation, automatic repository discovery, or first-class support-bundle/command integration, track those as separate follow-up tickets rather than widening this ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Extend live/schema preflight checks to validate provider-specific idempotency-critical constraints and indexes.

# Scope In
- Verify required uniqueness and access-path structures for hub, link, satellite latest-state, PIT, and bridge operations where metadata supports it.
- Explain provider caveats and fallback remediation without raw database data.

# Scope Out
No automatic schema repair, migration application, or destructive database operation.

# Acceptance Criteria
- Tests cover missing, mismatched, and valid idempotency/index structures.
- Output remains redacted and provider-aware.

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Added an explicit idempotency schema preflight/report surface plus an optional `DataVaultPreflight` lane that evaluates only caller-supplied live schema input.
- The expected baseline is built from the provider-shaped translated EF metadata and checks named primary keys plus idempotency/access-path indexes for hub, link, satellite, PIT, and bridge tables.
- Live-schema index snapshots now preserve descending key columns and native include-column metadata where provider catalogs expose them, while existing provider caveats such as include fallback and PK-covered index suppression stay provider-profile driven.

### Verification
- `dotnet build DVault.slnx --nologo`
- `dotnet test DVault.slnx --nologo`
- `bash tools/check-format.sh`
- Targeted checks also run: unit test project, API snapshot class, and `SqliteIdempotencyPreflightTests`.

### Notes
- The new preflight lane remains skipped unless `DataVaultPreflightRequest.IdempotencyLiveSchemaReadResult` is supplied; it does not open a live database implicitly.
- External PostgreSQL, SQL Server, Oracle, and MySQL live-schema integration tests remained skipped in this environment because their opt-in connection-string variables were not configured.
<!-- gicket-bot:developer-delivery:v1:end -->

<!-- gicket-bot:developer-delivery:v1:start -->
## Developer Delivery

### Summary
- Existing branch implementation remains the delivery for this story: additive `DataVaultIdempotencyPreflight` surface, explicit optional `DataVaultPreflight` idempotency schema lane, live-schema index metadata expansion, and focused source/test/API snapshot coverage.
- No repository source rework was needed in this return pass. The tester return identified a process/DoD documentation ambiguity, not a missing code artifact.
- This supplemental description block resolves the ambiguity: the delivery-contract phrase about no child tickets, relation writes, description updates, attachments, or planning documents being materialized during the refinement pass is scoped to the PO refinement activity. It does not prohibit this required developer delivery description artifact, and the PO refinement description update referenced in process evidence is not a repository implementation defect.

### Acceptance And DoD Confirmation
- The implementation validates provider-shaped idempotency-critical primary keys and indexes for hubs, links, satellites, PITs, and bridges using translated DVault metadata and provider capability rules.
- The aggregate preflight lane stays skipped unless `DataVaultPreflightRequest.IdempotencyLiveSchemaReadResult` is supplied by the caller, preserving the consumer-owned live-database boundary.
- Findings remain deterministic and redacted, identifying DVault table, operation family, structure, property path, expected/actual schema values, and remediation boundary without SQL, connection strings, raw row data, or unbounded provider exception details.
- No child tickets, relation writes, repository documentation rollout, attachments, or planning documents are requested by this developer rework. The existing documentation task remains the follow-up surface.

### Verification
- Branch inspection confirmed the committed implementation delta against `develop` includes the idempotency preflight source, preflight wiring, live-schema index metadata changes, SQLite integration coverage, unit coverage, provider discovery registration, and public API snapshot update.
- `git diff --check develop..HEAD -- ':!:.gicket' ':!:.gicket-bot'` completed without whitespace errors in this rework.
- Policy verification should be rerun by the test role: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh`. A local bounded targeted dotnet test attempt in this sandbox timed out after build startup while emitting read-only NuGet vulnerability-cache warnings, so this rework does not claim a fresh completed dotnet pass.
<!-- gicket-bot:developer-delivery:v1:end -->