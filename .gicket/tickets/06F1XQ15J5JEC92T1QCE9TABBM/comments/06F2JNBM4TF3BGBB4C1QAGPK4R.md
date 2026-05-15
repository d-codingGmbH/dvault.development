[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F1XQ15J5JEC92T1QCE9TABBM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.2` was applied to ticket `06F1XQ15J5JEC92T1QCE9TABBM`.
- Optimistic claim succeeded (`expectedRevision=06F2JKWA3XHXZJ8D2CSJ5T5TMW`, `currentRevision=06F2JM154X1YYSS1N2RHW4JGJ0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.2`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat' from source '46778b30b05ca59025a908c648d30c9fd4f81efc'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F1XQ15J5JEC92T1QCE9TABBM-story-add-dvault-roslyn-analyzer-package-foundat` as `31c808ce2d02`.

Open questions / Risiken
- Required PO action: Update the delivery contract to acknowledge the actual relation state: parent epic 06F1XQ0T5WQWN1AES5Z3E0RMSR, done child 06F1XQ1JNMDXAKMS9NFJA0A3GW, and done blockers 06F1XPS7KGKBP5SVMQPJC49J2G and 06F1XPX99KQRB09GRQG50Z75FM.
- Required PO action: Replace the stale no-child-tickets claim with a concise statement that the first analyzer rules/tests slice is already integrated via child 06F1XQ1JNMDXAKMS9NFJA0A3GW and this story's remaining dev work is packaging/build/test/docs readiness for the analyze...
- Risky assumption: Assuming DMV1901 and DMV1902 are the correct next analyzer ids relies on current local search showing no other DMV1901/DMV1902 usage outside analyzer code/tests; the stable diagnostic story currently documents DMV1001-DMV1801 as the seeded baseline.
- Risky assumption: Assuming IsPackable=false is acceptable for handoff depends on the dev completing either analyzer asset packability or a documented package-boundary rationale, which is present in the AC but not yet implemented in the current project file.
- Split recommendation: No new implementation split is required if PO corrects the relation/provenance text and keeps remaining work focused on analyzer package packability/docs/readiness.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9177`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `7ceb97c3ccdc43579e2882051176ec0f`
- completed-at-utc: `<redacted>-15T01:52:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/344084aa916b49aa01930536ba9a50df663a6a72c686b0102c7103ece8a32239/tickets/06F1XQ15J5JEC92T1QCE9TABBM/runs/20260515T015206553Z-7ceb97c3ccdc43579e2882051176ec0f.json`