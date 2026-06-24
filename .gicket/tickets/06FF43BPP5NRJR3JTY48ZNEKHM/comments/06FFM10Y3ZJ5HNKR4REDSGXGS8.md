[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF43BPP5NRJR3JTY48ZNEKHM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43BPP5NRJR3JTY48ZNEKHM`.
- Optimistic claim succeeded (`expectedRevision=06FF44JFT18WRDHY3S3X5QQ2EG`, `currentRevision=06FFKZ70EJKYHSYKT1HY2FAX7M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF43BPP5NRJR3JTY48ZNEKHM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF43BPP5NRJR3JTY48ZNEKHM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF43BPP5NRJR3JTY48ZNEKHM-task-normalize-provider-neutral-pit-maintenance' from source '147cd18f3b615f68488ab4eb4107f09a6673ef0f'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF43BPP5NRJR3JTY48ZNEKHM-task-normalize-provider-neutral-pit-maintenance` as `e70bc793ec29`.

Open questions / Risiken
- If comparator rows drift into provider-specific prose instead of bounded contract tokens, evidence-matrix and release-note consumers will need brittle special-case parsing.
- If PIT read or bridge read rows are cited as maintenance evidence, the resulting claims will violate the repository's documented evidence boundary.
- If one provider lane preserves provider-neutral fallback detail differently from the other, PostgreSQL and SQL Server citations will remain non-comparable even if both rows exist.
- Split recommendation: No split recommended; the repository evidence supports a single bounded benchmark-contract normalization slice for PostgreSQL and SQL Server comparator rows.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `43443`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0560`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1110f07b5cb54f118d1dcf1ff169b268`
- completed-at-utc: `<redacted>-24T14:23:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43BPP5NRJR3JTY48ZNEKHM/runs/20260624T142354395Z-1110f07b5cb54f118d1dcf1ff169b268.json`