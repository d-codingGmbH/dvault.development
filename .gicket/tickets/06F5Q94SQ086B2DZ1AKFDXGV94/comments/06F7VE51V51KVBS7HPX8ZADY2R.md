[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the ticket is a bounded docs-consolidation pass with no open questions, completed upstream dependencies, and direct repo evidence for the tracing and performance sources it must align.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q94SQ086B2DZ1AKFDXGV94/description.md` contains the authoritative Delivery Contract, `PO Handoff` = `ready_for_po_critic`, and `## Open Questions` = `- none`.
- `docs/performance-profiles.md:3-25` already declares `Status: v0.23.0 adopter guidance` and ties the guide to `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`; `benchmark-summary.md:5-25` records the checked-in run context as `3` iterations, `1` warmup, `ProviderDefault`, required provider `SQLite local temporary files`, and optional external-provider rows skipped when connection-string env vars are unset.
- `docs/architecture/dvault-v1-activity-tracing-contract.md:10-24,286-303` defines ActivitySource `DCoding.Data.DVault`, listener-driven tracing, sibling telemetry surfaces (`AddDVaultTelemetry()`, Metrics, `IDataVaultTelemetryObserver`), and the redaction boundary the public docs must preserve.
- `README.md:10-16,744-758` and `docs/production-adoption-checklist.md:9` still present v0.22.0 as the current public baseline, `docs/releases` currently has no `v0.23.0.md`, and `git diff --name-only e67eee4c083e8f1702df64e1e3312752f5c425cc..HEAD` shows only `.gicket/tickets/06F5Q94SQ086B2DZ1AKFDXGV94/**` changes so far.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A short adopter-facing example showing listener-driven Activity tracing with `AddDVault()` plus an application-owned `ActivityListener` or OpenTelemetry provider, without `AddDVaultTelemetry()`, would reduce confusion during the docs pass.
- The v0.23.0 release note should explicitly mention that PostgreSQL, SQL Server, MySQL, and Oracle benchmark rows are present in the root artifacts but currently `executionStatus=skipped` when the corresponding connection-string env vars are unset.

Risky assumptions
- The docs pass assumes developers will reuse the exact names and redaction wording from `docs/architecture/dvault-v1-activity-tracing-contract.md` instead of paraphrasing the closed span/event vocabulary.
- The docs pass assumes any performance prose stays bounded to the checked-in root benchmark triplet and does not imply measured external-provider wins while optional-provider rows remain skipped.
- The docs pass assumes manual touched-link and anchor review will be performed because no dedicated markdown/link checker is visible in repo automation.

AC / test suggestions
- Use the contract's recorded validation set: `dotnet build DVault.slnx --nologo`, `dotnet test DVault.slnx --nologo`, and `bash tools/check-format.sh`, plus manual verification of every touched doc link and anchor.
- During review, check that `README.md`, `docs/production-adoption-checklist.md`, `docs/performance-profiles.md`, and the new `docs/releases/v0.23.0.md` all present v0.23.0 as current and older releases as historical.
- Validate tracing prose against `docs/architecture/dvault-v1-activity-tracing-contract.md` and performance claims against `benchmark-summary.md`, `benchmark-summary.json`, and `docs/plans/performance-evidence-benchmark-artifact-contract.md`.

Implementation watchouts
- This is still a pre-development branch: `git diff --name-only e67eee4c083e8f1702df64e1e3312752f5c425cc..HEAD` shows only ticket metadata changes, so the developer must create all requested README, checklist, and release-note edits.
- `README.md:247-258` currently documents Metrics and `IDataVaultTelemetryObserver` but not listener-driven Activity tracing; the new prose must make tracing a sibling opt-in surface, not a telemetry prerequisite.
- `docs/releases/v0.23.0.md` does not exist yet, so the developer will need to carry forward compatibility notes and manual-publication caveats from the v0.22.0 release-note structure without implying package publication.

Non-blocking notes
- `docs/performance-profiles.md` already exists as the detailed v0.23.0 guide, so this ticket is a consolidation task rather than a wait-state for product-code work.
- `.gicket/tickets/06F5Q94SQ086B2DZ1AKFDXGV94/ticket.json` currently has no assignees; assign ownership if your workflow requires it, but that is not a PO blocker.

Split recommendations
- No split recommended; repository state already separates the tracing contract story, tracing implementation stories, performance-profile story, and this final documentation-consolidation task.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment