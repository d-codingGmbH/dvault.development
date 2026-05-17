[gicket-bot] PO-critic review contract

Summary
- Bounded, source-backed v0.13.0 documentation-closure ticket with no unresolved PO questions; ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGM9038RXVJH0RJFYEJEV0/description.md:7-9 sets PO Handoff to ready_for_po_critic; lines 61-62 show Open Questions = none.
- .gicket/releases/06F2PH9C2PY0EBJBJNQA9338XC.json:3-6 shows active release v0.13.0 - Code-First Parity Expansion.
- find docs/releases -maxdepth 1 -type f | sort lists only docs/releases/v0.5.0.md through docs/releases/v0.12.0.md; docs/releases/v0.13.0.md is not present yet.
- README.md:10-16 still uses package version 0.12.0; README.md:31 and 432 still frame Code-First as hub-parent/ordered hub-link only; README.md:477-500 still presents v0.12.0 as the current public baseline.
- examples/README.md:17-23 still uses 0.12.0, and examples/README.md:83 still limits Code-First to the older hub/satellite/ordered-link surface.
- docs/model-first-governance.md:3-9 still labels the guidance v0.12.0 and omits same-hub-role/link-parent-satellite parity; docs/production-adoption-checklist.md:19 still points to the older ordered-link framing; src/DCoding.Data.DVault.Analyzers/README.md:17 still pins the analyzer package to 0.12.0.
- src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs:31-49 already exposes Participant<TEntity>(string role) and Satellite<TSatellite>(...).
- src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:164-188 requires explicit relationship names and distinct non-blank roles for repeated same-hub participants.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs:42-77 verifies role-based same-hub link column names; lines 123-176 verify link-parent satellites project Parent.Kind = Link; lines 247-277 verify missing/duplicate-role failures.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactExporterTests.cs:62-92 round-trips link-parent satellites through model-artifact export/import; tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:80-143 persists a same-hub link using SourceCustomer and MatchedCustomer participant names.
- src/DCoding.Data.DVault/IDataVaultLinkMapper.cs:9-13 still states repeated same-hub typed mappings are unsupported, matching the ticket scope-out.
- .gicket/tickets/06F2PGM1HQ5W1M2H8T50MZ3EEC/description.md:4-6 and 25-29 scoped dependent child keys into follow-up work; .gicket/tickets/06F2PGKV9AFAMKGJEKKZ3AXHGC/description.md:13-21 and 30-34 ratify effectivity as generic link-parent satellite usage.
- git log --oneline --decorate -n 4 -- .gicket/tickets/06F2PGM9038RXVJH0RJFYEJEV0 README.md examples/README.md docs/model-first-governance.md docs/production-adoption-checklist.md docs/releases shows only workflow commits (adc3bc73c, 7faaf96d9, 53d758639, 12467aa46) on this ticket branch so far, which is consistent with a pre-development handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Assuming ticket 06F2PGM1HQ5W1M2H8T50MZ3EEC title means dependent child keys shipped would overclaim the current public surface; its refined contract scoped that capability out.
- Assuming same-hub runtime support implies same-hub typed mapper or source-generator parity would conflict with src/DCoding.Data.DVault/IDataVaultLinkMapper.cs:9-13 and this ticket's scope-out.
- Assuming effectivity needs a distinct fluent API or metadata kind would conflict with ticket 06F2PGKV9AFAMKGJEKKZ3AXHGC and current link-parent-satellite evidence.

AC / test suggestions
- Use DataVaultCodeFirstLinkTests.cs:42-77, 123-176, and 247-277 as release-note or README validation evidence for same-hub roles, link-parent satellites, and rejection rules.
- Use DataVaultModelArtifactExporterTests.cs:62-92 and ExplicitDataVaultSaveServiceSqliteTests.cs:80-143 as evidence that generic link-parent satellites and role-based participant names already round-trip through artifact and explicit-save surfaces.
- Validate every touched public doc against src/DCoding.Data.DVault/IDataVaultLinkMapper.cs:9-13 so v0.13 docs do not claim same-hub typed mapper or source-generator parity.

Implementation watchouts
- Keep the quickstarts metadata-first; examples/README.md:79-86 already frames them that way, and the contract scopes out a new runnable Code-First sample project.
- Use the repository term role and keep the explicit-link-name requirement visible for repeated same-hub links; src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:164-188 rejects derived-name repeated-hub links and blank or duplicate roles.
- Document effectivity through generic Link(...).Satellite<TSatellite>(...) plus Payload(...) and optional DrivingKey(...), not as a separate entity family or fluent API.
- Do not imply a Code-First-to-registry bridge or automatic typed-helper parity; this ticket is a documentation correction, not an architecture expansion.

Non-blocking notes
- git diff --stat adc3bc73c5b4482eaaaa1835768a165500b7d8a3..HEAD returned no output, so the review surface is still effectively the PO-handoff state.
- docs/releases/v0.12.0.md provides the current release-note structure and tone to mirror for the new v0.13.0 document.

Split recommendations
- No split needed for this v0.13 documentation-closure task.
- If product later wants a runnable same-as or effectivity example, or dependent-child-key documentation, track those as separate follow-on tickets rather than widening this release-closure story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment