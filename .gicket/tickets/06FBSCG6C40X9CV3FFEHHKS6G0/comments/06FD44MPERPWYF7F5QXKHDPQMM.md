[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff. The delivery contract is explicit, `## Open Questions` is `none`, and repository evidence consistently shows DB2 latest-satellite remains provider-neutral today while PIT/bridge candidate support exists.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket snapshot Delivery Contract shows `decision: ready_for_po_critic`, `## Open Questions` = `none`, and acceptance criteria that explicitly allow either a DB2 provider-specific latest/current/as-of strategy or an authoritative no-work-required closure.
- `git rev-list --left-right --count e800be1226efdbdf022f403b5c9ed9c888a58b49...HEAD` returned `0 0`, `git diff --stat e800be1226efdbdf022f403b5c9ed9c888a58b49...HEAD` returned empty output, and `git status --short` returned empty output, so the ticket branch currently carries no repository delta beyond the scratch source ref.
- `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` registers `Db2DataVaultReadStrategy` only as `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy`; there is no `IDataVaultProviderReadStrategy` registration for DB2 latest-satellite reads.
- `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs` exposes DB2 gate evaluators for PIT and bridge reads but no DB2 latest-satellite evaluator, while SQL Server has an explicit latest-satellite evaluator.
- `tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs` asserts DB2 latest-satellite diagnostics use provider-neutral fallback with `DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered`, while DB2 PIT and bridge diagnostics select `Db2DataVaultReadStrategy`.
- `benchmark-summary.md` and `benchmark-summary.json` keep the DB2 `latest-satellite-read` row as `skipped` with `selectedStrategy=<none>`, `plannedReadStrategy=<none>`, and `providerSpecificReadStrategy=not registered for latest satellite reads`, while DB2 PIT and bridge rows still name `Db2DataVaultReadStrategy` as the planned strategy.
- `docs/plans/provider-optimization-gap-matrix.md`, `docs/plans/provider-optimization-evidence-matrix.md`, `docs/performance-profiles.md`, and `docs/architecture/dvault-v1-pit-bridge-boundary.md` all align on the same boundary: DB2 latest-satellite remains provider-neutral, and DB2 provider-specific read support today is limited to PIT/bridge candidate paths.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Live `gicket-read-ticket` and `gicket-read-ticket-comments` calls were trust-blocked, so this review assumes the prompt snapshot is the latest persisted ticket state and that no newer comments, relations, or closure evidence reopened scope after the snapshot.
- Because the branch is unchanged from scratch ref `e800be1226efdbdf022f403b5c9ed9c888a58b49`, this review assumes the next role is expected to supply the actual implementation-or-closure decision work rather than rely on unpublished branch evidence.

AC / test suggestions
- Keep the existing acceptance wording that a no-work-required outcome must leave benchmark/provider-evidence rows truthful and must not turn skipped-placeholder or PIT/bridge evidence into a DB2 latest-satellite optimization claim.
- If the implementation branch is chosen, preserve the explicit supported-envelope wording already in the contract: hub-parent satellites, current/as-of latest reads, no multi-active driving-key support, and provider-neutral fallback for unsupported shapes or missing evidence.

Implementation watchouts
- Do not treat DB2 PIT/bridge strategy registration or smoke coverage as proof of DB2 latest-satellite optimization; repository evidence currently separates those surfaces.
- Do not claim DB2 latest-satellite optimization unless the repository also shows all three together: DB2 latest-satellite strategy registration, diagnostics selecting that strategy for supported shapes, and updated benchmark/evidence surfaces.
- If the ticket closes as no-work-required, keep the authoritative closure language on the provider-neutral boundary instead of widening product docs, release notes, or benchmark claims.

Non-blocking notes
- `git log --oneline --decorate -n 5 HEAD` shows the expected ticket ref `ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap`; `git rev-parse --abbrev-ref HEAD` returned `HEAD`, so the review was performed from a detached worktree pointing at the correct ticket branch.
- No branch diff is present yet. For this pre-development gate that is a developer-handoff watchout, not a PO blocker, because the delivery contract is already bounded and `## Open Questions` is `none`.

Split recommendations
- No split recommended. Repository and ticket evidence keep this as one bounded capability-decision ticket with either an implementation branch or an authoritative no-work-required closure branch.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment