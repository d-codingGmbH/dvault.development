[gicket-bot] PO-critic review contract

Summary
- The ticket is now internally consistent, bounded, and supported by current ticket/comment history and repository evidence, so it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket-comments` returned `26` persisted comments, including an older `PO-critic review contract` comment that returned the ticket to PO and a later `PO refinement contract` comment whose checklist says `critic-item-1` through `critic-item-4` are `answered` and re-hands the ticket as `ready_for_po_critic`.
- `git diff --name-only develop..a69f8647e1bdb6928a20de8260f8952fb72584d3` lists `.gicket/tickets/06FF43XM75680ZFRJJKKW2655R/description.md`, `ticket.json`, comment files, and event files; it does not list product source files, which matches a pre-development refinement branch.
- The current repository snapshot in the prompt aligns with the contract boundary: `src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs` rejects repeated same-hub links without an explicit relationship name or with duplicate produced participant names, `tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs` covers role-bearing same-hub accept/reject cases, `src/DCoding.Data.DVault.Analyzers/DataVaultMappingSourceGenerator.cs` rejects duplicate produced participant names for generated link mappings, and `docs/model-first-governance.md` plus `docs/production-adoption-checklist.md` keep model-first same-hub typed mapper generation, dependent child key modeling, and effectivity-specific expansion out of scope.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A short invalid example that reuses `Customer` for both same-hub endpoints would make the parent ticket faster to review without opening source files.
- A short example that maps declaration order to `SourceCustomerHashKey` then `MatchedCustomerHashKey` would make the deterministic-order guarantee easier to verify from the ticket alone.

Risky assumptions
- I am treating this parent story as still eligible for the normal dev handoff path even though the branch diff is ticket-metadata-only and the comment history frames it as an aggregate parent over already-completed child slices, because the ticket is not explicitly marked closure-only or no-work-required.

AC / test suggestions
- Keep negative coverage for repeated same-hub links without an explicit relationship name.
- Keep negative coverage for duplicate produced participant names in generated same-hub mappings.
- Keep at least one end-to-end proof that role-bearing participant names flow through the existing `IDataVaultLinkMapper<TSource>` plus `IDataVaultSaveService` explicit-save boundary in declaration order.

Implementation watchouts
- Do not widen this story into raw `dvault.model.v1` direct helper generation, provider-specific SQL, SaveChanges-driven writes, dependent-child key modeling, or effectivity-specific APIs; the current contract keeps those outside scope.
- Preserve produced participant names exactly and in declaration order; the same-hub boundary is about distinct produced participant names, not distinct hub types.
- Treat awkward public names such as `ParticipantHubName` and `ParticipantHubNames` as compatibility debt for a later additive ticket, not as scope for this bounded story.

Non-blocking notes
- `gicket-read-ticket` still shows `blocked/dev` and `blocked/test` alongside `critic-needed`; I am not treating that as a PO blocker because the later PO refinement comment supersedes the earlier blocking rationale.
- The prompt snapshot says `Recent comments: <none>`, but `gicket-read-ticket-comments` returned `26` persisted comments; the tool-read state is the authoritative one.
- The branch being ticket-metadata-only is a developer-handoff watchout rather than a PO-quality defect under the stated pre-development review rules.

Split recommendations
- No additional split recommended; the parent contract is bounded and the existing child-slice breakdown already covers the implementation and documentation decomposition.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment