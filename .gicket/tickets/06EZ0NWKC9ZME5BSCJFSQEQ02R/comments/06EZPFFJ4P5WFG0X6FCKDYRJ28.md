[gicket-bot] PO refinement contract

Summary
- Revalidated the closure-umbrella contract against current repository, relation, branch-diff, and bounded comment evidence; corrected the comment-history wording after the latest re-check and kept the parent story as ratification-only with no new materialized planning artifacts.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now uses qualitative comment-history wording instead of a brittle live count: it records persisted comments as bot-authored workflow/refinement/runtime records with no human scope-conflict comments observed.
- critic-item-2: `answered` - The current ticket comments were re-checked before handoff. The bounded comment read now reports 34 persisted comments, returning 20 of them, all with bot-authored '[gicket-bot]' headers, so the parent contract now records comment history qualitatively and does not claim a fixed count.

Clarifications
- This parent story remains a ratification/closure umbrella over done child tickets 06EZ0NWTM3EPBJS0SWVHXGDGTM, 06EZ0NX282R80VF5VBKS6ARFZC, and 06EZ0NX9SVP7MSB1R4PJ50EHGW; no new product-code work is expected on the parent branch.
- Authoritative delivery proof for the umbrella remains the three child tickets plus current source, tests, and public API snapshots that evidence AddDVault(Action<DataVaultOptions>), request-level timestamp and record-source resolution, provider-behavior selection, and hook-failure validation.
- docs/plans/optional-advanced-configuration-hooks.md and docs/plans/deferred-data-vault-capabilities.md remain architecture/background references only; they are not the source of truth for current implemented API names on this closure umbrella.
- The parent contract records comment history qualitatively as bot-authored workflow/refinement/runtime records with no human scope-conflict comments observed in the current bounded re-check; it does not rely on a live exact count.
- The legacy description placeholder 'Recent comments: <none>' is stale and should not be treated as authoritative comment evidence.
- Existing outgoing parentOf relations to the three child tickets and outgoing blocks relations to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG remain unchanged in this pass; no relation writes were materialized.

Scope In
- Ratify the already-completed advanced-hook delivery across the three child tickets: request-level load timestamp resolution, request-level record-source resolution, provider-behavior selection, and validation/failure-mode documentation.
- Document the authoritative evidence boundary for this umbrella story: done child tickets plus current source, tests, and public API snapshot coverage.
- Confirm that the zero-configuration AddDVault() path and explicit IDataVaultSaveService/DataVaultSaveRequest boundary remain the default when hooks are unset.
- Clarify the parent as closure/ratification work only, with branch evidence limited to ticket metadata updates.

Scope Out
- New product-code changes on the parent branch.
- Reopening naming-hook or hashing-hook implementation beyond the current defaults already evidenced elsewhere.
- Implementing PIT, bridge, or multi-active capabilities themselves.
- Treating architecture planning docs as the authoritative source for current implemented API names.
- Provider-specific option matrices, migrations, or SaveChanges interception changes.

Open questions
- none

Follow-up questions
- When this umbrella story is formally closed, should the live outgoing blocks relations from 06EZ0NWKC9ZME5BSCJFSQEQ02R to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG be removed in the same relation-cleanup pass?
- If future deferred-capability work truly needs naming or hashing customization beyond current defaults, should that arrive through new dedicated tickets rather than reopening this closure umbrella?

Risks
- If eventual closure does not clean up live outgoing blocks relations, downstream tracking can continue to show stale dependency edges even though the hook work is already delivered through done child tickets.
- Reviewers can still misread docs/plans/optional-advanced-configuration-hooks.md as current API truth unless the parent contract keeps the architecture/background limitation explicit.
- Any future attempt to record an exact persisted-comment count in the parent contract will drift again because automation continues appending claim, lease, orchestration, and run-report comments.

Split recommendations
- Existing split already materialized and remains sufficient: 06EZ0NWTM3EPBJS0SWVHXGDGTM for timestamp and record-source hooks, 06EZ0NX282R80VF5VBKS6ARFZC for provider behavior hooks, and 06EZ0NX9SVP7MSB1R4PJ50EHGW for validation and failure-mode documentation.
- No further split is recommended for this parent umbrella unless future naming or hashing customization becomes new implementation scope.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment