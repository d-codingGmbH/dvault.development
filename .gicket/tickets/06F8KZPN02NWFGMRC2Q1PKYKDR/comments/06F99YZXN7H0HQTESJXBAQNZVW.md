[gicket-bot] PO-critic review contract

Summary
- Ticket 06F8KZPN02NWFGMRC2Q1PKYKDR is ready for developer handoff: the delivery contract has no open questions, the support-bundle-only boundary is aligned with current docs/tests, and the remaining inconsistencies are documented as non-blocking follow-ups.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZPN02NWFGMRC2Q1PKYKDR/description.md contains the authoritative delivery contract, `## Open Questions` is `none`, and the contract now carries 6 acceptance criteria plus 4 definition-of-done items.
- git log --oneline -- .gicket/tickets/06F8KZPN02NWFGMRC2Q1PKYKDR shows `46e628364` as the PO->PO-critic handoff commit, and git diff --name-only 46e628364..HEAD only touches .gicket comment/event metadata plus ticket.json; no src/tests/docs files changed after handoff.
- docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md states typed helpers consume exactly one authoritative `dvault.support-bundle.v1`, use `DMV1960`/`DMV1961` for source and fingerprint issues, `DMV1963`/`DMV1964` for unsupported PIT/bridge evidence, and `DMV1967` for dynamic or unbounded shapes.
- src/DCoding.Data.DVault.Analyzers/README.md repeats the same `DMV1960`-`DMV1969` mapping, including `DMV1968` as a reserved raw/residual model-first outcome.
- tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs asserts `DMV1960` for raw `dvault.model.v1` additional files, `DMV1961` for stale fingerprints, `DMV1964` for unsupported bridge shapes, and `DMV1967` for dynamic/unbounded cases.
- src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelDiagnosticCatalog.cs defines `DMV1968`, but rg over src/tests found no generator or test call site using `UnsupportedModelFirstShape`; current executable behavior still routes raw model-first inputs through `DMV1960`.
- .gicket/tickets/06F8KZP9XJ868GY6GT934QVFH4/ticket.json shows the named blocker story is already `done`, and .gicket/tickets/06F8KZPN02NWFGMRC2Q1PKYKDR/comments/06F99XCSA159HRKC6CEXFBKPX4.md says the blocked-by follow-up was dropped because the base branch already contained that `done` state.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- An explicit example that distinguishes when bridge failures should be `DMV1964` versus `DMV1967` would make future review easier, even though the current docs/tests already define the split.
- A concrete mixed-bundle example where one PIT or bridge helper fails while unrelated helpers still generate would strengthen the regression intent already stated in the acceptance criteria.

Risky assumptions
- The story assumes current public behavior should keep raw or residual `dvault.model.v1` inputs on `DMV1960`, even though the catalog/README still reserve `DMV1968` for that family of cases.
- The story relies on developers following the existing repository contract for `DMV1967` dynamic-query cases rather than inferring that every bridge/PIT shape problem collapses into `DMV1964` or `DMV1963`.

AC / test suggestions
- Keep an explicit regression asserting raw `dvault.model.v1` additional files still report `DMV1960` until a separate ticket intentionally activates or retires `DMV1968`.
- Add or preserve mixed-bundle tests proving PIT/bridge failures skip only the affected helper and leave unrelated supported helpers generated.
- Add at least one bridge test for the `DMV1967` path tied to unbounded hierarchy traversal or another documented dynamic-query requirement, so the `DMV1964`/`DMV1967` boundary stays executable.

Implementation watchouts
- Do not widen generator inputs beyond exactly one authoritative `dvault.support-bundle.v1` additional file.
- Do not let PIT or bridge diagnostic failures suppress unrelated satellite, PIT, or bridge helpers in the same bundle.
- If documentation or catalog text is touched, keep the shipped diagnostic mapping aligned with executable tests so `DMV1968` does not become a silent documentation-only branch.

Non-blocking notes
- The ticket has no assignee yet; that is acceptable for PO-critic approval but will still need normal developer triage/dispatch.

Split recommendations
- No split recommended; the remaining work is still a single bounded generator-diagnostics/tests/documentation pass.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment