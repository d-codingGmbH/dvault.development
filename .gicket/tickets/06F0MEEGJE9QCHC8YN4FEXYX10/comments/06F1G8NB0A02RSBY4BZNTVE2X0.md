[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff. The persisted contract has no unresolved Open Questions, is backed by the completed dvault.model.v1 planning contract, and aligns with current metadata/modeling surfaces and known adapter gaps.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEEGJE9QCHC8YN4FEXYX10/comments/06F1G0CK4DHEQJJK8N030YSVYW.md records the PO refinement contract, decision ready_for_po_critic, Open questions: none, and Split recommendations: none.
- The v1 schema contract defines strict schemaVersion behavior and defaults at lines 21, 40, and 50; declaration sections for hubs, links, satellites, PITs, and bridges appear at lines 65, 83, 108, 135, and 155.
- The v1 schema contract fixes naming and collision rules at lines 216-222, unknown-field rejection at line 238, diagnostic structure and codes at lines 240-274, fixture expectations at lines 436-439, and states completion criteria at line 468.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs exposes DataVaultMetadataModel and IReadOnlyList properties for Hubs, Links, Satellites, Bridges, and Pits at lines 6, 135, 140, 145, 155, and 160.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs exposes public metadata types for DataVaultBridgeMetadata, DataVaultHubMetadata, DataVaultLinkMetadata, DataVaultSatelliteMetadata, and DataVaultPitMetadata at lines 229, 570, 640, 722, and 899.
- src/DCoding.Data.DVault/Modeling/DefaultNamingPolicy.cs line 9 defines DefaultNamingPolicy and line 137 exposes NormalizeObjectName; DataVaultMetadataRegistry.cs and DataVaultDiagnostics.cs use StringComparer.Ordinal/StringComparison.Ordinal in validation/indexing paths.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Role-bearing recursive link support is not fully public in the current source: DataVaultCodeFirstModelBuilder.cs line 127 says repeated same-hub participants require explicit participant role or alias support and are not supported by v1 code-first link projection. The ticket mitigates this by allowing narrow model-first adapters where current public metadata APIs do not expose the shape.
- Bridge endpoint role types are internal in DataVaultMetadata.cs, so parser implementation may need to stay inside the core assembly or add a narrow internal adapter rather than assume an external public bridge-endpoint construction surface.

AC / test suggestions
- Keep the ticket's diagnostic tests data-first: <redacted> severity, category/code, path, and deterministic ordering, not only exception text.
- Include at least one syntax or top-level shape failure in addition to the named invalid artifacts, using the existing shape diagnostic category from the v1 contract.

Implementation watchouts
- Validate fully before creating or registering a DataVaultMetadataRegistry so invalid documents cannot leave a partial model applied.
- Use the repository DefaultNamingPolicy for normalized collision checks and ordinal string semantics for declaration names and token validation.
- Preserve declaration order in mapped metadata and diagnostic ordering, especially for links, bridge endpoints, PIT satellite references, and duplicate/collision diagnostics.

Non-blocking notes
- The persisted follow-up questions are correctly future-facing and do not reopen this parser ticket: diagnostic CLI formatting, broader public APIs for PIT/bridge/role-bearing metadata, and future schema compatibility are deferred.
- The work is broad but bounded to parser/import, validation diagnostics, fixtures, and narrow metadata adapters; CLI/build integration, YAML, export, and provider-specific behavior are explicitly out of scope.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment