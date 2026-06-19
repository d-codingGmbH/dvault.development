[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCGGN528A2NC6TTA5A99X0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCGGN528A2NC6TTA5A99X0`.
- Optimistic claim succeeded (`expectedRevision=06FD6DXWGYVDS61CVDZ89JTGDG`, `currentRevision=06FDRA4FR4TVCHR3XXFRS50AVW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCGGN528A2NC6TTA5A99X0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCGGN528A2NC6TTA5A99X0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCGGN528A2NC6TTA5A99X0-task-close-postgresql-pit-and-bridge-read-gaps' from source 'f01ed4f563063a64828925b90ba45e70463e123d'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Current repository guidance is internally inconsistent: the smoke-read artifact bundle already contains completed PostgreSQL PIT/bridge rows, while the evidence matrix, gap matrix, current checklist, and architecture text still describe PostgreSQL PIT/bridge as skipped/evidenc...
- If implementers try to close the ticket by converting the root quick baseline into a completed PostgreSQL timing surface instead of citing the existing provider-configured artifact bundle, scope may widen into unnecessary benchmark reruns or artifact-contract churn.
- Because the same smoke-read bundle also contains other provider rows, overly broad documentation edits could accidentally promote MySQL, Oracle, or SQL Server PIT/bridge evidence beyond the exact PostgreSQL rows this ticket owns.
- Split recommendation: No split recommended; the verified repository state keeps this as one bounded evidence-closure task covering artifact adoption, evidence/gap-matrix alignment, and preservation of the existing PostgreSQL PIT/bridge fallback boundary.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9139`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `1b4cd89617b84056ac3ed0c9f1fa1902`
- completed-at-utc: `<redacted>-18T19:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCGGN528A2NC6TTA5A99X0/runs/20260618T192837394Z-1b4cd89617b84056ac3ed0c9f1fa1902.json`