[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the current persisted epic contract has no open questions, explicitly bounds the broken formatting gate as a separate tooling prerequisite, and repository evidence supports the cited modeling and hashing baseline.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB74DC57F8HC98X4D6ZBHXW/description.md contains PO handoff decision ready_for_po_critic and ## Open Questions with only '- none'.
- Relation files .gicket/relations/XW/SW, XW/YG, and XW/38 confirm parentOf links from 06EXB74DC57F8HC98X4D6ZBHXW to 06EXB74NRVRX18GD33CH1C12SW, 06EXB75DX3YAJFMJ6TNHVPAWYG, and 06EXB765S2X2MR2K18ZBV8RC38.
- git rev-parse HEAD and git log show the reviewed branch ticket/06EXB74DC57F8HC98X4D6ZBHXW-epic-data-vault-2-x-modeling-core at 3764eb1f732dbaf3d0a4cf5593e0a50d52e48ab9.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs defines public hub, link, satellite, metadata reference, business-key, participant, and satellite payload metadata types with required hash key/hash diff/load timestamp/record source contracts.
- src/DCoding.Data.DVault/Modeling/DataVaultConventions.cs exposes default concepts Hub, Link, Satellite, HashKey, HashDiff, LoadTimestamp, RecordSource plus stable hash algorithm sha256-v1 and logical objects dvault_records, dvault_record_payloads, dvault_record_metadata.
- src/DCoding.Data.DVault/DefaultStableHashService.cs uses AlgorithmId sha256-v1, UTF8Encoding without BOM, SHA256.HashData, and lowercase hexadecimal output; DefaultStableHashNormalizer.cs sorts field paths ordinally and uses invariant formatting.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs covers hub/link/satellite metadata, required metadata roles, provider-neutral CLR contracts, invalid names/collections, and link endpoint validation.
- tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs and StableHashNormalizerTests.cs cover published SHA-256 vectors, null/empty behavior, UTF-8 without BOM, repeated determinism, field ordering, culture independence, duplicates/invalid paths, unsupported values, and DI overrides.
- docs/architecture/mvp-data-vault-concepts.md defines the v1 concept baseline as hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources with portable examples.
- tools/check-format.sh is still broken as recorded: nl shows script_repo_root referenced on lines 10 and 12, rg finds only those references, and bash tools/check-format.sh exits with unbound variable errors for lines 10 and 12.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The executable formatting gate remains unavailable until a separate tooling/governance fix repairs tools/check-format.sh; the current epic contract correctly avoids making that broken command a developer closure requirement for this modeling work.

AC / test suggestions
- Keep the current acceptance criteria requiring deterministic naming, metadata contracts, concept classification, stable hash vectors, culture independence, duplicate/invalid metadata handling, and provider-neutral behavior.

Implementation watchouts
- Do not reopen this epic as one large implementation unit; continue routing execution through bounded child tickets or closure work tied to the already persisted children.
- Do not introduce provider-specific DDL, migrations, schema generation, EF provider adapters, PIT/bridge/multi-active satellite behavior, or security hashing commitments under this epic.

Non-blocking notes
- git status reported unrelated local modifications in .gicket-bot/.gitignore, .gicket/.gitignore, .gicket/types.json, and unrelated ticket files; they were not used as blockers.

Split recommendations
- No additional child tickets are required before developer handoff; the epic already has three direct parentOf children for modeling metadata, deterministic naming/model behavior, and hashing services.
- Treat tools/check-format.sh restoration as a separate tooling/governance follow-up, not as a modeling-core child split.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment