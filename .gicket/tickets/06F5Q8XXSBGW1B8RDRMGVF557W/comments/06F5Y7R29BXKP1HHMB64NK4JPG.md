[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q8XXSBGW1B8RDRMGVF557W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8XXSBGW1B8RDRMGVF557W`.
- Optimistic claim succeeded (`expectedRevision=06F5Y2YY9J81K3VS872NR8SA18`, `currentRevision=06F5Y6CAFZT9GFTPCQ3A127F8R`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q8XXSBGW1B8RDRMGVF557W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q8XXSBGW1B8RDRMGVF557W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q8XXSBGW1B8RDRMGVF557W-story-add-streaming-save-benchmark-evidence' from source 'e69f3d5cc885e75fc867f9ef9c633994056b31ea'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q8XXSBGW1B8RDRMGVF557W-story-add-streaming-save-benchmark-evidence` as `df42f99674ef`.

Open questions / Risiken
- Benchmark timings and allocations are machine-sensitive; before and after comparisons must preserve the same run-context inputs already required by the artifact contract.
- Chunked-save results can vary by selected strategy or retained-state fallback behavior, so rows will be misleading if executionDetail does not expose the exercised path.
- The ticket currently has two incoming blocks relations, so delivery sequencing may still depend on upstream work even though PO scope is ready.
- Split recommendation: No split is recommended if the ticket stays bounded to SQLite evidence, existing artifact files, and documentation or evidence updates.
- Split recommendation: If future work needs provider-specific chunk matrices or chunk-optimization feature changes, create a follow-up story instead of expanding this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.6097`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `a59ba8ece8db40d4845e85e5adfbfdc1`
- completed-at-utc: `<redacted>-25T12:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8XXSBGW1B8RDRMGVF557W/runs/20260525T123208261Z-a59ba8ece8db40d4845e85e5adfbfdc1.json`