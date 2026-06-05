[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the epic contract is now bounded, evidence-backed, and has no open PO questions; the remaining repository gaps are implementation work on the queued replacement documentation carrier, not a PO-refinement blocker.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/description.md:7-22 marks the ticket `ready_for_po_critic`, scopes the remaining work to `README.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, and a new `docs/releases/v0.30.0.md`, and description.md:50-51 records `## Open Questions` as `none`.
- .gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/description.md:44-48 explicitly says the queued replay `mutation-d16ba25963e2af83` remains the authoritative carrier until its ULID is visible and that the future `parentOf` link is a downstream follow-up rather than an open PO clarification question.
- .gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG/comments/06F9EWPJZJECT2DHCY45XV19P0.md:3-29 reports PO refinement outcome `po-refinement-ready`, says the ticket is ready for handoff to `po-critic`, and tells PO-critic to validate the updated acceptance criteria.
- README.md:25 still points readers at `docs/releases/v0.29.0.md` as the current coordinated baseline, and `git ls-files docs/releases/v0.30.0.md` returned no path, so the v0.30.0 note is confirmed future implementation work rather than missing ticket clarity.
- README.md:371-390 and docs/architecture/dvault-dotnet-ef-design-time-workflow.md:153-182 already document the existing support-bundle and `CreateSupportBundleDiagnostics` boundary, while docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md:12-20,134-141 and src/DCoding.Data.DVault.Analyzers/README.md:67-92 directly prove the authoritative `dvault.support-bundle.v1`, `DMV1960`, `DMV1961`, request-bound `ReadShape`, and skip-only-the-affected-helper contract the new docs must reuse.
- .gicket/relations/0R/DG/06F8KZQAWZ7QRGB68KB21C9B0R--06F8KZP0VKMXGE0JXPZRD1RQDG--blocks.json:1-6 still records the incoming `blocks` edge from `06F8KZQAWZ7QRGB68KB21C9B0R`, matching the epic contract's closure-stage housekeeping note.
- A relation listing for epic `06F8KZP0VKMXGE0JXPZRD1RQDG` shows only four visible `parentOf` files, all to existing children including historical child `06F8KZQAWZ7QRGB68KB21C9B0R`; the queued replacement carrier `mutation-d16ba25963e2af83` is not yet materialized as a visible linked ticket.
- .gicket/tickets/06F8KZQAWZ7QRGB68KB21C9B0R/ticket.json:7-17 still marks the earlier documentation child `done` with `closure/no-work-required`, which is why the epic contract now treats it as historical only.
- `git log --oneline -n 5 -- .gicket/tickets/06F8KZP0VKMXGE0JXPZRD1RQDG README.md docs/architecture/dvault-dotnet-ef-design-time-workflow.md docs/releases/v0.29.0.md` returned only ticket-orchestration commits (`b34dcb91e`, `fd6402ea4`, `147cf80b5`, `84ef52bc1`, `fb7a86f15`), so this review branch currently carries ticket-state history rather than landed documentation edits.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Approval assumes the queued create-ticket replay `mutation-d16ba25963e2af83` will materialize cleanly and become the visible active carrier before delivery tracking or closure automation depends on a concrete replacement-ticket ULID.
- Approval also assumes developers will treat the stale incoming `blocks` relation as closure-stage housekeeping only and will not reopen analyzer/generator scope that the epic explicitly marked out of scope.

AC / test suggestions
- When the implementation ticket lands, keep the docs explicit about the recovery sequence: metadata changes -> regenerate the authoritative support bundle -> update or remove any stale pinned `DVaultTypedReadModelMetadataSourceFingerprint` -> rebuild; for PIT/bridge helpers, also regenerate representative `CreateSupportBundleDiagnostics` requests so the bundle carries the required request-bound `ReadShape` evidence.

Implementation watchouts
- Do not create a second replacement documentation ticket while queued replay `mutation-d16ba25963e2af83` remains the authoritative carrier.
- Keep the pass bounded to `README.md`, `docs/architecture/dvault-dotnet-ef-design-time-workflow.md`, and `docs/releases/v0.30.0.md`, using docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md and src/DCoding.Data.DVault.Analyzers/README.md as the wording baseline.

Non-blocking notes
- The current visible repo still lacks the v0.30.0 note and the freshness/troubleshooting wording, but for this pre-development review those are developer-handoff gaps, not PO-clarification failures.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment