[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7QYF1BB1REM7HQZ4WWVMM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7QYF1BB1REM7HQZ4WWVMM`.
- Optimistic claim succeeded (`expectedRevision=06EYM54A6977A1PD43KR7RAZQ0`, `currentRevision=06EYM57QVDNBXEYNJGWV5QSR5W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7QYF1BB1REM7HQZ4WWVMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7QYF1BB1REM7HQZ4WWVMM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7QYF1BB1REM7HQZ4WWVMM-story-write-getting-started-documentation' from source 'b5459b88cfda2e8a0ef77d70e7fb6918a008b6c1'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- README snippets can still drift from the tested API surface over time if later library changes do not keep docs aligned with integration tests.
- Project-reference guidance will confuse future package consumers unless post-publication work under 06EXB8202A88KJJP7WEGBESBYM clearly flips the README to the published-install baseline.
- Split recommendation: No additional split recommended; the parent story should now remain only as the aggregation record over completed child tasks 06EXB7R6MTJW1PYRN172MW34DM and 06EXB7REMY41DF7RE8J3N1RZYC.
- Split recommendation: Do not create a new parent-only dev ticket from this story; any future publication or examples work should be scheduled separately, with 06EXB8202A88KJJP7WEGBESBYM already carrying the release-gate follow-up.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `92968`
- cached-tokens: `10624`
- effective-cache-ratio: `0.1143`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ea4bb4720b6940439178c26abd1a5d86`
- completed-at-utc: `<redacted>-02T19:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7QYF1BB1REM7HQZ4WWVMM/runs/20260502T191028914Z-ea4bb4720b6940439178c26abd1a5d86.json`