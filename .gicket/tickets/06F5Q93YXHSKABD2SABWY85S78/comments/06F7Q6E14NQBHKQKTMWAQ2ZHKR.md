[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the ticket contract is specific, `## Open Questions` is resolved to `none`, repository evidence confirms the telemetry baseline to reuse, and local search found no landed Activity tracing doc or `ActivitySource` implementation that would make the story redundant.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q93YXHSKABD2SABWY85S78/description.md` contains the persisted Delivery Contract with `## Open Questions` set to `- none`; the same file also carries the exact span/tag/value lists under `## Original Ticket Draft`, which the contract says to use when not conflicting.
- `git -C /mnt/c/Projects/DVault show --stat --oneline --no-patch HEAD` reported `da50debd5 (HEAD -> ticket/06F5Q93YXHSKABD2SABWY85S78-story-define-opt-in-activity-tracing-contract-an)`, and `git diff --stat da50debd5de56bc7a1bf1efd2be714d2304cca67..HEAD` was empty, so the review surface matches the supplied scratch snapshot.
- `ls -1 /mnt/c/Projects/DVault/docs/architecture` listed only existing contract/implementation notes such as `dvault-v1-streaming-explicit-save-contract.md` and `dvault-v1-pit-bridge-boundary.md`; no Activity tracing contract document is present there.
- `rg -n -F "ActivitySource" /mnt/c/Projects/DVault/src /mnt/c/Projects/DVault/docs /mnt/c/Projects/DVault/tests` and `rg -n -i "activity tracing|activity span|listener-driven" ...` returned no matches, supporting the ticket claim that tracing is not already landed.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers `AddDVault()` core services without any `IDataVaultTelemetryObserver`, while `src/DCoding.Data.DVault/DataVaultTelemetryServiceCollectionExtensions.cs` adds the opt-in `IDataVaultTelemetryObserver` -> `DataVaultMeterTelemetryObserver` registration.
- `src/DCoding.Data.DVault/IDataVaultTelemetryObserver.cs`, `DataVaultSaveTelemetrySummary.cs`, `DataVaultReadTelemetrySummary.cs`, `DataVaultSaveTelemetryOperationKind.cs`, `DataVaultReadTelemetryFamily.cs`, and `DataVaultDiagnostics.cs` provide the existing bounded save/read vocabulary the story tells the contract doc to reuse: `SingleRequest|BulkRequest|ChunkedRequest`, `LatestSatellite|Pit|Bridge`, `DataVaultSaveStrategyDiagnosticsStatus`, `DataVaultReadStrategyDiagnosticsStatus`, and finite save/read fallback-cause enums.
- `src/DCoding.Data.DVault/DataVaultMeterTelemetryObserver.cs` sets built-in meter name `DCoding.Data.DVault`, and `docs/releases/v0.16.0.md` states `AddDVaultTelemetry()` is opt-in while `AddDVault()` remains telemetry-free by default.
- Related downstream stories already exist as separate `todo` tickets in `.gicket/tickets/06F5Q9463M0RSHAJJX0F3D1DB0/ticket.json`, `.gicket/tickets/06F5Q94D0JDMMWDXSRGWX1E4F0/ticket.json`, and `.gicket/tickets/06F5Q94KX65TXQ8EC75FWSD01W/ticket.json`, matching the ticket's current split recommendation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Non-blocking: the eventual contract document should include one explicit maintenance `noop` example that shows `dvault.maintenance.noop` emission and omission of non-applicable common tags such as `dvault.strategy.status` and `dvault.strategy.type`.
- Non-blocking: the eventual contract document should include one explicit cancellation example showing `ActivityStatusCode.Error`, `dvault.outcome=canceled`, and the redacted failure metadata shape.

Risky assumptions
- Implementers must not assume the existing Metrics tag spellings are the Activity tag keys; `DataVaultMeterTelemetryObserver.cs` uses underscore-based tags such as `dvault.strategy_status` and `dvault.read_family`, while this ticket intentionally specifies dotted Activity tags such as `dvault.strategy.status` and `dvault.read.family`.
- Maintenance operations currently have no existing public strategy-selection telemetry surface, so the tracing contract needs to make the omission rule explicit instead of assuming maintenance can populate the same strategy tags as save/read.

AC / test suggestions
- In the authored contract doc, add one mapping table from Activity tag/value families to the existing source enums/types: `DataVaultSaveTelemetryOperationKind`, `DataVaultReadTelemetryFamily`, `DataVaultSaveStrategyDiagnosticsStatus`, `DataVaultReadStrategyDiagnosticsStatus`, `DataVaultSaveStrategyFallbackCauseKind`, and `DataVaultReadStrategyFallbackCauseKind`.
- Downstream verification guidance should explicitly include a no-listener check, a listener-enabled success path, a fault path, a cancellation path, and a redaction proof for status descriptions and exception metadata.

Implementation watchouts
- Do not copy the current Metrics surface verbatim; the story is defining a new Activity contract that shares bounded vocabularies but not necessarily the same tag-key names or outcome enum values.
- No local source currently contains `ActivitySource` or `StartActivity` usage, so the contract document must be the first authoritative place that defines listener checks, status mapping, and maintenance span behavior.
- Keep the no-telemetry default tied to `ActivitySource` listener/sampling checks only; `AddDVault()` currently has no tracing registration path to piggyback on.

Non-blocking notes
- The comment history under `.gicket/tickets/06F5Q93YXHSKABD2SABWY85S78/comments/*.md` is bot-generated; I found no human clarification comment that reopens naming, redaction, or rollout decisions.
- The repository already has multiple `docs/architecture/*.md` contract-style precedents, so the requested document location and format are consistent with existing documentation patterns.

Split recommendations
- No additional split recommended; the existing downstream stories already separate save/read tracing, PIT/bridge maintenance tracing, and performance/profile follow-on.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment