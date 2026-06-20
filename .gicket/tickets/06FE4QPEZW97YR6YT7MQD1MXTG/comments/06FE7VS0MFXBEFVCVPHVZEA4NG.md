[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FE4QPEZW97YR6YT7MQD1MXTG'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QPEZW97YR6YT7MQD1MXTG`.
- Optimistic claim succeeded (`expectedRevision=06FE62WQFTRPT8X3RN4GKHCTK8`, `currentRevision=06FE7R71BKMVD96NYPT66SQAY0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FE4QPEZW97YR6YT7MQD1MXTG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FE4QPEZW97YR6YT7MQD1MXTG': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FE4QPEZW97YR6YT7MQD1MXTG-task-add-db2-benchmark-promotion-guardrails' from source '8682f621d67705e8194bd81077d7632c75a8b1ac'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- The root quick benchmark triplet currently contains only skipped DB2 external-provider rows, so downstream docs can still overread planned execution-path or selected-strategy tokens as completed timing evidence.
- DB2 support stays narrow and conditional on clean context, explicit PIT or bridge maintenance, supported read shapes, diagnostics selection, and a reachable `DVAULT_TEST_DB2_CONNECTION_STRING`.
- The repository already documents DB2 live-schema reading as out of scope; inconsistent follow-on wording could still imply support that this guardrail contract forbids.
- This ticket only fixes the promotion boundary; measurable DB2 evidence still depends on downstream delivery in 06FE4QR3DD7EFZ4F35SBTFGWSR.
- Split recommendation: No additional split is recommended. The live downstream `blocks` relation to 06FE4QR3DD7EFZ4F35SBTFGWSR already captures the provider-configured DB2 tuning and evidence work.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8220`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `32d985f102904db78729fa9b2972039a`
- completed-at-utc: `<redacted>-20T07:29:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QPEZW97YR6YT7MQD1MXTG/runs/20260620T072920029Z-32d985f102904db78729fa9b2972039a.json`