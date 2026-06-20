[gicket-bot] PO-critic review contract

Summary
- The delivery contract is specific, bounded, and locally backed by current ticket, relation, repository, and branch-history evidence; with `## Open Questions` resolved to `none`, this ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FE4QR3DD7EFZ4F35SBTFGWSR/description.md` is the authoritative contract and contains `## Open Questions` = `- none` and `## Follow-Up Questions` = `- none`.
- `.gicket/tickets/06FE4QPEZW97YR6YT7MQD1MXTG/ticket.json` is `done`, and `.gicket/relations/TG/SR/06FE4QPEZW97YR6YT7MQD1MXTG--06FE4QR3DD7EFZ4F35SBTFGWSR--blocks.json` confirms the upstream DB2 guardrail relation exists but is historical context for this ticket.
- `.gicket/relations/SR/K8/06FE4QR3DD7EFZ4F35SBTFGWSR--06FE4QRMXVGJVA65ZR5MZ817K8--blocks.json` plus `.gicket/tickets/06FE4QRMXVGJVA65ZR5MZ817K8/ticket.json` show this ticket still owns the upstream DB2 evidence handoff before the downstream v0.42 docs task.
- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` registers `Db2DataVaultSaveStrategy`, `Db2DataVaultReadStrategy`, and the PIT/bridge read strategy interfaces through `AddDVaultDb2()`.
- `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs` and `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs` keep DB2 bounded to clean-context save and provider-matched supported read shapes with explicit fallback gates; `src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs` routes `IBM.EntityFrameworkCore` to `UnsupportedDataVaultLiveSchemaReader`.
- `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` asserts diagnostics select `Db2DataVaultSaveStrategy` and `Db2DataVaultReadStrategy` for representative configured save, latest-satellite, PIT, and bridge execution.
- `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` keep DB2 `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` rows at `executionStatus=skipped`, `iterations=0`, and `persistedOutcome=not executed` when `DVAULT_TEST_DB2_CONNECTION_STRING` is unset.
- `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, `docs/performance-profiles.md`, and `docs/releases/v0.42.0.md` all repeat the same bounded DB2 posture: diagnostics/smoke prove candidate paths, but completed DB2 timing, staged DB2 bulk, provider-native chunk execution, and live-schema reading are not claimed.
- Branch history is ticket-level only: `git show --stat --oneline 0a7156a63` added the delivery contract and `docs/plans/db2-hotspot-evidence-refinement-06FE4QR3DD7EFZ4F35SBTFGWSR.md`, while `git show --stat --oneline f5de0d315` only added po-critic claim metadata at the current head.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The ticket does not give a concrete example of the expected naming/location for a new provider-configured DB2 benchmark artifact triplet, although existing repository patterns make the intended shape inferable.
- The ticket states PIT/bridge evidence requires fresh maintenance signals, but it does not include a concrete positive and negative example for that freshness boundary in the ticket text itself.

Risky assumptions
- A reachable `DVAULT_TEST_DB2_CONNECTION_STRING` can be supplied when the developer needs the provider-configured DB2 benchmark triplet.
- The current root row identities `provider-native-bulk-ingestion`, `latest-satellite-read`, `pit-as-of-read`, and `bridge-traversal-read` remain the exact rows intended for any future DB2 completed-timing promotion.
- Developers will continue to treat smoke and diagnostics evidence as support for bounded strategy selection, not as measured timing evidence.

AC / test suggestions
- When DB2 evidence lands, cite the exact matrix identity for every promoted row and the matching provider-configured `benchmark-summary.md/.csv/.json` triplet in the ticket outcome.
- Capture diagnostics showing `selectedStrategy=Db2DataVaultSaveStrategy` or `Db2DataVaultReadStrategy` for the same configured run that produced each promoted timing row.
- Keep explicit fallback proof for dirty save contexts, unsupported latest-satellite shapes, incomplete read-shape evidence, and stale PIT/bridge maintenance so the bounded support line stays auditable.

Implementation watchouts
- Do not widen scope to staged DB2 bulk, provider-native chunk execution, broader latest-satellite shapes, automatic PIT/bridge maintenance, or DB2 live-schema reading.
- Do not convert skipped-placeholder, diagnostics-only, or smoke-only DB2 rows into timing claims without a completed provider-configured artifact triplet.
- PIT and bridge evidence is only valid for supported maintained shapes; stale or incomplete maintenance evidence must continue to fall back to provider-neutral reads.

Non-blocking notes
- The comment history under `.gicket/tickets/06FE4QR3DD7EFZ4F35SBTFGWSR/comments/` is bot-only and contains no unresolved reviewer questions.

Split recommendations
- No additional PO split is recommended; the checked relations already keep this ticket between done guardrail ticket `06FE4QPEZW97YR6YT7MQD1MXTG` and downstream docs ticket `06FE4QRMXVGJVA65ZR5MZ817K8`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment