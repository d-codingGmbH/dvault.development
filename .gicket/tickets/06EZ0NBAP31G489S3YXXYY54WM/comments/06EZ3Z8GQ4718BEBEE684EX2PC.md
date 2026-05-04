[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06EZ0NBAP31G489S3YXXYY54WM'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EZ0NBAP31G489S3YXXYY54WM`.
- Optimistic claim succeeded (`expectedRevision=06EZ0Y3F34E7N05V4A9T8GG25W`, `currentRevision=06EZ3W10MZYD5ZW3Y47ET3ECN8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-write-p...
- Bounded PO planning surfaces enabled for ticket '06EZ0NBAP31G489S3YXXYY54WM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06EZ0NBAP31G489S3YXXYY54WM': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil' from source '5c3929044270d19ec5c40afb46960d933ccc18b4'.
- Interactive PO tool loop hit bounded stop reason 'tool_call_limit_reached' and fell back to legacy planning.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Without Oracle-backed integration infrastructure in this ticket, provider-specific SQL correctness will rely mostly on unit and smoke coverage and may leave provider-runtime edge cases for later validation.
- Any additive core model-configuration API introduced for provider selection becomes a long-term public compatibility commitment.
- Whole-batch fallback keeps behavior safe but can reduce performance when a batch mixes shapes the Oracle strategy can and cannot optimize.
- Split recommendation: If the ticket grows, separate the shared Oracle capability-profile and model-selection work in src/DCoding.Data.DVault from the Oracle save-strategy implementation in src/DCoding.Data.DVault.Oracle.
- Split recommendation: If provider-specific SQL needs real Oracle runtime proof, schedule Oracle integration harness and contract coverage as follow-up validation work instead of inflating this refinement ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9520`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0e7f6cc15e554054a23a8998410a87e2`
- completed-at-utc: `<redacted>-04T07:56:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EZ0NBAP31G489S3YXXYY54WM/runs/20260504T075626499Z-0e7f6cc15e554054a23a8998410a87e2.json`