[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F8KZP9XJ868GY6GT934QVFH4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZP9XJ868GY6GT934QVFH4`.
- Optimistic claim succeeded (`expectedRevision=06F8M01RFQA6HF4MRYT8WJGZTW`, `currentRevision=06F99943W4N3PP4GDAWPQ554RM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F8KZP9XJ868GY6GT934QVFH4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F8KZP9XJ868GY6GT934QVFH4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F8KZP9XJ868GY6GT934QVFH4-story-define-support-bundle-freshness-and-finger' from source 'c6300ded47c7b7c39ac683ead9f7d2ea3c585398'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F8KZP9XJ868GY6GT934QVFH4-story-define-support-bundle-freshness-and-finger` as `6669f1c9986c`.

Open questions / Risiken
- The contract is currently split across architecture docs, model-first guidance, analyzer README text, source-generator code, and tests; if downstream tickets paraphrase it loosely, wording drift can recreate ambiguity about freshness versus shape compatibility.
- Ticket state may lag repository state: the blocked diagnostics story is still todo even though the current repository already shows a substantial diagnostic implementation baseline.
- Split recommendation: No new split is needed; the parent epic already separates contract definition, diagnostics implementation or verification, and documentation refresh.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9492`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `91fec52b2f21438dac84c846330d3261`
- completed-at-utc: `<redacted>-04T22:13:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZP9XJ868GY6GT934QVFH4/runs/20260604T221331140Z-91fec52b2f21438dac84c846330d3261.json`