[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEGAGJCEHQ8QRHGH8W7804'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEGAGJCEHQ8QRHGH8W7804`.
- Optimistic claim succeeded (`expectedRevision=06F1TSNNPE47PB03GWXYFKRW20`, `currentRevision=06F1TSRDG8FSHT035GK5FSZAYM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEGAGJCEHQ8QRHGH8W7804': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEGAGJCEHQ8QRHGH8W7804': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow' from source '3f554bbdba48e511ca21cd3dac76e80f19ec1b0a'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow` as `cf8039ea0a08`.

Open questions / Risiken
- The main risk is stale wording copied from v0.6.0 release notes causing docs to understate the current v0.7.0 public API surface.
- README is packaged with NuGet, so long governance detail could obscure the quickstart; a concise README entry with a linked guide remains the safer documentation shape.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `48481`
- cached-tokens: `12160`
- effective-cache-ratio: `0.2508`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `e4686444a5774e7dbd19d0ea6ce768d6`
- completed-at-utc: `<redacted>-12T18:19:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/runs/20260512T181941727Z-e4686444a5774e7dbd19d0ea6ce768d6.json`