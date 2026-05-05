[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NSQFCD3W4CDCJ44GFSKA0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NSQFCD3W4CDCJ44GFSKA0`.
- Optimistic claim succeeded (`expectedRevision=06EZM85A8RTEM79DBC0MEPXNDM`, `currentRevision=06EZM87WSH64FTYAHWSCMP0ET4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0NSQFCD3W4CDCJ44GFSKA0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NSQFCD3W4CDCJ44GFSKA0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NSQFCD3W4CDCJ44GFSKA0-task-add-api-snapshot-guardrails-for-deferred-ca' from source '4bdda3d7ec806691f87aab7a30a6911459ffce35'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06EZ0NSQFCD3W4CDCJ44GFSKA0-task-add-api-snapshot-guardrails-for-deferred-ca` as `7297a0093c48`.

Open questions / Risiken
- An implementation may still try to create placeholder public APIs to prove the guardrail instead of waiting for a real exported contract.
- Shared namespace usage across packages can confuse review if change notes fail to name the affected package even though the snapshot files are package-specific.
- Moving a capability between internal and public across successive tickets can create noisy snapshot churn unless each change explicitly records the chosen boundary.
- Split recommendation: No split is recommended; this remains a shared guardrail task, while concrete deferred-capability API shape stays with the existing PIT, bridge, multi-active, or hook owning stories.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `35781`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0680`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `ca1e0b66fd854625b4af47023c551d3a`
- completed-at-utc: `<redacted>-05T21:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NSQFCD3W4CDCJ44GFSKA0/runs/20260505T215535590Z-ca1e0b66fd854625b4af47023c551d3a.json`