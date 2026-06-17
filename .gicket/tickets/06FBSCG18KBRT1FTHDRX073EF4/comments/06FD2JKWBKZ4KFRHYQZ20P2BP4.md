[gicket-bot] Run report (outcome: po-refinement-ready)

Summary
- PO refinement processed ticket '06FBSCG18KBRT1FTHDRX073EF4'. Ticket is ready for handoff to role 'po-critic' according to runtime-orchestration policy.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCG18KBRT1FTHDRX073EF4`.
- Optimistic claim succeeded (`expectedRevision=06FBSD0HEJS266SW4T7FHH0H88`, `currentRevision=06FD2FWN3FY0CMNH7DJMVERJS4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Bounded PO capability profile 'po-refinement-interactive-v1' enabled tool surface(s): gicket-add-attachment, gicket-add-relation, gicket-create-ticket, gicket-read-ticket, gicket-read-ticket-attachments, gicket-read-ticket-comments, gicket-read-ticket-relations, gicket-remove-...
- Bounded PO planning surfaces enabled for ticket '06FBSCG18KBRT1FTHDRX073EF4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Bounded PO attachment surfaces enabled for ticket '06FBSCG18KBRT1FTHDRX073EF4': .gicket-bot/planning/*.md, docs/plans/**/*.md.
- Prepared interactive po scratch worktree for target branch 'ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap' from source 'b9dbdf207981df80e46734fc090c6d7ddc165803'.
- Published PO refinement comment with decisions/questions and planned ticket updates.
- Updated the durable refinement contract in the ticket description.
- Updated ticket fields (labels: added [critic-needed]; removed [needs-po]).
- Published runtime write-group comment template 'handover-po-critic'.
- Committed transactional ticket writeback for TP `TP1` on branch `ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap` as `fd8b898eea07`.

Open questions / Risiken
- Oracle benchmark execution may remain skipped in unattended validation if DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so the ticket must avoid overclaiming completed Oracle timing evidence.
- Docs drift is likely unless all evidence surfaces move together; multiple checked-in files currently state that Oracle latest-satellite has no provider-specific strategy.
- If Oracle-specific SQL behavior cannot preserve provider-neutral latest or as-of parity within the existing narrow shape, the safe outcome is explicit no-work-required documentation rather than a widened or partially specified strategy contract.

Next steps
- Role 'po-critic' can pick up the ticket via policy-matched status/labels.
- Validate implementation scope against updated acceptance criteria.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9571`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po`
- run-id: `7134bfcdcbdd4badb50295423bf5e807`
- completed-at-utc: `<redacted>-16T16:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCG18KBRT1FTHDRX073EF4/runs/20260616T163620049Z-7134bfcdcbdd4badb50295423bf5e807.json`