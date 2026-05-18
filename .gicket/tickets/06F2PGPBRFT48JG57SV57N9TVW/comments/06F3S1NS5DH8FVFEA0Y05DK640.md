[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F2PGPBRFT48JG57SV57N9TVW'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPBRFT48JG57SV57N9TVW`.
- Optimistic claim succeeded (`expectedRevision=06F2PNMHZPD8ZRM967TN5BD8J8`, `currentRevision=06F3RXW59MMPEK10KW7KTCTAFG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F2PGPBRFT48JG57SV57N9TVW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F2PGPBRFT48JG57SV57N9TVW': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F2PGPBRFT48JG57SV57N9TVW-story-add-pit-maintenance-service' from source '9d4f79da4cf7b6b99803ac78d1b1648ff9dc913b'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Queued bounded PO planning artifact(s) for transactional writeback: docs/plans/pit-maintenance-service-v1-contract.md, docs/plans/README.md.
- 2 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Provider-neutral full rebuild and parent-scoped recomputation can be expensive on large PIT tables, so v1 should not imply provider-specific physical tuning or hosted orchestration.
- The repository still contains both legacy PointInTime and newer DataVaultPitMetadata surfaces, so the implementation must avoid accidentally merging or renaming those contracts.
- If the downstream documentation ticket does not update README and v0.15.0 release notes promptly, public guidance will still describe PIT reads without the new maintained-PIT baseline.
- Split recommendation: No further split is recommended; the repository already contains the durable planning split through docs/plans/pit-maintenance-service-v1-contract.md and tickets 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, and 06F2PGPXVAYRBC94RQ7X5V4DVG.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9021`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `75b070f6ce8e4b248156ad5157f865e2`
- completed-at-utc: `<redacted>-18T19:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPBRFT48JG57SV57N9TVW/runs/20260518T191842544Z-75b070f6ce8e4b248156ad5157f865e2.json`