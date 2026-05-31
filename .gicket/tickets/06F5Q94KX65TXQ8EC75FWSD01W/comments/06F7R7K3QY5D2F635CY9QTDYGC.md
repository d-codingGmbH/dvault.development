[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q94KX65TXQ8EC75FWSD01W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q94KX65TXQ8EC75FWSD01W`.
- Optimistic claim succeeded (`expectedRevision=06F72ZVXPT9CBZFAVBV6GHC07M`, `currentRevision=06F7R4GCG72WKYS4SXZ2746W7M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q94KX65TXQ8EC75FWSD01W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q94KX65TXQ8EC75FWSD01W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g' from source '49ca05c09b296ee0246174a265ddfb1bb254f8c0'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q94KX65TXQ8EC75FWSD01W-story-add-benchmark-backed-performance-profile-g` as `afd3774bebaf`.

Open questions / Risiken
- Because the checked-in optional-provider rows are all skipped, provider-specific sections can easily overclaim unless they stay disciplined about describing gates, fallback behavior, and skip reasons rather than measured wins.
- Timing values are machine-specific and must stay attached to the artifact run context; copying raw numbers without iterations, provider filter, and hardware/runtime context would violate the benchmark evidence contract.
- This story already blocks ticket `06F5Q94SQ086B2DZ1AKFDXGV94`, so expanding it into full coordinated README or release-note consolidation would create unnecessary schedule coupling.
- Split recommendation: No split recommended. Keep one detailed performance-guidance story here and leave the broader repo-wide documentation summary work to ticket `06F5Q94SQ086B2DZ1AKFDXGV94`.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7903`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a7aaf930e07247a189b8758295490dc1`
- completed-at-utc: `<redacted>-31T03:40:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q94KX65TXQ8EC75FWSD01W/runs/20260531T034026939Z-a7aaf930e07247a189b8758295490dc1.json`