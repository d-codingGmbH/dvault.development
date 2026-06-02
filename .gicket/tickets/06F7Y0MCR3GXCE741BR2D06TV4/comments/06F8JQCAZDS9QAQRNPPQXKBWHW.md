[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the delivery contract is specific, grounded in existing repository evidence, and has no open questions; the remaining work is to consolidate that boundary into one authoritative documentation surface.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `/mnt/c/Projects/DVault/.gicket/tickets/06F7Y0MCR3GXCE741BR2D06TV4/description.md` contains `## Open Questions` -> `none` and acceptance criteria that explicitly require opt-in only, design-time outputs only, no auto runtime dispatch or migration synchronization, staged-provider comparison, and an authoritative future-ticket gate.
- `/mnt/c/Projects/DVault/docs/releases/v0.20.0.md` already has `## Stored Procedure Boundary` and `## Benchmark Evidence` sections that describe stored procedures as a non-default escape hatch and require provider evidence plus migration-synchronization, deployment, rollback, transaction, cleanup, and parity documentation.
- `/mnt/c/Projects/DVault/docs/production-adoption-checklist.md` repeats that stored procedures are only an explicit design-time/provider-specific escape hatch after provider evidence, migration synchronization rules, deployment ownership, rollback behavior, and cleanup expectations are documented.
- `/mnt/c/Projects/DVault/docs/performance-profiles.md` contains the `Staged provider ingestion` profile with diagnostics-gated and benchmark-backed optional-provider guidance, which matches the comparison baseline the contract tells the developer to reuse.
- `/mnt/c/Projects/DVault/src/DCoding.Data.DVault/DataVaultSaveService.cs` and `/mnt/c/Projects/DVault/src/DCoding.Data.DVault/IDataVaultReadService.cs` expose the current explicit save/read boundaries, and `rg -n -i "stored procedure|stored-procedure|createprocedure|executeprocedure|migration synchronization|auto-manage.*procedure" /mnt/c/Projects/DVault/src /mnt/c/Projects/DVault/tests` returned no source/test matches for a stored-procedure runtime surface.
- `git -C /mnt/c/Projects/DVault diff --name-only afb8f3eaa1a945f2df018ab3ac8a01173ab687dd...HEAD` returned no paths, and `git -C /mnt/c/Projects/DVault show --stat --oneline HEAD` shows the branch tip `afb8f3eaa` only touches `.gicket/...`; the documentation implementation has not started yet, which is expected at this gate.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract intentionally leaves the first provider/workload example for any future additive experiment unspecified; the future implementation ticket will need to name the exact provider, workload, diagnostics evidence, and benchmark triplet.
- The contract does not decide whether downstream ticket `06F7Y0NBHXQ6CK8R3AH4DEP9V4` needs a provider-specific worked example or only a generic reference to this gate; that can stay with downstream refinement.

Risky assumptions
- This handoff assumes the developer will turn the currently distributed guidance across release notes, checklist, and performance docs into one authoritative reference without leaving conflicting duplicate wording behind.
- This handoff assumes downstream consumers will treat the new boundary doc as normative and the older `v0.20.0` stored-procedure caveats as supporting history rather than a second competing source of truth.

AC / test suggestions
- When the documentation lands, verify it states all three hard negatives in one place: no default runtime execution path, no automatic deployment ownership, and no automatic migration/model synchronization.
- Have the final document explicitly cross-reference the staged-provider evidence baseline in `docs/performance-profiles.md` and the current public runtime boundaries in `DataVaultSaveService.cs` and `IDataVaultReadService.cs` so future tickets have one citation chain.

Implementation watchouts
- Relevant guidance already exists in `docs/releases/v0.20.0.md`, `docs/production-adoption-checklist.md`, and `docs/performance-profiles.md`; implementation should consolidate or clearly nominate authority instead of creating another competing boundary statement.
- For the generator/design-time contrast, the live baseline is `docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md` plus `src/DCoding.Data.DVault.Analyzers/README.md`; `docs/plans/typed-read-model-generator-contract.md` is superseded historical context and should not be treated as the authoritative current generator boundary.
- Because ticket `06F7Y0MCR3GXCE741BR2D06TV4` blocks `06F7Y0NBHXQ6CK8R3AH4DEP9V4`, the finished document needs an obvious stable path/title that downstream documentation work can reference without re-explaining the boundary.

Non-blocking notes
- Current branch tip `afb8f3eaa` is a PO-critic lease-claim metadata commit only; no repository documentation files are changed yet.
- The persisted ticket already reflects blocker clearance via `isBlocked: false`, even though the contract text still describes the historical blocked-by relation.

Split recommendations
- No split needed now; keep this ticket as the single generic artifact-boundary/evidence-gate document, then let downstream provider-documentation tickets cite it rather than duplicating the policy.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment