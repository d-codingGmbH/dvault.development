[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FF438KMPKSBT6KXZ5DBY85QC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF438KMPKSBT6KXZ5DBY85QC`.
- Optimistic claim succeeded (`expectedRevision=06FFD6PAC7Y3M9T7WGD634K8D0`, `currentRevision=06FFD6Z5GQH066T8D71KMNWK90`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi' from source '668d9d85c756c122f39ab499ead920d22acd0e55'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FF438KMPKSBT6KXZ5DBY85QC-task-add-maintenance-timing-rows-to-provider-evi` as `fa7ec7b6cb54`.

Open questions / Risiken
- Blocking finding: This closure-only audit cannot pass because the repository branch does not contain the documentation changes that the ticket's Definition of Done requires; the observed branch diff is ticket metadata only.
- Blocking finding: The runtime is treating this as a closure-only ticket, but the persisted contract still describes unapplied documentation work on `docs/plans/provider-optimization-evidence-matrix.md` and possibly the shared benchmark contract. That routing mismatch must be c...
- Required PO action: Return the ticket to PO refinement and correct the workflow classification: either remove the closure-only expectation and hand this ticket to development as an implementation task, or attach landed repository evidence proving the required docs changes alre...
- Required PO action: If closure-only handling is still intended, update the ticket contract to cite the exact landed commit/path evidence that satisfies the PIT maintenance timing row requirements; the current branch evidence does not support closure.
- Risky assumption: The contract assumes a future PIT maintenance row can reuse existing benchmark row fields and `executionDetail` vocabulary without an explicit maintenance-row mapping in the shared artifact contract.
- Risky assumption: The contract assumes sibling tickets `06FF43AH9SK6J07GV5EKYV3AMM`, `06FF43AYQYZKFF400CK5Q84WYR`, and `06FF43BPP5NRJR3JTY48ZNEKHM` will land compatible scenario naming and comparator wording without reopening this ticket.
- Split recommendation: No additional split recommended; the existing sibling breakdown still looks sufficient once the ticket is routed out of closure-only handling.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8890`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `5b040c49e1ae428582a3304f01e05730`
- completed-at-utc: `<redacted>-23T22:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF438KMPKSBT6KXZ5DBY85QC/runs/20260623T223731226Z-5b040c49e1ae428582a3304f01e05730.json`