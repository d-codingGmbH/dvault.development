[gicket-bot] PO refinement contract

Summary
- Repository evidence already fixes the product decision: new projects should opt into the binary-first profile, existing HexString stores remain valid until an intentional reviewed migration, and DVault does not auto-migrate persisted hash storage; no ticket-side writes or splits were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already ratifies the bounded v1 decision: README.md and docs/getting-started.md say new projects should choose the binary-first profile explicitly while public hash-key values stay lowercase hexadecimal strings.
- Existing persisted HexString-compatible databases and configurations remain supported until the application owner chooses a separate reviewed migration, reset, or data-move plan; this ticket should not reopen that baseline.
- The current release-note trail already establishes the no-automatic-migration posture: docs/releases/v0.36.0.md introduces it, docs/releases/v0.37.0.md carries it forward, and CHANGELOG.md summarizes the same storage-profile story.
- Relations and attachments were reviewed from the ticket snapshot; no child-ticket split, relation cleanup, description update, planning document, or attachment materialization was necessary for refinement.

Scope In
- Make the public documentation tell one consistent three-part story: binary-first is the recommendation for new projects, HexString remains the compatibility posture for existing persisted stores, and automatic hash-storage migration is out of scope.
- Align README.md, CHANGELOG.md, and the versioned release-note surfaces relevant to the current baseline so they use the same adoption and non-goal language.
- Preserve the public API and storage boundary statement that logical hash-key values remain lowercase hexadecimal strings even when Binary physical storage is selected.

Scope Out
- Changing runtime defaults, stable hash algorithm defaults, or physical storage behavior in code.
- Adding automatic rehash, backfill, dual-write, repair, or persisted-key migration tooling.
- Defining provider-specific migration runbooks, operational cutover steps, or new benchmark evidence beyond the checked-in documentation baseline.
- Any product-code, schema, or test changes outside documentation alignment.

Open questions
- none

Follow-up questions
- If the product later wants a supported HexString-to-Binary migration playbook, should that be tracked as a separate provider-aware adoption ticket with rollback and operational preconditions rather than folded into this documentation-alignment task?

Risks
- The biggest documentation risk is collapsing two different ideas into one sentence: binary-first is the recommendation for new projects, but it is not a silent runtime-default change for existing deployments.
- If only the historical v0.36.0 release note carries the strongest adoption language while CHANGELOG.md or the current carried-forward release note stays softer, adopters may still read the public guidance as inconsistent.

Split recommendations
- No split recommended; the current repository evidence already bounds this as one documentation-alignment task.

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