[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06EXB7RYFJ3YQDB1E4QHPP8034'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7RYFJ3YQDB1E4QHPP8034`.
- Optimistic claim succeeded (`expectedRevision=06EYJ5GG68KBYR1VMESRTAHWJ0`, `currentRevision=06EYJ5M9JDNTY6S01BSC7D885W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7RYFJ3YQDB1E4QHPP8034-task-implement-normal-ef-baseline-for-customer-p' from source '6065dbc3826ca15df11592e8f21fae4f8068a548'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06EXB7RYFJ3YQDB1E4QHPP8034-task-implement-normal-ef-baseline-for-customer-p` as `7befbdcf18b3`.

Open questions / Risiken
- Blocking finding: The authoritative baseline source the ticket points to documents only the initial customer profile row, not the required subsequent changed state. Developers would have to invent the exact second state, timestamps, and expected stored rows.
- Blocking finding: Cross-ticket comparison is a stated goal, but the paired DVault ticket is not yet refined to the same scenario detail. Without a shared sequence/assertion contract, this ticket can still drift in the exact history it establishes.
- Required PO action: Add one concrete history sequence to this ticket or a shared linked artifact: business key, initial values, changed values, deterministic timestamps, and the expected persisted rows the plain EF baseline must assert.
- Required PO action: Either refine ticket `06EXB7S6DB97GVVTS2GGZ3CCX8` to reference the same exact sequence or attach one shared comparison contract that both tickets must follow.
- Required PO action: Clarify whether the comparison baseline must assert only understandable history or an exact row-by-row persisted outcome contract.
- Risky assumption: Assuming the developer will choose the same changed-state sequence that the later DVault ticket should follow.
- Risky assumption: Assuming the single MVP example row is enough to infer a deterministic two-state comparison contract.
- Split recommendation: Keep the runnable-example question separate, as already suggested in `.gicket/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/description.md:59-60`; first lock the exact comparison scenario contract.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9304`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `03ed3f12c34841aaad0c1ed94beb8b7f`
- completed-at-utc: `<redacted>-02T14:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7RYFJ3YQDB1E4QHPP8034/runs/20260502T143328854Z-03ed3f12c34841aaad0c1ed94beb8b7f.json`