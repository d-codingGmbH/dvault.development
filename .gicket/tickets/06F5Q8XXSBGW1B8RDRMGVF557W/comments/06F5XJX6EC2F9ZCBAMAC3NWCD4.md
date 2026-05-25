[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06F5Q8XXSBGW1B8RDRMGVF557W'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q8XXSBGW1B8RDRMGVF557W`.
- Optimistic claim succeeded (`expectedRevision=06F5Q97RA9C6Y61RR782TAPSH4`, `currentRevision=06F5XG06BD88A62SBS2537XFB4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06F5Q8XXSBGW1B8RDRMGVF557W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06F5Q8XXSBGW1B8RDRMGVF557W': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06F5Q8XXSBGW1B8RDRMGVF557W-story-add-streaming-save-benchmark-evidence' from source '4d216276032f77476d70eddc7b92268980e1b5bc'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06F5Q8XXSBGW1B8RDRMGVF557W-story-add-streaming-save-benchmark-evidence` as `fe4553732f85`.

Open questions / Risiken
- Benchmark timings and allocations are machine-sensitive; before/after comparisons must preserve the same run-context inputs already required by the artifact contract.
- Chunked-save results can vary by selected strategy or retained-state fallback behavior, so rows will be misleading if `executionDetail` does not expose the exercised path.
- The ticket currently has two incoming `blocks` relations, so delivery sequencing may still depend on upstream work even though PO scope is ready.
- Split recommendation: No split is recommended if the ticket stays bounded to SQLite evidence, existing artifact files, and documentation or evidence updates.
- Split recommendation: If future work needs provider-specific chunk matrices or chunk-optimization feature changes, create a follow-up story instead of expanding this ticket.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8882`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `0f34a84f7ccf405d81ad5beacc312986`
- completed-at-utc: `<redacted>-25T11:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q8XXSBGW1B8RDRMGVF557W/runs/20260525T110105259Z-0f34a84f7ccf405d81ad5beacc312986.json`