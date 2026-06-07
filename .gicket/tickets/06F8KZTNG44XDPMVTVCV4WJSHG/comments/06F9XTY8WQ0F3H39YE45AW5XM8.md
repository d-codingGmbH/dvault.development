[gicket-bot] PO-critic review contract

Summary
- Contract is durably materialized and internally consistent: the planning doc exists, README indexing is committed in branch history, the persisted contract has no open questions, and the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/description.md:31-52` defines the acceptance boundary and shows `## Open Questions` as `- none`, so the persisted delivery contract has no unresolved open questions.
- `docs/plans/provider-specific-sql-artifact-contract.md:8-146` defines a design-time-only `dvault.sql-artifact.v1` manifest contract, consumer-owned deployment/invocation/rollback/cleanup, an evidence gate, and explicit non-goals for runtime dispatch and automatic migration sync.
- `docs/plans/README.md:5-24` currently lists `provider-specific-sql-artifact-contract.md`, and `git show 81a4026fa:docs/plans/README.md | rg -n "provider-specific-sql-artifact-contract.md"` returns line `22`, confirming the index entry is committed in branch history.
- `docs/architecture/dvault-dotnet-ef-design-time-workflow.md:8-32` matches the ticket boundary: single-project, consumer-owned `IDesignTimeDbContextFactory<TContext>` and no standalone EF CLI shim.
- `docs/performance-profiles.md:291-309` and `docs/architecture/dvault-v1-explicit-save-service.md:54-60` match the ticket boundary: provider-specific SQL artifacts are opt-in design-time outputs only, with consumer-owned deployment/invocation and prerequisite diagnostics plus benchmark evidence.
- `git diff --name-only develop...HEAD | rg -v '^\.gicket/'` returns only `docs/plans/README.md` and `docs/plans/provider-specific-sql-artifact-contract.md`, which matches the DoD claim that this ticket did not introduce product-code or runtime-dispatch changes.
- The earlier PO-critic blocker in `.gicket/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/comments/06F9XKM3EDWH8NXVZ2BFGEYF80.md:3-28` is explicitly superseded by `.gicket/tickets/06F8KZTNG44XDPMVTVCV4WJSHG/comments/06F9XRKRXS79WDRWFDRA4W7N2W.md:10-23`, and current HEAD state matches the later resolution.
- `.gicket/tickets/06F8KZVCVRPS3NAGQA7J55EAA4/ticket.json`, `.gicket/tickets/06F8KZV18BQ0GN3CE4G02ATVA0/ticket.json`, and `.gicket/tickets/06F8KZVRARQPG482YKCQ686PNM/ticket.json` exist with the expected evidence/prototype/documentation titles, matching the parent contract's three-way split.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The parent contract intentionally leaves the first prototype provider unspecified; child story `06F8KZV18BQ0GN3CE4G02ATVA0` still needs one concrete provider/workload example.
- The parent contract intentionally defers the manifest and sidecar repository path convention; child work should supply a deterministic example before any non-dry-run artifact lane is proposed.

Risky assumptions
- Developer follow-up must treat the deferred provider choice, repository path convention, and any future deployable-payload decision as child-ticket work, not as already-approved scope in this parent.
- Any later implementation that widens this lane into runtime dispatch, automatic invocation, or automatic migration synchronization would violate the verified parent contract and should reopen PO review.

AC / test suggestions
- For child evidence/prototype work, keep acceptance checks tied to one exact provider and workload, request-bound diagnostics, the shared benchmark artifact triplet, and the semantic-parity checklist named in `docs/plans/provider-specific-sql-artifact-contract.md:104-112`.
- Verify prototype manifests stay deterministic: schemaVersion `dvault.sql-artifact.v1`, exact provider/profile binding, metadata fingerprint traceability, dry-run status, and no secrets or machine-specific paths.

Implementation watchouts
- Do not turn the dry-run prototype into a default runtime save/read path; the verified boundary remains `IDataVaultSaveService` and `IDataVaultReadService` plus opt-in design-time output only.
- Keep the workflow inside the existing single-project design-time host boundary; no standalone CLI, multi-project orchestration, or automatic deployment/cleanup.
- Preserve consumer-owned responsibilities for deployment, invocation, rollback, cleanup, credentials, environment routing, migration compatibility, and observability.

Non-blocking notes
- The live `blocks` relation to child story `06F8KZV18BQ0GN3CE4G02ATVA0` is present at `.gicket/relations/HG/A0/06F8KZTNG44XDPMVTVCV4WJSHG--06F8KZV18BQ0GN3CE4G02ATVA0--blocks.json`; the other split tickets are evidenced by the persisted contract text and their ticket files.

Split recommendations
- No new split is needed; the parent already separates architecture contract work from evidence (`06F8KZVCVRPS3NAGQA7J55EAA4`), dry-run prototype (`06F8KZV18BQ0GN3CE4G02ATVA0`), and documentation alignment (`06F8KZVRARQPG482YKCQ686PNM`).
- If later work wants deployable SQL payload emission, runtime invocation helpers, or provider-specific validators, keep those as separate follow-up tickets instead of widening this parent contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment