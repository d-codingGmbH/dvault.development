[gicket-bot] PO refinement contract

Summary
- Refined the MySQL latest-satellite ticket as evidence-backed tuning work: current 2026-06-20 code/tests already prove strategy registration, gate behavior, and the window-function SQL shape, while measured MySQL latest-satellite timing still remains a gap; no child tickets, relation writes, description updates, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence on 2026-06-20 already proves MySQL latest-satellite strategy registration, gate behavior, SQL-shape coverage, and configured integration selection, so this ticket should refine evidence-backed tuning work rather than reopen basic provider strategy design.
- The checked-in v0.32.0 smoke-read artifact dated 2026-06-07 still matters as the last preserved configured MySQL latest-satellite benchmark lane, and it completed through provider-neutral fallback with `selectedStrategy=<none>` because provider-specific latest-satellite dispatch was not yet registered in that artifact baseline.
- The current root benchmark triplet keeps the MySQL latest-satellite row as a skipped placeholder when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is unset, with `selectedStrategy=MySqlDataVaultReadStrategy`, `plannedReadStrategy=MySqlDataVaultReadStrategy`, `readShape=LatestSatellite`, and `persistedOutcome=not executed`; that row is guidance, not measured timing evidence.
- MySQL PIT and bridge timing are already closed separately by the 2026-06-07 configured smoke-read bundle, so this ticket must not reuse PIT/bridge timing as proof for latest-satellite tuning.
- Live relation state still includes an incoming `blocks` link from done task `06FE4QP6FB892E7TJMB47A3MSR` and an incoming `relates` link from done story `06FE4QNWP9606HTB92MTVQMYDG`; treat them as historical routing context, while the outgoing `blocks` link to `06FE4QRMXVGJVA65ZR5MZ817K8` remains the active downstream documentation dependency.
- No child tickets, relation writes, description updates, attachments, or planning documents were materialized in this refinement run.

Scope In
- Verify the current MySQL latest-satellite capability posture from repository code, tests, diagnostics-facing docs, benchmark artifacts, and live ticket relations.
- Tune or affirm MySQL latest-satellite strategy selection and SQL shape only for supported MySQL-provider hub-parent, non-multi-active current/as-of satellite reads.
- Require evidence-backed comparison against provider-neutral fallback before claiming any MySQL latest-satellite improvement.
- Keep diagnostics and benchmark tokens aligned so `selectedStrategy`, `plannedReadStrategy`, `readShape=LatestSatellite`, and fallback causes remain consistent across artifacts and docs.
- Preserve the downstream handoff to docs ticket `06FE4QRMXVGJVA65ZR5MZ817K8` once the MySQL latest-satellite tuning decision is settled.

Scope Out
- Broadening latest-satellite support beyond MySQL-provider hub-parent, non-multi-active satellites or adding a new public read API.
- Using MySQL PIT/bridge completed timing or skipped root guidance rows as substitute proof of MySQL latest-satellite improvement.
- Automatic PIT/bridge maintenance, raw SQL advisor or physical-plan promises, and provider-specific physical design guarantees.
- Promoting measured MySQL latest-satellite timing in release-facing docs without a provider-configured artifact triplet and preserved run context.
- Implementing relation housekeeping or downstream documentation delivery beyond recording the current dependency state.

Open questions
- none

Follow-up questions
- After any MySQL latest-satellite SQL-shape change, should a later configured evidence pass exercise both `MySql.EntityFrameworkCore` and `Pomelo.EntityFrameworkCore.MySql`, or is one configured provider lane sufficient because the strategy implementation is shared?

Risks
- The current 2026-06-20 root MySQL latest-satellite row is intentionally skipped; if downstream work treats that guidance row as measured timing, release or performance docs will overclaim MySQL behavior.
- The 2026-06-07 configured smoke-read bundle proves MySQL PIT/bridge timing but latest-satellite provider-neutral fallback; mixing those lanes would misstate latest-satellite evidence.
- Configured latest-satellite integration evidence currently exercises `MySql.EntityFrameworkCore`; if the SQL shape changes, the shared implementation still needs to remain safe for `Pomelo.EntityFrameworkCore.MySql`, which is currently covered by gate and SQL-shape tests rather than a checked-in configured benchmark lane.
- Until the stale incoming `blocks` relation from done task `06FE4QP6FB892E7TJMB47A3MSR` is cleaned up, relation-driven automation or human readers may misread current dependency state.

Split recommendations
- No additional split is justified. The shared latest-satellite lane normalization is already done in `06FE4QP6FB892E7TJMB47A3MSR`, sibling provider follow-ups already exist for PostgreSQL `06FE4QPR8TF8R6PXNM3RMXN8JG`, SQL Server `06FE4QQ0YTHD7624MGVPKKK1C0`, and Oracle `06FE4QQJCJH7J9AWQTPDR5DSSG`, and downstream docs work already sits in `06FE4QRMXVGJVA65ZR5MZ817K8`.
- If a later workflow wants relation cleanup, handle the stale incoming `blocks` edge from done ticket `06FE4QP6FB892E7TJMB47A3MSR` as housekeeping rather than as a new child split.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment