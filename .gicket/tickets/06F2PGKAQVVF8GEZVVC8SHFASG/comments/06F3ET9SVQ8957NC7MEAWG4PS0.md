[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGKAQVVF8GEZVVC8SHFASG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F3ER3D12DVYPTNSY9S9CDW2M`, `currentRevision=06F3ER78XQ4D559686VSG4M7EG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source '155a61e51625231255fb1e607a890a0e13d79582'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites` as `39be00e8407d`.

Open questions / Risiken
- Scope creep into participant aliases, roles, or same-hub recursive link shapes remains the main delivery risk because the current ticket stays bounded to link-parent satellite parity.
- The prompt-backed branch snapshot does not prove an existing reusable satellite builder or storage path, so implementation may need both new public API and new internal declaration plumbing inside the same story.
- Public documentation remains hub-parent-focused until ticket 06F2PGM9038RXVJH0RJFYEJEV0 lands, so short-term documentation drift is still possible.
- Split recommendation: No further split is recommended from current prompt-backed evidence; keep this story focused on the additive link-parent satellite declaration and projection gap.
- Split recommendation: Keep documentation and release-note work on ticket 06F2PGM9038RXVJH0RJFYEJEV0, and raise any later generator, mapping, or example expansion as separate follow-up work if needed.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `83456`
- effective-cache-ratio: `0.5087`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `69f3058f48fd4658bda09d10324c76fd`
- completed-at-utc: `<redacted>-17T19:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T192823356Z-69f3058f48fd4658bda09d10324c76fd.json`