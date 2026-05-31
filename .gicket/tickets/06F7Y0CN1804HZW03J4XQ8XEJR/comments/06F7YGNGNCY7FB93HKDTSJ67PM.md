[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F7Y0CN1804HZW03J4XQ8XEJR'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F7Y0CN1804HZW03J4XQ8XEJR`.
- Optimistic claim succeeded (`expectedRevision=06F7Y6KVQAZWKGR3RSSR02A7FG`, `currentRevision=06F7YCPF77PMK66KCSK2KCT8GW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F7Y0CN1804HZW03J4XQ8XEJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F7Y0CN1804HZW03J4XQ8XEJR': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F7Y0CN1804HZW03J4XQ8XEJR-story-define-async-streaming-save-contract-and-b' from source 'aa67a4a93ab661e5f75bd4a02f4e7fc6d65dc0ab'.
- Removed bot-owned orchestration details from the persisted PO delivery contract.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- 1 additional item(s) omitted. See the local context artifact for full run details.

Open questions / Risiken
- Long-running or poorly behaved async sources can defer faults or ignore cancellation until enumeration advances, so implementation tests need explicit coverage to keep the public contract caller-visible and deterministic.
- Very large streams can hit the existing retained-state high-water limit and fall back to per-chunk persisted latest-state lookup, which preserves correctness but may change performance characteristics.
- Reusing the existing chunked-save telemetry family avoids API sprawl, but the docs must stay explicit that async streaming is a source-shape difference, not a new provider strategy or optimized ingestion claim.
- Split recommendation: No additional split is needed; the live blocks relation 06F7Y0CN1804HZW03J4XQ8XEJR -> 06F7Y0DCHTWCN3H25XQF18QE2G already routes code, API snapshot, and test work to the existing implementation story.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9396`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `113531718d534b279099253ea3a5fc29`
- completed-at-utc: `<redacted>-31T18:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F7Y0CN1804HZW03J4XQ8XEJR/runs/20260531T181857575Z-113531718d534b279099253ea3a5fc29.json`