[gicket-bot] PO refinement contract

Summary
- Refined this as a tracking story for explicit PIT and bridge maintenance push-down boundaries: current repo evidence keeps maintenance caller-owned and provider-neutral by default, scopes initial push-down work to redacted dry-run diagnostics plus bounded PIT provider prototypes, and leaves bridge runtime commitment evidence-gated.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current baseline keeps IDataVaultPitMaintenanceService and IDataVaultBridgeMaintenanceService as explicit caller-invoked maintenance surfaces in AddDVault(), while provider packages currently register save and read strategies only; there is no existing provider-specific maintenance dispatch seam to treat as already approved.
- The finite provider baseline for any future maintenance push-down discussion stays the existing AddDVaultSqlite, AddDVaultPostgres, AddDVaultSqlServer, AddDVaultMySql, AddDVaultOracle, and AddDVaultDb2 family; this story does not reopen provider naming or add new providers.
- Any future server-side maintenance push-down must stay opt-in behind provider libraries and diagnostics, preserve the current explicit maintenance entry points, and fall back to the existing provider-neutral maintenance services when provider match, supported shape, or proof is missing.
- Dry-run maintenance diagnostics are evaluation-only: they may describe candidate translated target tables, selected or declined provider paths, and deterministic stop reasons, but they must not execute writes, emit raw SQL, expose request values, or imply deployment or runtime platform behavior.
- The existing save-artifact lane remains the comparison pattern for review-only design-time output: no standalone DVault CLI, no automatic migration or deployment sync, no background scheduler, no runtime artifact dispatch, and no automatic PIT or bridge refresh on save, read, or startup.
- The current context already contains bounded follow-on tickets for PIT dry-run diagnostics, bridge feasibility, PostgreSQL and SQL Server PIT rebuild prototypes, and architecture-doc updates; this parent story should track that decomposition rather than reopen the boundary.

Scope In
- Define the explicit boundary for provider-library-owned server-side PIT and bridge maintenance push-down while preserving caller-invoked IDataVaultPitMaintenanceService and IDataVaultBridgeMaintenanceService semantics.
- Define request-bound, redacted dry-run diagnostics for maintenance candidates, including selected or declined provider path, translated target identity, bounded supported-shape facts, and deterministic stop reasons.
- Define provider-neutral fallback rules when provider name, maintenance shape, diagnostics evidence, or provider capability is incompatible.
- Keep initial push-down exploration bounded to the PIT rebuild provider prototypes already present in current context for PostgreSQL and SQL Server.
- Require bridge maintenance push-down to stay evidence-gated through a separate feasibility decision instead of assuming bridge runtime implementation.

Scope Out
- No automatic PIT or bridge maintenance during reads, saves, EF SaveChanges, startup, or background scheduling.
- No standalone deployment or runtime platform, no stored-procedure or artifact deployment automation, and no default runtime dispatch of provider-generated SQL.
- No raw SQL, query-plan, credentials, hash-key, or request-value exposure in diagnostics, telemetry, attachments, or support-bundle outputs.
- No provider-wide performance guarantee or support claim beyond bounded provider tickets and preserved evidence.
- No bridge push-down implementation commitment for unsupported or not-yet-evidenced shapes, including delete-aware hierarchy repair beyond explicit rebuild semantics.

Open questions
- none

Follow-up questions
- After PIT prototype evidence lands, should the team standardize one shared provider-specific maintenance strategy seam in core, or keep provider-specific maintenance dispatch as a narrower PIT-only extension until bridge evidence exists?
- If bridge feasibility is positive, which bridge shapes are admitted first: <redacted> only, or hierarchy rebuilds that exclude delete-aware incremental repair?
- Should a later design-time artifact lane be introduced for maintenance candidates, or is request-bound dry-run diagnostics sufficient without a separate manifest contract?

Risks
- If the story blurs diagnostics, prototype, and runtime platform boundaries, downstream work may accidentally promise deployable SQL, stored procedures, or automatic dispatch that the repository explicitly excludes today.
- If bridge push-down is treated as pre-approved instead of evidence-gated, the team may overcommit to hierarchy and delete-aware semantics that current maintenance behavior does not support.
- Because provider packages currently expose save and read strategy seams but not maintenance strategy seams, implementation work may expand into shared-core API design unless the child tickets stay tightly bounded.
- This parent story now depends on the bounded child-ticket outcomes for concrete evidence and doc updates, so it should be treated as a tracking parent rather than direct implementation work.

Split recommendations
- Keep the current bounded decomposition visible in current context: 06FE4RJD5Z6MWC2E66YB3EZ5YW for PIT dry-run diagnostics, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, 06FE4RJP5KG02DF7AEMCQYGNVW for the PostgreSQL PIT rebuild prototype, 06FE4RJZ4PA0DZ3HXDSEG2BQMM for the SQL Server PIT rebuild prototype, and 06FE4RKGASKV6F7DF0RD1WTAV4 for documentation updates.
- Do not add a separate bridge implementation ticket until the feasibility task decides whether any bounded bridge push-down shape is worth carrying forward.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment