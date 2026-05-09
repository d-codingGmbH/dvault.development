[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEAD1BAA5QEVM3F9QJA38G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEAD1BAA5QEVM3F9QJA38G`.
- Optimistic claim succeeded (`expectedRevision=06F0QZ638T1DMK09E589J7ZDJR`, `currentRevision=06F0THAT8KAPA0YC3X55JSRF8C`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEAD1BAA5QEVM3F9QJA38G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEAD1BAA5QEVM3F9QJA38G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity' from source 'a58b466d98b2c74f1279e54474303035accbeab1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEAD1BAA5QEVM3F9QJA38G-task-add-code-first-migration-and-schema-parity` as `0c04b0d4ab19`.

Open questions / Risiken
- If parity coverage only checks translator-level metadata and never exercises SQLite schema creation, EF relational-name or index-order drift can slip through.
- If any built-in provider profile is omitted from the inspection matrix, provider-specific behaviors such as Oracle primary-key-covered indexes or MySQL identifier limits can regress unnoticed.
- If the code-first and metadata-first assertions share too much normalization logic, the test suite can produce false positives and miss real schema divergence.
- Split recommendation: No new split is required. Keep the existing parent and sibling dependency structure: `06F0ME8NFJX6CD20MEA10J761R` remains the parent, and done tickets `06F0ME976PM5455JK04S6GPNNW`, `06F0ME9PM8KXH3VP59TQR0ETA8`, and `06F0MEA1FF743S14XQW02H4A3W` remain the ...
- Split recommendation: Keep the current downstream relation unchanged: this ticket still blocks `06F0MEDBFZ25YA1M7RJ71Z7ZCM` for runnable quickstart examples.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9192`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `74000c97e1f84653a17612fe6d44a91a`
- completed-at-utc: `<redacted>-09T15:12:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEAD1BAA5QEVM3F9QJA38G/runs/20260509T151240867Z-74000c97e1f84653a17612fe6d44a91a.json`