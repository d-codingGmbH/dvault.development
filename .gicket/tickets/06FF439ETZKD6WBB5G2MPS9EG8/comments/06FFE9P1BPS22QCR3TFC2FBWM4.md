[gicket-bot] PO-critic review contract

Summary
- Ticket 06FF439ETZKD6WBB5G2MPS9EG8 is a bounded pre-development docs-alignment task with a persisted delivery contract, no open questions, direct repository citation targets, and a clean branch scope, so it is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF439ETZKD6WBB5G2MPS9EG8/description.md:4-59` contains the active delivery contract, targets only `docs/performance-profiles.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md`, and records `## Open Questions` as `none` at lines 46-47.
- `git -C /mnt/c/Projects/DVault rev-parse --abbrev-ref HEAD` returned `ticket/06FF439ETZKD6WBB5G2MPS9EG8-task-separate-read-evidence-from-maintenance-evi`, `git rev-parse HEAD` returned `f414a5fcb483481e59806fadc63c85406ba771d8`, and `git diff --name-only $(git merge-base develop HEAD)..HEAD` listed only `.gicket/tickets/06FF439ETZKD6WBB5G2MPS9EG8/**`, so this branch is still a clean pre-development handoff surface rather than mixed implementation work.
- `docs/performance-profiles.md:65-78` already defines the v0.45.0 PIT maintenance prototype boundary as source/test evidence only, forbids citing maintenance work as benchmark-backed timing, and keeps bridge maintenance push-down deferred; `docs/performance-profiles.md:209-215` says PIT and bridge reads consume already-maintained rows; `docs/performance-profiles.md:463-495` contains the read timing tables that this ticket is intended to clarify.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md:12-14` states PIT and bridge are explicit read models consumed as already-maintained rows and that read paths do not perform maintenance; `docs/architecture/dvault-v1-pit-bridge-boundary.md:128-130` separately cites completed read timing rows and the evidence matrix as citation surfaces.
- `docs/releases/v0.45.0.md:64-68` already states that completed `pit-as-of-read` and `bridge-traversal-read` rows are read evidence over already-maintained rows, not proof that provider SQL maintained those rows, which gives the developer an existing citation surface instead of requiring new evidence creation.
- `docs/plans/provider-optimization-gap-matrix.md` already preserves the deferred bridge-maintenance and maintained-row boundary for PIT/bridge read evidence, matching the contract's requested citation posture.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- When the docs are updated, cover both the root quick SQLite read rows and the 2026-06-23 external-provider closure rows so the disclaimer is not attached to only one evidence lane.
- If wording mentions bridge maintenance defer posture, keep it valid for both many-to-many and hierarchy semantics, consistent with the existing gap-matrix language.
- Any clarification that touches `latest-satellite-read` should avoid implying latest-satellite reads depend on maintained PIT or bridge rows; that maintained-row prerequisite applies to PIT and bridge reads.

Risky assumptions
- The scope-in bullet at `description.md:18-20` groups `latest-satellite` with PIT/bridge read evidence; implementation must keep the maintained-row prerequisite scoped to PIT/bridge reads rather than implying latest-satellite uses maintained read-model rows.
- The ticket assumes the existing v0.45.0 release note, architecture note, performance guide, evidence matrix, and gap matrix are sufficient citation surfaces, so no new benchmark artifacts or taxonomy changes are needed.
- The ticket assumes only documentation alignment is needed because the branch diff from `develop` to `HEAD` contains ticket metadata only and no partial repo implementation to reconcile.

AC / test suggestions
- Review final wording against both `docs/performance-profiles.md` narrative sections and the `### Supporting Rows` tables so the read-versus-maintenance distinction is visible where readers consume timing claims.
- Verify every new citation points to already checked-in evidence such as `docs/releases/v0.45.0.md`, `docs/plans/provider-optimization-evidence-matrix.md`, `docs/plans/provider-optimization-gap-matrix.md`, or the architecture note, without adding fresh timing claims.
- Do a final contract check that no edited sentence turns completed `pit-as-of-read` or `bridge-traversal-read` rows into maintenance implementation proof or push-down approval.

Implementation watchouts
- Keep this ticket on documentation only; the contract explicitly scopes out product code, diagnostics, benchmarks, and provider registration changes.
- Do not widen bridge-maintenance wording beyond the existing deferred posture or reopen MySQL PIT maintenance implementation scope from this ticket.
- Use the existing v0.45.0 maintenance contract wording instead of inventing a parallel maintenance evidence taxonomy or a new timing table.

Non-blocking notes
- Current repository docs already contain most of the required boundary facts, so the expected dev work is a wording/alignment pass rather than new evidence design.
- The ticket is internally bounded to two live docs plus citations to existing repository evidence, which is appropriate for a single documentation task.

Split recommendations
- No split recommended; the persisted contract and current branch evidence bound this to one documentation-alignment task across `docs/performance-profiles.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md`.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment