[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEGAGJCEHQ8QRHGH8W7804'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEGAGJCEHQ8QRHGH8W7804`.
- Optimistic claim succeeded (`expectedRevision=06F0QH3Z8KS980C4JS8CHT1180`, `currentRevision=06F1TN92A107Z3F1KCZZ7NETJW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEGAGJCEHQ8QRHGH8W7804': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEGAGJCEHQ8QRHGH8W7804': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow' from source '5de545c5f4924c47177db8f7ce0e068e31f9d184'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow` as `91f9a99bb52f`.

Open questions / Risiken
- The main risk is overstating model-first support while v0.6.0 release notes still defer import/export specs; the docs should separate governance workflow from shipped tooling.
- README is packaged with NuGet, so long governance detail could obscure the quickstart; a concise README entry with a linked guide is the safer documentation shape.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `48776`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0499`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `de155929353e40358a5fede1263b0b72`
- completed-at-utc: `<redacted>-12T18:02:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/runs/20260512T180224272Z-de155929353e40358a5fede1263b0b72.json`