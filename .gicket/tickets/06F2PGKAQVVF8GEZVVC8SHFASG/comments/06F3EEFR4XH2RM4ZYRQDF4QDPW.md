[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGKAQVVF8GEZVVC8SHFASG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F3ED3SW5W6FM9PMH0N3S75TC`, `currentRevision=06F3ED94M1TZCANF7CZQX4JDVG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source 'e6e983f8a2aab01cbd31e8e4663a2140e93a6e9a'.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites` as `7bedc82b616a`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: State, SatCustomerOrderState, DataVaultCodeFirstLinkBuilder, Participant, TEntity :: - Repository evidence shows link-parent satellites already exist in metadata-first projection and tests, including the CustomerOrder/State bas...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: State, SatCustomerOrderState, DataVaultCodeFirstLinkBuilder, Participant, TEntity :: - Repository evidence shows link-parent satellites already exist in metadata-first projection and tests, including the Cus...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8508`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `24a9ca754cf0448892e30be83f4ad25f`
- completed-at-utc: `<redacted>-17T18:36:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T183646346Z-24a9ca754cf0448892e30be83f4ad25f.json`