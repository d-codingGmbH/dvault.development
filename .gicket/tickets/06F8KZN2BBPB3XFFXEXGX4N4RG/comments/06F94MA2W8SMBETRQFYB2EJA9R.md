[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the delivery contract is bounded, repository anchors match the stated scope, and Open Questions are explicitly none.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- '.gicket/tickets/06F8KZN2BBPB3XFFXEXGX4N4RG/description.md' contains a full Delivery Contract with Scope In/Out, Acceptance Criteria, Definition of Done, and Implementation Notes; 'rg -n' on that file returned '## Open Questions' at line 53 followed by '- none' at line 54.
- '.gicket/tickets/06F8KZMRXRHRKHV56Y96M4S90G/ticket.json' shows predecessor ticket '06F8KZMRXRHRKHV56Y96M4S90G' is 'done', and 'git log --oneline develop -- .gicket/tickets/06F8KZMRXRHRKHV56Y96M4S90G | head -n 1' returned commit 'ef35f304c' with message 'AUTO-INTEGRATION squash into develop'.
- Relation event '.gicket/tickets/06F8KZMRXRHRKHV56Y96M4S90G/events/06F8M01AGYMPKNYEM4R8XJ76QR.json' records '06F8KZMRXRHRKHV56Y96M4S90G' blocks this ticket, and '.gicket/tickets/06F8KZN2BBPB3XFFXEXGX4N4RG/events/06F8M01ETSK42ABZNF6KWF6HFR.json' records this ticket blocks downstream story '06F8KZNBGB8FPW6TK5A8SAJMVC'.
- 'docs/plans/provider-identifier-ddl-guardrail-contract.md' lines 27-39 define the supported provider baseline as exactly SQLite, Oracle, PostgreSQL, SQL Server, and MySQL; line 37 states MySQL has a 64-character identifier cap; line 39 says no other provider is part of the contract.
- 'src/DCoding.Data.DVault/DataVaultModelArtifactImporter.cs:35-43' registers exactly Sqlite, Oracle, Postgres, SqlServer, and MySql; 'src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs:557-558' sets MySQL maximumIdentifierLength 64 and Ignore include-column mode, and ':472' sets Oracle allowsIndexesCoveredByPrimaryKey false.
- 'src/DCoding.Data.DVault/DataVaultAnnotationNames.cs' defines ProducedName, MetadataName, ProviderProfile, ProviderLogicalPropertyKind, ProviderStorageType, and ProviderValueFormat; 'src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:852-859' and 'src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted>' already use those traceability and diagnostic surfaces named in the ticket.
- 'git diff --name-only develop...ticket/06F8KZN2BBPB3XFFXEXGX4N4RG-story-add-provider-identifier-preflight-checks' listed only '.gicket/tickets/06F8KZN2BBPB3XFFXEXGX4N4RG/...' files, so the branch is still a ticket-prep surface with no src/ or tests/ implementation changes yet.
- 'git rev-parse 268fc0b4f0968f4fdb6bc38bfe6d1ae107fba524' and 'git rev-parse ticket/06F8KZN2BBPB3XFFXEXGX4N4RG-story-add-provider-identifier-preflight-checks' both resolved to '268fc0b4f0968f4fdb6bc38bfe6d1ae107fba524', so the current review branch head matches the provided scratch-source-ref.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete worked example is embedded in the ticket for a reserved word that also requires truncation plus hash-suffix expansion.
- No explicit same-scope collision example is given for key/index/constraint names after provider-specific projection.
- The unrecognized-provider fallback path is defined at contract level but not exemplified in the ticket text.

Risky assumptions
- Developers will source reserved-word catalogs and unquoted-identifier rules from finite repository-controlled profile data, not live vendor docs or implicit EF behavior.
- Collision scope will follow the contract's provider plus object-class plus natural EF relational scope and will not use declaration order as the uniqueness key.
- Existing diagnostics and report surfaces can expose attempted physical names and remediation boundaries without introducing a second public naming API.

AC / test suggestions
- Add a five-provider matrix that covers unchanged-safe names, reserved words, length overflow, post-truncation collision, and included-index caveat behavior.
- Add deterministic repeatability tests across runs and cultures for hash-suffix generation and collision-hash expansion.
- Add migration-guardrail tests proving failures occur before DDL emission and include provider profile, object class, logical name, attempted physical name, and failure reason.

Implementation watchouts
- Do not create a second source of truth for provider selection; anchor profile resolution on DataVaultModelArtifactImporter.CreateProviderCapabilityProfiles(...) and DataVaultProviderCapabilityProfiles.*.
- Keep DataVaultAnnotationNames.ProducedName as the logical name and surface provider-specific physical names through EF relational metadata plus existing diagnostics and report channels.
- Treat quoting-required cases, unresolved post-projection collisions, and unsupported provider-specific index, key, or constraint shapes as fail-fast errors instead of silent rewrites or dropped constraints.

Non-blocking notes
- The predecessor contract is already landed on develop, so this story can implement against a stable baseline instead of reopening scope.
- No human clarification comment was observed in the current ticket comment set; the reviewed comment files were all gicket-bot claim, handoff, or audit comments.

Split recommendations
- No PO split is needed; keep this ticket limited to provider identifier preflight and leave broader provider-specific migration guardrails in '06F8KZNBGB8FPW6TK5A8SAJMVC'.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment