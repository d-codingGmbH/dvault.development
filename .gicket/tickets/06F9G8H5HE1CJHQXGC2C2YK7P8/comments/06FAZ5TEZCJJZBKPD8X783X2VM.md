[gicket-bot] PO refinement contract

Summary
- Refined the DB2 schema/naming/live-schema guardrail story against the landed DB2 contract and current repository state; no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already includes the DB2 package/project, `KnownProviderNames.Db2`, `IBM.EntityFrameworkCore` provider selection, and a `db2-v1` capability profile, so this story should close remaining schema/guardrail gaps rather than reopen package wiring.
- The authoritative DB2 contract from done ticket `06F9G8GS08VNH0DT09Q4PC2HRC` fixes the baseline for this story: provider name `IBM.EntityFrameworkCore`, profile `db2-v1`, `MaximumIdentifierLength = 128`, `AllowsIndexesCoveredByPrimaryKey = false`, `UnsupportedIncludedIndexColumnMode = AppendToKey`, `VARCHAR(33)` ISO-8601 UTC load timestamps, `BIGINT` for `UtcTicks`, and opt-in DB2 validation via `DVAULT_TEST_DB2_CONNECTION_STRING`.
- The current branch still shows five-provider assumptions in the guardrail/live-schema surfaces, especially identifier-preflight/test baselines and live-schema reader coverage, so this ticket owns bringing those schema-safety surfaces into DB2-aligned behavior or an explicit DB2 unsupported boundary.
- The ticket remains a child of epic `06F9G8GH969DQXD7WZ8JHD1GRR` and correctly sequences work before integration story `06F9G8HBXS7Y42J7XFSQKZ2AZ8`; the incoming `blocks` edge from done package story `06F9G8GZ384VKA7RVF039WKX1M` should be treated as satisfied prerequisite evidence, not a remaining PO blocker.
- No persistent planning writes were applied in this run.

Scope In
- Update DB2 schema guardrail facts in the provider capability/profile surfaces so `db2-v1` actually enforces the contract-defined identifier length, duplicate-index, include-column, and load-timestamp behavior.
- Extend provider identifier preflight, physical-name projection, schema parity, and explain/diagnostic coverage so DB2 is part of the finite supported guardrail baseline instead of a partial package-only add-on.
- Add DB2 migration-guardrail coverage that rejects unsafe or contract-incompatible generated DDL using the existing deterministic diagnostics model.
- Make DB2 live-schema behavior explicit and tested: either implement a bounded DB2 reader or keep `IBM.EntityFrameworkCore` on the intentional `UnsupportedProvider` path until such a reader exists.
- Keep all DB2 tests secret-free and default-local safe; any live DB2 execution must remain opt-in behind `DVAULT_TEST_DB2_CONNECTION_STRING`.

Scope Out
- Creating or reworking the `DCoding.Data.DVault.Db2` package/project wiring; that landed with story `06F9G8GZ384VKA7RVF039WKX1M`.
- DB2 save/read execution strategy proof and external integration behavior; that stays with story `06F9G8HBXS7Y42J7XFSQKZ2AZ8`.
- Package verification matrix changes and release pack expectations; that stays with task `06F9G8HJJDJH4KF9VK6TZ8B1Z0`.
- Broad README, release-note, and adopter-guidance refresh; that stays with task `06F9G8HRZ72XP5Z7FNWM6MBMQC`.
- DB2 provisioning, container recipes, credential handling, schema lifecycle, or CI infrastructure.

Open questions
- none

Follow-up questions
- After this schema/guardrail slice lands, should documentation task `06F9G8HRZ72XP5Z7FNWM6MBMQC` update the remaining README, release, and adoption surfaces that still describe a seven-package family and five live-schema providers?
- If teams need real DB2 live-schema drift proof beyond the explicit unsupported boundary, should a later follow-up implement a bounded DB2 reader plus external opt-in evidence rather than expanding this ticket further?
- Once baseline DB2 support is stable, should DB2 join the provider-specific SQL-artifact or performance-planning lanes, or remain provider-neutral until separate evidence exists?

Risks
- Current repo evidence mixes a landed DB2 package/profile with older five-provider guardrail docs/tests; partial implementation could leave the public safety story inconsistent if this ticket updates code without closing the remaining guardrail gaps.
- If DB2 identifier or DDL behavior differs from the contract's 128-character and unquoted-name assumptions, fail-fast diagnostics must catch it rather than emitting provider-specific DB2 DDL that the current model cannot safely review.
- Leaving DB2 live-schema behavior implicit would risk accidental reader misdispatch or false support claims; this ticket must explicitly choose and test reader support versus `UnsupportedProvider`.
- The live relation graph still carries an incoming `blocks` edge from done package story `06F9G8GZ384VKA7RVF039WKX1M`; if tracker automation interprets done-source `blocks` strictly, later relation housekeeping may still be needed even though refinement is otherwise complete.

Split recommendations
- No additional split is recommended; epic `06F9G8GH969DQXD7WZ8JHD1GRR` already separates DB2 contract, package, schema/guardrail, integration, package-verification, and documentation work.

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