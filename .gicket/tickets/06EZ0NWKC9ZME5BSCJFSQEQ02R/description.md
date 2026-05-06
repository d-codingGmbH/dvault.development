<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Revalidated the closure-umbrella contract against current repository, relation, branch-diff, and bounded comment evidence; corrected the comment-history wording after the latest re-check and kept the parent story as ratification-only with no new materialized planning artifacts.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This parent story remains a ratification/closure umbrella over done child tickets 06EZ0NWTM3EPBJS0SWVHXGDGTM, 06EZ0NX282R80VF5VBKS6ARFZC, and 06EZ0NX9SVP7MSB1R4PJ50EHGW; no new product-code work is expected on the parent branch.
- Authoritative delivery proof for the umbrella remains the three child tickets plus current source, tests, and public API snapshots that evidence AddDVault(Action<DataVaultOptions>), request-level timestamp and record-source resolution, provider-behavior selection, and hook-failure validation.
- docs/plans/optional-advanced-configuration-hooks.md and docs/plans/deferred-data-vault-capabilities.md remain architecture/background references only; they are not the source of truth for current implemented API names on this closure umbrella.
- The parent contract records comment history qualitatively as bot-authored workflow/refinement/runtime records with no human scope-conflict comments observed in the current bounded re-check; it does not rely on a live exact count.
- The legacy description placeholder 'Recent comments: <none>' is stale and should not be treated as authoritative comment evidence.
- Existing outgoing parentOf relations to the three child tickets and outgoing blocks relations to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG remain unchanged in this pass; no relation writes were materialized.

### Scope In
- Ratify the already-completed advanced-hook delivery across the three child tickets: request-level load timestamp resolution, request-level record-source resolution, provider-behavior selection, and validation/failure-mode documentation.
- Document the authoritative evidence boundary for this umbrella story: done child tickets plus current source, tests, and public API snapshot coverage.
- Confirm that the zero-configuration AddDVault() path and explicit IDataVaultSaveService/DataVaultSaveRequest boundary remain the default when hooks are unset.
- Clarify the parent as closure/ratification work only, with branch evidence limited to ticket metadata updates.

### Scope Out
- New product-code changes on the parent branch.
- Reopening naming-hook or hashing-hook implementation beyond the current defaults already evidenced elsewhere.
- Implementing PIT, bridge, or multi-active capabilities themselves.
- Treating architecture planning docs as the authoritative source for current implemented API names.
- Provider-specific option matrices, migrations, or SaveChanges interception changes.

## Acceptance Criteria
- The existing child tickets 06EZ0NWTM3EPBJS0SWVHXGDGTM, 06EZ0NX282R80VF5VBKS6ARFZC, and 06EZ0NX9SVP7MSB1R4PJ50EHGW together cover timestamp/record-source hooks, provider behavior hooks, and validation/failure-mode documentation for this story.
- Current source and test evidence, rather than architecture planning prose, is the authoritative proof that the branch exposes AddDVault(Action<DataVaultOptions>), request-level load timestamp and record-source resolution, provider-behavior selection, public API snapshot coverage, and failure-mode tests.
- With hooks unset, the zero-configuration AddDVault() path and explicit IDataVaultSaveService/DataVaultSaveRequest boundary remain the ratified default behavior across the delivered hook surface.
- shell-command git diff --name-only develop...HEAD shows only .gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R metadata changes on this branch, so parent completion is ratification/closure of existing delivered child work rather than new product-code delivery.

## Definition of Done
- The parent contract is internally consistent about source of truth: the three child tickets plus current source, tests, and public API snapshots are the delivery proof, while docs/plans/optional-advanced-configuration-hooks.md and docs/plans/deferred-data-vault-capabilities.md are architecture/background references only.
- The contract records comment history qualitatively as bot-authored workflow/refinement/runtime records with no human scope-conflict comments observed, rather than as a live exact count.
- No new child tickets, relation mutations, attachments, or planning documents are required for this refinement pass.
- No remaining blocking PO questions or contract-level source-of-truth conflicts remain after this update.

## Implementation Notes
- Use the child tickets as the delivery breakdown: 06EZ0NWTM3EPBJS0SWVHXGDGTM for timestamp and record-source hooks, 06EZ0NX282R80VF5VBKS6ARFZC for provider behavior hooks, and 06EZ0NX9SVP7MSB1R4PJ50EHGW for validation and failure-mode documentation.
- Use current repository evidence as delivery proof, especially src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs, src/DCoding.Data.DVault/DataVaultOptions.cs, src/DCoding.Data.DVault/DataVaultSaveService.cs, src/DCoding.Data.DVault/DefaultDataVaultProviderBehaviorSelector.cs, tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderBehaviorTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.
- Keep docs/plans/optional-advanced-configuration-hooks.md and docs/plans/deferred-data-vault-capabilities.md as architecture/background references only because both documents are architecture-level and do not finalize public API names.
- shell-command git diff --name-only develop...HEAD returns only .gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R changes on this branch, so any remaining parent work is ticket-contract closure rather than source delivery.
- gicket-read-ticket-relations in this pass shows outgoing parentOf relations to the three child tickets, outgoing blocks relations to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG, plus incoming parentOf from 06EZ0NS59T2SW9976HHSGP2GF0 and incoming blocks from 06EZ0NSBM3GD7DY11Y4PZMXD28; no relation cleanup was materialized in this pass.

## Open Questions
- none

## Follow-Up Questions
- When this umbrella story is formally closed, should the live outgoing blocks relations from 06EZ0NWKC9ZME5BSCJFSQEQ02R to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG be removed in the same relation-cleanup pass?
- If future deferred-capability work truly needs naming or hashing customization beyond current defaults, should that arrive through new dedicated tickets rather than reopening this closure umbrella?

## Risks
- If eventual closure does not clean up live outgoing blocks relations, downstream tracking can continue to show stale dependency edges even though the hook work is already delivered through done child tickets.
- Reviewers can still misread docs/plans/optional-advanced-configuration-hooks.md as current API truth unless the parent contract keeps the architecture/background limitation explicit.
- Any future attempt to record an exact persisted-comment count in the parent contract will drift again because automation continues appending claim, lease, orchestration, and run-report comments.

## Split Recommendations
- Existing split already materialized and remains sufficient: 06EZ0NWTM3EPBJS0SWVHXGDGTM for timestamp and record-source hooks, 06EZ0NX282R80VF5VBKS6ARFZC for provider behavior hooks, and 06EZ0NX9SVP7MSB1R4PJ50EHGW for validation and failure-mode documentation.
- No further split is recommended for this parent umbrella unless future naming or hashing customization becomes new implementation scope.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: expose advanced hooks needed by deferred capabilities while preserving deterministic defaults.

Scope:
- Add or refine hooks for timestamp, record source, provider behavior, and validation policies used by PIT, bridge, and multi-active scenarios.
- Keep default behavior unchanged for existing v0.4 usage.
- Document hook failure modes and validation behavior.

Acceptance Criteria:
- Hooks are opt-in and covered by tests.
- Default behavior remains deterministic and compatible with existing examples.
- Hook contracts do not couple core code to one database provider.