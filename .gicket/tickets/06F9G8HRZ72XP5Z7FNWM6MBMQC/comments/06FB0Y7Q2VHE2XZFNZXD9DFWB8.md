[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F9G8HRZ72XP5Z7FNWM6MBMQC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F9G8HRZ72XP5Z7FNWM6MBMQC`.
- Optimistic claim succeeded (`expectedRevision=06F9GFH1R1FBTBFTAFWMRHSR8W`, `currentRevision=06FB0WSZ441FVJ8C4F99B9B8NM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F9G8HRZ72XP5Z7FNWM6MBMQC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F9G8HRZ72XP5Z7FNWM6MBMQC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F9G8HRZ72XP5Z7FNWM6MBMQC-task-update-v0-34-0-db2-provider-documentation' from source '7f95015b73aaa58f1bb62cf8ea7d1900fd1f4e70'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F9G8HRZ72XP5Z7FNWM6MBMQC-task-update-v0-34-0-db2-provider-documentation` as `d78ad28eed15`.

Open questions / Risiken
- If README, production-adoption guidance, provider-support docs, external-test instructions, and v0.34.0 release notes are not updated together, the repository will keep conflicting `8.33.0` / `10.33.0` versus `8.34.0` / `10.34.0` guidance.
- DB2 external-test documentation must stay explicit about developer-managed opt-in setup; otherwise readers may infer unsupported default CI or runtime requirements.
- DB2 behavior claims must stay bounded to the documented support and caveat surface; overclaiming provider-native optimization, migration, or validation guarantees would exceed the current evidence baseline.
- Split recommendation: No split recommended; the remaining work is one coordinated documentation slice across README, provider-support guidance, external-test guidance, production-adoption notes, and v0.34.0 release notes.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `46287`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0525`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a626c3ce66484103b4136b55b5a39264`
- completed-at-utc: `<redacted>-10T07:39:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F9G8HRZ72XP5Z7FNWM6MBMQC/runs/20260610T073938128Z-a626c3ce66484103b4136b55b5a39264.json`