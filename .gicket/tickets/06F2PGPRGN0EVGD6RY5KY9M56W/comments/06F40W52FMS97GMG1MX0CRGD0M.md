[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGPRGN0EVGD6RY5KY9M56W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGPRGN0EVGD6RY5KY9M56W`.
- Optimistic claim succeeded (`expectedRevision=06F40TSZ68ASFCKXS4P667EZH4`, `currentRevision=06F40TWE6D20EW0ZYBRD5RWRFC`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt' from source '7c24f3f6656acd137bdd519961f401d1334703ee'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGPRGN0EVGD6RY5KY9M56W-story-add-provider-aware-pit-and-bridge-read-opt` as `a38ab8341432`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Internal :: - Internal/provider-package dispatch that can choose optimized PIT-backed read execution for supported provider and request-shape combinations behind the existing public read APIs.
- Blocking finding: Unsupported inferred API claim: Internal :: - Internal/provider-package dispatch that can choose optimized bridge read execution for supported provider and request-shape combinations behind the existing public read APIs.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Internal :: - Internal/provider-package dispatch that can choose optimized PIT-backed read execution for supported provider and request-shape combinations behind the existing public read APIs.
- Risky assumption: Existing API/type assumption lacks source evidence: Internal :: - Internal/provider-package dispatch that can choose optimized bridge read execution for supported provider and request-shape combinations behind the existing public read APIs.

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.7917`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `43ca59fcee2444e88bb8aa7646a49d0e`
- completed-at-utc: `<redacted>-19T13:33:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGPRGN0EVGD6RY5KY9M56W/runs/20260519T133303869Z-43ca59fcee2444e88bb8aa7646a49d0e.json`