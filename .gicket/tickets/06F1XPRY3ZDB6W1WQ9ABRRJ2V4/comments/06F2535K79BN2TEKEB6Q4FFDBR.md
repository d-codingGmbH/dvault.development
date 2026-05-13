[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XPRY3ZDB6W1WQ9ABRRJ2V4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F1XPRY3ZDB6W1WQ9ABRRJ2V4`.
- Optimistic claim succeeded (`expectedRevision=06F251DMD6PK5QR6PADW1C84Z8`, `currentRevision=06F251QXDPT8NWJ5QXRDY2E0PG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails' from source '57283e6590b2283de79d3c8b0540ae79cf9ef544'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XPRY3ZDB6W1WQ9ABRRJ2V4-epic-ef-core-lifecycle-guardrails` as `9d095eaa4595`.

Open questions / Risiken
- Blocking finding: The persisted delivery contract does not explicitly mark this tracking-only epic as closure/tracking with no parent-owned implementation slice.
- Required PO action: Resolve the tracking-epic closure audit findings before this parent ticket can be closed.
- Risky assumption: This assessment assumes the workflow's `approve_for_dev` outcome is the correct success signal for a tracking-only closure epic; the repository shows no remaining parent-owned implementation slice.
- Risky assumption: This assessment assumes the `done` state and later tester/integrator comments on child `06F23Z08K0W49K5JMEHP60WZC0` are the authoritative current evidence over its stale pre-delivery clarification text.
- Split recommendation: No further split recommended; the epic already resolves into four done implementation stories plus done release-summary task `06F23Z08K0W49K5JMEHP60WZC0`.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8643`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `d73cd0249db44e229fcbaf8e0de0ce98`
- completed-at-utc: `<redacted>-13T18:15:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/runs/20260513T181506134Z-d73cd0249db44e229fcbaf8e0de0ce98.json`