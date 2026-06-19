[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the persisted contract is bounded, `## Open Questions` is `none`, and repository evidence confirms the exact SQL Server PIT/bridge documentation inconsistency the ticket describes.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket contract for `06FBSCGNY2R6PC7P4Y91RD0HVR` sets `PO Handoff` to `ready_for_po_critic`, keeps `## Open Questions` as `none`, and scopes the work to documentation/evidence closure rather than new product code or API work.
- `git rev-parse --abbrev-ref HEAD` returned `ticket/06FBSCGNY2R6PC7P4Y91RD0HVR-task-close-sql-server-pit-and-bridge-read-gaps`; `git rev-list --left-right --count 15bc86cbac9bb60bf174972923d519d5bb6fc2d7...HEAD` returned `0 0`; `git log --oneline --max-count=3 HEAD` shows only PO->PO-critic handoff commits (`cb621a806`, `dd143dada`, `15bc86cba`).
- `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-<redacted>/benchmark-summary.md:69-70` contains completed SQL Server `pit-as-of-read` and `bridge-traversal-read` rows with non-empty timings and `selectedStrategy=SqlServerDataVaultReadStrategy`.
- `benchmark-summary.md:79-80` still shows the root quick-triplet SQL Server `pit-as-of-read` and `bridge-traversal-read` rows as `skipped` because `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` is unset.
- `docs/plans/provider-optimization-evidence-matrix.md:259-261` still marks SQL Server latest-satellite/PIT/bridge rows as `skipped-placeholder` from the root benchmark triplet, including PIT/bridge guidance rows for `SqlServerDataVaultReadStrategy`.
- `docs/plans/provider-optimization-gap-matrix.md:62` and `docs/plans/provider-optimization-gap-matrix.md:67` still publish `P2.02` and `P3.02` as SQL Server evidence gaps and say no completed SQL Server PIT/bridge timing claim is available.
- `docs/performance-profiles.md:17` still says SQL Server `pit-as-of-read` and `bridge-traversal-read` remain evidence-gap recommendations until provider-configured benchmark triplets exist.
- `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:24-26` directly registers `SqlServerDataVaultReadStrategy` for general read, PIT read, and bridge read; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs` includes fallback-closed PIT/bridge coverage for unsupported shapes, incomplete read-shape evidence, and stale-maintenance signals.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The ticket assumes the preserved v0.32 external artifact triplet is acceptable `completed-timing` evidence under the shared benchmark artifact contract and that closure does not require the root quick-triplet SQL Server rows themselves to become completed.
- The ticket assumes developers will treat every still-contradictory documentation surface as in scope, including `docs/performance-profiles.md`, even though the implementation notes call out the two plan matrices most directly.

AC / test suggestions
- Make the acceptance proof explicit that `docs/performance-profiles.md` must stop describing SQL Server PIT/bridge as open evidence gaps if that wording remains after the docs update.
- Add a reviewer check that searches `docs/plans/` and `docs/performance-profiles.md` for `SQL Server external provider`, `pit-as-of-read`, `bridge-traversal-read`, `P2.02`, and `P3.02` to confirm no remaining text says completed SQL Server PIT/bridge timing evidence is unavailable.

Implementation watchouts
- Do not turn the root `benchmark-summary.*` SQL Server skipped placeholders into completed timing claims; cite the checked-in v0.32 artifact triplet as the authoritative completed evidence surface.
- Keep SQL Server `latest-satellite-read` out of scope; the ticket contract leaves that row as a separate follow-up evidence gap.
- Preserve the documented read boundary: explicit PIT/bridge maintenance, incomplete read-shape fallback, stale-maintenance fallback, unsupported-shape fallback, and no new public read API.

Non-blocking notes
- The current branch is still pre-development handoff state rather than implementation state; that is consistent with PO-critic timing and is not a blocker.
- Prompt snapshot shows no recent comments and no closure evidence amendments, so the decision rests on the persisted delivery contract plus repository and branch evidence.

Split recommendations
- No split recommended; SQL Server PIT and bridge closure share one provider, one artifact bundle, and one documentation-consistency problem, while `latest-satellite-read` remains a separate follow-up.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment