[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NX282R80VF5VBKS6ARFZC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NX282R80VF5VBKS6ARFZC`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y4XF6HQ01RG00G1C2FSS8`, `currentRevision=06EZN8FXKADFZYTAXV1SH2CG6W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06EZ0NX282R80VF5VBKS6ARFZC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NX282R80VF5VBKS6ARFZC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NX282R80VF5VBKS6ARFZC-task-implement-provider-behavior-hook-surface-wi' from source '991fd3c69fc4f9756b67557e34fc4c3e4c23c414'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NX282R80VF5VBKS6ARFZC-task-implement-provider-behavior-hook-surface-wi` as `612b5727a79e`.

Open questions / Risiken
- If implementation assumes every provider package already auto-registers provider capability profiles, it will overstate the current baseline and may misroute Postgres or SQL Server behavior.
- If the default inheritance path changes the current fallback selection semantics, existing `AddDVault()` model-translation annotations or provider-neutral save behavior could regress even when no override is configured.
- If this ticket expands into concrete provider option matrices or release-posture commitments, it will reopen scope that the advanced-hooks planning docs explicitly keep deferred.
- Split recommendation: No split recommended; the task is already bounded to the provider-behavior hook surface, default inheritance, explicit provider registration, and regression coverage under parent 06EZ0NWKC9ZME5BSCJFSQEQ02R.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9547`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `56072223570941a7a1ad9bbd531c3e7c`
- completed-at-utc: `<redacted>-06T00:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NX282R80VF5VBKS6ARFZC/runs/20260506T001849919Z-56072223570941a7a1ad9bbd531c3e7c.json`