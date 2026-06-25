[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FF43W243BZM340V86CAXQC00'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43W243BZM340V86CAXQC00`.
- Optimistic claim succeeded (`expectedRevision=06FFY92R42EP6QY07VWQJHT32M`, `currentRevision=06FFY9CNS90ZWHY4EJ8QRR6EZG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43W243BZM340V86CAXQC00-task-prototype-analyzer-package-retargeting-if-a' from source '83731e6cae84e3ffe71932dbed0adbe23401e50c'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FF43W243BZM340V86CAXQC00-task-prototype-analyzer-package-retargeting-if-a` as `f07690ceb62b`.

Open questions / Risiken
- Risky assumption: Any future doc or release statement that implies the `8.47.0` analyzer package is validated on a pure `.NET 8 SDK` host would exceed the repository-backed proof accepted by this ticket.
- Risky assumption: Developers must treat this as a bounded no-work/closure handoff unless they find a concrete mismatch against the documented baseline; otherwise they could accidentally reopen retargeting scope that the audit rejected.
- Split recommendation: No split recommended; the current ticket is already bounded to ratifying the existing analyzer baseline, and any pure `.NET 8 SDK` analyzer-host expansion belongs in a separate follow-up ticket.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8873`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `764a8a7a7e9843c6876e013a77c8219c`
- completed-at-utc: `<redacted>-25T14:26:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43W243BZM340V86CAXQC00/runs/20260625T142614050Z-764a8a7a7e9843c6876e013a77c8219c.json`