[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the epic contract is internally consistent, has no open questions, and current repo evidence matches the intended v0.22 satellite-only typed-read/hash-governance boundary; the stale planning-doc supersession is still outstanding work, but that is a developer handoff watchout rather than a PO refinement blocker.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/description.md` sets `### PO Handoff` to `ready_for_po_critic` and `## Open Questions` to `none`.
- `docs/releases/v0.22.0.md` lines 26, 30, and 40 define v0.22 as opt-in satellite-only typed read generation from one authoritative `dvault.support-bundle.v1` and say it does not emit PIT or bridge helpers.
- `src/DCoding.Data.DVault.Analyzers/README.md` lines 54, 58, 67, 68, and 73 describe a satellite-only generator and reserve `DMV1963`, `DMV1964`, and `DMV1969` for PIT/bridge unsupported or skipped outcomes.
- `docs/model-first-governance.md` line 13 says a raw `dvault.model.v1` file is not generator input until projected into an authoritative `dvault.support-bundle.v1` diagnostics artifact.
- `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs` line 13 covers hub/link/multi-active satellite helper generation, and lines 355, 380, 403, 430, and 458 assert `DMV1963`, `DMV1964`, `DMV1967`, `DMV1968`, and `DMV1969` for PIT/bridge/dynamic/model-first skipped cases.
- `docs/plans/typed-read-model-generator-contract.md` lines 15-16 still say the generator emits PIT and bridge projections, and `docs/plans/README.md` line 23 still lists `typed-read-model-generator-contract.md` under `Current Contracts`.
- The owner branch currently has no non-`.gicket` diff versus `develop`; recent comment `.gicket/tickets/06F5Q91V0YGSA6SH9WDS02GH0M/comments/06F6XQPR0TPXJZ0YWRJEPDHBAR.md` records queued mutations `mutation-baa7edf5136439f2` and `mutation-67556c67217d884c` rather than landed planning-document edits.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- It is assumed the queued planning-document supersession is remaining implementation work on the dev path, not missing PO clarification, even though `docs/plans/typed-read-model-generator-contract.md` and `docs/plans/README.md` are still stale on the current branch.
- It is assumed downstream reviewers will honor the epic contract's statement that child `06F5Q922T5B21GJN49FYN6DJH0` and its planning document are historical context, despite `docs/plans/README.md` still advertising that document as current.

AC / test suggestions
- Add an explicit completion check that the queued replay either removes `typed-read-model-generator-contract.md` from `docs/plans/README.md` Current Contracts or marks it historical in place.
- Keep a regression check that PIT/bridge support-bundle shapes continue to raise `DMV1963`/`DMV1964`/`DMV1969` while satellite helper generation still succeeds, so the v0.22 satellite-only boundary does not drift.

Implementation watchouts
- The current owner branch is `.gicket`-only relative to `develop`; the queued planning-document supersession has not landed yet and still needs repository work.
- Do not let implementation or docs reintroduce PIT/bridge typed helpers into the v0.22 contract; the authoritative repo surfaces currently keep those shapes on runtime/diagnostic paths only.
- Do not treat raw `dvault.model.v1` additional files as generator inputs; the authoritative repo boundary requires a projected `dvault.support-bundle.v1` source with fingerprint validation.

Non-blocking notes
- `git log --oneline -n 5` shows the ticket workflow commits `6d9fbfbfe` (handoff `po->po-critic`) and `9d30886cc` (current `po-critic` claim), which is consistent with the ticket still being in critic review rather than already in dev execution.

Split recommendations
- No additional split is recommended; the existing seven-child decomposition is already persisted and all child tickets are `done`.
- If later work wants shipped PIT/bridge typed helpers, automatic hashDiff generation, or new hash encodings, open additive follow-up tickets instead of reopening this epic.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment