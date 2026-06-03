[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZGNRG5FY4WWCY3FAX2NS4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZGNRG5FY4WWCY3FAX2NS4`.
- Optimistic claim succeeded (`expectedRevision=06F8M00FPHHE55349A8A78RX3R`, `currentRevision=06F8MGKMJ4Q7ZZQGN0YGGJ0CPC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZGNRG5FY4WWCY3FAX2NS4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZGNRG5FY4WWCY3FAX2NS4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault' from source '4e0eb426141eac121cadaa846736302b787b2cac'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault` as `ac8304173729`.

Open questions / Risiken
- If implementation expands beyond direct source-visible evidence, it is likely to create false positives against safe fixed-model compiled-model and registry-backed metadata lanes.
- If implementation remains intentionally high-confidence, some real unsafe caller-owned variable-shape cases will remain guidance-only and must be explained by the separate documentation lane.
- The boundary between targeted implementation tests here and the broader fixture story must stay explicit or the ticket will regrow into already-split test-surface work.
- Split recommendation: No further split is recommended; visible evidence already separates contract, implementation, fixtures, and documentation into dedicated tickets.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `40226`
- cached-tokens: `7552`
- effective-cache-ratio: `0.1877`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `402e83a9e49a415b96e689d8dde7c6ad`
- completed-at-utc: `<redacted>-02T21:41:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZGNRG5FY4WWCY3FAX2NS4/runs/20260602T214132714Z-402e83a9e49a415b96e689d8dde7c6ad.json`