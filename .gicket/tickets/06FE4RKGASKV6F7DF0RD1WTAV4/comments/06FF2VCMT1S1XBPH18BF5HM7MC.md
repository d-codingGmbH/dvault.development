[gicket-bot] PO refinement contract

Summary
- Refined the docs follow-through ticket around the current branch's two PIT maintenance prototypes, deferred bridge maintenance push-down, and the missing v0.45 release-note/changelog surfaces.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current branch evidence fixes this as the documentation follow-through ticket for release v0.45.0 - Server-Side PIT and Bridge Maintenance Exploration; docs/releases/v0.45.0.md is not present yet, while the ticket release metadata already names that release.
- The accepted PIT maintenance baseline is asymmetric and must be documented exactly as implemented: AddDVaultPostgres() adds IDataVaultProviderPitMaintenanceStrategy via PostgresDataVaultPitMaintenanceStrategy, while AddDVaultSqlServer() replaces IDataVaultPitMaintenanceService with SqlServerDataVaultPitMaintenanceService.
- PostgreSQL full PIT rebuild scope is the supported repository baseline from PostgresProviderCapabilityTests and PostgresPitMaintenanceServiceTests: ordinary hub-parent PITs, shared-driving-key multi-active hub-parent PITs, and link-parent non-multi-active PITs.
- SQL Server full PIT rebuild scope stays narrower: clean SQL Server RebuildAsync only for ordinary hub-parent PITs, with MaintainParentsAsync, multi-active PITs, and link-parent PITs falling back to provider-neutral maintenance and fault/cancellation rolling back to pre-rebuild rows.
- Bridge maintenance remains provider-neutral IDataVaultBridgeMaintenanceService; no bridge-provider maintenance seam is present, and maintained-bridge read evidence must stay separate from write-side bridge push-down claims.
- No ticket description update, relation change, attachment, child ticket, or planning-document write was applied in this refinement pass.

Scope In
- Update docs/architecture/dvault-v1-pit-bridge-boundary.md so the authoritative boundary matches the current branch's PIT maintenance implementations, exact fallback posture, and deferred bridge push-down boundary.
- Update docs/performance-profiles.md so it separates benchmark-backed read evidence from PIT maintenance prototype availability and keeps bridge write-side push-down explicitly deferred.
- Add docs/releases/v0.45.0.md and a matching top CHANGELOG.md entry for the server-side PIT and bridge maintenance exploration outcome.
- Cite the current source and test evidence for PostgreSQL PIT maintenance, SQL Server PIT maintenance, and bridge maintenance semantics instead of restating the older exploration plan generically.

Scope Out
- No provider-specific bridge maintenance implementation, no bridge-maintenance benchmark lane, and no bridge runtime commitment beyond the current defer posture.
- No new PIT maintenance prototype/provider beyond the current PostgreSQL and SQL Server branch evidence.
- No automatic PIT or bridge maintenance, deployment artifacts, background scheduling, SaveChanges interception, or support-bundle/runtime platform broadening.
- No README, package-compatibility, manual-publication, local-validation, or package-verifier version-line sweep unless a separate release-alignment ticket is opened.

Open questions
- none

Follow-up questions
- After the v0.45 documentation lands, should a separate ticket add provider-configured PIT maintenance benchmark/artifact lanes before any public performance claim is promoted?
- If release management wants visible consumer install/version guidance to move from the current v0.44.0 baseline to v0.45.0, should that broader README/package-compatibility/manual-publication sweep stay separate from this bounded push-down documentation ticket?

Risks
- Until this ticket lands, docs/architecture/dvault-v1-pit-bridge-boundary.md and docs/performance-profiles.md understate the current SQL Server PIT prototype and can mislead readers with stale PostgreSQL-only maintenance wording.
- If the v0.45 release note blurs architecture/test evidence with benchmark evidence, it will overstate PIT maintenance push-down as a measured performance win that the current branch does not preserve.
- Broader repository install/version guidance still points at the v0.44.0 baseline; if this ticket is widened informally, it can turn into a full release-version alignment sweep instead of a bounded exploration-doc update.

Split recommendations
- Keep the current decomposition unchanged: 06FE4RJD5Z6MWC2E66YB3EZ5YW for dry-run diagnostics context, 06FE4RJP5KG02DF7AEMCQYGNVW for PostgreSQL PIT rebuild, 06FE4RJZ4PA0DZ3HXDSEG2BQMM for SQL Server PIT rebuild, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, and this ticket for documentation follow-through.
- If the work expands into README, package-compatibility, manual-publication, local-validation, or package-verifier updates for 8.45.0 / 10.45.0, split that into a separate release-alignment ticket instead of broadening this bounded architecture/performance/release-note task.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment