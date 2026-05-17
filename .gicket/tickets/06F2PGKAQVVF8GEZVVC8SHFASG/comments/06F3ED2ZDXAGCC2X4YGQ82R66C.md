[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGKAQVVF8GEZVVC8SHFASG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F2PNKAVH7HHFZY1XA5B84J0W`, `currentRevision=06F3EAX2SBPCV9QCFRVX88RJCG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source 'f5b5df990154e66748c84bbe8bab0806626b29a2'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites` as `35d303c785e8`.

Open questions / Risiken
- The main scope-creep risk is accidentally folding effectivity, same-as, dependent-child-key, or typed-save-helper work into this story because those capabilities are adjacent but separately tracked.
- Public documentation currently still describes link-parent satellite declarations as metadata-first only; if task 06F2PGM9038RXVJH0RJFYEJEV0 is not updated promptly after delivery, shipped behavior and docs will diverge.
- If only the API surface changes without updating Code-First parity and export baselines, regressions could slip past because metadata-first tests already cover SatCustomerOrderState while current Code-First baselines do not.
- Split recommendation: No additional split recommended. Existing child 06F2PGKJBG7NGNVBN0ZDSBE6B8 already closed the coverage-only work, and blocked task 06F2PGM9038RXVJH0RJFYEJEV0 already isolates documentation and release-note follow-through.
- Split recommendation: Keep any later advanced link-satellite variants or compile-time mapping parity as separate follow-up tickets rather than expanding this story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9485`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `2a454a9769584eec96bbc8e471c1805b`
- completed-at-utc: `<redacted>-17T18:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T183039570Z-2a454a9769584eec96bbc8e471c1805b.json`