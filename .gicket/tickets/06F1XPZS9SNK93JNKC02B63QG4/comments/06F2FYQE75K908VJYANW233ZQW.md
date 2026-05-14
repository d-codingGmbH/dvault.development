[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is close, but the persisted contract misstates the current technical-role surface and needs one scope clarification before developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4/description.md currently sets PO Handoff to `ready_for_po_critic` and `## Open Questions` to `- none`.
- src/DCoding.Data.DVault/TechnicalMetadataColumnRole.cs defines four public technical roles: `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs asserts the default contract set contains exactly those four technical roles.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs shows generated hub/link/satellite metadata already carries `HashKey` and, for satellites, `HashDiff`, alongside `LoadTimestamp` and `RecordSource` annotations.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs show the default `AddDVault()` path registers the explicit save service and resolves zero `ISaveChangesInterceptor` instances.
- docs/architecture/dvault-v1-explicit-save-service.md states SaveChanges interceptors remain outside the default v1 write path.
- .gicket/tickets/06F1XPRY3ZDB6W1WQ9ABRRJ2V4/ticket.json marks the related epic `done`.
- git log --oneline -- .gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4 shows only workflow commits around PO handoff/claims, and git show --stat 8e2f87789984 touched only `.gicket` ticket artifacts.

Blocking findings
- The persisted clarification in .gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4/description.md says 'the only existing technical metadata roles are LoadTimestamp and RecordSource', but repository source and tests show a closed four-role set (`HashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`). Because the ticket also requires annotation-based property discovery, that contradiction leaves the developer-facing scope inaccurate.

Required PO actions
- Correct the delivery contract wording so it matches repository truth: current technical roles include `HashKey` and `HashDiff`, while this interceptor slice may auto-populate only `LoadTimestamp` and `RecordSource`.
- Replace any wording that implies there are only two technical roles with wording that explicitly says the slice must ignore the other existing technical roles rather than pretending they do not exist.

Open issues ledger
- critic-item-1 [required-po-action] Correct the delivery contract wording so it matches repository truth: current technical roles include `HashKey` and `HashDiff`, while this interceptor slice may auto-populate only `LoadTimestamp` and `RecordSource`.
- critic-item-2 [required-po-action] Replace any wording that implies there are only two technical roles with wording that explicitly says the slice must ignore the other existing technical roles rather than pretending they do not exist.
- critic-item-3 [blocking-finding] The persisted clarification in .gicket/tickets/06F1XPZS9SNK93JNKC02B63QG4/description.md says 'the only existing technical metadata roles are LoadTimestamp and RecordSource', but repository source and tests show a closed four-role set (`HashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`). Because the ticket also requires annotation-based property discovery, that contradiction leaves the developer-facing scope inaccurate.

Missing examples / edge cases
- A row with `LoadTimestamp` preset but `RecordSource` missing should fill only `RecordSource`.
- A row with `RecordSource` preset but `LoadTimestamp` missing should fill only `LoadTimestamp`.
- A projected technical-column override such as `RecordSource -> SourceSystem` should be exercised on an actual DVault shared-type entity, not only described in prose.

Risky assumptions
- Assuming developers will read 'only existing technical metadata roles' as shorthand instead of literal repository fact.

AC / test suggestions
- Keep an explicit test that `AddDVault()` still resolves zero `ISaveChangesInterceptor` instances after the new opt-in API is added.
- Add SQLite coverage for the mixed partial-manual cases where one metadata value is preset and the other is missing, across both sync and async save paths.
- Add a SQLite/shared-type proof that annotation-driven discovery honors an effective technical-column rename such as `SourceSystem`.

Implementation watchouts
- Hub, link, and satellite generated entities already contain other technical roles (`HashKey`, `HashDiff`), so annotation-based discovery cannot equate 'technical' with 'safe to auto-populate'.
- The existing `IDataVaultLoadTimestampResolver` and `IDataVaultRecordSourceResolver` contracts are explicit-save-request based, so the interceptor slice should not silently blur the explicit save-service boundary.

Non-blocking notes
- The formal open-questions gate is satisfied because the persisted contract has `## Open Questions` set to `- none`.
- The related epic being `done` supports treating that relation as historical rather than a current delivery blocker.
- No code or product-doc changes accompanied the PO handoff; the review surface is the ticket contract plus existing repository evidence.

Split recommendations
- No split is needed once the contract wording is corrected; keep this as one bounded interceptor slice.
- If scope grows into `HashKey`/`HashDiff` population, non-Added behavior, or broader batch/correlation/tenant audit metadata, split that work into follow-up tickets.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment