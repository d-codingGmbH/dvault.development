[gicket-bot] PO refinement contract

Summary
- Refined this child ticket to a request-bound, redacted PIT rebuild dry-run diagnostics contract that keeps provider-neutral maintenance as the default and supplies the prerequisite fallback/stop-reason boundary for the PostgreSQL and SQL Server prototype tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This ticket covers evaluation-only dry-run diagnostics for PIT rebuild candidates; it does not cover bridge maintenance and does not widen runtime PIT maintenance behavior.
- The input baseline is the existing PIT maintenance v1 shape set already visible in repository evidence: unique DataVaultPitMetadata satellite references, hub-parent ordinary PITs, hub-parent shared-driving-key multi-active PITs, and link-parent non-multi-active PITs.
- The v0.45 rollout is bounded to PostgreSQL and SQL Server PIT rebuild prototypes; other providers may still be evaluated but must remain provider-neutral fallback unless a bounded candidate path is explicitly selected later.
- Dry-run diagnostics may report translated PIT target identity, bounded shape facts, selected or declined provider path, and deterministic stop reasons, but they must not execute writes, emit raw SQL, or expose request values.
- The done parent story 06FE4RJ4CC2YRVK0P98NBSXRKC already fixed the higher-level push-down boundary; this child only narrows the PIT rebuild diagnostics contract that blocks the PostgreSQL and SQL Server prototype tickets.

Scope In
- Define a request-bound dry-run diagnostics contract for one PIT rebuild candidate evaluation.
- Report either a selected provider-specific PIT rebuild candidate path or an explicit provider-neutral fallback outcome.
- Surface bounded facts needed by downstream PIT rebuild prototypes, including translated PIT target identity, parent kind, participating satellites, multi-active/shared-driving-key facts, and deterministic stop reasons.
- Keep the diagnostics contract aligned with the existing redacted request-bound diagnostics style and the explicit caller-owned PIT maintenance boundary.

Scope Out
- No provider-specific PIT rebuild execution, no database writes, and no runtime behavior change to RebuildAsync in this ticket.
- No MaintainParentsAsync push-down, no bridge maintenance push-down, and no bridge feasibility decision work.
- No raw SQL, query-plan export, standalone manifest/exporter, deployment workflow, or runtime SQL artifact dispatch.
- No automatic maintenance on save, read, EF SaveChanges, startup, or background scheduling.
- No new provider commitment beyond the existing provider family and the bounded PostgreSQL/SQL Server prototype rollout.

Open questions
- none

Follow-up questions
- After the PostgreSQL and SQL Server PIT rebuild prototypes land, should a shared maintenance-strategy seam move into core, or should provider-specific PIT rebuild dispatch stay narrower until more evidence exists?
- If the team later wants design-time persistence of maintenance candidate reviews, should it reuse request-bound diagnostics or add a separate review manifest contract?
- Which non-PostgreSQL/SQL Server provider, if any, earns a future PIT rebuild candidate ticket once evidence exists beyond the current v0.45 prototype rollout?

Risks
- If this child ticket drifts from diagnostics into runtime execution, it may accidentally promise provider-specific maintenance behavior that the current PIT boundary and release notes explicitly exclude.
- If stop reasons are not finite and machine-readable, the PostgreSQL and SQL Server prototype tickets may reintroduce ad hoc fallback prose and incompatible gating behavior.
- If the scope is not kept to full rebuild evaluation, implementation may spill into parent-maintenance or bridge push-down work before separate evidence and boundary decisions exist.

Split recommendations
- Keep the current decomposition unchanged: this ticket for PIT rebuild dry-run diagnostics, 06FE4RJP5KG02DF7AEMCQYGNVW for the PostgreSQL prototype, 06FE4RJZ4PA0DZ3HXDSEG2BQMM for the SQL Server prototype, 06FE4RK80ZXGCZ62CMSAYP164W for bridge feasibility, and 06FE4RKGASKV6F7DF0RD1WTAV4 for documentation follow-through.
- Do not split again unless a later ticket needs a separate manifest/export surface beyond request-bound diagnostics.

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