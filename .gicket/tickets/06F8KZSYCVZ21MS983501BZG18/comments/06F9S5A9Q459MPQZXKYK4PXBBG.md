[gicket-bot] PO refinement contract

Summary
- Repository evidence already lands the v0.31 decision-tree and example inputs, so this ticket can stay bounded to adding docs/releases/v0.31.0.md plus minimal README/checklist baseline-link updates; no ticket writes or planning artifacts were materialized in this refinement.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- docs/releases/v0.31.0.md is currently missing, while docs/performance-profiles.md already carries the authoritative v0.31 decision-tree contract and examples/README.md already carries the realistic customer-profile scenario, observability examples, and sanitized diagnostics guidance.
- README.md still names v0.30.0 as the current coordinated release baseline and docs/production-adoption-checklist.md still names v0.29.0 as the current public baseline, so this ticket should align those current-baseline pointers when it adds the v0.31.0 release note.
- docs/README.md does not exist on this branch, so it is not a required edit surface for this ticket.
- Completed child work already supplies the needed source material: 06F8KZRSTHAGSP6GPGFBFQGY08 is done for the decision-tree documentation, 06F8KZSCGZBKAC4YZH5SY3NX68 is done for observability examples, and 06F8KZSNDXXEEHF53HN14QFK14 is done for the realistic EF Core quickstart scenario.
- Incoming blocks relations from the done observability and example tickets are historical dependency evidence rather than open blockers, and parent epic 06F8KZQNH8CCMTJW9P95W1N388 already expects this final release-documentation pass.
- The live relation from this ticket to 06F8KZTNG44XDPMVTVCV4WJSHG keeps the provider-specific SQL artifact contract in future v0.32 scope, so v0.31 should mention that lane only as a forward non-goal boundary.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized during this refinement.

Scope In
- Add docs/releases/v0.31.0.md as the coordinated release note for the already-landed performance guidance, observability guidance, and realistic quickstart evidence.
- Summarize the authoritative decision-tree contract, the application-owned observability posture, and the customer-profile quickstart outputs with links back to the existing docs and example surfaces instead of re-documenting them in full.
- Update only the navigation or baseline surfaces that are actually needed outside .gicket, such as README.md and docs/production-adoption-checklist.md, so they point at the v0.31.0 release note and no longer leave older releases labeled as current.
- Keep the release wording consistent about telemetry-free AddDVault defaults, application-owned metrics/tracing/exporters, no automatic PIT or bridge maintenance, no ingestion orchestration, and no provider-specific SQL artifact workflow in v0.31.

Scope Out
- Rewriting docs/performance-profiles.md or examples/README.md from scratch; those source documents are already the landed inputs this release note should summarize.
- New runtime behavior, benchmark reruns, new provider-specific artifacts, new sample families, or broad README restructuring.
- Specifying the v0.32 design-time artifact workflow beyond a short forward-boundary statement.
- Inventing edits to docs/README.md, which does not exist on the current branch.

Open questions
- none

Follow-up questions
- If a later documentation cleanup wants a central docs index, track that separately instead of expanding this ticket because docs/README.md is absent on the current branch.
- When the v0.32 artifact-lane release note exists, decide in a separate maintenance pass whether the v0.31 note should gain a forward cross-link without expanding its scope now.

Risks
- The release note can over-specify future provider-specific SQL artifact workflow unless it keeps v0.32 as a short non-goal boundary only.
- Partial navigation updates can leave conflicting current-baseline claims if README.md, examples/README.md, and docs/production-adoption-checklist.md are not aligned together when touched.
- Copying too much from the example or observability docs can accidentally over-promise raw SQL visibility, hosted observability, automatic maintenance, or runtime routing that the existing contracts explicitly exclude.

Split recommendations
- No immediate split is needed; the remaining work is one release note plus small baseline-link adjustments.
- If someone wants a repo-wide version-sweep or a dedicated v0.32 artifact-lane explainer, create separate follow-up tickets rather than enlarging this v0.31 release-doc task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment