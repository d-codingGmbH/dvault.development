[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0ME976PM5455JK04S6GPNNW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0ME976PM5455JK04S6GPNNW`.
- Optimistic claim succeeded (`expectedRevision=06F0QH1F460RRWJYNFNHR8NME0`, `currentRevision=06F0QJ772C023BRV1AR7JCX6VR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0ME976PM5455JK04S6GPNNW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0ME976PM5455JK04S6GPNNW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co' from source '45adb3d512da83d40143bc7f52966c0b406bc5bf'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co` as `3245b4a9b88a`.

Open questions / Risiken
- If implementation broadens selector support beyond direct scalar member access without updating the contract, validation behavior and schema parity tests can drift.
- If future work repurposes the existing `DCoding.Data.DVault.Modeling` builders instead of keeping the new EF-specific surface additive, the public API becomes harder to reason about and migrate.
- If consumers immediately need non-CLR logical hub names, the v1 default-to-type-name decision may force temporary fallback to metadata-first declarations.
- Split recommendation: No new split is required for this ticket; keep the existing child plan of 06F0ME9PM8KXH3VP59TQR0ETA8, 06F0MEA1FF743S14XQW02H4A3W, and 06F0MEAD1BAA5QEVM3F9QJA38G.
- Split recommendation: If fluent multi-active projection or link-parent satellites become in-scope for the same release train, split them into dedicated implementation tickets rather than widening the current ordinary-satellite and link tasks.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9625`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `098af0119fe949efaffc1aaf8866f23f`
- completed-at-utc: `<redacted>-09T08:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0ME976PM5455JK04S6GPNNW/runs/20260509T082633697Z-098af0119fe949efaffc1aaf8866f23f.json`