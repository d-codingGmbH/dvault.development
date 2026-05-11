[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEGYHADPVN575H64D56W2G'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEGYHADPVN575H64D56W2G`.
- Optimistic claim succeeded (`expectedRevision=06F0QH3ME0NBKNK8FEC7SQDNHG`, `currentRevision=06F1F9NXN74NJ60VZNJNW2V6MR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEGYHADPVN575H64D56W2G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEGYHADPVN575H64D56W2G': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract' from source 'beb379fa260633f5306a70f58b8b0d8df2cb71b1'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEGYHADPVN575H64D56W2G-task-define-pit-backed-as-of-read-api-contract` as `a7e546eb6af9`.

Open questions / Risiken
- The main scope-creep risk is pulling legacy `PointInTime` naming, PIT maintenance, or provider-specific optimization into this ticket; any of those would turn a bounded contract task into multi-ticket design work.
- If the raw PIT read-record shape does not make missing satellite snapshot state explicit, downstream typed projectors may implement inconsistent null-or-absence behavior across satellites.
- The live upstream `blocks` relation means a later change to PIT metadata rules could still force this contract to be revised, even though the current repository documents are strong enough for PO refinement now.
- Split recommendation: No new split is recommended from current evidence; keep this ticket as the bounded public-contract-and-examples decision and let the already-related downstream work consume the finalized contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `84608`
- effective-cache-ratio: `0.6764`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a254715eec5243088b9428c2f58e5412`
- completed-at-utc: `<redacted>-11T15:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEGYHADPVN575H64D56W2G/runs/20260511T153651674Z-a254715eec5243088b9428c2f58e5412.json`