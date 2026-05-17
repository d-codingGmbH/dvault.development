[gicket-bot] Run report (outcome: po-critic-blocking-apply)

Summary
- PO-critic review completed with a blocking assessment for ticket '06F2PGKAQVVF8GEZVVC8SHFASG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F2PGKAQVVF8GEZVVC8SHFASG`.
- Optimistic claim succeeded (`expectedRevision=06F3EMSTB05AFNNY9HQ4Y665C4`, `currentRevision=06F3EMZP80YCG5R1ABR0178QRR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites' from source '497ceb47eca25d860e05fcd8670f751ec2435d57'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Guardrail converted PO-critic approval into a return-to-PO outcome because the persisted delivery contract still contains unresolved open questions.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed]).
- Published runtime write-group comment template 'handover-po'.
- Committed transactional ticket writeback for TP `TP8` on branch `ticket/06F2PGKAQVVF8GEZVVC8SHFASG-story-add-code-first-link-parent-satellites` as `3c7fdcc1d620`.

Open questions / Risiken
- Blocking finding: Persisted delivery contract infers an existing public API/type without visible source evidence in the current branch snapshot.
- Blocking finding: Unsupported inferred API claim: DataVaultCodeFirstSatelliteBuilder, T, DataVaultCodeFirstHubBuilder :: - The existing public reuse point for satellite member selection is `DataVaultCodeFirstSatelliteBuilder<T>`, already used by `DataVaultCodeFirstHubBuilder<T...
- Blocking finding: Unsupported inferred API claim: TSatellite, DataVaultCodeFirstSatelliteBuilder :: - Add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` on the existing public `DataVaultCodeFirstLinkBuild...
- Blocking finding: Unsupported inferred API claim: State :: - Any assumption that a product CLR type named `State` already exists; tests may introduce a local sample type if useful.
- Blocking finding: Unsupported inferred API claim: Reuse, DataVaultCodeFirstSatelliteBuilder, T :: - Reuse the existing public `DataVaultCodeFirstSatelliteBuilder<T>` rather than introducing a parallel link-satellite builder type; the current hub builder already proves that bui...
- Required PO action: Replace inferred existing-API/type claims with source-backed evidence from the current branch, or restate the contract so the missing type/API may be created explicitly.
- Risky assumption: Existing API/type assumption lacks source evidence: DataVaultCodeFirstSatelliteBuilder, T, DataVaultCodeFirstHubBuilder :: - The existing public reuse point for satellite member selection is `DataVaultCodeFirstSatelliteBuilder<T>`, already used by `DataVaultC...
- Risky assumption: Existing API/type assumption lacks source evidence: TSatellite, DataVaultCodeFirstSatelliteBuilder :: - Add `Satellite<TSatellite>(string satelliteName, Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)` on the existing public `DataVau...
- Risky assumption: Existing API/type assumption lacks source evidence: State :: - Any assumption that a product CLR type named `State` already exists; tests may introduce a local sample type if useful.
- Risky assumption: Existing API/type assumption lacks source evidence: Reuse, DataVaultCodeFirstSatelliteBuilder, T :: - Reuse the existing public `DataVaultCodeFirstSatelliteBuilder<T>` rather than introducing a parallel link-satellite builder type; the current hub builder alr...

Next steps
- Resolve blocking findings and required Product Owner actions before implementation continues.
- Re-run PO-critic after clarification updates are applied.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9417`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `582b46ef5acc472992e16de5a7128e31`
- completed-at-utc: `<redacted>-17T19:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F2PGKAQVVF8GEZVVC8SHFASG/runs/20260517T191135439Z-582b46ef5acc472992e16de5a7128e31.json`