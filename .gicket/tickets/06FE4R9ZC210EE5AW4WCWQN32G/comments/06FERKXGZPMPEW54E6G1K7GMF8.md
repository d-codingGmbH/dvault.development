[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff: the ticket is a clear pre-development contract-design task with closed open questions, explicit additive scope, and repository-backed satellite/privacy baseline seams.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06FE4R9ZC210EE5AW4WCWQN32G/description.md contains the Delivery Contract, marks PO handoff `ready_for_po_critic`, and its `## Open Questions` section says `- none`.
- docs/plans/dvault-model-v1-schema-contract.md `## Satellite Declarations` defines satellites through exact-name `payload` plus optional `drivingKeys`, and says projection maps ordinary satellites to `DataVaultSatelliteMetadata`.
- src/DCoding.Data.DVault/DataVaultModelSatelliteDeclaration.cs currently models a satellite artifact as `Name, Parent, Payload, DrivingKeys` only, so privacy metadata is additive work rather than an existing baseline field.
- src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs has closed `SatelliteProperties = { name, parent, payload, drivingKeys }`; `ReadSatellites(...)` and `ValidateSatelliteDeclarations(...)` currently enforce parent resolution and `drivingKeys`/`payload` overlap (`DMV1701`) but no privacy-specific fields yet.
- src/DCoding.Data.DVault/Modeling/DataVaultSatelliteMetadata.cs carries descriptive payload names, driving keys, and fixed technical metadata (`HashDiffMetadata`, `LoadTimestampMetadata`, `RecordSourceMetadata`), matching the ticket's compatibility boundary.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md allows opt-in `metadata annotations or sidecar metadata` and requires provider-neutral shared contracts while keeping provider-specific DDL/storage promises out of scope.
- git diff --name-only develop..ticket/06FE4R9ZC210EE5AW4WCWQN32G-task-design-personal-data-satellite-field-metada lists only `.gicket/tickets/06FE4R9ZC210EE5AW4WCWQN32G/*`, so the branch currently contains ticket/handoff metadata only; for this pre-development gate that is consistent with a definition-only task.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Add one worked example for an ordinary hub-parent satellite that shows exact payload-name tagging, one encrypted-payload alias per marked field, and unchanged behavior for unannotated payload fields.
- Add one negative example that explicitly rejects tagging a driving key, hash diff, load timestamp, or record source field through this metadata surface.
- Add one example for a multi-active or link-parent satellite so later work does not assume the contract only covers ordinary hub-parent satellites.

Risky assumptions
- The ticket assumes one per-field encrypted-payload alias is sufficient for the v1 baseline and that shared-container or cross-field encryption shapes can remain later additive work.
- The ticket assumes model-first parsing, code-first/registry registration, and EF translation can all consume one shared metadata contract without reopening the additive-vs-replacement decision.
- The ticket assumes downstream implementation sequencing can be decided later even though the currently related follow-on package task `06FE4RAGWXQCQFCTX7QW1T9NAC` is still `todo` with `needs-po`.

AC / test suggestions
- Require the contract document to cite the current baseline paths directly: `docs/plans/dvault-model-v1-schema-contract.md`, `src/DCoding.Data.DVault/DataVaultModelSatelliteDeclaration.cs`, `src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs`, `src/DCoding.Data.DVault/Modeling/DataVaultSatelliteMetadata.cs`, and `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`.
- Require a finite validation matrix covering unknown payload references, duplicate marked-field declarations, duplicate encrypted-payload aliases within one satellite, and attempts to tag driving keys or satellite technical columns.
- Require worked examples to prove both sides of compatibility: annotated payload fields gain descriptive privacy metadata, while unannotated payload fields preserve existing payload ordering, parent identity, multi-active behavior, hash diff, load timestamp, record source, and provider-neutral EF mapping expectations.

Implementation watchouts
- The current artifact parser has a closed satellite property set, so any future metadata shape must be added deliberately and documented as additive rather than silently replacing `payload` or widening the v1 contract by accident.
- The current translator builds satellite produced columns from parent hash key, driving keys, technical metadata, and payload names; privacy markers must not implicitly rename or reorder those baseline columns.
- Current diagnostics already treat finite validation failures such as `DMV1701` as explicit contract errors; the privacy design should keep the same bounded validation posture instead of vague prose-only guidance.

Non-blocking notes
- git log --oneline --decorate -n 12 -- .gicket/tickets/06FE4R9ZC210EE5AW4WCWQN32G docs/plans/dvault-model-v1-schema-contract.md docs/architecture/dvault-v1-optional-privacy-extension-boundary.md shows only PO handoff commits `cec2491b0`, `9e1ca1304`, and the current PO-critic lease claim `ce8840c07` on top of develop.
- No repository contract document for this ticket exists on the branch yet, which is acceptable because this is a pre-development contract-definition gate rather than a post-development verification pass.
- The upstream privacy boundary story already landed in develop, so this ticket is correctly framed as a downstream satellite-metadata contract lane instead of reopening the broader privacy-boundary decision.

Split recommendations
- No additional split is required before developer handoff; keep this ticket as the single authoritative contract-definition lane for personal-data satellite field metadata.
- Keep parser/API implementation, privacy package skeleton work (`06FE4RAGWXQCQFCTX7QW1T9NAC`), and any provider-specific execution/storage lanes as separate follow-on tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment