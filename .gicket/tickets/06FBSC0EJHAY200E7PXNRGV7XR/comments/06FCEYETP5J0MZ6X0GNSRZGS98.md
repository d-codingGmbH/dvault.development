[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSC0EJHAY200E7PXNRGV7XR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC0EJHAY200E7PXNRGV7XR`.
- Optimistic claim succeeded (`expectedRevision=06FCENHZ841AW9G9YZN4P2P2CW`, `currentRevision=06FCEXBS6Z50ARGC21XCCY7MG8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSC0EJHAY200E7PXNRGV7XR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSC0EJHAY200E7PXNRGV7XR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi' from source '388b75421258d0e17789bad1052d77f46437aae4'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSC0EJHAY200E7PXNRGV7XR-task-update-new-project-quickstart-for-binary-fi` as `fa47767d99ab`.

Open questions / Risiken
- If the binary-first recommendation is added without an equally visible compatibility caveat in the quickstart path, readers can misread the docs as promising automatic migration for existing persisted databases.
- If README or getting-started text is updated but examples/README.md or the runnable quickstart programs stay on the default-only setup, the quickstart path remains internally inconsistent and weakens the recommendation.
- If future routing treats this as closure-ready before the named surfaces are actually landed, the ticket can regress into the same unsupported closure posture flagged by PO-critic.
- Split recommendation: No further split is justified. The remaining work is a bounded quickstart and runnable-example documentation pass, while broader release-note or changelog follow-up already has sibling ownership in ticket 06FBSC0TMZBXVVECGQGESWPCY4.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `32027`
- cached-tokens: `7552`
- effective-cache-ratio: `0.2358`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `c6b01d0a0d6b4bca894899d7d31b9315`
- completed-at-utc: `<redacted>-14T18:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC0EJHAY200E7PXNRGV7XR/runs/20260614T185152364Z-c6b01d0a0d6b4bca894899d7d31b9315.json`