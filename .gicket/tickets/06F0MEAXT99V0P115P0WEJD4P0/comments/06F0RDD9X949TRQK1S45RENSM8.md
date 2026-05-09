[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F0MEAXT99V0P115P0WEJD4P0'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F0MEAXT99V0P115P0WEJD4P0`.
- Optimistic claim succeeded (`expectedRevision=06F0QH27SFNBZT8XFR6APA6CEC`, `currentRevision=06F0RC64C02EYQYG7BFZ4GD1KR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F0MEAXT99V0P115P0WEJD4P0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F0MEAXT99V0P115P0WEJD4P0': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F0MEAXT99V0P115P0WEJD4P0-task-define-immutable-model-registry-and-lookup' from source 'b091e8136785439bc59031c4c2a5f39e784bbf7e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F0MEAXT99V0P115P0WEJD4P0-task-define-immutable-model-registry-and-lookup` as `626e74e52a48`.

Open questions / Risiken
- The repository currently exposes both PointInTimeTables and Pits; weak registry naming could cause one of those existing surfaces to be collapsed or lost during adaptation.
- If the lookup domains are underspecified, implementers may accidentally require global uniqueness for parent-scoped metadata such as satellites, creating a breaking contract the current model does not require.
- If provider capability metadata is omitted from the first registry contract, downstream translator or persistence tickets are likely to create parallel lookup paths and erode the single-source-of-truth goal.
- Split recommendation: No additional split is recommended at PO stage; this contract ticket already has four outgoing blocks dependents and should remain the shared contract gate for them.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `43259`
- cached-tokens: `10624`
- effective-cache-ratio: `0.2456`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `d3136001db7240c59c832e34d35b4ace`
- completed-at-utc: `<redacted>-09T10:08:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F0MEAXT99V0P115P0WEJD4P0/runs/20260509T100823249Z-d3136001db7240c59c832e34d35b4ace.json`