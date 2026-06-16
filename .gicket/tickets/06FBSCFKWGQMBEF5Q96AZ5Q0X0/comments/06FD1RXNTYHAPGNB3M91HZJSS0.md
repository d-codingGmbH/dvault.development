[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCFKWGQMBEF5Q96AZ5Q0X0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCFKWGQMBEF5Q96AZ5Q0X0`.
- Optimistic claim succeeded (`expectedRevision=06FBSD0E9XZVMEY529PPY3REC8`, `currentRevision=06FD1PDECY8EW7VZCN73GNP6CW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCFKWGQMBEF5Q96AZ5Q0X0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCFKWGQMBEF5Q96AZ5Q0X0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap' from source 'f3336ddcf3ce69cf77ac5c179c7e83dc78453d54'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCFKWGQMBEF5Q96AZ5Q0X0-task-close-sql-server-latest-satellite-read-gap` as `c9985c34cf6b`.

Open questions / Risiken
- The SQL Server benchmark lane is opt-in and connection-string-gated; if local SQL Server execution is unavailable, reviewers must accept skipped-placeholder artifact evidence rather than completed external timing for this ticket.
- If code changes land without matching updates to the evidence matrix, gap matrix, benchmark expectations, and performance guidance, the repository will carry contradictory SQL Server latest-satellite claims.
- Live ticket comment/relation state could not be refreshed because the `gicket-read-ticket*` and relation tool calls were trust-policy blocked; no blocker is visible in the supplied snapshot, but relation housekeeping was not re-verified.
- Split recommendation: No split recommended; the visible repository evidence keeps this as one bounded SQL Server capability-gap task covering strategy registration, gating, diagnostics, tests, benchmark/evidence alignment, and fallback preservation.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9287`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1a12d8bfa1864f309b7bc9604be05e31`
- completed-at-utc: `<redacted>-16T14:44:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCFKWGQMBEF5Q96AZ5Q0X0/runs/20260616T144404556Z-1a12d8bfa1864f309b7bc9604be05e31.json`