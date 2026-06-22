[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FE4RKGASKV6F7DF0RD1WTAV4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4RKGASKV6F7DF0RD1WTAV4`.
- Optimistic claim succeeded (`expectedRevision=06FF2VZX5XWZDQHXGG9SQ9H0X0`, `currentRevision=06FF2W90C187G9RCGJB03MRFVR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4RKGASKV6F7DF0RD1WTAV4-task-update-pit-and-bridge-push-down-architectur' from source '0ecba2751b761211af72ab43877a88ec3aac7764'.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FE4RKGASKV6F7DF0RD1WTAV4-task-update-pit-and-bridge-push-down-architectur` as `c907eecd78d8`.

Open questions / Risiken
- Risky assumption: Assumes v0.45.0 should follow the existing release-label pattern where the release note and changelog record the exploration outcome without widening into the separate README and package-version alignment sweep for `8.45.0` and `10.45.0`.
- Risky assumption: Assumes the docs will distinguish the two PIT maintenance seams correctly: PostgreSQL is a provider strategy registration, while SQL Server is a service replacement with narrower eligibility.
- Split recommendation: Keep the existing decomposition. If the work expands into README, package compatibility, manual publication, local validation, package verifier, or broader `8.45.0` and `10.45.0` consumer-version guidance, keep that in a separate release-alignment ticket ...

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9465`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `4f1652fc3fde497ab9a5cc681d99d8c1`
- completed-at-utc: `<redacted>-22T22:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4RKGASKV6F7DF0RD1WTAV4/runs/20260622T223316183Z-4f1652fc3fde497ab9a5cc681d99d8c1.json`