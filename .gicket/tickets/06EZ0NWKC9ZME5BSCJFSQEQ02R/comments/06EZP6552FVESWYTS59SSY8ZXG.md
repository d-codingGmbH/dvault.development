[gicket-bot] PO refinement contract

Summary
- Refined the advanced-hooks story as an umbrella contract over three already-materialized child tickets: 06EZ0NWTM3EPBJS0SWVHXGDGTM for timestamp/record-source hooks, 06EZ0NX282R80VF5VBKS6ARFZC for provider behavior hooks, and 06EZ0NX9SVP7MSB1R4PJ50EHGW for validation/failure-mode docs. No new child tickets, relation updates, or planning documents were created; existing parentOf and blocks relations were left unchanged.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current branch evidence already exposes the concrete advanced configuration surface through AddDVault(Action<DataVaultOptions>), DataVaultOptions.UseLoadTimestampResolver(...), UseRecordSourceResolver(...), and UseProviderBehavior(...); this parent story should ratify that surface rather than reopen API naming.
- The live split is already materialized and done through child tickets 06EZ0NWTM3EPBJS0SWVHXGDGTM, 06EZ0NX282R80VF5VBKS6ARFZC, and 06EZ0NX9SVP7MSB1R4PJ50EHGW, each linked from this story by existing parentOf relations.
- Only persisted comments on this ticket are bot claim and lease entries; there are no human comments adding scope or conflicting decisions.
- Validation here is cross-cutting hook output and selection validation, not a sixth advanced-hook category; the governing plan still treats the categories as naming, hashing, record source, timestamp, and provider behavior.
- The zero-configuration AddDVault() path and explicit IDataVaultSaveService/DataVaultSaveRequest write boundary remain the default path; hooks are opt-in and must fail clearly on invalid output instead of silently falling back.
- The existing blocks relations from this story to PIT 06EZ0NSXY2Y1JZ8SSCX177C770, bridge 06EZ0NTV4SVAKV98C418T8A3CC, and multi-active 06EZ0NVN71BN0QWJDCWGVZ2PYG were left unchanged in this refinement pass so the contract matches live relation state.

Scope In
- Ratify the bounded advanced-hook surface that deferred capabilities may depend on: request-level load timestamp resolution, request-level record-source resolution, provider-behavior selection, and documented validation/failure behavior.
- Preserve deterministic default inheritance when hooks are unset, including the current zero-configuration startup path and existing example behavior.
- Keep hook behavior provider-neutral in core so deferred capability code can consume the hook contracts without hard-coding one database provider.
- Treat the three existing child tickets as the authoritative implementation split for this story.

Scope Out
- Implementing PIT tables, bridge tables, or multi-active satellites themselves.
- Introducing naming-hook or hashing-hook implementation work beyond the current default naming policy and stable hash services already on the branch.
- Adding a separate runtime validation-policy framework, provider-specific option matrices, migrations, or adapter-specific rollout commitments.
- Changing the explicit save boundary into SaveChanges interception or making an options object mandatory for ordinary AddDVault() callers.

Open questions
- none

Follow-up questions
- If future deferred-capability work truly needs naming or hashing customization beyond the current defaults, should that arrive as separate tickets instead of reopening this umbrella story?
- When the runtime closes or re-scopes this umbrella story, should the existing blocks relations to 06EZ0NSXY2Y1JZ8SSCX177C770, 06EZ0NTV4SVAKV98C418T8A3CC, and 06EZ0NVN71BN0QWJDCWGVZ2PYG be removed in the same change set?
- Should later provider-specific option matrices or deterministic wall-clock versus test-time timestamp modes be split under new follow-up tickets rather than added to this parent story?

Risks
- The parent ticket still carries broad legacy prose; if reviewers ignore the three done child tickets and the governing planning docs, they can reopen already-bounded scope or reintroduce a non-existent sixth validation-policy hook.
- Deferred-capability work can regress portability if it bypasses the existing request-level resolver pipeline or provider-behavior selector and hard-codes provider-specific behavior in core code.
- The current live blocks relations may become stale once this umbrella story is formally closed unless relation cleanup is handled with the eventual closure or re-scope.

Split recommendations
- Existing split already materialized: 06EZ0NWTM3EPBJS0SWVHXGDGTM for timestamp/record-source hooks, 06EZ0NX282R80VF5VBKS6ARFZC for provider behavior hooks, and 06EZ0NX9SVP7MSB1R4PJ50EHGW for validation/failure-mode documentation.
- No further split is recommended for this parent story unless future work adds naming or hashing hook implementation or provider-specific option matrices, in which case that work should go to new dedicated tickets.

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