[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC0EJHAY200E7PXNRGV7XR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC0EJHAY200E7PXNRGV7XR`.
- Optimistic claim succeeded (`expectedRevision=06FBSCXFCEV8E6KG2EXY02BGAR`, `currentRevision=06FCEGX771BPZ32WM643CSK1E0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC0EJHAY200E7PXNRGV7XR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC0EJHAY200E7PXNRGV7XR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi' from source '1bbf3fba363c56964f2d61b45986734a0b872af3'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi` as `7da02ddc87ae`.

Open questions / Risiken
- If the quickstart recommendation changes without an equally visible compatibility caveat, readers can misread the docs as promising automatic migration for existing persisted databases.
- If README/getting-started text is updated but the runnable quickstart programs or examples/README.md stay on the default-only setup, the quickstart path remains inconsistent and weakens the recommendation.
- If the quickstart uses low-level provider capability shaping instead of the shipped named APIs, the docs will undercut the product goal of a clear high-level binary-first setup choice.
- Split recommendation: No further split is needed. The remaining work is a bounded quickstart/example documentation pass, while broader adoption/release-documentation follow-up already has sibling ownership in 06FBSC0TMZBXVVECGQGESWPCY4.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8926`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `232f4fe260d94442a7693e34b802ad63`
- completed-at-utc: `<redacted>-14T18:04:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC0EJHAY200E7PXNRGV7XR/runs/20260614T180438625Z-232f4fe260d94442a7693e34b802ad63.json`