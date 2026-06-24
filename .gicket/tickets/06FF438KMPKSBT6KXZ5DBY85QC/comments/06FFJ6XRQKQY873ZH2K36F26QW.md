[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FF438KMPKSBT6KXZ5DBY85QC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF438KMPKSBT6KXZ5DBY85QC`.
- Optimistic claim succeeded (`expectedRevision=06FFD8JWVEG40R87M1VHAGGN9R`, `currentRevision=06FFJ5FPNEE2HNXVV5803A2HAG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FF438KMPKSBT6KXZ5DBY85QC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FF438KMPKSBT6KXZ5DBY85QC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi' from source '04562e56157aa77e0ed795c2236c984ca45b5a3e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi` as `53b253fa3727`.

Open questions / Risiken
- If maintenance scenario naming remains ambiguous in the docs update, downstream work could continue to misread `pit-as-of-read` rows as PIT maintenance evidence.
- Until sibling tickets land preserved provider-neutral, PostgreSQL, and SQL Server maintenance artifacts, this ticket can only define the contract and cannot itself promote completed maintenance timing claims.
- If the evidence matrix and shared benchmark artifact contract diverge on required maintenance-row fields or token mapping, later evidence rows could become inconsistent across lanes.
- Split recommendation: No additional split is justified; the existing decomposition already separates this contract update from the provider-neutral comparator row work and the PostgreSQL and SQL Server provider-specific benchmark lanes.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `36370`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0669`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `40380dbb247e496baf373d0232b71a91`
- completed-at-utc: `<redacted>-24T10:10:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF438KMPKSBT6KXZ5DBY85QC/runs/20260624T101004078Z-40380dbb247e496baf373d0232b71a91.json`