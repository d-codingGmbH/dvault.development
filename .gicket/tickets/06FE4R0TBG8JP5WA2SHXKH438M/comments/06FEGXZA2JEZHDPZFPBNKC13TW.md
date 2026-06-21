[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FE4R0TBG8JP5WA2SHXKH438M'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4R0TBG8JP5WA2SHXKH438M`.
- Optimistic claim succeeded (`expectedRevision=06FEGRF3MMYXPT5RQNW6EYVWE0`, `currentRevision=06FEGWGEENRTYWM01ZEQ8HZHT4`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4R0TBG8JP5WA2SHXKH438M-task-add-caller-owned-binary-storage-migration-m' from source '4bf872dfff3587c1b8d1ba815fe7b83e6e69362a'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FE4R0TBG8JP5WA2SHXKH438M-task-add-caller-owned-binary-storage-migration-m` as `1d410bb1c377`.

Open questions / Risiken
- Risky assumption: Developers will fit the artifact into the existing consumer-owned preflight surface without needing a PO-level decision on exact command naming or schema naming.
- Risky assumption: Equivalent persisted-shape drift can be derived from the existing metadata and support-bundle vocabulary without reopening the product contract.
- Split recommendation: No split recommended; the persisted delivery contract already bounds this as one preflight-artifact task, and the parent story 06FE4R089MT3BYRCVH7Q4EX6CG is already done.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8766`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `faec1156414e4d028b46b3999f1bbb7f`
- completed-at-utc: `<redacted>-21T04:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4R0TBG8JP5WA2SHXKH438M/runs/20260621T043713360Z-faec1156414e4d028b46b3999f1bbb7f.json`