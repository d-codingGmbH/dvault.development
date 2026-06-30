[gicket-bot] PO-critic review contract

Summary
- The ticket is mostly well-scoped and repository-backed, but the delivery contract currently relies on one existing API/type reference that was not directly source-verifiable, so it should return to PO for correction before dev handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `gicket-read-ticket-comments` returned 10 comments; the relevant history is automation and PO handoff only, with a published handoff to `po-critic` and no later human clarification comment.
- `git status --short --branch` returned `## HEAD (no branch)` and `git rev-parse HEAD` returned `dc17993f85022edfdf0f8513a70af49003dd4ad1`, matching the scratch-source ref and showing a clean review surface.
- `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` directly states the shared lane is opt-in, alias-driven, provider-neutral privacy behavior and that provider-native encryption remains guidance-only rather than shared runtime behavior.
- `docs/plans/dvault-model-v1-schema-contract.md` directly defines `satellites[].personalData[].encryptedPayloadAlias` and its validation rules, including duplicate-alias and non-payload rejection.
- `docs/releases/v0.44.0.md` and `docs/releases/v0.50.0.md` both directly document the opt-in privacy package, caller-owned `IDataVaultEncryptedPayloadKeyProvider`, fail-closed conversion, and no provider-native encrypted DDL/runtime dispatch.
- `src/DCoding.Data.DVault.Privacy/IDataVaultEncryptedPayloadKeyProvider.cs`, `DataVaultEncryptedPayloadConversionDirection.cs`, and `DataVaultEncryptedPayloadValueConverter.cs` directly verify the caller-owned `ConvertEncryptedPayload(...)` seam, explicit `Encrypt`/`Decrypt` directions, and fail-closed checks for missing alias/provider or declined conversion.
- `git grep -n` located `personal-data-privacy-proof-missing` in `src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:1910` and matching unit assertions in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:468,498,523,545,566,589`.
- `repository-read-text` for `src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeEncryptionBoundaryFact.cs` returned `REPOSITORY-FILE-NOT-FOUND`, so the ticket's cited source anchor for that named type was not directly verifiable.

Blocking findings
- The delivery contract states that current code already includes `DataVaultProviderNativeEncryptionBoundaryFact`, but the cited source path `src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeEncryptionBoundaryFact.cs` could not be read because the file does not exist. Because the ticket depends on an existing public API/type reference to define compatibility and scope, PO needs to correct this citation or restate it as future work instead of present-state evidence.

Required PO actions
- Update the implementation note and any dependent acceptance/DoD wording so the provider-native boundary diagnostics surface is backed by a directly verifiable source reference. Either cite the actual existing type/path or explicitly state that this ticket must introduce that surface.
- If the intended diagnostics carrier is not a public type named `DataVaultProviderNativeEncryptionBoundaryFact`, rewrite the ticket to name the actual source-backed contract so devs do not have to infer the target from prose.

Open issues ledger
- critic-item-1 [required-po-action] Update the implementation note and any dependent acceptance/DoD wording so the provider-native boundary diagnostics surface is backed by a directly verifiable source reference. Either cite the actual existing type/path or explicitly state that this ticket must introduce that surface.
- critic-item-2 [required-po-action] If the intended diagnostics carrier is not a public type named `DataVaultProviderNativeEncryptionBoundaryFact`, rewrite the ticket to name the actual source-backed contract so devs do not have to infer the target from prose.
- critic-item-3 [blocking-finding] The delivery contract states that current code already includes `DataVaultProviderNativeEncryptionBoundaryFact`, but the cited source path `src/DCoding.Data.DVault.Privacy/DataVaultProviderNativeEncryptionBoundaryFact.cs` could not be read because the file does not exist. Because the ticket depends on an existing public API/type reference to define compatibility and scope, PO needs to correct this citation or restate it as future work instead of present-state evidence.

Missing examples / edge cases
- Clarify whether `unsupported providers or shapes` should be expressed through the same already-shipped diagnostics surface as `personal-data-privacy-proof-missing` / `personal-data-privacy-coverage-unusable`, or through a new provider-native boundary fact surface if that is still future work.
- Clarify what counts as `missing observable converter coverage` when a model has personal-data metadata but only some marked fields opt into the value-converter seam.

Risky assumptions
- The current contract assumes the named provider-native boundary fact is already present and citable in source, but direct source verification only confirmed the release-note prose and the other privacy APIs.
- If PO leaves the incorrect type citation in place, developers may either search for a non-existent API or widen scope by inventing a new diagnostics surface without a clear contract anchor.

AC / test suggestions
- Make one acceptance/test statement explicitly source-backed: the provider-native boundary status must point to a concrete diagnostics contract or type, not just to release-note prose.
- Keep direct fail-closed checks for missing alias registration, marker-only or missing key-provider posture, declined conversion, and the existing diagnostic-code distinction between `personal-data-privacy-proof-missing` and `personal-data-privacy-coverage-unusable`.

Implementation watchouts
- Do not let the missing diagnostics-type citation turn this into provider-native crypto implementation work; the verified docs still constrain v1 to guidance-only native encryption and opt-in alias-driven conversion.
- If the actual diagnostics carrier is differently named or internal, align ticket text, docs, and code references in one pass so the dev handoff does not carry a documentation/source mismatch forward.

Non-blocking notes
- Aside from the unverifiable boundary-fact citation, the ticket is otherwise well-bounded: `## Open Questions` is `none`, the opt-in privacy seam is explicit, and the schema plus caller-owned conversion anchors are source-backed.
- The scratch review surface was clean at `dc17993f85022edfdf0f8513a70af49003dd4ad1`; there was no evidence of pending repository edits affecting this review.

Split recommendations
- Keep future provider-native encryption work split one provider and one exact capability per ticket, as the current contract already recommends.
- If the missing provider-native boundary diagnostics surface turns out to be separable from the alias-driven privacy seam, consider a narrow follow-up ticket for that diagnostics contract rather than broadening this story.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment