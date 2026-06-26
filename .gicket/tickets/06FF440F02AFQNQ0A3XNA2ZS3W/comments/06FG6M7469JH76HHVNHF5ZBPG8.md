[gicket-bot] PO-critic review contract

Summary
- Approved for developer handoff: the persisted contract now clearly defers first-class dependent child key modeling, names the finite supported baseline, and leaves no open PO questions; remaining items are follow-on routing and implementation watchouts, not PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FF440F02AFQNQ0A3XNA2ZS3W/description.md:25-57 defines the defer-now contract, explicitly defers dependent child key modeling, and shows Open Questions = none at lines 44-45.
- git log --oneline -n 5 on branch ticket/06FF440F02AFQNQ0A3XNA2ZS3W-task-evaluate-dependent-child-key-modeling-contr at 0e58dbab83d52e35f3c423a41cc913cad8192c90 shows only PO handoff/claim commits after develop: 84b33f7edf, fc0202c374, and 0e58dbab83.
- git diff --name-only 3059c68353..0e58dbab83d52e35f3c423a41cc913cad8192c90 lists only .gicket/tickets/06FF440F02AFQNQ0A3XNA2ZS3W files, so this branch carries ticket/contract metadata rather than source or API changes.
- src/DCoding.Data.DVault/Modeling/DataVaultTableKind.cs:6-35 enumerates Hub, Link, Satellite, PointInTime, Pit, and Bridge only; src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:13-80 aggregates hubs, links, satellites, point-in-time tables, bridges, and PITs only.
- src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs:25-60 exposes explicit participant roles and link-parent satellites, and src/DCoding.Data.DVault/Modeling/DataVaultSatelliteBuilder.cs:23-31 exposes DrivingKey(string) for multi-active satellites.
- docs/model-first-governance.md:9,19,262, docs/plans/dvault-model-v1-schema-contract.md:113,119, and docs/releases/v0.13.0.md:105 document the current public baseline and explicitly keep dependent child key modeling outside the current claim set.
- An rg -n -i search for dependent child / dependent-child over src, docs, and the two related ticket folders returned docs and ticket hits but no src/DCoding.Data.DVault API, type, or member introducing dependent-child support.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No illustrative rejected dependent-child declaration is attached to show exactly how the existing validation or unsupported-shape boundary should surface the defer-now outcome; that would strengthen downstream closure evidence but is not required for developer handoff.

Risky assumptions
- Downstream work will read the supported baseline narrowly: repeated same-hub roles, link-parent satellites, and multi-active driving keys remain supported only where the current surface already documents them, not as precedent for new dependent-child parity.
- The follow-on ticket 06FF441DM4F4ZDTHY9ZZD9RA8R will be rerouted to no-work, closure, or renewed PO refinement after this defer-now contract instead of being treated as implicit approval to prototype the feature.

AC / test suggestions
- If developer handling closes this as documentation-only or no-work, capture proof that no new public API, dvault.model.v1 section or token, metadata reference kind, support-bundle shape, or provider-identifier widening was introduced.
- Use one representative rejected dependent-child request in validation, import, or diagnostics review so the existing failure boundary is evidenced explicitly.

Implementation watchouts
- Do not silently project dependent-child requests into existing hub, link, or satellite constructs; keep rejection on the existing validation or unsupported-shape paths named in the contract.
- IDataVaultLinkMapper.cs:10-12 still says repeated same-hub typed mappings are unsupported, so repeated same-hub support elsewhere is not precedent for full typed or runtime parity.

Non-blocking notes
- The PO Summary in .gicket/tickets/06FF440F02AFQNQ0A3XNA2ZS3W/description.md:5 says no description write occurred in the refinement run, but .gicket/tickets/06FF440F02AFQNQ0A3XNA2ZS3W/comments/06FG6J63TEF92PTVADW2D9M2C4.md:16-17 and the branch diff both show description.md was updated; that provenance mismatch is worth cleaning up later but does not obscure scope or acceptance.

Split recommendations
- If product later reopens first-class dependent child support, keep the current split: separate tickets for contract/design, metadata or dvault.model.v1 shape, code-first API surface, runtime translation and migrations, and diagnostics or tooling parity.
- If the follow-on prototype ticket is kept, rewrite it explicitly as no-work, closure, or a future-contract placeholder so it no longer reads if accepted after this defer-now decision.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment