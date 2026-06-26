[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF441DM4F4ZDTHY9ZZD9RA8R/description.md persists a delivery contract with PO Handoff = ready_for_po_critic, Open Questions = none, and acceptance criteria that close this ticket as no-work after upstream defer-now ticket 06FF440F02AFQNQ0A3XNA2ZS3W.
- git diff --stat develop...HEAD -- .gicket/tickets/06FF441DM4F4ZDTHY9ZZD9RA8R reported 27 changed files only under this ticket's .gicket folder; no src/, tests/, or docs/ paths were in the branch diff, which matches a refinement/no-work path.
- rg -n -i 'dependent[- ]child|dependent child key' src tests returned no matches; the broader repo search found only docs statements plus generic DMV1501 parser/catalog references, so the current source/test surface does not advertise dependent-child support.
- src/DCoding.Data.DVault/Modeling/DataVaultTableKind.cs exposes only Hub, Link, Satellite, PointInTime, Pit, and Bridge, and src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs only aggregates hubs, links, satellites, point-in-time tables, bridges, and PITs.
- src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs defines DMV1501 as Unsupported metadata capability, and src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs raises DMV1501 when an artifact cannot map to the current metadata surface or when a bridge kind is unsupported.
- docs/model-first-governance.md, docs/production-adoption-checklist.md, and docs/releases/v0.13.0.md each directly state that dependent child key modeling remains outside the current public claim set/baseline.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If product reopens this later, the future ticket should include one concrete dependent-child declaration example that is expected to fail with DMV1501 instead of a different reference or shape diagnostic.
- If relation cleanup later matters for routing, a follow-on housekeeping ticket should name the exact stale blocks edges to remove or re-route.

Risky assumptions
- Downstream roles and automation will treat the delivery contract in .gicket/tickets/06FF441DM4F4ZDTHY9ZZD9RA8R/description.md as authoritative over the still implementation-leaning ticket title and legacy draft text.
- The existing unsupported-capability boundary is sufficient for out-of-baseline dependent-child requests without adding a new diagnostic code or metadata shape in this closure ticket.

AC / test suggestions
- When this ticket is closed, preserve the observed proof set in the closure note: upstream ticket 06FF440F02AFQNQ0A3XNA2ZS3W is done, rg found no dependent-child matches under src/ or tests/, and the current public surface remains the finite DataVaultTableKind/DataVaultMetadataModel baseline.
- If a downstream validation example is needed, use one model-first dependent-child artifact example that is expected to fail through DMV1501 rather than being projected into existing hub/link/satellite metadata.

Implementation watchouts
- Treat the handoff as no-work/closure handling only; do not interpret this ticket as approval to add a dependent-child metadata concept, builder verb, dvault.model.v1 token, runtime mapper contract, save/read API, or support-bundle shape.
- Do not infer dependent-child parity from existing repeated same-hub roles, link-parent satellites, multi-active driving keys, PIT support, or bridge support.

Non-blocking notes
- The current title in .gicket/tickets/06FF441DM4F4ZDTHY9ZZD9RA8R/ticket.json still says Task: Prototype bounded dependent child key metadata if accepted, and the legacy draft in description.md still mentions prototype implementation; the authoritative contract resolves that, but the wording remains easy to misread in list views.

Split recommendations
- If product reopens dependent child modeling, split follow-on work into separate tickets for contract/design, metadata and model-first schema, Code-First API, runtime translation and migration behavior, and diagnostics/tooling parity.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment