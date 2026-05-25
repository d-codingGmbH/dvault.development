[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q8XPXEQPJTKGJ7BQGCY438'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8XPXEQPJTKGJ7BQGCY438`.
- Optimistic claim succeeded (`expectedRevision=06F5Q97NXJXVW5VP8YT2JATHF0`, `currentRevision=06F5X7S27GK491J0GMTGC8HB28`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q8XPXEQPJTKGJ7BQGCY438': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q8XPXEQPJTKGJ7BQGCY438': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q8XPXEQPJTKGJ7BQGCY438-story-explain-streaming-fallback-and-remediation' from source '0b1caa35633951259af2b6b13dac2283ba55e298'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q8XPXEQPJTKGJ7BQGCY438-story-explain-streaming-fallback-and-remediation` as `c566063f0786`.

Open questions / Risiken
- Because the default AddDVault() path stays telemetry-free, consumers that do not register AddDVaultTelemetry() or a custom IDataVaultTelemetryObserver may miss the new streaming remediation guidance unless docs clearly show how to opt in.
- Cause-to-remediation mappings can drift when provider strategy gates change; tests should assert that every currently exposed fallback enum and retained-state classification has a stable bounded explanation.
- Chunked attempts aggregate causes across chunks, so remediation text must stay aggregate and deterministic rather than implying a raw per-chunk execution trace.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8342`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `77eb930c9faa4e478320485dad54f6eb`
- completed-at-utc: `<redacted>-25T10:20:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8XPXEQPJTKGJ7BQGCY438/runs/20260525T102050463Z-77eb930c9faa4e478320485dad54f6eb.json`