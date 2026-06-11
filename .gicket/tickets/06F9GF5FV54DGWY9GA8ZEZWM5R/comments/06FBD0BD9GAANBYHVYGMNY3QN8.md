[gicket-bot] PO-critic review contract

Summary
- Contract is now explicit on same-length algorithmId drift and the reviewed support-bundle baseline, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/description.md has ## Open Questions = none and Acceptance Criteria explicitly require sha1-v1 -> sha256-160-v1 to fail closed even though both are 20-byte / 40-hex digests.
- .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/comments/06FBCSE59A8YCKQ931BTVAK314.md records the earlier PO-critic return, and .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/comments/06FBCY2Y7GKGQPB5EC2JVS92YC.md records both critic items as answered and points to description revision 06FBCX58QQPZQY6G5W84MV487G.
- git rev-parse HEAD returned a9f9d2de8e7ecf51393949858723005027d3279f, matching the provided scratch-source-ref, and git log shows the prior PO-critic return at d72fbd489 followed by the renewed PO handoff at 1ac143eb8.
- git diff --name-only d72fbd489440..a9f9d2de8e7ecf51393949858723005027d3279f only touches .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/*, confirming the follow-up was ticket-refinement work rather than implementation.
- src/DCoding.Data.DVault/BuiltInStableHashService.cs and src/DCoding.Data.DVault/StableHashDigest.cs define distinct built-in ids sha1-v1 and sha256-160-v1, and both resolve to 20 digest bytes / 40 lowercase-hex characters.
- src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs lists six built-in provider profiles: sqlite-v1, oracle-v1, postgres-v1, db2-v1, sqlserver-v1, and mysql-pomelo-v1, matching the ticket's six-profile baseline.
- src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs routes IBM.EntityFrameworkCore through UnsupportedDataVaultLiveSchemaReader, which matches the ticket's DB2 fail-closed live-schema scope note.
- src/DCoding.Data.DVault/DataVaultSupportBundle.cs sets schema version dvault.support-bundle.v1; tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs asserts support-bundle explain output includes algorithmId, digestByteLength, and digestEncoding without raw digest leakage; docs/architecture/dvault-dotnet-ef-design-time-workflow.md documents that support-bundle as the reviewed redacted baseline.
- Relation files .gicket/relations/5C/5R/06F9GF5A8V7G3PAKGRXNYEBW5C--06F9GF5FV54DGWY9GA8ZEZWM5R--parentOf.json and .gicket/relations/5R/00/06F9GF5FV54DGWY9GA8ZEZWM5R--06F9GF5N4N3Q685XQPKTM5EC00--blocks.json confirm the stated parent/blocking context.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A worked Binary round-trip example for one 20-byte algorithm and one 32-byte algorithm would make downstream test intent faster to read, even though the current AC/DoD are already testable.
- An explicit DB2 support-bundle/preflight-only example would help reviewers distinguish no-live-schema fail-closed behavior from providers with live catalog checks.

Risky assumptions
- docs/plans/provider-identifier-ddl-guardrail-contract.md still reflects an older five-provider baseline, so downstream readers must treat this ticket contract plus DataVaultProviderCapabilityProfiles.cs as the current six-profile source of truth.
- The drift-check story assumes teams actually regenerate and review a fresh dvault.support-bundle.v1 baseline when algorithm or storage-profile inputs change.

AC / test suggestions
- Keep one guardrail test that proves sha1-v1 -> sha256-160-v1 is rejected when width and provider store type stay unchanged.
- Cover six built-in provider profiles under default HexString storage and explicit Binary opt-in, asserting storage profile, provider store type, logical property kind, digestByteLength, and algorithmId facts.
- Add one support-bundle freshness/preflight test that fails when the reviewed baseline is stale after an algorithmId or storage-profile change.

Implementation watchouts
- Current HashKey and ParticipantReference mappings in src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs are still string/text-based across all six built-in profiles, so Binary opt-in will require explicit per-profile projection/conversion decisions.
- src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs keeps DB2 on UnsupportedDataVaultLiveSchemaReader, so DB2 drift enforcement cannot rely on live catalog reads under this ticket.
- Older five-provider planning text remains in docs/plans/provider-identifier-ddl-guardrail-contract.md, so final handoff documentation and tests need to anchor to the six-profile baseline called out by this ticket.

Non-blocking notes
- Assignees are empty in .gicket/tickets/06F9GF5FV54DGWY9GA8ZEZWM5R/ticket.json, but the current workflow does not make assignee absence a PO blocker.

Split recommendations
- If scope has to shrink later, keep this ticket as the contract parent and split provider-profile/EF-annotation storage-shape work from migration, live-schema, and explain/preflight guardrail work, matching the current Delivery Contract.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment