[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F492BTNHRPBC7D24E13ECFKM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492BTNHRPBC7D24E13ECFKM`.
- Optimistic claim succeeded (`expectedRevision=06F4NV0PVKPRB0T5XNKSNZK4CG`, `currentRevision=06F5E8NF9CHE0FCXCC8NQA3G18`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F492BTNHRPBC7D24E13ECFKM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F492BTNHRPBC7D24E13ECFKM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F492BTNHRPBC7D24E13ECFKM-epic-performance-analysis-and-query-tuning' from source '0559b16a9a0cbfaa8e5f8b93a1afe004a5c016c6'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Final publication approval remains manual, so docs must keep using the pending-approval placeholder until the approval record supplies the exact v0.18.0 date.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle evidence remains environment-dependent, which can leave those rows skipped locally even when the artifact contract is satisfied.
- Split recommendation: No new split recommended; the epic is already materialized as seven done child tickets spanning contract, diagnostics, tuning, benchmark evidence, provider baselines, and documentation rollout.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8092`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7153a00cac694746a27f9b29dd39dd5f`
- completed-at-utc: `<redacted>-23T23:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492BTNHRPBC7D24E13ECFKM/runs/20260523T233056088Z-7153a00cac694746a27f9b29dd39dd5f.json`