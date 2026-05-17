[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGKAQVVF8GEZVVC8SHFASG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F3EHY9RK0FSCHAK7TCB2E29R`, `currentRevision=06F3EJ2NK0A7Q1WYVWY6MVZXQC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGKAQVVF8GEZVVC8SHFASG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source '3d241b7ffa9ce14bc01fe3813286fa49d9499a3a'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites` as `ebf73ad05138`.

Open questions / Risiken
- Scope creep into participant roles, aliases, or same-hub recursive link shapes is the main delivery risk because current Code-First link validation already marks those shapes as unsupported.
- Partial delivery is possible if the new API is added but link-satellite projection or downstream regression coverage stops before the documented `parent.kind = link` contract is exercised.
- Public docs remain hub-parent-focused until ticket `06F2PGM9038RXVJH0RJFYEJEV0` lands, so short-term documentation drift remains possible.
- Split recommendation: No further split is recommended from current evidence; keep this ticket focused on the additive Code-First link-parent satellite API and projection gap.
- Split recommendation: Leave documentation follow-through on ticket `06F2PGM9038RXVJH0RJFYEJEV0`, and handle any future mapping or example work as separate follow-up tickets if needed.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `43183`
- cached-tokens: `10624`
- effective-cache-ratio: `0.2460`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5a966cbf22204b9e8220003eae53949f`
- completed-at-utc: `<redacted>-17T19:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T190414234Z-5a966cbf22204b9e8220003eae53949f.json`