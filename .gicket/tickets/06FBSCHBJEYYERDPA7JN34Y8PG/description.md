<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the read-parity documentation task against the checked-in evidence matrix, gap matrix, benchmark artifacts, and local `.gicket` ticket state, and wrote a ticket-bound planning note at `docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md`. No child tickets, relation writes, attachments, or ticket-description mutations were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the current v1 read baseline: all provider packages now register latest-satellite read strategies, but only SQLite has completed-timing latest-satellite evidence in the root benchmark triplet.
- PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge timing is already completed and must be cited from the checked-in v0.32.0 smoke-read bundle rather than from skipped root quick-baseline rows.
- DB2 PIT/bridge stays in the defer/no-completed-timing lane: root rows are skipped placeholders and the remaining proof is diagnostics-only and smoke-only.
- No human comments or closure-evidence amendments add new blocker questions for this ticket.
- Created ticket-bound refinement note `docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md` to persist the verified documentation boundary.

### Scope In
- Update live documentation surfaces to align `docs/performance-profiles.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md`, and `docs/releases/v0.40.0.md` around the current provider-read evidence baseline.
- Document provider strategy availability separately from measured benchmark wins, especially for non-SQLite latest-satellite reads.
- Cite the evidence matrix and gap matrix as the row-level source of truth for scenario/provider/baseline/posture facts.
- Preserve the finite provider-neutral fallback and explicit PIT/bridge maintenance caveats already proved in code and architecture docs.

### Scope Out
- Rerunning benchmarks, changing benchmark schemas, or inventing new artifact lanes.
- Changing provider read code, supported read shapes, or adding new public read APIs.
- Promoting skipped-placeholder, diagnostics-only, or smoke-only rows into completed timing claims.
- Cleaning up the historical incoming `blocks` relations from done provider-specific tickets as part of this documentation ticket.

## Acceptance Criteria
- `docs/performance-profiles.md` clearly separates measured provider-read evidence from implemented-but-unmeasured latest-satellite strategy lanes and from DB2 defer-lane posture.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` stays aligned with current provider registrations, explicit PIT/bridge maintenance requirements, and finite fallback causes without implying automatic maintenance or new APIs.
- `docs/releases/v0.40.0.md` records the accepted read-parity posture without claiming benchmark reruns or completed timing beyond the checked-in evidence.
- PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge outcomes are documented as completed timing only through the preserved v0.32.0 smoke-read bundle and not through skipped root quick-baseline rows.
- PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite outcomes are documented as diagnostics-gated strategy or parity outcomes unless and until a provider-configured completed-timing lane exists.
- DB2 PIT and bridge remain explicitly documented as unmeasured/deferred and no doc claims completed DB2 timing.

## Definition of Done
- The three live documentation surfaces named in scope are updated together and do not contradict `docs/plans/provider-optimization-evidence-matrix.md` or `docs/plans/provider-optimization-gap-matrix.md`.
- No updated doc still claims that non-SQLite latest-satellite reads already have completed timing evidence, or that DB2 PIT/bridge has completed timing evidence.
- The documentation keeps root skipped-placeholder rows framed as guidance with planned strategy facts, not as measured wins.
- No benchmark rerun, provider implementation change, or supported-shape expansion is required to satisfy this ticket.

## Implementation Notes
- Use `docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md` as the durable ticket-bound handoff artifact for the verified scope.
- Repository code shows all provider packages currently register `IDataVaultProviderReadStrategy`, so the documentation task is about evidence posture and caveats, not about reopening provider-registration questions.
- Use the root benchmark triplet for SQLite completed latest/PIT/bridge timing plus external-provider skipped placeholders; use `artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/` for completed PostgreSQL/SQL Server/MySQL/Oracle PIT/bridge timing.
- Keep the fallback wording finite and consistent with the existing boundary docs: provider mismatch, unsupported latest-satellite parent or multi-active shape, unsupported PIT/bridge shape, incomplete read-shape evidence, and stale PIT/bridge maintenance all fall back to provider-neutral reads.
- Live `.gicket` relation state still contains incoming `blocks` links from done tickets `06FBSCFDFFYQXBK17RT3E8W4CM`, `06FBSCFKWGQMBEF5Q96AZ5Q0X0`, `06FBSCFVT3SBHKMDGNEXWVWFXG`, `06FBSCG18KBRT1FTHDRX073EF4`, `06FBSCG6C40X9CV3FFEHHKS6G0`, `06FBSCGGN528A2NC6TTA5A99X0`, `06FBSCGNY2R6PC7P4Y91RD0HVR`, `06FBSCGVAZ5G8NP1TRXFNEP6DW`, `06FBSCH0M358R5J3RGFB6GRDM4`, and `06FBSCH65R88BT6PS7XV32NQ1M`; treat those as housekeeping context, not as PO blockers.

## Open Questions
- none

## Follow-Up Questions
- Which later ticket, if any, should own provider-configured latest-satellite timing collection for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 now that strategy-registration posture is documented?
- If DB2 PIT/bridge timing work is later approved, which explicit environment and benchmark artifact lane will be authoritative for promoting DB2 out of the defer/no-completed-timing lane?

## Risks
- Docs can easily overclaim non-SQLite latest-satellite performance because the root benchmark rows already carry planned strategy names while remaining skipped placeholders.
- Docs drift remains likely unless the performance guide, PIT/bridge boundary note, and v0.40.0 release note are updated together against the evidence matrix and gap matrix.
- The stale incoming `blocks` relations from done tickets may confuse later workflow review if they are not cleaned up after documentation delivery.

## Split Recommendations
- No additional split is justified for this ticket; the current repository already provides a finite documentation baseline.
- If future work is opened, keep it split between latest-satellite timing collection and DB2 PIT/bridge environment-backed evidence activation rather than reopening this documentation ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Document accepted read strategy changes, no-op decisions, benchmark outcomes, fallback behavior, and provider caveats. Acceptance: docs distinguish measured provider wins from unsupported or deferred read shapes.