[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q934MSKVCQAHPCWEM29CZW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q934MSKVCQAHPCWEM29CZW`.
- Optimistic claim succeeded (`expectedRevision=06F5Q99FP3B24DQ4ZGKCHEN1S8`, `currentRevision=06F6R8ZEMEM90Y1FXGP3AE5F80`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q934MSKVCQAHPCWEM29CZW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q934MSKVCQAHPCWEM29CZW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q934MSKVCQAHPCWEM29CZW-story-add-hash-canonicalization-manifest-and-com' from source 'f51dcdd1fef5ce9001e2a0e7d99ab07d020ed20f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q934MSKVCQAHPCWEM29CZW-story-add-hash-canonicalization-manifest-and-com` as `5aaf21907224`.

Open questions / Risiken
- The current downstream `blocks` relation should remain until this story's documented contract and tests are accepted on the canonical target branch; current branch evidence alone is not closure evidence.
- Changing published scalar encodings, ordering, or failure behavior later without versioning would break the compatibility vectors that downstream hash-key producers depend on.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9345`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `5a8925ec7a6c4b57956974fd8b24a4c3`
- completed-at-utc: `<redacted>-28T01:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q934MSKVCQAHPCWEM29CZW/runs/20260528T012324273Z-5a8925ec7a6c4b57956974fd8b24a4c3.json`