<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Repository evidence already lands the v0.31 decision-tree and example inputs, so this ticket can stay bounded to adding docs/releases/v0.31.0.md plus minimal README/checklist baseline-link updates; no ticket writes or planning artifacts were materialized in this refinement.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- docs/releases/v0.31.0.md is currently missing, while docs/performance-profiles.md already carries the authoritative v0.31 decision-tree contract and examples/README.md already carries the realistic customer-profile scenario, observability examples, and sanitized diagnostics guidance.
- README.md still names v0.30.0 as the current coordinated release baseline and docs/production-adoption-checklist.md still names v0.29.0 as the current public baseline, so this ticket should align those current-baseline pointers when it adds the v0.31.0 release note.
- docs/README.md does not exist on this branch, so it is not a required edit surface for this ticket.
- Completed child work already supplies the needed source material: 06F8KZRSTHAGSP6GPGFBFQGY08 is done for the decision-tree documentation, 06F8KZSCGZBKAC4YZH5SY3NX68 is done for observability examples, and 06F8KZSNDXXEEHF53HN14QFK14 is done for the realistic EF Core quickstart scenario.
- Incoming blocks relations from the done observability and example tickets are historical dependency evidence rather than open blockers, and parent epic 06F8KZQNH8CCMTJW9P95W1N388 already expects this final release-documentation pass.
- The live relation from this ticket to 06F8KZTNG44XDPMVTVCV4WJSHG keeps the provider-specific SQL artifact contract in future v0.32 scope, so v0.31 should mention that lane only as a forward non-goal boundary.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement.

### Scope In
- Add docs/releases/v0.31.0.md as the coordinated release note for the already-landed performance guidance, observability guidance, and realistic quickstart evidence.
- Summarize the authoritative decision-tree contract, the application-owned observability posture, and the customer-profile quickstart outputs with links back to the existing docs and example surfaces instead of re-documenting them in full.
- Update only the navigation or baseline surfaces that are actually needed outside .gicket, such as README.md and docs/production-adoption-checklist.md, so they point at the v0.31.0 release note and no longer leave older releases labeled as current.
- Keep the release wording consistent about telemetry-free AddDVault defaults, application-owned metrics/tracing/exporters, no automatic PIT or bridge maintenance, no ingestion orchestration, and no provider-specific SQL artifact workflow in v0.31.

### Scope Out
- Rewriting docs/performance-profiles.md or examples/README.md from scratch; those source documents are already the landed inputs this release note should summarize.
- New runtime behavior, benchmark reruns, new provider-specific artifacts, new sample families, or broad README restructuring.
- Specifying the v0.32 design-time artifact workflow beyond a short forward-boundary statement.
- Inventing edits to docs/README.md, which does not exist on the current branch.

## Acceptance Criteria
- docs/releases/v0.31.0.md exists and follows the coordinated release-note pattern with package scope, a boundary shift from v0.30.0, evidence anchors, validation evidence, and explicit non-goals.
- The release note links to the landed v0.31 sources it summarizes, including docs/performance-profiles.md, examples/README.md, the root benchmark-summary.md / benchmark-summary.csv / benchmark-summary.json triplet, and the existing observability contract surfaces rather than duplicating their full detail.
- The release note summarizes the realistic quickstart evidence at release-note level: fixed CRM import/change timestamps, explicit load-timestamp and record-source saves, typed latest/as-of customer-profile reads, and bounded save/read diagnostics, without copying full console output or unsanitized values.
- Any touched current-baseline or version-example text in README.md and examples/README.md is internally aligned to v0.31.0, and docs/production-adoption-checklist.md no longer leaves v0.29.0 labeled as the current public baseline.
- The final wording explicitly keeps observability application-owned and excludes dashboards, exporters, collectors, hosting, automatic PIT or bridge maintenance, ingestion orchestration, provider-specific SQL artifact workflow, benchmark reruns, and package-publication claims.
- The v0.32 artifact-lane work is mentioned only as a future boundary and is not specified or implemented inside the v0.31 release note.

