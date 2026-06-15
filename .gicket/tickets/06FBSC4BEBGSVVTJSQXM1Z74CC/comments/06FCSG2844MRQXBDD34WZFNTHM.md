[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC4BEBGSVVTJSQXM1Z74CC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC4BEBGSVVTJSQXM1Z74CC`.
- Optimistic claim succeeded (`expectedRevision=06FBSCY19HH2KSADB5FJ5PX0B8`, `currentRevision=06FCSE2W6QZE7BZ0VR753X8XF4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC4BEBGSVVTJSQXM1Z74CC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC4BEBGSVVTJSQXM1Z74CC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC4BEBGSVVTJSQXM1Z74CC-task-collect-baseline-provider-optimization-evid' from source 'ce7cd42547718f98d6ccca47ff97ccd1d4cfe6d7'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC4BEBGSVVTJSQXM1Z74CC-task-collect-baseline-provider-optimization-evid` as `4439715d660c`.

Open questions / Risiken
- If downstream docs cite skipped-placeholder rows as timing proof, the repository will overstate external-provider evidence that is only preserved as unavailable guidance.
- DB2 remains a non-timing baseline unless a reachable DB2 connection string produces a checked-in benchmark triplet, so its baseline can still be limited to skipped-placeholder, diagnostics-only, or smoke-only evidence.
- The live stale blocks relations from done tickets may confuse relation reports even though the current ticket record is not blocked.
- Split recommendation: No split recommended; the baseline evidence contract is already bounded and downstream publication work is separated into story 06FBSC4HSXFJ5FM6GWECH2CTGG.
- Split recommendation: Any future work to generate new provider bundles or broaden into binary-vs-hex cross-provider evidence should be handled as follow-up tickets rather than expanding this refinement.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9442`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `b985d227bb7541d9a20c557bdd5d1c6e`
- completed-at-utc: `<redacted>-15T19:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC4BEBGSVVTJSQXM1Z74CC/runs/20260615T192653838Z-b985d227bb7541d9a20c557bdd5d1c6e.json`