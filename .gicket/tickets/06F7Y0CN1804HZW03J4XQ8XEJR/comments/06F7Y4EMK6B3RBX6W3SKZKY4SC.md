[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0CN1804HZW03J4XQ8XEJR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0CN1804HZW03J4XQ8XEJR`.
- Optimistic claim succeeded (`expectedRevision=06F7Y0WVA1BSC4G7PMA9V200G8`, `currentRevision=06F7Y1RXTZZ2WNFQ6R05CBN3JR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0CN1804HZW03J4XQ8XEJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0CN1804HZW03J4XQ8XEJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0CN1804HZW03J4XQ8XEJR-story-define-async-streaming-save-contract-and-b' from source '741b4b9a78dd4e2490281f5d192d5a1971929c92'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F7Y0CN1804HZW03J4XQ8XEJR-story-define-async-streaming-save-contract-and-b` as `149f75f07e3d`.

Open questions / Risiken
- Long-running or poorly behaved async sources can defer faults or ignore cancellation until enumeration advances, so implementation tests need explicit coverage to keep the public contract caller-visible and deterministic.
- Very large streams can hit the existing retained-state high-water limit and fall back to per-chunk persisted latest-state lookup, which preserves correctness but may change performance characteristics.
- Reusing the existing chunked-save telemetry family avoids API sprawl, but the docs must be explicit that async streaming is a source-shape difference, not a new provider strategy or optimized ingestion claim.
- Split recommendation: No additional split is needed; use the existing downstream implementation story `06F7Y0DCHTWCN3H25XQF18QE2G` for the code, API snapshot, and test work after this ticket clears PO critic.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `26896`
- cached-tokens: `2432`
- effective-cache-ratio: `0.0904`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `f7c64ff206b744a8844bac66c1f65478`
- completed-at-utc: `<redacted>-31T17:25:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0CN1804HZW03J4XQ8XEJR/runs/20260531T172535499Z-f7c64ff206b744a8844bac66c1f65478.json`