## Definition of Done
- docs/releases/v0.31.0.md and any touched navigation docs are updated outside .gicket and remain documentation-only changes.
- README.md, docs/production-adoption-checklist.md, and any touched example doc no longer disagree about which release is the current public documentation baseline.
- The new release note's evidence anchors and non-goals stay consistent with the already-landed guidance and example sources in the repository.
- No code/runtime changes, benchmark artifacts, ticket relations, child tickets, attachments, or planning documents are introduced by this ticket.

## Implementation Notes
- Use docs/releases/v0.30.0.md as the structural template, but make the boundary shift explicitly about v0.31.0 documentation for performance guidance, observability examples, and realistic EF Core quickstart evidence.
- Treat docs/performance-profiles.md as the authoritative performance contract, README.md plus docs/architecture/dvault-v1-activity-tracing-contract.md as the authoritative observability contract, and examples/README.md plus examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs as the authoritative example/output source.
- Summarize and cross-link the landed docs rather than pasting long decision trees, tracing rules, or full quickstart console output into the release note.
- Because README.md still points at v0.30.0 and docs/production-adoption-checklist.md still points at v0.29.0, baseline-link updates should move together so the repository does not keep split current-release claims after v0.31 lands.
- If installation version snippets are touched as part of the baseline update, advance README.md and examples/README.md together instead of leaving 0.30.0 examples beside a v0.31 current-baseline claim.
- No persistent ticket/planning writes were materialized during this refinement.

## Open Questions
- none

## Follow-Up Questions
- If a later documentation cleanup wants a central docs index, track that separately instead of expanding this ticket because docs/README.md is absent on the current branch.
- When the v0.32 artifact-lane release note exists, decide in a separate maintenance pass whether the v0.31 note should gain a forward cross-link without expanding its scope now.

## Risks
- The release note can over-specify future provider-specific SQL artifact workflow unless it keeps v0.32 as a short non-goal boundary only.
- Partial navigation updates can leave conflicting current-baseline claims if README.md, examples/README.md, and docs/production-adoption-checklist.md are not aligned together when touched.
- Copying too much from the example or observability docs can accidentally over-promise raw SQL visibility, hosted observability, automatic maintenance, or runtime routing that the existing contracts explicitly exclude.

## Split Recommendations
- No immediate split is needed; the remaining work is one release note plus small baseline-link adjustments.
- If someone wants a repo-wide version-sweep or a dedicated v0.32 artifact-lane explainer, create separate follow-up tickets rather than enlarging this v0.31 release-doc task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Finalize v0.31.0 release documentation after the decision guidance and examples have landed.

Required repository output
- Add `docs/releases/v0.31.0.md` with the final release summary, evidence links, and non-goals.
- Update top-level navigation only where needed, such as `README.md`, `docs/README.md`, `docs/performance-profiles.md`, `docs/production-adoption-checklist.md`, or `examples/README.md`, to point at the already-landed v0.31.0 guidance and examples.
- This ticket must produce release-documentation changes outside `.gicket`.

Scope in
- Summarize the final decision-tree contract, practical guidance, observability examples, and realistic EF Core example outputs from completed v0.31.0 tickets.
- Ensure release wording is consistent about application-owned observability, no dashboards/exporters/hosting, no automatic PIT/bridge maintenance, no ingestion orchestration, and no provider-specific SQL artifact workflow in v0.31.0.
- Keep the existing cross-release dependency to the v0.32 artifact-lane contract as a forward boundary: v0.31.0 may mention what is out of scope, but it must not implement or specify the v0.32 artifact workflow.

Scope out
- Rewriting the core decision-tree or examples from scratch; this is the final alignment and release-note pass.
- New runtime behavior, benchmark reruns, new provider-specific artifacts, or broad README restructuring.