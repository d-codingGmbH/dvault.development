[gicket-bot] PO-critic review contract

Summary
- Refined contract matches local source, tests, relations, and branch state; no PO-side ambiguity remains.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGKV9AFAMKGJEKKZ3AXHGC/description.md:30-34 defines effectivity as existing Link(...).Satellite<TSatellite>(...) plus optional DrivingKey(...), and description.md:49-50 shows Open Questions = none.
- src/DCoding.Data.DVault/DataVaultCodeFirstLinkBuilder.cs:25-45 exposes Satellite<TSatellite>(...) on links, and src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilder.cs:111-126 projects link satellites into DataVaultSatelliteMetadata via link.ToReference().
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs:747-839 defines satellites only by parent, payload names, optional driving-key names, and HashDiff/LoadTimestamp/RecordSource; src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:86-121 has no effectivity-specific satellite property role.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs:89-115, tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelArtifactExporterTests.cs:62-91, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs:80-162 already cover link-parent satellite translation, artifact export/import, registry save, and latest/as-of read flows.
- src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs:257-275 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs:315-344 show typed CreateOrdinaryHubSatelliteRegistrySaveRequest(...) is intentionally limited to ordinary hub-parent satellites and rejects link-parent or driving-key shapes.
- README.md:432 and docs/plans/fluent-code-first-api-contract.md:81 still describe link-parent satellites as outside the bounded Code-First surface, while docs/plans/dvault-model-v1-schema-contract.md:46 already allows link-parent satellites; .gicket/relations/GC/V0/06F2PGKV9AFAMKGJEKKZ3AXHGC--06F2PGM9038RXVJH0RJFYEJEV0--blocks.json keeps that cleanup on the documentation task.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not yet include one canonical example showing an effectivity-shaped link satellite with and without DrivingKey(...); description.md:52-55 leaves that as follow-up clarification rather than an open question.
- No release-facing example currently names the effectivity pattern explicitly; README.md:432 still describes link-parent satellites as metadata-first-only until 06F2PGM9038RXVJH0RJFYEJEV0 lands.

Risky assumptions
- Developers may over-read the story title and add an EffectivitySatellite(...) API or new metadata kinds even though description.md:24-28 explicitly scopes that out.
- Consumers wanting convenience saves may assume typed helper parity, but tests/DCoding.Data.DVault.Tests/Unit/DataVaultTypedMapperContractTests.cs:315-344 proves link-parent and driving-key helpers remain intentionally out of scope.

AC / test suggestions
- Keep acceptance proof tied to the existing evidence chain: DataVaultCodeFirstLinkTests for Code-First translation, DataVaultModelArtifactExporterTests for artifact export/import, and DataVaultTypedSatelliteReadServiceSqliteTests for registry save/read behavior.
- If closure evidence is updated later, call out explicitly that generic registry save/read support is in scope while CreateOrdinaryHubSatelliteRegistrySaveRequest(...) convenience remains intentionally excluded.

Implementation watchouts
- Treat effectivity as a naming/modeling pattern over existing link-parent satellite support; do not add EffectivitySatellite(...), effectivity-specific metadata/entity kinds, or extra technical columns.
- Use the generic DataVaultMetadataReference.Link(...) and link-parent satellite path for persistence and reads; typed ordinary-hub helpers are not the supported convenience surface here.
- Do not collapse this story with doc cleanup or same-as/dependent-child modeling; those remain on 06F2PGM9038RXVJH0RJFYEJEV0 and 06F2PGM1HQ5W1M2H8T50MZ3EEC.

Non-blocking notes
- The current branch history is ticket-metadata-only through commits e1e8be640 and 52120680f, which is consistent with a PO/PO-critic gate and not a missing-implementation blocker for this pre-development story.

Split recommendations
- No additional split recommended; the story is already bounded as a contract/ratification ticket around existing link-parent satellite support.
- If product later wants first-class effectivity-specific APIs, validation, or typed-helper convenience, create separate follow-on tickets instead of reopening this generic satellite baseline.
- Keep README and release-note cleanup on 06F2PGM9038RXVJH0RJFYEJEV0.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment