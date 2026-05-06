[gicket-bot] PO-critic review contract

Summary
- Re-refined ticket is ready for developer handoff. The prior PO-critic blocker was resolved by limiting concrete API examples to source-backed DataVaultOptions resolver methods and labeling the remaining hook categories as planned/future boundaries.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NX9SVP7MSB1R4PJ50EHGW/description.md:12-16 constrains the work to documentation only, limits current API evidence to DataVaultOptions resolver methods, and requires future hook APIs to be described as planned expansion boundaries.
- .gicket/tickets/06EZ0NX9SVP7MSB1R4PJ50EHGW/description.md:33-48 contains concrete acceptance criteria and DoD; description.md:58-59 says Open Questions: none.
- Previous PO-critic comment .gicket/tickets/06EZ0NX9SVP7MSB1R4PJ50EHGW/comments/06EZNAB3V4W5Q9XDJPNG36HAT4.md:13-18 returned the ticket for unsupported API/type assumptions; latest PO refinement comment 06EZNB4MR0PD1DY6CGN1YSH674.md:10-19 answers those items by requiring source-backed API claims and excluding future API names.
- src/DCoding.Data.DVault/DataVaultOptions.cs:17-31 and :40-54 define UseLoadTimestampResolver and UseRecordSourceResolver overloads; IDataVaultLoadTimestampResolver.cs:6-12 and IDataVaultRecordSourceResolver.cs:6-12 define the resolver interfaces.
- docs/plans/optional-advanced-configuration-hooks.md:15-21 enumerates the five hook categories; :31-39 documents zero-configuration defaults; :57-62, :80-85, :103-108, :127-132, and :150-155 document validation and future-expansion boundaries.
- docs/architecture/dvault-v1-explicit-save-service.md:8-19 documents the explicit save boundary carrying record source and load timestamp with UTC normalization; DataVaultSaveService.cs:71-77 normalizes request timestamps and preserves record source; DataVaultSaveService.cs:489-501 rejects null/non-UTC timestamp resolver output and empty record-source output.
- git log shows current branch HEAD 28a2ff039, prior PO handoff <redacted>, and earlier PO-critic return cf05e4df0 for this ticket; relation file .gicket/relations/2R/GW/06EZ0NWKC9ZME5BSCJFSQEQ02R--06EZ0NX9SVP7MSB1R4PJ50EHGW--parentOf.json:3-5 confirms the incoming parentOf relation.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Implementation must continue to treat naming, hashing, provider behavior, timestamp formatting, and broader hook APIs as planned/future unless direct source evidence is added before documentation claims them as implemented.
- Provider override documentation can mislead if it implies approved provider-specific option matrices; the contract correctly keeps those out of scope.

AC / test suggestions
- Review the completed documentation for exactly one code-shaped custom resolver configuration path, grounded in the observed DataVaultOptions resolver methods.
- Run available repository documentation/format validation during implementation, or record any validation limitation for reviewer handoff.

Implementation watchouts
- Do not implement product code, public APIs, provider matrices, migrations, or provider-specific dialect behavior under this ticket.
- Avoid invented method/type names for future hooks; conceptual examples for future categories should be non-API prose.
- Preserve the explicit-save-service boundary: load timestamp and record source are supplied or resolved at the request boundary and timestamps remain UTC-normalized.

Non-blocking notes
- No attachments directory was present under .gicket/tickets/06EZ0NX9SVP7MSB1R4PJ50EHGW, and the only observed relation is the existing incoming parentOf relation.

Split recommendations
- No split recommended; the contract is bounded to one documentation task under existing docs surfaces.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment