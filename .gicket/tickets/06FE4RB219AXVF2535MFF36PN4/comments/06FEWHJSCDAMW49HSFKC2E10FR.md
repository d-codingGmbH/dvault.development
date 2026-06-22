[gicket-bot] PO-critic review contract

Summary
- Provider matrix and non-goals are well specified, but the ticket is not ready for handoff because its core test target depends on an upstream encrypted-payload seam that is still unrefined and not directly observable in the repository.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FE4RB219AXVF2535MFF36PN4/description.md` says this ticket must target the privacy-specific encrypted payload mapping path from ticket `06FE4RASEQZN7XEYH1XR4H06PR` and keeps `## Open Questions` at `none`.
- `rg -n '06FE4RB219AXVF2535MFF36PN4' /mnt/c/Projects/DVault/.gicket/relations -g '*.json'` showed only relation files with `06FE4R9PP99G6Q1PTPK4TKD460`, `06FE4RAGWXQCQFCTX7QW1T9NAC`, `06FE4SENE1ZV45P8DKRQTMG0A0`, and `06FE4RBK2MJBS5K3C15JTB8Z9W`; a separate `rg` for `06FE4RASEQZN7XEYH1XR4H06PR` plus this ticket returned no match.
- `src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs` and `src/DCoding.Data.DVault.Privacy/IDataVaultPrivacyConfiguration.cs` expose alias registration and a key-provider placeholder only; `rg -n 'EncryptedPayload|encrypted payload|encryptedPayloadAlias'` found no encrypted-payload provider-mapping translator or mapping tests beyond those skeleton files.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs` maps `DataVaultPropertyRole.Payload` to `DataVaultLogicalPropertyKind.PayloadText`, `src/DCoding.Data.DVault/DataVaultLogicalPropertyKind.cs` defines `PayloadText` but no privacy-specific encrypted payload kind, and existing tests in `DataVaultProviderCapabilityProfileTests.cs`, `DataVaultEfMetadataTranslationTests.cs`, and `DataVaultPrivacyServiceCollectionExtensionsTests.cs` cover the ordinary payload baseline plus privacy DI skeleton only.
- `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD` lists only `.gicket/tickets/06FE4RB219AXVF2535MFF36PN4/...` files, so the current branch carries ticket refinement only and no landed repository evidence for the promised encrypted-payload mapping lane.

Blocking findings
- The main test target is not anchored: the contract tells developers to test the encrypted-payload mapping lane from `06FE4RASEQZN7XEYH1XR4H06PR`, but that ticket is still `todo`/`needs-po` and the repository does not yet expose a concrete mapping API, annotation, or translator seam for that lane.
- Because no persisted Gicket relation links this ticket to `06FE4RASEQZN7XEYH1XR4H06PR`, sequencing does not enforce the dependency the prose contract relies on and developers can satisfy the ticket with generic `PayloadText` coverage instead of the intended privacy-specific lane.
- If workflow is intentionally treating this as closure-only or no-work-required, the persisted contract contradicts that mode because its acceptance criteria and definition of done still require adding new automated tests and having them pass.

Required PO actions
- Refine or complete ticket `06FE4RASEQZN7XEYH1XR4H06PR`, then update this ticket to name the exact source seam, public type or member, or translated metadata surface that the tests must bind to.
- Persist the dependency between `06FE4RASEQZN7XEYH1XR4H06PR` and `06FE4RB219AXVF2535MFF36PN4` as a Gicket relation or equivalent blocked sequencing state instead of leaving it as prose only.
- Clarify whether this ticket is standard pre-development work or a closure-only or no-work-required audit case; the current contract text still describes new implementation and test execution.

Open issues ledger
- critic-item-1 [required-po-action] Refine or complete ticket `06FE4RASEQZN7XEYH1XR4H06PR`, then update this ticket to name the exact source seam, public type or member, or translated metadata surface that the tests must bind to.
- critic-item-2 [required-po-action] Persist the dependency between `06FE4RASEQZN7XEYH1XR4H06PR` and `06FE4RB219AXVF2535MFF36PN4` as a Gicket relation or equivalent blocked sequencing state instead of leaving it as prose only.
- critic-item-3 [required-po-action] Clarify whether this ticket is standard pre-development work or a closure-only or no-work-required audit case; the current contract text still describes new implementation and test execution.
- critic-item-4 [blocking-finding] The main test target is not anchored: the contract tells developers to test the encrypted-payload mapping lane from `06FE4RASEQZN7XEYH1XR4H06PR`, but that ticket is still `todo`/`needs-po` and the repository does not yet expose a concrete mapping API, annotation, or translator seam for that lane.
- critic-item-5 [blocking-finding] Because no persisted Gicket relation links this ticket to `06FE4RASEQZN7XEYH1XR4H06PR`, sequencing does not enforce the dependency the prose contract relies on and developers can satisfy the ticket with generic `PayloadText` coverage instead of the intended privacy-specific lane.
- critic-item-6 [blocking-finding] If workflow is intentionally treating this as closure-only or no-work-required, the persisted contract contradicts that mode because its acceptance criteria and definition of done still require adding new automated tests and having them pass.

Missing examples / edge cases
- A concrete example of the exact encrypted-payload mapping surface to assert once available, such as the public annotation, type, or member names exposed to tests.
- A concrete unsupported-profile example for the negative path, including the expected profile name and missing-capability wording that should appear in the deterministic diagnostic.
- An explicit answer on whether unit or metadata translation tests alone satisfy the matrix or whether any live-schema or provider-gated assertions are required for acceptance.

Risky assumptions
- Assumes `06FE4RASEQZN7XEYH1XR4H06PR` will land a testable provider-neutral encrypted-payload seam without changing the agreed `PayloadText` storage baseline.
- Assumes developers will not stop at existing generic provider mapping tests unless this ticket names a concrete privacy-specific hook.
- Assumes the shared MySQL capability profile is still sufficient coverage without a separate provider-name-selection assertion.

AC / test suggestions
- Add one acceptance criterion or implementation note that cites the exact source seam to assert after the upstream conversion proof lands.
- State one canonical negative test shape and the expected diagnostic wording pattern so unsupported-case coverage is unambiguous.
- State explicitly whether live-provider or live-schema coverage is optional follow-up evidence or part of this ticket's required acceptance.

Implementation watchouts
- Do not let the ticket be satisfied by ordinary `PayloadText` provider coverage alone; the privacy-specific lane must be distinguishable from the existing baseline.
- Keep MySQL alias cases on the shared `mysql-pomelo-v1` capability profile unless the ticket is explicitly widened to provider-name selection behavior.
- Do not widen this ticket into provider-native encryption features or non-text ciphertext storage.

Non-blocking notes
- The provider matrix itself is grounded by `src/DCoding.Data.DVault/DataVaultProviderCapabilityProfiles.cs`, which already declares the current store-type baseline for SQLite, PostgreSQL, SQL Server, Oracle, DB2, and MySQL.
- The current contract has `## Open Questions` = `none`.
- Incoming blockers `06FE4RAGWXQCQFCTX7QW1T9NAC` and `06FE4SENE1ZV45P8DKRQTMG0A0` are already `done`.

Split recommendations
- Keep the finite provider-matrix assertions in this ticket once the upstream encrypted-payload seam is concretely anchored.
- If `06FE4RASEQZN7XEYH1XR4H06PR` slips or expands its API surface, mark this ticket explicitly blocked-by that work instead of letting it proceed against generic payload coverage.
- If live provider coverage becomes necessary, keep the unit or metadata matrix here and split heavier provider-gated smoke coverage into a follow-up ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment