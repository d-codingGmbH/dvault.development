[gicket-bot] PO-critic review contract

Summary
- Approve for dev: the ticket now explicitly owns the missing personalData transport plus the bounded diagnostics slice, and the contract is aligned with current repository reality with no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The latest PO refinement comment .gicket/tickets/06FF43MQ3AXXK2S5TK65X4Y9S8/comments/06FFWRWJM02YPT4ZV51Y1YSEYR.md explicitly answers the prior critic checklist and states that this ticket itself owns model-first personalData import, metadata-first runtime carriage, and diagnostics over the shared carrier.
- git rev-parse --abbrev-ref HEAD reports ticket/06FF43MQ3AXXK2S5TK65X4Y9S8-task-diagnose-personaldata-metadata-without-conf and git rev-parse HEAD reports b28c4983d87d9aa66974cb90da8baaf2a5b6e426; git show --stat --summary b28c4983d87d9aa66974cb90da8baaf2a5b6e426 shows only .gicket ticket metadata changes, which is acceptable at this pre-development gate.
- src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs currently defines SatelliteProperties as name, parent, payload, and drivingKeys only, and ReadSatellites(...) builds DataVaultModelSatelliteDeclaration without personalData; src/DCoding.Data.DVault/DataVaultModelSatelliteDeclaration.cs contains only Name, Parent, Payload, DrivingKeys, and Path. This directly confirms the current model-first transport gap that the ticket now explicitly owns.
- src/DCoding.Data.DVault/Modeling/DataVaultSatelliteMetadata.cs currently exposes Name, Parent, DescriptiveAttributeNames, DrivingKeyNames, PayloadColumns, and technical columns only, and src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs aggregates satellites without any personalData or encryptedPayloadAlias carrier. This directly confirms the current metadata-first runtime carriage gap that the ticket now explicitly owns.
- docs/plans/dvault-model-v1-schema-contract.md defines satellite personalData[] and personalData[].encryptedPayloadAlias as the authoritative model-first contract, while docs/model-first-governance.md and src/DCoding.Data.DVault/DataVaultDbContextOptionsBuilderExtensions.cs show model-first import and metadata-first UseDataVaultMetadata(...) are the existing declaration paths this ticket needs to converge.
- docs/architecture/dvault-v1-optional-privacy-extension-boundary.md states personal-data metadata is descriptive unless a later opt-in privacy package consumes it and requires alias-driven caller-owned behavior, and src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs provides the existing fail-closed alias-based converter proof the ticket references for opted-in unusable coverage semantics.
- Repository relations remain consistent with the refined scope: .gicket/relations/DC/S8/06FF43K0B0MJF45078STZ3H6DC--06FF43MQ3AXXK2S5TK65X4Y9S8--parentOf.json links the parent story, and .gicket/relations/S8/M4/06FF43MQ3AXXK2S5TK65X4Y9S8--06FF43NAAR3WXH759TVG2RS2M4--blocks.json plus the sibling blocks relations show downstream test and documentation tasks depend on this bounded implementation slice.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A mixed-coverage example where one marked payload field on a satellite is covered and another is not, so the diagnostic output makes per-field alias coverage explicit.
- An opted-in example where the alias exists but coverage is still unusable because the key provider is missing, does not implement IDataVaultEncryptedPayloadKeyProvider, or declines conversion.
- An invalid model-first example proving duplicate personalData field names or duplicate encryptedPayloadAlias values are rejected before diagnostics run.

Risky assumptions
- Approval assumes the additive metadata-first carrier can be introduced without reopening broader code-first authoring or provider-specific privacy behavior, which the ticket correctly keeps out of scope.
- Approval assumes converter-coverage evaluation can stay bounded to the existing alias registration and fail-closed privacy proof rather than expanding into broader runtime privacy orchestration.

AC / test suggestions
- Cover one valid model-first import path where satellite.personalData[] reaches the shared runtime carrier and becomes visible to diagnostics.
- Cover one metadata-first declaration path built through DataVaultMetadataModel and UseDataVaultMetadata(...) that produces the same marked-field evidence as the model-first path.
- Cover the advisory outcome when marked fields exist but no privacy proof is configured for the boundary.
- Cover the fail-closed outcome when privacy is opted in but alias or converter coverage is missing or unusable.
- Cover the unchanged baseline when no marked personal-data fields are declared.

Implementation watchouts
- Keep the carrier additive and keyed by exact logical satellite payload field names plus encryptedPayloadAlias; do not switch to column names, SQL, store types, or provider identifiers.
- Do not let the transport work grow into a new code-first personalData authoring surface, automatic encryption, implicit SaveChanges behavior, or privacy lifecycle ownership.
- Keep diagnostic wording provider-neutral and explicit that unconfigured privacy markers do not imply automatic encryption.
- For opted-in privacy flows, preserve the existing fail-closed posture established by DataVaultEncryptedPayloadValueConverter rather than allowing silent plaintext handling or silent bypass.

Non-blocking notes
- The current branch history shows a prior po-critic round returned the ticket to PO (git log includes 7644808c63 handoff po-critic->po), and the current PO refinement comment explicitly addresses that feedback before handing the ticket back for review.
- The working branch currently contains only ticket metadata changes; no implementation has landed yet, which is normal for this pre-development PO-critic gate.
- Done tickets 06FE4R9ZC210EE5AW4WCWQN32G and 06FE4RASEQZN7XEYH1XR4H06PR provide historical contract context for the personalData schema and fail-closed converter proof, but this ticket no longer depends on an unstated prerequisite to transport personalData into runtime diagnostics.

Split recommendations
- No split recommended. The refined contract now keeps the missing transport and the consuming diagnostics in one bounded slice, which is the smallest complete developer handoff for this behavior.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment