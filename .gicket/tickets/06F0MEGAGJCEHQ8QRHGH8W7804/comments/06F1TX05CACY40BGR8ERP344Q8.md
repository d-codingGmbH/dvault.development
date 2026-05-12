[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F0MEGAGJCEHQ8QRHGH8W7804'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.4` was applied to ticket `06F0MEGAGJCEHQ8QRHGH8W7804`.
- Optimistic claim succeeded (`expectedRevision=06F1TTNJ1909XG3FXB35Z653G4`, `currentRevision=06F1TTTM7HAHYJYFFXHSVFEBH8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.4`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow' from source 'b7170597f4c4b773f9ced392e9be945a0749e3a6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F0MEGAGJCEHQ8QRHGH8W7804-task-document-model-first-governance-workflow` as `7e731e7afdce`.

Open questions / Risiken
- Risky assumption: README currently still contains v0.6.0 package guidance, so implementation must clearly distinguish current branch v0.7.0 API capability from already-published v0.6.0 release notes/package wording.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9174`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d935e329fc934108a390896a66a712da`
- completed-at-utc: `<redacted>-12T18:30:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/d189cefa058d781d6e64d979814d17ab804061edc525b3e1e95f172607e8edb3/tickets/06F0MEGAGJCEHQ8QRHGH8W7804/runs/20260512T183001887Z-d935e329fc934108a390896a66a712da.json`