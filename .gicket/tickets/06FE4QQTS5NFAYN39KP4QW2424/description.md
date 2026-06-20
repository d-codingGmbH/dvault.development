<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the Oracle PIT hotspot ticket around the confirmed Oracle PIT outlier, comparable benchmark-evidence rules, and the existing split between PIT tuning, latest-satellite evidence, and downstream documentation.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The repository already ratifies Oracle `pit-as-of-read` as a completed-timing evidence row; this ticket is about tuning or bounding that measured Oracle PIT hotspot, not about proving strategy registration or reopening PIT eligibility.
- The current public Oracle PIT baseline is the configured v0.32.0 smoke-read bundle dated 2026-06-07: Oracle `pit-as-of-read` completed at `475.258` ms with `selectedStrategy=OracleDataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes, while the Oracle `bridge-traversal-read` row in the same bundle completed at `7.388` ms.
- The repository also contains a newer checked-in benchmark triplet under `artifacts/benchmarks/provider-bulk-podman-network-v040/` where Oracle `pit-as-of-read` is `929.144` ms on the same `100 customers, 100 PIT rows, 2 satellite segments` dataset while Oracle bridge remains `11.203` ms, so the hotspot is PIT-specific rather than a generic Oracle read failure.
- Oracle latest-satellite timing is already separated: done ticket `06FE4QQJCJH7J9AWQTPDR5DSSG` closed the latest-satellite evidence/fallback lane and explicitly left Oracle PIT/bridge evidence independent of that unresolved latest-satellite timing lane. The still-present `blocks` relation from that done ticket is historical workflow context only and does not block this ticket.

### Scope In
- Investigate the Oracle provider-optimized `pit-as-of-read` hotspot only, for supported explicitly maintained PIT shapes that already sit inside the published PIT boundary and select `OracleDataVaultReadStrategy`.
- Land either an evidence-backed Oracle PIT improvement or a documented Oracle PIT limitation that explains why the current hotspot remains acceptable or unavoidable within the supported boundary.
- Preserve Oracle PIT functional parity, explicit PIT-maintenance requirements, and provider-neutral fallback behavior while evaluating the hotspot.
- Capture benchmark evidence and, when the claim depends on query shape, index use, batching, or materialization behavior, preserve representative SQL using the repository benchmark-artifact contract.

### Scope Out
- Oracle latest-satellite evidence or timing work; that lane is already separated and should not be reopened here.
- Oracle bridge tuning unless PIT changes require parity or regression verification; bridge is not the hotspot in the checked-in smoke evidence.
- New public read APIs, automatic PIT maintenance or scheduling, or widened PIT shape support beyond the current v1 PIT boundary.
- Non-Oracle provider retuning or the coordinated matrix and release documentation rollup already assigned to `06FE4QRMXVGJVA65ZR5MZ817K8`.

## Acceptance Criteria
- The ticket records the current official Oracle PIT comparator row exactly: the v0.32.0 smoke-read Oracle `pit-as-of-read` row completed at `475.258` ms with `OracleDataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes, while the matching Oracle bridge row completed at `7.388` ms.
- If code changes land, they keep Oracle PIT execution inside the existing supported boundary: Oracle provider only, explicitly maintained PIT rows, diagnostics-selected `OracleDataVaultReadStrategy`, complete read-shape evidence, and clean-context freshness requirements.
- A measurable-improvement outcome checks in comparable before-and-after benchmark artifact triplets under one explicit label using the same scenario mode, provider filter, iteration count, warmup count, load-timestamp storage, and provider configuration unless any input change is explicitly justified.
- When the claimed win depends on emitted PIT SQL shape, index usage, batching, or materialization behavior, the evidence set also preserves representative SQL beside the before-and-after artifact triplets.
- The targeted Oracle PIT metric improves or holds, and any configured optional-provider allocation regression above 10% is called out and justified in the same evidence set.
- If no measurable Oracle PIT win lands, the ticket may still close by documenting the limitation against the checked-in Oracle PIT evidence, the supported boundary, and the provider-neutral fallback posture without reopening latest-satellite, bridge, or shape-expansion work.
- Whichever outcome lands, Oracle PIT fallback behavior remains intact: provider mismatch, unsupported PIT shape, incomplete read-shape evidence, stale read-model maintenance, or missing strategy selection continue to fall back to provider-neutral reads.

## Definition of Done
- The next reviewer can tell from checked-in repository evidence whether Oracle PIT improved, held, or remained bounded by a documented limitation, without reopening what counts as measured timing evidence.
- Any benchmark-backed outcome keeps the full artifact triplet, preserved run context, and, when relevant, the SQL capture needed to justify PIT query-shape claims.
- Oracle PIT changes do not break the existing parity or fallback contract for unsupported, stale, or diagnostics-declined PIT requests.
- The ticket outcome is ready for downstream documentation rollup in `06FE4QRMXVGJVA65ZR5MZ817K8` without requiring another PO split.

## Implementation Notes
- Oracle PIT dispatch is already fixed: `AddDVaultOracle()` registers `OracleDataVaultReadStrategy` for latest-satellite, PIT, and bridge candidate paths, and the public boundary remains explicit PIT maintenance plus provider-neutral fallback when diagnostics do not select the strategy.
- The main tuning surface here is PIT, not the Oracle latest-satellite lane. Oracle PIT execution currently flows through the shared `DataVaultRelationalPitBridgeReadStrategy` PIT pipeline rather than the Oracle-specific latest-satellite ROW_NUMBER path, so PIT tuning can be pursued without reopening latest-satellite design work.
- The shared PIT pipeline currently loads PIT rows by parent hash key, orders by parent and driving-key columns plus load timestamp, and applies the as-of/latest PIT-row selection in managed code after reading matched rows. If the claimed fix depends on pushing more of that work into Oracle SQL, capture representative before-and-after SQL with the benchmark evidence.
- Use the checked-in Oracle smoke-read bundle dated 2026-06-07 as the authoritative current public PIT timing citation, and treat newer checked-in local triplets as supplemental repository evidence until the downstream docs ticket promotes or supersedes them.
- Do not fold Oracle latest-satellite timing into this ticket's success criteria; that lane is already separated and can remain unresolved while Oracle PIT is tuned or bounded.

## Open Questions
- none

## Follow-Up Questions
- After this ticket lands, should `06FE4QRMXVGJVA65ZR5MZ817K8` replace the current Oracle PIT citation source with the new outcome artifact, or keep the v0.32.0 smoke-read bundle as the historical baseline and describe the new result only in v0.42 follow-up materials?
- If the ticket closes on a documented limitation rather than a measured win, should downstream adopter-facing docs compare Oracle PIT explicitly against the PostgreSQL, MySQL, and SQL Server PIT rows, or only record the limitation and fallback boundary?

## Risks
- The current public Oracle PIT claim is based on configured smoke-style artifact triplets with `iterations=1` and `warmupIterations=0`; overclaiming beyond that preserved run context would be misleading.
- Oracle PIT and Oracle bridge share the same strategy registration but not the same hotspot profile; a PIT-only tune must avoid unintentionally changing bridge behavior or widening scope into separate bridge work.
- Because the PIT read implementation is parity-driven across providers, an Oracle-specific optimization could introduce behavior drift unless parity and fallback checks stay green.

## Split Recommendations
- No new split is needed: Oracle latest-satellite is already separated in done ticket `06FE4QQJCJH7J9AWQTPDR5DSSG`, and coordinated documentation propagation is already separated in blocked ticket `06FE4QRMXVGJVA65ZR5MZ817K8`.
- Keep this ticket focused on the Oracle PIT hotspot only; do not absorb Oracle bridge or cross-provider tuning unless a later benchmark shows a new distinct hotspot.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: investigate and tune the Oracle PIT read outlier seen in smoke evidence. Acceptance: either a measurable improvement is landed or the documented fallback/limitation is justified.