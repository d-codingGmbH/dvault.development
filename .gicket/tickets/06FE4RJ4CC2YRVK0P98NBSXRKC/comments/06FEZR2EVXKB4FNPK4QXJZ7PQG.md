[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4RJ4CC2YRVK0P98NBSXRKC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RJ4CC2YRVK0P98NBSXRKC`.
- Optimistic claim succeeded (`expectedRevision=06FE4VEPVPZZ97H1HAA5K6WRV8`, `currentRevision=06FEZNE463Q7ZY412BZ4ZRFYFC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4RJ4CC2YRVK0P98NBSXRKC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4RJ4CC2YRVK0P98NBSXRKC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena' from source '6910342b7991d1e842f75727f3320cab1473f080'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FE4RJ4CC2YRVK0P98NBSXRKC-story-define-server-side-pit-and-bridge-maintena` as `e978a7fc02ad`.

Open questions / Risiken
- If the story blurs diagnostics, prototype, and runtime platform boundaries, downstream work may accidentally promise deployable SQL, stored procedures, or automatic dispatch that the repository explicitly excludes today.
- If bridge push-down is treated as pre-approved instead of evidence-gated, the team may overcommit to hierarchy and delete-aware semantics that current maintenance behavior does not support.
- Because provider packages currently expose save and read strategy seams but not maintenance strategy seams, implementation work may expand into shared-core API design unless the child tickets stay tightly bounded.
- This parent story now depends on the bounded child-ticket outcomes for concrete evidence and doc updates, so it should be treated as a tracking parent rather than direct implementation work.
- Split recommendation: Keep the current bounded decomposition visible in current context: 06FE4RJD5Z6MWC2E66YB3EZ5YW for PIT dry-run diagnostics, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, 06FE4RJP5KG02DF7AEMCQYGNVW for the PostgreSQL PIT rebuild prototype, 06FE4RJZ4PA0...
- Split recommendation: Do not add a separate bridge implementation ticket until the feasibility task decides whether any bounded bridge push-down shape is worth carrying forward.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8824`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f6565b93a4934163ac4bd1a24db490b9`
- completed-at-utc: `<redacted>-22T15:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RJ4CC2YRVK0P98NBSXRKC/runs/20260622T150835409Z-f6565b93a4934163ac4bd1a24db490b9.json`