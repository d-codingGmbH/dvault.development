[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the contract is bounded, `## Open Questions` is `none`, and local repo evidence consistently shows PostgreSQL latest-satellite is still a provider-neutral fallback lane with no latest-satellite strategy registered.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FBSCFDFFYQXBK17RT3E8W4CM/description.md:27-38 defines two explicit allowed outcomes, keeps scope to PostgreSQL latest-satellite only, and .gicket/tickets/06FBSCFDFFYQXBK17RT3E8W4CM/description.md:48-49 records `## Open Questions` as `- none`.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-25 registers `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy`, but no `IDataVaultProviderReadStrategy` for PostgreSQL latest-satellite reads.
- src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:564-568 adds fallback cause `NoProviderSpecificStrategyRegistered` when no provider-specific read strategy is registered.
- benchmark-summary.md:75 records PostgreSQL `latest-satellite-read` as `skipped` with `selectedStrategy=<none>`, `plannedReadStrategy=<none>`, and `providerSpecificReadStrategy=not registered for latest satellite reads`; `benchmark-summary.md:76-77` still plan `PostgresDataVaultReadStrategy` only for PIT and bridge.
- docs/plans/provider-optimization-evidence-matrix.md:255-257 keeps the PostgreSQL latest-satellite row at `skipped-placeholder` with no optimization claim, while PIT and bridge keep planned `PostgresDataVaultReadStrategy` rows.
- docs/plans/provider-optimization-gap-matrix.md:51 classifies PostgreSQL `latest-satellite-read` as capability gap `P0.01` and says provider-neutral fallback remains correct while diagnostics report `NoProviderSpecificStrategyRegistered`.
- docs/architecture/dvault-v1-pit-bridge-boundary.md:11-13 says SQLite is still the only optimized latest-satellite read provider path and non-SQLite latest-satellite requests keep provider-neutral read pipelines.
- `git -C /mnt/c/Projects/DVault rev-parse HEAD` returned `bc542c0dca115ebca056ed141d048d8ef85c7dce`, and `git -C /mnt/c/Projects/DVault diff --name-status bc542c0dca115ebca056ed141d048d8ef85c7dce..HEAD --` returned no files, so this branch still has no landed implementation or explicit no-work-required closure text beyond the current scratch snapshot.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assuming PIT/bridge candidate registration implies PostgreSQL latest-satellite coverage would be wrong; current repo evidence limits `PostgresDataVaultReadStrategy` to PIT/bridge lanes.
- Assuming skipped-placeholder guidance rows can support a PostgreSQL performance claim would be wrong; the contract and evidence matrices treat them as non-timing evidence only.

AC / test suggestions
- If this ticket closes as `no-work-required`, the closure note should explicitly cite the same repository surfaces named in the contract: the PostgreSQL registration surface, the benchmark guidance row, the evidence matrix row, and the gap-matrix P0.01 row.
- If this ticket closes as implemented, closure evidence should include request-bound diagnostics for latest-satellite strategy selection and bounded fallback, plus a completed benchmark triplet against the provider-neutral latest-satellite baseline.

Implementation watchouts
- Do not widen this ticket into PostgreSQL PIT/bridge work; the contract keeps that lane separate.
- Do not treat diagnostics-only or skipped-placeholder evidence as sufficient for a PostgreSQL latest-satellite optimization claim.
- The branch is currently unchanged from scratch-source `bc542c0dca115ebca056ed141d048d8ef85c7dce`, so whichever outcome is chosen still needs explicit closure evidence to land on this ticket branch.

Non-blocking notes
- .gicket/tickets/06FBSCHBJEYYERDPA7JN34Y8PG/ticket.json currently shows the downstream documentation/benchmark ticket is still `todo`; keep that follow-up aligned with whichever outcome this ticket chooses.

Split recommendations
- No split recommended; the current ticket already isolates PostgreSQL latest-satellite from PIT/bridge work, and downstream ticket `06FBSCHBJEYYERDPA7JN34Y8PG` already exists to publish the final outcome.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment