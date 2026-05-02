[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB7R6MTJW1PYRN172MW34DM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7R6MTJW1PYRN172MW34DM`.
- Optimistic claim succeeded (`expectedRevision=06EXNNPCEYR3JWRT22370QK0K4`, `currentRevision=06EYJ9RWBJ42VNS6HE3JBV0TWR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB7R6MTJW1PYRN172MW34DM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB7R6MTJW1PYRN172MW34DM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB7R6MTJW1PYRN172MW34DM-task-create-readme-quickstart-with-minimal-confi' from source '6a5cba7dfeac77c06abbcd2e35a0674fb7ab12f4'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EXB7R6MTJW1PYRN172MW34DM-task-create-readme-quickstart-with-minimal-confi` as `16f965d36b63`.

Open questions / Risiken
- If the README example drifts away from the existing tested API shape, the quickstart will become misleading faster than normal code review catches it.
- If the quickstart expands into installation, provider, or publication detail, it will overlap the sibling install ticket and weaken the minimal-configuration message.
- If the query example is presented as a polished long-term read API instead of the current shared-type baseline, users may infer capabilities the package does not yet provide.
- Split recommendation: No new split is recommended; the existing parent story 06EXB7QYF1BB1REM7HQZ4WWVMM and sibling ticket 06EXB7REMY41DF7RE8J3N1RZYC already cover the main planning boundaries.
- Split recommendation: Keep this ticket focused on the README usage quickstart slice and leave installation and publication wording to ticket 06EXB7REMY41DF7RE8J3N1RZYC.
- Split recommendation: Create a later follow-up only if the team intentionally wants a runnable example project or a new higher-level read API example that no longer fits a README-only change.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9578`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `790ab38e71fd4fd6906b6e7598b5a014`
- completed-at-utc: `<redacted>-02T14:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7R6MTJW1PYRN172MW34DM/runs/20260502T145132727Z-790ab38e71fd4fd6906b6e7598b5a014.json`