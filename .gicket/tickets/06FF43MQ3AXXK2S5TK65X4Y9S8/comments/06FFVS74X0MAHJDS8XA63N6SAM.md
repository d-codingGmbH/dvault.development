[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06FF43MQ3AXXK2S5TK65X4Y9S8'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FF43MQ3AXXK2S5TK65X4Y9S8`.
- Optimistic claim succeeded (`expectedRevision=06FFVKT2WKXCRGWQ67BT49CM9C`, `currentRevision=06FFVPWHFF6NTCW2SY8FHH7G5W`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FF43MQ3AXXK2S5TK65X4Y9S8-task-diagnose-personaldata-metadata-without-conf' from source 'a3120e15b5cdaf63fd48d2036fd0c6d22c60089f'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06FF43MQ3AXXK2S5TK65X4Y9S8-task-diagnose-personaldata-metadata-without-conf` as `7644808c6319`.

Open questions / Risiken
- Blocking finding: The ticket's acceptance criteria and implementation notes assume model-first and metadata-first `personalData` markers are already available on the runtime/preflight path, but direct source evidence shows the parser and metadata model still lack any `personal...
- Required PO action: Reconcile the ticket text with current repository reality: either narrow the ticket to diagnostics over an already-existing runtime personal-data representation, or explicitly state that this ticket also owns the missing model-first parser and metadata-firs...
- Required PO action: If `personalData` transport is meant to land elsewhere, name the authoritative prerequisite ticket and update relations so this ticket no longer relies on an implicit dependency.
- Required PO action: Specify how metadata-first input is expected to present a marked field on the diagnostic path, because the currently observed `DataVaultMetadataModel` and `DataVaultSatelliteMetadata` surfaces do not carry `personalData` evidence.
- Risky assumption: Assuming the documentation-only `personalData` contract is enough for developers to choose a runtime carrier shape without reopening scope.
- Risky assumption: Assuming `usable alias/converter coverage` can be evaluated before product defines how model-first and metadata-first marked-field evidence reaches diagnostics.
- Split recommendation: If PO wants to keep this ticket narrow, split or relate a prerequisite ticket that surfaces `personalData` and `encryptedPayloadAlias` into the model-first parser and metadata-first runtime/diagnostic representation first, then leave this ticket focused o...
- Split recommendation: If PO wants one developer task instead, explicitly fold that prerequisite carrier work into this ticket and update scope and Definition of Done so the hidden dependency is no longer implicit.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9082`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `2d1203a0402147568aeae38ef106212f`
- completed-at-utc: `<redacted>-25T08:28:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FF43MQ3AXXK2S5TK65X4Y9S8/runs/20260625T082816995Z-2d1203a0402147568aeae38ef106212f.json`