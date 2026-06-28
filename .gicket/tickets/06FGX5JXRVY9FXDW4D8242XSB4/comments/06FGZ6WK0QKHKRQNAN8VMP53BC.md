[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FGX5JXRVY9FXDW4D8242XSB4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FGX5JXRVY9FXDW4D8242XSB4`.
- Optimistic claim succeeded (`expectedRevision=06FGZ4YZW99SE2K1X8JRY1E2CR`, `currentRevision=06FGZ5B4JZ7VF0CHST34KW8998`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FGX5JXRVY9FXDW4D8242XSB4-task-add-analyzer-package-verifier-and-sdk-host' from source '9b4d7bddff4e867a0250448dc0f06d65759886e6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FGX5JXRVY9FXDW4D8242XSB4-task-add-analyzer-package-verifier-and-sdk-host` as `7b51180e9002`.

Open questions / Risiken
- Risky assumption: Unsupported pure .NET 8 SDK analyzer-host behavior may stay documentation-backed and verifier-backed rather than executed as a negative lane; the contract already accepts that risk.
- Risky assumption: The smoke proof is expected to build on the existing integration fixture rather than requiring a brand-new publication flow; if developers choose an equivalent fixture instead, it still needs to prove the same supported host claim.
- Split recommendation: No split is needed for the current verifier, smoke, and documentation-alignment slice.
- Split recommendation: If pure .NET 8 SDK analyzer-host support becomes a real product requirement, split it into a separate follow-up for analyzer target/package/dependency changes and a second validation/documentation follow-up.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8060`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `91b44e45b95142b6b71a33f000da03b8`
- completed-at-utc: `<redacted>-28T19:01:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FGX5JXRVY9FXDW4D8242XSB4/runs/20260628T190121794Z-91b44e45b95142b6b71a33f000da03b8.json`