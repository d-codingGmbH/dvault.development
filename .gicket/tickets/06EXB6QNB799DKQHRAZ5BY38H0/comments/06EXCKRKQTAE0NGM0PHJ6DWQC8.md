[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EXB6QNB799DKQHRAZ5BY38H0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6QNB799DKQHRAZ5BY38H0`.
- Optimistic claim succeeded (`expectedRevision=06EXBF7W9AQJG4MH3V7PYJ41Q8`, `currentRevision=06EXCK7W1TZ87TDSXM2VX6K1BW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EXB6QNB799DKQHRAZ5BY38H0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EXB6QNB799DKQHRAZ5BY38H0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EXB6QNB799DKQHRAZ5BY38H0-task-draft-default-convention-policy' from source 'c47747b82eb65eafd2f9b1fb1741d5b0f5ef0b6d'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- A policy that is too abstract may still leave implementers with divergent physical schemas; acceptance criteria require deterministic logical defaults to reduce that risk.
- Provider-neutral language can accidentally hide provider-specific constraints; examples should be clearly labeled and avoid becoming unofficial provider commitments.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `18801`
- cached-tokens: `12160`
- effective-cache-ratio: `0.6468`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `76595ab68fe342819ab9eb96e965e582`
- completed-at-utc: `<redacted>-28T22:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6QNB799DKQHRAZ5BY38H0/runs/20260428T225639219Z-76595ab68fe342819ab9eb96e965e582.json`