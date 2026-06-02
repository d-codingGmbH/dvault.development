<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story around the existing `guardrail --migration` preflight: strengthen blocking and suspicious diagnostics for destructive DVault-generated structure changes using metadata and produced-name evidence; no bounded planning writes were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository docs already establish `guardrail --migration <name>` as the existing consumer-owned preflight entrypoint, so this story strengthens that analyzer rather than adding a new CLI, `dotnet ef` interception, or deployment-time enforcement.
- DVault-generated ownership should be identified from provider-neutral metadata such as produced names, metadata names, entity kinds, property roles, and related generated-structure annotations instead of raw SQL or arbitrary object names.
- PIT and bridge remain first-class generated structures in the current repository context and stay in scope alongside hub, link, and satellite guardrails.
- Live ticket evidence shows no human comments and no attachments changing scope; live relations show this ticket as a child of `06F7Y0J8PRFRSSWZ3GGT91S0TW`, blocked by `06F7Y0HZKHBHMYX9EYDYFRYXZ0`, and blocking `06F7Y0KVHGTTVS216ERSG4XNMM`.
- No child tickets, relation edits, description updates, attachments, or planning documents were materialized in this refinement run.

### Scope In
- Strengthen the existing migration guardrail preflight that analyzes scaffolded EF migration operations before apply time.
- Detect destructive or suspicious changes to DVault-owned generated hub, link, satellite, PIT, and bridge tables plus their generated columns, secondary indexes, and named generated constraints.
- Differentiate explicit intentional evolution operations, such as true EF rename flows, from suspicious drop-and-add patterns that imply metadata or naming drift.
- Keep diagnostics provider-neutral and expressed in DVault vocabulary with actionable remediation guidance.

### Scope Out
- No automatic migration rewrite, schema repair, or deployment.
- No new CLI or tooling surface beyond the existing consumer-owned `guardrail` preflight command path.
- No guardrail coverage for arbitrary non-DVault or consumer-authored database objects.
- No live-schema drift reader expansion or provider physical-plan analysis in this story.

## Acceptance Criteria
- On the existing `guardrail --migration` path, dropping a DVault-generated table, column, index, or named generated constraint for a hub, link, satellite, PIT, or bridge surfaces a blocking destructive-change diagnostic.
- Explicit intentional rename or evolution operations for DVault-generated structures are recognized as intentional and do not raise the destructive-drift diagnostic, while suspicious drop-plus-add replacements still raise actionable diagnostics.
- Pure additive generated changes remain allowed and do not emit blocking diagnostics.
- Each diagnostic identifies the affected DVault structure kind and generated object, references the logical metadata or produced-name context, and tells the caller how to remediate or intentionally re-author the migration.
- Automated tests cover destructive drop, suspicious rename or drift, explicit rename or evolution, and safe additive cases across the supported generated structure kinds.

## Definition of Done
- The existing migration guardrail implementation is updated to classify DVault-owned destructive and suspicious generated-structure changes at migration-operation level.
- Automated coverage in the existing DVault test roots proves the agreed destructive, suspicious, explicit-rename, and additive cases and guards against regressions.
- Any public documentation or release-note surface affected by the stronger guardrail behavior is updated to describe the new expectations and remediation path.

## Implementation Notes
- Anchor detection on repository-established DVault metadata signals, especially produced-name and metadata-name annotations plus generated structure kind and role metadata, rather than provider-specific SQL text.
- Treat explicit EF rename or evolution operations as the intentional path; treat drop-and-add sequences that replace one generated object with another on the same logical DVault shape as suspicious drift unless the migration preserves clear intent.
- Keep classification provider-neutral at the migration-operation layer so provider packages can share the same guardrail semantics even when scaffolding details vary.
- Use diagnostics that speak in DVault concepts such as hub, link, satellite, PIT, bridge, parent reference, produced name, and metadata name.
- Preserve the existing separation between migration guardrails and live-schema drift analysis; this story strengthens pre-apply migration analysis only.

## Open Questions
- none

## Follow-Up Questions
- After this story lands, decide whether a later ticket should extend similar guardrails to consumer-authored or provider-specific objects that are outside current DVault-generated ownership.
- Reconfirm downstream sequencing with related tickets `06F7Y0HZKHBHMYX9EYDYFRYXZ0` and `06F7Y0KVHGTTVS216ERSG4XNMM` once their scopes settle, but do not reopen this refinement unless they change the `guardrail` preflight boundary.

## Risks
- Complex provider-specific scaffolding can decompose one logical rename into multiple migration operations, so some legitimate changes may still be classified as suspicious unless the migration preserves enough continuity evidence.
- Broader structure coverage across tables, columns, indexes, and named constraints increases the test matrix needed to prove provider-neutral behavior.
- The live dependency chain around the current `blocks` relations remains a delivery-sequencing risk even though refinement itself is ready.

## Split Recommendations
- No split recommended: strengthening destructive-change classification, diagnostics, and tests on the existing `guardrail --migration` surface is one cohesive story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Improve EF migration guardrails for generated DVault structures.

# Scope In
- Detect destructive or suspicious changes to generated hub, link, satellite, PIT, bridge, index, and constraint structures.
- Emit actionable diagnostics that distinguish intentional evolution from accidental metadata/naming drift.

# Scope Out
No automatic migration rewrite, schema repair, or deployment.

# Acceptance Criteria
- Tests cover generated table/column/index/constraint drop/rename cases and safe additive cases.
- Diagnostics reference DVault metadata concepts and remediation steps.