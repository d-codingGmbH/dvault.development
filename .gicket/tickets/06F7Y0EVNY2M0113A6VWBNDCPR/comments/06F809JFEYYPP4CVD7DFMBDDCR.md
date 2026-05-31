[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0EVNY2M0113A6VWBNDCPR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0EVNY2M0113A6VWBNDCPR`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0XG33GKN1H4CC3S01CNWC`, `currentRevision=06F8072KHCB3N1H55ZST9PPB0G`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0EVNY2M0113A6VWBNDCPR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0EVNY2M0113A6VWBNDCPR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0EVNY2M0113A6VWBNDCPR-task-add-async-streaming-benchmark-and-allocatio' from source '42d57b2a454494100ee6c5d3a8e2cf4460207556'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Current benchmark tests enumerate exactly three `customer-profile-streaming-save` baselines and explicitly inspect only the two `chunked-save-bounded-*` rows, so harness changes without matching test updates will fail quickly.
- The shared artifact contract currently names materialized bulk and bounded chunked streaming evidence; if async rows are added without contract or README updates, downstream docs can drift or cite ambiguous evidence.
- Repository docs already constrain async streaming to the existing provider-neutral chunked boundary, so careless baseline naming or release wording could accidentally imply a provider-native async optimization that the repository does not prove.
- Split recommendation: No further split is recommended; keep benchmark/allocation evidence on this ticket and keep the downstream public-doc rewrite on `06F7Y0F650KM61BQXMEQPZ86DR`.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9588`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `9d503d5188974211b83b08b7b00b0fad`
- completed-at-utc: `<redacted>-31T22:27:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0EVNY2M0113A6VWBNDCPR/runs/20260531T222734895Z-9d503d5188974211b83b08b7b00b0fad.json`