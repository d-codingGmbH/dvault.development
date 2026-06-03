[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZGNRG5FY4WWCY3FAX2NS4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZGNRG5FY4WWCY3FAX2NS4`.
- Optimistic claim succeeded (`expectedRevision=06F8MJCA1HW9BR3SD1WCQV1RNC`, `currentRevision=06F8MJP5MBAG4AV07ES9H79PF0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault' from source 'fa5b64be068b1b93921e6eecc9169c83bbf8b41f'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZGNRG5FY4WWCY3FAX2NS4-story-add-analyzer-diagnostics-for-unsafe-dvault` as `93a3c061089e`.

Open questions / Risiken
- Risky assumption: Assuming the analyzer should expand helper methods, infer DI graphs, or reason across assemblies would contradict the direct-source-only boundary in the ticket and docs.
- Risky assumption: Assuming any `UseModel(...)` call is unsafe would conflict with the existing compiled-compatibility proof and documented fixed-shape safe lane.
- Risky assumption: Assuming pooling coverage should already include `AddPooledDbContextFactory<TContext>` or other entrypoints would exceed the current ticket scope; the ticket leaves that explicitly as follow-up work.
- Split recommendation: No further split recommended. The existing separation across contract `06F8KZGC4NY41PRYB2RP00ZA1M`, implementation `06F8KZGNRG5FY4WWCY3FAX2NS4`, fixtures `06F8KZGZND5ZCH147PVBRWXYN4`, and docs `06F8KZHAB717MJJNAWWK7S0A5W` is already appropriate for develo...

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8353`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `38d94c69e49e45faae80e1b874cd1927`
- completed-at-utc: `<redacted>-02T21:50:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZGNRG5FY4WWCY3FAX2NS4/runs/20260602T215008942Z-38d94c69e49e45faae80e1b874cd1927.json`