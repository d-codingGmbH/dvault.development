[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F2PGQQJB5FJGDB16M2G7CPCM/comments/06F46GK15QKX6RCPV30YG1K6QC.md` records PO handoff `decision: ready_for_po_critic`, says `Open questions - none`, and scopes the work to documentation-only updates across `docs/releases/v0.16.0.md`, `README.md`, `examples/README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/model-first-governance.md`, and `docs/production-adoption-checklist.md`.
- `git diff --name-only develop..HEAD` lists only `.gicket/tickets/06F2PGQQJB5FJGDB16M2G7CPCM/...` files, so the branch currently contains ticket-state updates and no repository documentation edits yet.
- Repository source directly supports the documented behavior: `src/DCoding.Data.DVault/DataVaultTelemetryServiceCollectionExtensions.cs` exposes `AddDVaultTelemetry()`, `src/DCoding.Data.DVault/IDataVaultTelemetryObserver.cs` plus `DataVaultSaveTelemetrySummary.cs` and `DataVaultReadTelemetrySummary.cs` exist, `src/DCoding.Data.DVault/DataVaultSupportBundle.cs` sets `CurrentSchemaVersion = "dvault.support-bundle.v1"`, and `src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs` includes the `support-bundle` verb.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md` documents the consumer-owned `support-bundle` command host workflow and states the default bundle serializes under the `dvault.support-bundle.v1` contract without opening a live database connection.
- Current docs still show the stale baseline the ticket is supposed to refresh: `README.md`, `examples/README.md`, and `src/DCoding.Data.DVault.Analyzers/README.md` still reference version `0.15.0`; `README.md` still contains `## v0.15.0 Release Notes` and `## Current v0.15.0 Limitations`; `docs/model-first-governance.md` still says `Status: v0.15.0 public guidance`.
- `rg -n "telemetry|support-bundle|v0\.16\.0" docs/production-adoption-checklist.md` returned no matches, which matches the contract's need to refresh current operational guidance there.
- `docs/releases/v0.16.0.md` already exists, but `rg -n "support-bundle|Compatibility|Validation|Known Limitations|Documentation Updates" docs/releases/v0.16.0.md` returned no matches, matching the contract note that the current file covers telemetry only and still needs the support-bundle and release-record sections.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract intentionally leaves operator-facing troubleshooting examples out of scope; if the developer is tempted to add example metric dashboards, telemetry export wiring, or support-bundle distribution workflows, those should remain follow-up work rather than widening this ticket.
- The contract does not require a new top-level runnable example for the `support-bundle` verb; the existing architecture guide is the documented source of truth, so the implementation should link or summarize that workflow without inventing new CLI surface claims.

Risky assumptions
- The developer will need to keep every v0.16.0 doc claim pinned to existing source-backed behavior only; the ticket assumes no new telemetry backend guidance, automatic instrumentation, or standalone DVault tooling is introduced while updating the docs.
- Validation evidence for this doc-only ticket is expected to come from repository inspection, consistency checks, and existing validation commands rather than new automation; that assumption should be preserved in the completion note.

AC / test suggestions
- When the ticket is completed, cite the exact changed doc paths in the completion evidence: `docs/releases/v0.16.0.md`, `README.md`, `examples/README.md`, `src/DCoding.Data.DVault.Analyzers/README.md`, `docs/model-first-governance.md`, and `docs/production-adoption-checklist.md`.
- Use repository-backed validation wording that references the existing command baseline from `docs/production-adoption-checklist.md`: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh`, or explicitly state why only formatting and repository inspection were applicable for a documentation-only change.

Implementation watchouts
- Keep the public operations boundary consistent with source evidence: `AddDVault()` remains telemetry-free by default, telemetry is explicit opt-in through `AddDVaultTelemetry()`, and `support-bundle` is consumer-invoked from the design-time command host.
- Do not let the doc refresh imply a standalone DVault CLI, automatic schema orchestration, automatic observability wiring, or live-database access in the default support-bundle path; the repository documentation explicitly rejects those claims.
- Because `git diff --name-only develop..HEAD` currently shows only `.gicket` changes, the developer work should stay bounded to the documentation files named in the contract and avoid code, release publication, or relation-management changes.

Non-blocking notes
- The persisted contract already says `No split recommended`, and the repository evidence still supports one bounded documentation rollout rather than child tickets.

Split recommendations
- No split recommended. The remaining work is a bounded cross-documentation refresh over existing files and one existing release-note document.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment