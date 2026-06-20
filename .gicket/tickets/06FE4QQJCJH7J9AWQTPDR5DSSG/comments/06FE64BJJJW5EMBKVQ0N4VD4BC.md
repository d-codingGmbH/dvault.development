[gicket-bot] PO refinement contract

Summary
- Refined the ticket around an evidence-backed deferral: current repository state proves Oracle latest-satellite strategy registration, SQL shape, parity, and finite fallback gates, but it still does not provide completed Oracle latest-satellite timing evidence. No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this run.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Treat this as an evidence-and-decision ticket, not a PIT tuning implementation ticket: the immediate output is a verified recommendation on whether Oracle latest-satellite evidence is strong enough to support PIT tuning claims.
- The authoritative current capability baseline is newer than the historical v0.32.0 smoke-read artifact dated 2026-06-07: current code and tests show Oracle latest-satellite strategy registration and parity, but that historical artifact still matters because it is the last checked-in configured Oracle latest-satellite run and it completed through provider-neutral fallback with selectedStrategy=<none>.
- Current root benchmark guidance remains non-timing evidence for Oracle latest-satellite: benchmark-summary.csv keeps the Oracle latest-satellite row as a skipped placeholder when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, even though the row now names OracleDataVaultReadStrategy as the planned strategy.

Scope In
- Verify the current Oracle latest-satellite capability posture from repository code, tests, benchmark artifacts, and planning docs.
- Document the Oracle latest-satellite SQL shape used by the provider strategy for supported requests.
- Document the finite runtime fallback and diagnostics boundary for Oracle latest-satellite reads and the separate benchmark-evidence stop conditions.
- Decide whether Oracle latest-satellite evidence is strong enough to support PIT tuning claims now, or whether that tuning claim should be deferred.

Scope Out
- Rerunning Oracle benchmarks or provisioning DVAULT_TEST_ORACLE_CONNECTION_STRING.
- Implementing Oracle PIT tuning or changing provider read/write code.
- Broadening Oracle latest-satellite support beyond hub-parent, non-multi-active satellites.
- Adding automatic PIT maintenance, new public read APIs, or release-document changes outside this ticket's refinement contract.

Open questions
- none

Follow-up questions
- When an Oracle-configured benchmark environment is available, should the next evidence-gap ticket collect a dedicated Oracle latest-satellite comparator before any Oracle PIT tuning claim is promoted in release-facing docs?
- If Oracle PIT tuning proceeds before Oracle latest-satellite timing exists, should release documentation explicitly call out that PIT evidence is accepted independently from the missing latest-satellite timing lane?

Risks
- If downstream PIT tuning work treats current Oracle latest-satellite capability evidence as equivalent to measured timing evidence, release or performance guidance could overclaim Oracle read performance.
- The historical 2026-06-07 smoke-read artifact still shows provider-neutral fallback for Oracle latest-satellite, so documentation must clearly distinguish that historical configured run from the newer v0.41+ registration and parity baseline.
- Until a configured Oracle latest-satellite benchmark lane exists, provider-specific tuning thresholds for adjacent read models can be justified, but end-to-end Oracle latest-satellite improvement claims remain unproven.

Split recommendations
- No additional split is justified from current evidence; the existing Oracle latest-satellite evidence-gap track is already bounded by docs/plans/provider-optimization-gap-matrix.md P0.04.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment