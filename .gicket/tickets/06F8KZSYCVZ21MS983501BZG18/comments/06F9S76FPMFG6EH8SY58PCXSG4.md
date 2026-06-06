[gicket-bot] PO-critic review contract

Summary
- Ticket scope is developer-ready: the contract is explicit, `## Open Questions` is `none`, source material is already landed, and the remaining work is a bounded release-note plus baseline-pointer alignment.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F8KZSYCVZ21MS983501BZG18/description.md` lines 20-52 scope the work to adding `docs/releases/v0.31.0.md`, linking existing sources, and aligning baseline pointers; lines 54-55 show `## Open Questions` -> `- none`.
- `docs/releases/v0.31.0.md` is currently missing and `docs/README.md` is missing (`test -e` check returned `docs/releases/v0.31.0.md: missing` and `docs/README.md: missing`).
- `docs/performance-profiles.md` lines 3-15 already state `Status: v0.31.0 decision-tree contract and adopter guidance` and anchor the work to `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`.
- `examples/README.md` lines 18-23 already document the realistic customer-profile scenario, sanitized diagnostics, and observability posture; `examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs` lines 19-27 and 138-170 define the same `crm-import`/`crm-change` record sources, `<redacted>-29T10:15:00Z` and `<redacted>-29T11:30:00Z` timestamps, latest/as-of reads, and bounded diagnostics output.
- `README.md` lines 10-25 still use `0.30.0` package examples and name `docs/releases/v0.30.0.md` as the current coordinated release baseline; `docs/production-adoption-checklist.md` line 9 still says `v0.29.0` is the current public baseline; `examples/README.md` lines 29-36 still show `0.30.0` package commands.
- Related ticket snapshots show `06F8KZRSTHAGSP6GPGFBFQGY08`, `06F8KZSCGZBKAC4YZH5SY3NX68`, and `06F8KZSNDXXEEHF53HN14QFK14` are `done`, while `06F8KZTNG44XDPMVTVCV4WJSHG` remains `todo` as the future provider-specific SQL artifact lane named in the contract.
- `git diff --name-only develop...HEAD -- . ':(exclude).gicket/**'` returned no output, so the ticket branch currently has no non-`.gicket` repository changes yet; this matches a pre-development handoff state rather than an already-implemented doc change.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The contract assumes only `README.md`, `docs/production-adoption-checklist.md`, and any intentionally touched example version text need current-baseline alignment; developers should not widen the ticket into a repo-wide version sweep.
- The live relation to `06F8KZTNG44XDPMVTVCV4WJSHG` must stay a forward-boundary mention only; the ticket should not assume the v0.32 provider-specific SQL artifact contract is available for specification in v0.31.

AC / test suggestions
- Verify the landed release note links back to `docs/performance-profiles.md`, `examples/README.md`, the root benchmark triplet, and the observability contract surfaces instead of duplicating them.
- Verify any baseline-pointer updates remove the current-baseline split across `README.md`, `docs/production-adoption-checklist.md`, and any touched version-example text in `examples/README.md`.

Implementation watchouts
- Keep observability wording bounded to the already-landed contracts: `AddDVault()` remains telemetry-free by default, tracing is listener-driven, and exporters/collectors/dashboards/hosting stay application-owned.
- Do not invent a `docs/README.md` edit surface; the contract explicitly says that path does not exist on this branch.
- Keep the v0.32 provider-specific SQL artifact lane as a short non-goal boundary and do not turn it into runtime routing, artifact workflow specification, or package-publication language.

Non-blocking notes
- The PO refinement comment `06F9S5A9Q459MPQZXKYK4PXBBG.md` already marks the ticket `ready_for_po_critic` and matches the persisted delivery contract.

Split recommendations
- No split is needed; the remaining work is still one coordinated v0.31.0 release note plus small baseline-link adjustments.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment