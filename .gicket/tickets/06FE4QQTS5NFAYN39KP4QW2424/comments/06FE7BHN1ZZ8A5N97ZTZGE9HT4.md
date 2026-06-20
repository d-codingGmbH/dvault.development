[gicket-bot] PO refinement contract

Summary
- Refined the Oracle PIT hotspot ticket around the confirmed Oracle PIT outlier, comparable benchmark-evidence rules, and the existing split between PIT tuning, latest-satellite evidence, and downstream documentation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The repository already ratifies Oracle `pit-as-of-read` as a completed-timing evidence row; this ticket is about tuning or bounding that measured Oracle PIT hotspot, not about proving strategy registration or reopening PIT eligibility.
- The current public Oracle PIT baseline is the configured v0.32.0 smoke-read bundle dated 2026-06-07: Oracle `pit-as-of-read` completed at `475.258` ms with `selectedStrategy=OracleDataVaultReadStrategy`, `readShape=PitAsOf`, and no fallback causes, while the Oracle `bridge-traversal-read` row in the same bundle completed at `7.388` ms.
- The repository also contains a newer checked-in benchmark triplet under `artifacts/benchmarks/provider-bulk-podman-network-v040/` where Oracle `pit-as-of-read` is `929.144` ms on the same `100 customers, 100 PIT rows, 2 satellite segments` dataset while Oracle bridge remains `11.203` ms, so the hotspot is PIT-specific rather than a generic Oracle read failure.
- Oracle latest-satellite timing is already separated: done ticket `06FE4QQJCJH7J9AWQTPDR5DSSG` closed the latest-satellite evidence/fallback lane and explicitly left Oracle PIT/bridge evidence independent of that unresolved latest-satellite timing lane. The still-present `blocks` relation from that done ticket is historical workflow context only and does not block this ticket.

Scope In
- Investigate the Oracle provider-optimized `pit-as-of-read` hotspot only, for supported explicitly maintained PIT shapes that already sit inside the published PIT boundary and select `OracleDataVaultReadStrategy`.
- Land either an evidence-backed Oracle PIT improvement or a documented Oracle PIT limitation that explains why the current hotspot remains acceptable or unavoidable within the supported boundary.
- Preserve Oracle PIT functional parity, explicit PIT-maintenance requirements, and provider-neutral fallback behavior while evaluating the hotspot.
- Capture benchmark evidence and, when the claim depends on query shape, index use, batching, or materialization behavior, preserve representative SQL using the repository benchmark-artifact contract.

Scope Out
- Oracle latest-satellite evidence or timing work; that lane is already separated and should not be reopened here.
- Oracle bridge tuning unless PIT changes require parity or regression verification; bridge is not the hotspot in the checked-in smoke evidence.
- New public read APIs, automatic PIT maintenance or scheduling, or widened PIT shape support beyond the current v1 PIT boundary.
- Non-Oracle provider retuning or the coordinated matrix and release documentation rollup already assigned to `06FE4QRMXVGJVA65ZR5MZ817K8`.

Open questions
- none

Follow-up questions
- After this ticket lands, should `06FE4QRMXVGJVA65ZR5MZ817K8` replace the current Oracle PIT citation source with the new outcome artifact, or keep the v0.32.0 smoke-read bundle as the historical baseline and describe the new result only in v0.42 follow-up materials?
- If the ticket closes on a documented limitation rather than a measured win, should downstream adopter-facing docs compare Oracle PIT explicitly against the PostgreSQL, MySQL, and SQL Server PIT rows, or only record the limitation and fallback boundary?

Risks
- The current public Oracle PIT claim is based on configured smoke-style artifact triplets with `iterations=1` and `warmupIterations=0`; overclaiming beyond that preserved run context would be misleading.
- Oracle PIT and Oracle bridge share the same strategy registration but not the same hotspot profile; a PIT-only tune must avoid unintentionally changing bridge behavior or widening scope into separate bridge work.
- Because the PIT read implementation is parity-driven across providers, an Oracle-specific optimization could introduce behavior drift unless parity and fallback checks stay green.

Split recommendations
- No new split is needed: Oracle latest-satellite is already separated in done ticket `06FE4QQJCJH7J9AWQTPDR5DSSG`, and coordinated documentation propagation is already separated in blocked ticket `06FE4QRMXVGJVA65ZR5MZ817K8`.
- Keep this ticket focused on the Oracle PIT hotspot only; do not absorb Oracle bridge or cross-provider tuning unless a later benchmark shows a new distinct hotspot.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment