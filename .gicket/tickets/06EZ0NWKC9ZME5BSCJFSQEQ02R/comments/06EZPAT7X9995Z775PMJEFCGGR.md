[gicket-bot] PO refinement contract

Summary
- Reframed ticket 06EZ0NWKC9ZME5BSCJFSQEQ02R as a ratification/closure umbrella over done child tickets 06EZ0NWTM3EPBJS0SWVHXGDGTM, 06EZ0NX282R80VF5VBKS6ARFZC, and 06EZ0NX9SVP7MSB1R4PJ50EHGW; authoritative proof is now the child tickets plus current source/tests, planning docs are architecture/background only, and the comment-history clarification was corrected. No new child tickets, relations, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Revised the parent contract so authoritative delivery proof is the three done child tickets plus current source/tests and public API snapshot evidence. docs/plans/optional-advanced-configuration-hooks.md and docs/plans/deferred-data-vault-capabilities.md remain architecture/background guardrails, not the source of truth for current implemented API names on this umbrella story.
- critic-item-2: `answered` - Corrected the comment-history clarification. Persisted history includes 18 bot-authored workflow/refinement comments, including prior PO refinement, PO-critic return, runtime orchestration/writeback, relation automation, claim, and lease entries; there are no human-authored comments adding conflicting scope.
- critic-item-3: `answered` - Clarified the parent as a ratification/closure umbrella over already-completed child work. The parent branch does not deliver product-code or plan-document changes for this story; git diff --name-only develop...HEAD shows only .gicket/tickets/06EZ0NWKC9ZME5BSCJFSQEQ02R/** metadata changes.
- critic-item-4: `answered` - Resolved the source-of-truth conflict by demoting docs/plans/optional-advanced-configuration-hooks.md to architecture/background for this umbrella and relying on current source/tests and public API snapshot evidence for the implemented timestamp, record-source, and provider-behavior surface.
- critic-item-5: `answered` - Removed the false statement that only claim/lease comments exist. The contract now accurately states that the persisted comments are bot-authored workflow/refinement records and that none of them introduce human scope conflicts.

Clarifications
- This parent story is a ratification/closure umbrella over done child tickets 06EZ0NWTM3EPBJS0SWVHXGDGTM, 06EZ0NX282R80VF5VBKS6ARFZC, and 06EZ0NX9SVP7MSB1R4PJ50EHGW; no new product-code work is expected on the parent branch.
- Authoritative delivery proof for the umbrella is the three done child tickets plus current source/tests/public API snapshots that back AddDVault(Action<DataVaultOptions>), request-level timestamp and record-source resolution, provider-behavior selection, and hook-failure validation.
- docs/plans/optional-advanced-configuration-hooks.md and docs/plans/deferred-data-vault-capabilities.md remain architecture/background guardrails about hook categories, defaults, and deferred-capability scope; they are not the source of truth for current implemented API names on this parent story.
- The legacy description placeholder 'Recent comments: <none>' is stale and should not be treated as authoritative comment evidence.
- Existing parentOf relations to the three child tickets and live blocks relations to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG were left unchanged in this pass so the contract matches live relation state.

Scope In
- Ratify the already-completed advanced-hook delivery across the three child tickets: request-level load timestamp resolution, request-level record-source resolution, provider-behavior selection, and validation/failure-mode documentation.
- Document the authoritative evidence boundary for this umbrella story: done child tickets plus current source/tests/public API snapshot coverage.
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
- When this umbrella story is formally closed, should the live blocks relations from 06EZ0NWKC9ZME5BSCJFSQEQ02R to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG be removed in the same relation-cleanup pass?
- If future deferred-capability work truly needs naming or hashing customization beyond current defaults, should that arrive through new dedicated tickets rather than reopening this closure umbrella?

Risks
- If the eventual closure does not clean up live blocks relations, downstream tracking can continue to show stale dependency edges even though the hook work is already delivered through done child tickets.
- Reviewers can still misread docs/plans/optional-advanced-configuration-hooks.md as current API truth unless the parent contract keeps the architecture/background limitation explicit.
- Future hook API changes can drift again if child/source/test evidence and planning docs are not kept synchronized.

Split recommendations
- Existing split already materialized and remains sufficient: 06EZ0NWTM3EPBJS0SWVHXGDGTM for timestamp/record-source hooks, 06EZ0NX282R80VF5VBKS6ARFZC for provider behavior hooks, and 06EZ0NX9SVP7MSB1R4PJ50EHGW for validation/failure-mode documentation.
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