[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEE0NC2009J73PP0ATE6YW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F0MEE0NC2009J73PP0ATE6YW`.
- Optimistic claim succeeded (`expectedRevision=06F0QH2Y088EAA8DQFZG5M5RNR`, `currentRevision=06F1S3AP1NZZE36JKE2DT50A9C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEE0NC2009J73PP0ATE6YW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEE0NC2009J73PP0ATE6YW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEE0NC2009J73PP0ATE6YW-story-add-model-first-specification-import' from source '40810bad6210c71154cd49a86eb8709aee8d6353'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEE0NC2009J73PP0ATE6YW-story-add-model-first-specification-import` as `bf45728d15cd`.

Open questions / Risiken
- If unknown fields are ignored, misspelled artifacts can silently drift from intended metadata.
- If loadTimestampStorage is not propagated into provider capability profiles, imported projection can diverge from metadata-first and Code-First behavior.
- If post-parse failures collapse to generic metadata exceptions, users will lose the source-path diagnostics promised by the story.
- Recursive-role and hierarchy bridge cases remain sensitive because current public link metadata may not carry enough role information without a narrow model-first adapter.
- Split recommendation: No new split is recommended. The existing child-ticket set already covers schema, parser/diagnostics, YAML boundary, import/projection, and downstream governance documentation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `47387`
- cached-tokens: `12160`
- effective-cache-ratio: `0.2566`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `e886fb7d8a8d48c1bc0cccd25eb1f917`
- completed-at-utc: `<redacted>-12T14:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F0MEE0NC2009J73PP0ATE6YW/runs/20260512T142553867Z-e886fb7d8a8d48c1bc0cccd25eb1f917.json`