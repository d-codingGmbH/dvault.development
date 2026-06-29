[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FGX5VQ9Y665A727EFJ677SBC'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5VQ9Y665A727EFJ677SBC`.
- Optimistic claim succeeded (`expectedRevision=06FGX6M1H06NS5PZGC7NSZ6SRC`, `currentRevision=06FH5YK0HH38TKEC82XYTBQNQ4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FGX5VQ9Y665A727EFJ677SBC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FGX5VQ9Y665A727EFJ677SBC': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FGX5VQ9Y665A727EFJ677SBC-story-make-binary-hash-storage-migration-manifes' from source '2466ccc246eade15b165a9311756479b8efec24e'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FGX5VQ9Y665A727EFJ677SBC-story-make-binary-hash-storage-migration-manifes` as `7804a096f9d7`.

Open questions / Risiken
- Future drift between conceptual documentation and the checked-in serialized v1 shape could reintroduce ambiguity if later tickets change exporter fields without updating docs and tests together.
- Coverage correctness still depends on authoritative support-bundle or translated-metadata capture for the full selected boundary; incomplete source evidence can underreport PIT, bridge, or participant-reference columns.
- If future work collapses manifest findings into migration-guardrail output or emits raw manifest or support-bundle payloads, it will break the current separation and redaction boundary.
- Split recommendation: No further split recommended; the parent already has four bounded child tickets for contract, validator, preflight integration, and documentation, and all four are done.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7259`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `eb98cd18c459454ebb08b9fc23b2a773`
- completed-at-utc: `<redacted>-29T10:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5VQ9Y665A727EFJ677SBC/runs/20260629T105028377Z-eb98cd18c459454ebb08b9fc23b2a773.json`