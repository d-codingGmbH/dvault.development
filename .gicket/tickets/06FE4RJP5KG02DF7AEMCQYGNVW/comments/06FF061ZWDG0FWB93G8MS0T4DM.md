[gicket-bot] PO refinement contract

Summary
- Refined the PostgreSQL PIT rebuild prototype as a full-rebuild-only, AddDVaultPostgres/Npgsql-gated provider-library path that preserves IDataVaultPitMaintenanceService semantics, requires bounded gate-and-fallback behavior, and leaves documentation follow-through to 06FE4RKGASKV6F7DF0RD1WTAV4. No description, relation, attachment, child-ticket, or planning-document write was applied in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The caller-visible API stays IDataVaultPitMaintenanceService; this ticket is about a provider-owned internal PostgreSQL prototype path, not a new application entrypoint.
- The prototype is full-rebuild-only and must not broaden to MaintainParentsAsync just because targeted delete-and-insert logic already exists in the provider-neutral service.
- The supported PIT baseline is the repo-backed v1 PIT shape set already accepted by maintenance and PIT-read tests: hub-parent ordinary PITs, hub-parent shared-driving-key multi-active PITs, and link-parent non-multi-active PITs.
- Runtime selection must never be broader than the approved dry-run contract: if the PostgreSQL candidate cannot be selected or proven for a request, provider-neutral rebuild remains the runtime default.
- The rollout remains bounded to the existing PostgreSQL and SQL Server PIT rebuild prototype tickets; other providers stay provider-neutral fallback unless later tickets add explicit proof.

Scope In
- Prototype a PostgreSQL-owned full PIT rebuild INSERT SELECT candidate path for IDataVaultPitMaintenanceService.RebuildAsync.
- Add the bounded PostgreSQL gate-and-diagnostic evaluation needed to select or decline that provider path consistently with the approved dry-run contract.
- Keep execution scoped to the repo-backed PIT baseline already proven elsewhere: hub-parent ordinary PITs, hub-parent shared-driving-key multi-active PITs, and link-parent non-multi-active PITs.
- Preserve explicit caller-owned maintenance semantics, provider-neutral fallback as the default, result-count parity, and redacted tracing and diagnostics behavior.
- Add repo evidence through unit coverage plus opt-in external PostgreSQL integration coverage using the existing Npgsql test harness.

Scope Out
- No MaintainParentsAsync push-down or parent-targeted incremental maintenance path.
- No bridge maintenance push-down, bridge feasibility work, or bridge runtime commitment.
- No new public maintenance entrypoint, no standalone maintenance diagnostics or export artifact lane, and no cross-provider deployment or runtime platform.
- No automatic maintenance on reads, saves, SaveChanges, startup, migrations, or background scheduling.
- No non-PostgreSQL provider implementation in this ticket; SQL Server remains in 06FE4RJZ4PA0DZ3HXDSEG2BQMM and other providers remain provider-neutral fallback.

Open questions
- none

Follow-up questions
- After the PostgreSQL and SQL Server prototypes land, should provider-specific PIT rebuild selection stay prototype-local in provider packages, or should the repo promote a shared maintenance-strategy seam in core?
- If one repo-backed PIT baseline shape materially diverges in SQL complexity or proof burden, should it get a follow-up ticket instead of widening this prototype mid-stream?
- Should 06FE4RKGASKV6F7DF0RD1WTAV4 publish any provider-specific performance or profile language only after both PIT prototype tickets have local evidence?

Risks
- Because provider packages currently have no PIT maintenance strategy registration seam, this ticket can sprawl into shared-core API design unless the work stays internal and provider-owned.
- If execution gating drifts from the approved dry-run contract, PostgreSQL behavior may silently diverge from fallback expectations or from the forthcoming SQL Server sibling.
- If the prototype grows beyond full rebuild into parent maintenance or bridge maintenance, it will reopen scope the parent story deliberately split out.
- External PostgreSQL integration coverage is opt-in through the existing environment-based harness; without that proof, provider-path regressions may survive unit-only validation.

Split recommendations
- Keep the existing decomposition unchanged: 06FE4RJD5Z6MWC2E66YB3EZ5YW for dry-run contract context, this ticket for the PostgreSQL prototype, 06FE4RJZ4PA0DZ3HXDSEG2BQMM for SQL Server, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, and 06FE4RKGASKV6F7DF0RD1WTAV4 for documentation follow-through.
- If implementation evidence shows one supported PIT baseline shape cannot safely share the same PostgreSQL INSERT SELECT prototype, create a shape-specific follow-up ticket instead of reopening PO scope here.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment