[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FH8RMZPSZ7H3AQRP8FX72S08'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FH8RMZPSZ7H3AQRP8FX72S08`.
- Optimistic claim succeeded (`expectedRevision=06FH8SKSD5SZWKAG3GCD51J37R`, `currentRevision=06FHMN8JTZC76V28M5WJKJWVSW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FH8RMZPSZ7H3AQRP8FX72S08': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FH8RMZPSZ7H3AQRP8FX72S08': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FH8RMZPSZ7H3AQRP8FX72S08-task-document-provider-native-crypto-capabilitie' from source '41ff3e7e4c78e24a1bda377246be0edd0e7990e7'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FH8RMZPSZ7H3AQRP8FX72S08-task-document-provider-native-crypto-capabilitie` as `d3688ea76f3b`.

Open questions / Risiken
- If docs overstate `conditional` capabilities as implemented runtime behavior, adopters may infer unsupported DDL, probing, or key-management behavior.
- If docs omit coexistence and migration caveats, adopters may expect automatic cutover from custom conversion to native crypto and make unsafe rollout assumptions.
- If docs do not restate non-goals around deletion, backup purge or shredding, and provider provisioning, privacy language could drift into compliance promises the codebase explicitly rejects.
- Split recommendation: No split is recommended; the current scope is already bounded to documentation alignment around the existing provider matrix and the single SQL Server proof-level selection seam.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9141`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9343aaefa92d4d02950e3cb1eeda661c`
- completed-at-utc: `<redacted>-30T21:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FH8RMZPSZ7H3AQRP8FX72S08/runs/20260630T210853518Z-9343aaefa92d4d02950e3cb1eeda661c.json`