[gicket-bot] PO-critic review contract

Summary
- PO refinement resolved the earlier closure-only routing problem; the persisted contract is now a clear pre-development documentation handoff for dev with no open questions and concrete repository-backed evidence sources.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/comments/06FEPJEASBRPC3XBMQ07PGD01R.md` shows the earlier PO-critic decision was `return_to_po` specifically because closure-only routing was invalid while `docs/releases/v0.43.0.md` and a `CHANGELOG.md` v0.43.0 entry were still missing.
- `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/comments/06FEPMKTCAVWD2VD22AVMA3B8M.md` answers critic-item-1 through critic-item-6 and explicitly re-routes the ticket to a normal pre-development documentation task for `dev` rather than closure-only handling.
- `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/description.md` states `## Open Questions` is `- none`, keeps scope to documentation-only work, and lists the exact touched surfaces plus artifact paths to cite.
- Repository inspection confirms the deliverables are still dev work: `docs/releases/v0.43.0.md` is missing, `CHANGELOG.md` still starts with `## v0.42.0 - Provider Performance Evidence and Tuning`, and `README.md`, `docs/production-adoption-checklist.md`, `docs/performance-profiles.md`, `docs/package-compatibility.md`, `docs/manual-nuget-publication.md`, `docs/local-validation.md`, and `src/DCoding.Data.DVault.Analyzers/README.md` still carry v0.42.0 / 8.42.0 / 10.42.0 baseline text.
- The evidence sources named in the contract exist locally: `docs/hash-key-storage-migration.md`, `artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-<redacted>/`, `artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-<redacted>/`, and `artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-<redacted>/`.
- `git diff --name-only develop...HEAD` currently shows only `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/...` metadata/comment files; because the contract now explicitly frames this as pre-development work, the absence of doc implementation is handoff context rather than a PO blocker.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The ticket assumes one shared v0.43 baseline note is sufficient and that any provider-specific binary-storage caveats can stay out of scope for now; `description.md` records that as a follow-up question rather than settled scope.
- The ticket assumes analyzer guidance must remain on the current `.NET 10 SDK` build-host baseline with one `net10.0` analyzer asset; any future pure `.NET 8 SDK` compatibility claim still needs a separate ticket instead of being broadened implicitly here.

AC / test suggestions
- Close the ticket only when the repo diff actually contains `docs/releases/v0.43.0.md`, a `CHANGELOG.md` v0.43.0 entry, and the coordinated current-baseline doc updates named in `.gicket/tickets/06FE4R2EGQ444EGPKZBRZCDEV8/description.md`.
- Verify the final docs cite the exact checked-in artifact labels under `artifacts/benchmarks/06FE4R1N2ADN77NDFDP4GR7020-provider-hash-key-matrix-<redacted>/`, `artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-<redacted>/`, and `artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-<redacted>/`, while preserving skipped/failed/diagnostics-only/storage-footprint caveats.
- During dev review, explicitly confirm that binary-first guidance still routes existing persisted `HexString` adopters to `docs/hash-key-storage-migration.md` and the dry-run manifest lane instead of implying automatic migration.

Implementation watchouts
- Current public docs still advertise the v0.42.0 baseline across README, adoption, performance, package, publication, validation, and analyzer surfaces; dev needs one coherent v0.43 update rather than a mixed baseline.
- Keep the public hash-key contract on lowercase hexadecimal strings even when physical storage is `Binary`, and keep post-persistence storage or algorithm changes caller-owned migration work.
- Keep analyzer guidance bounded to project-local `PrivateAssets="all"`, one `net10.0` analyzer asset, and a `.NET 10 SDK` build-host baseline; do not imply runtime guards, provider lifecycle guarantees, or pure `.NET 8 SDK` analyzer-host support.
- Do not widen this lane into runtime, analyzer, benchmark-harness, provider implementation, or package-publication work; those are explicitly scope-out items in the persisted contract.

Non-blocking notes
- `git rev-parse HEAD` and the provided scratch source ref both resolve to `bf2889e45e7357e673f4c56df2a7ad4b9644524c`, so there is no additional repository change since this PO-critic claim began.

Split recommendations
- No split is needed; the ticket is already a bounded v0.43 documentation consolidation lane with concrete evidence sources and explicit non-goals.
- If provider-specific binary-storage caveats later need materially different adopter guidance, capture that as a separate post-v0.43 documentation ticket instead of widening this shared baseline update.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment