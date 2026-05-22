[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract has no open questions and is anchored to existing diagnostics, support-bundle, provider-capability, and strategy-gate surfaces already present in the repository.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F492B40K7B0WWPKH8N3PPG3G/description.md` persists `PO Handoff` = `ready_for_po_critic` and `## Open Questions` = `- none`, so the delivery contract satisfies the stated gate rule for dev handoff.
- `git log --oneline` shows HEAD `4da66e598` on `ticket/06F492B40K7B0WWPKH8N3PPG3G-story-expand-provider-capability-and-strategy-ex`; `git diff --name-only develop..HEAD` touches only `.gicket/tickets/06F492B40K7B0WWPKH8N3PPG3G/*`, so this remains a metadata-only pre-development branch.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` already exposes `DataVaultExplainDiagnostics` with `ProviderName`, `CapabilityProfileName`, `CapabilityProfileDefaulted`, `LoadTimestampValueFormat`, `LoadTimestampStoreType`, `ProviderBehaviorProfileName`, and `ProviderBehaviorDefaulted`, plus save/read strategy diagnostics with candidate `Ordinal`, `StrategyName`, `Priority`, selected-strategy name/priority, and bounded fallback causes.
- `src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs` is the authoritative source for the provider explain facts named by the ticket: built-in SQLite/Postgres/SQL Server/Oracle/MySQL profiles, `NoneInV1Unsupported` SQL-function and concurrency support, MySQL `maximumIdentifierLength: 64`, Oracle `allowsIndexesCoveredByPrimaryKey: false`, and MySQL `unsupportedIncludedIndexColumnMode: Ignore`.
- `src/DCoding.Data.DVault/DataVaultDiagnostics.cs` gate evaluators already define the bounded strategy reasons called out by the contract: dirty context, multi-active, provider-name mismatch, unknown/unregistered provider, SQL Server minimum 50 total operations and maximum 500 satellite operations, MySQL minimum 50 total operations, Oracle minimum 50 total operations, and SQLite-only read-shape fallback reasons for latest/as-of, PIT, and bridge reads.
- `src/DCoding.Data.DVault/DataVaultSupportBundle.cs` and `src/DCoding.Data.DVault/DataVaultSupportBundleExporter.cs` already serialize `DataVaultDiagnosticsResult` into deterministic redacted `dvault.support-bundle.v1` output; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs:71` verifies deterministic redacted export and preserved save/read strategy sections.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:14`, `:67`, and `:128` prove the current baseline already reports provider/explain data, selected SQLite strategy details, and provider-neutral fallback causes, which matches the story's additive-expansion framing.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A test example for the unknown/unregistered-provider path would reduce ambiguity around how `CapabilityProfileDefaulted` and `ProviderBehaviorDefaulted` should appear when diagnostics fall back to the default profile.
- A test example covering equal-priority strategy tie-break behavior would make the requested candidate ordering more explicit, because the repository currently documents DI registration order as the deterministic tie-break for equal priorities.
- A read-fallback example beyond latest/as-of satellite, such as PIT or bridge unsupported-shape fallback, would better lock in the `SQLite-only optimized read-shape constraints` language already referenced by the contract.

Risky assumptions
- Implementers will derive new explain fields from `DataVaultProviderCapabilityProfile`, `DataVaultProviderBehaviorProfile`, and the existing gate evaluators instead of copying thresholds or message text into a second taxonomy.
- The story remains additive only; no current field names, fallback enums, or support-bundle section names are expected to change.
- The ticket's provider-behavior wording is interpreted as reuse of the existing selector/profile output rather than collapsing behavior reporting to `provider-neutral-v1` for every provider.

AC / test suggestions
- Keep one explicit selected-strategy case and one explicit provider-neutral fallback case for both save and read diagnostics, then include at least one unsupported PIT or bridge read-shape fallback so the SQLite-only read baseline is not inferred indirectly.
- Add snapshot or API-surface coverage that proves support-bundle JSON growth is additive and deterministic when the new explain members are populated.
- Add a redaction-focused assertion that the new explain members cannot surface raw SQL, connection fragments, hash keys, or record sources.

Implementation watchouts
- Repository behavior output is not uniformly `provider-neutral-v1`: provider packages already register `sqlite-provider-v1`, `postgres-provider-v1`, `sqlserver-provider-v1`, `oracle-provider-v1`, and `mysql-provider-v1`, while the selector defaults to `provider-neutral-v1` only when no override applies.
- Current read optimization is SQLite-only, but save optimization spans SQLite, Postgres, SQL Server, Oracle, and MySQL; explain output should preserve that asymmetry instead of implying read-provider parity.
- Support-bundle export already serializes diagnostics and redacts secret-like substrings; any new explain fields must stay bounded and redacted to avoid contract drift.

Non-blocking notes
- This review did not expect implementation evidence: the branch diff from `develop` to `4da66e598` is ticket metadata only, which is consistent with a pre-development PO gate.
- The persisted contract is already aligned with downstream dependency context: relation events under `.gicket/tickets/06F492B40K7B0WWPKH8N3PPG3G/events/` show `blocks` links to `06F492B9PR036PDNN52S06S9BC`, `06F492BG6BZYYFMBE5WK7CB024`, and `06F492BNDPWS9P4EDSV0W7G6VM`.

Split recommendations
- No split needed at PO-critic time; the ticket stays focused on the reusable diagnostics and support-bundle contract that downstream stories depend on.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment