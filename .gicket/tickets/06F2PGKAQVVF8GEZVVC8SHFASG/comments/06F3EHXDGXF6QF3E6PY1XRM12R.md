[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGKAQVVF8GEZVVC8SHFASG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F3EG3TXZBVQ9JT4W46A34504`, `currentRevision=06F3EGA19KM4T6TBPPBJ6DBT54`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source 'd86599a9724f1a227c91bd2cc7eee8e2b6fd4461'.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites` as `ba57869a4ad1`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: Reframed :: - Reframed the story as an explicit additive Code-First API expansion: metadata-first link-parent satellites already exist in the current branch, while Code-First currently lacks link satellite declaration and proje...
- Blocking finding: Unsupported inferred API claim: TSatellite, DataVaultCodeFirstSatelliteBuilder, DataVaultCodeFirstLinkBuilder :: - Add Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null) to the existing public...
- Blocking finding: Unsupported inferred API claim: State :: - Any assumption that a CLR type named State already exists in the product surface; test coverage may introduce a local sample CLR type as needed, but the ticket does not depend on a pre-existing domain type.
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: Reframed :: - Reframed the story as an explicit additive Code-First API expansion: metadata-first link-parent satellites already exist in the current branch, while Code-First currently lacks link satellite d...
- Risky assumption: Existing API/type assumption lacks source evidence: TSatellite, DataVaultCodeFirstSatelliteBuilder, DataVaultCodeFirstLinkBuilder :: - Add Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null) to...
- Risky assumption: Existing API/type assumption lacks source evidence: State :: - Any assumption that a CLR type named State already exists in the product surface; test coverage may introduce a local sample CLR type as needed, but the ticket does not depend on a pre-existing do...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9423`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `1ffe6cb3dcda42eeaedc8ed9ae77d97e`
- completed-at-utc: `<redacted>-17T18:51:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T185144717Z-1ffe6cb3dcda42eeaedc8ed9ae77d97e.json`