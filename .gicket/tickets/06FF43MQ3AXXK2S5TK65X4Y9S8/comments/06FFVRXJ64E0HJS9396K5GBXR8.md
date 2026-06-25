[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is well-structured, but its core scope assumes model-first and metadata-first `personalData` markers already reach runtime/preflight surfaces, and direct source evidence shows those carrier surfaces are not present yet.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `git rev-parse --abbrev-ref HEAD` reports branch `ticket/06FF43MQ3AXXK2S5TK65X4Y9S8-task-diagnose-personaldata-metadata-without-conf` and `git rev-parse HEAD` reports `a3120e15b5cdaf63fd48d2036fd0c6d22c60089f`; `git show --stat --summary a3120e15...` and `git diff --stat main..HEAD` show only `.gicket/tickets/06FF43MQ3AXXK2S5TK65X4Y9S8/**` changes, so no implementation has landed yet, which is acceptable at this gate.
- `src/DCoding.Data.DVault/DataVaultModelSatelliteDeclaration.cs` currently carries only `Name`, `Parent`, `Payload`, `DrivingKeys`, and `Path`; `src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs` `ReadSatellites(...)` and `CreateSatelliteMetadata(...)` read/build only `name`, `parent`, `payload`, and `drivingKeys`, with no `personalData` or `encryptedPayloadAlias` handling.
- `src/DCoding.Data.DVault/Modeling/DataVaultSatelliteMetadata.cs` and `src/DCoding.Data.DVault/Modeling/DataVaultSatellitePayloadMetadata.cs` expose payload and driving-key metadata only; there is no directly observed metadata-first runtime carrier for `personalData` markers or alias coverage.
- Done ticket `06FE4R9ZC210EE5AW4WCWQN32G` says in its developer delivery supplement that parser support, code-first or registry APIs, and EF metadata/diagnostics translation for `personalData` remain separate implementation tickets; done ticket `06FE4RASEQZN7XEYH1XR4H06PR` says current repo code does not yet surface `personalData` metadata into runtime mapping.
- `docs/plans/dvault-model-v1-schema-contract.md` and `docs/getting-started.md` define `personalData[].encryptedPayloadAlias`, while `src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs`, `src/DCoding.Data.DVault.Privacy/DVaultPrivacyServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs` prove that the manual alias-registration and fail-closed privacy seam already exists.

Blocking findings
- The ticket's acceptance criteria and implementation notes assume model-first and metadata-first `personalData` markers are already available on the runtime/preflight path, but direct source evidence shows the parser and metadata model still lack any `personalData` or `encryptedPayloadAlias` carrier. As written, the developer cannot tell whether this ticket must also add those prerequisite transport surfaces or whether another prerequisite ticket should land first.

Required PO actions
- Reconcile the ticket text with current repository reality: either narrow the ticket to diagnostics over an already-existing runtime personal-data representation, or explicitly state that this ticket also owns the missing model-first parser and metadata-first/runtime carrier work.
- If `personalData` transport is meant to land elsewhere, name the authoritative prerequisite ticket and update relations so this ticket no longer relies on an implicit dependency.
- Specify how metadata-first input is expected to present a marked field on the diagnostic path, because the currently observed `DataVaultMetadataModel` and `DataVaultSatelliteMetadata` surfaces do not carry `personalData` evidence.

Open issues ledger
- critic-item-1 [required-po-action] Reconcile the ticket text with current repository reality: either narrow the ticket to diagnostics over an already-existing runtime personal-data representation, or explicitly state that this ticket also owns the missing model-first parser and metadata-first/runtime carrier work.
- critic-item-2 [required-po-action] If `personalData` transport is meant to land elsewhere, name the authoritative prerequisite ticket and update relations so this ticket no longer relies on an implicit dependency.
- critic-item-3 [required-po-action] Specify how metadata-first input is expected to present a marked field on the diagnostic path, because the currently observed `DataVaultMetadataModel` and `DataVaultSatelliteMetadata` surfaces do not carry `personalData` evidence.
- critic-item-4 [blocking-finding] The ticket's acceptance criteria and implementation notes assume model-first and metadata-first `personalData` markers are already available on the runtime/preflight path, but direct source evidence shows the parser and metadata model still lack any `personalData` or `encryptedPayloadAlias` carrier. As written, the developer cannot tell whether this ticket must also add those prerequisite transport surfaces or whether another prerequisite ticket should land first.

Missing examples / edge cases
- A mixed-coverage example where one marked field on the same model boundary is covered and another is not, so advisory versus fail-closed output is unambiguous.
- An opted-in boundary where the alias is registered but conversion is still unusable because the provider is missing, does not implement `IDataVaultEncryptedPayloadKeyProvider`, or declines conversion.
- A metadata-first example showing exactly where the marked-field alias comes from on the diagnostic/preflight path once scope is clarified.

Risky assumptions
- Assuming the documentation-only `personalData` contract is enough for developers to choose a runtime carrier shape without reopening scope.
- Assuming `usable alias/converter coverage` can be evaluated before product defines how model-first and metadata-first marked-field evidence reaches diagnostics.

AC / test suggestions
- Add one explicit acceptance/test example per mode: model-first marker with no `AddDVaultPrivacy(...)` proof -> advisory; opted-in boundary with unusable alias/converter coverage -> fail-closed; no `personalData` markers -> unchanged behavior.
- Once scope is clarified, add a metadata-first example using `UseDataVaultMetadata(...)` that proves the same marked-field scenario reaches the same diagnostic lane as model-first.

Implementation watchouts
- Keep diagnostics provider-neutral and keyed to logical field names plus `encryptedPayloadAlias`, not store columns, SQL, algorithms, or key ids.
- Do not let advisory wording imply automatic encryption; the current privacy proof is still explicit opt-in via `AddDVaultPrivacy(...)`, alias registration, and caller-owned key-provider wiring.
- Avoid stable identifier or message churn because this ticket blocks downstream test and documentation tickets.

Non-blocking notes
- Relations are present in repo: `06FF43K0B0MJF45078STZ3H6DC --parentOf--> 06FF43MQ3AXXK2S5TK65X4Y9S8`, and this ticket blocks `06FF43NAAR3WXH759TVG2RS2M4`, `06FF43NJES6S8NBZVWR4FGHWGW`, and `06FF43QFBQ185N3WPRFD544H00`.
- Current branch head `a3120e15b5cdaf63fd48d2036fd0c6d22c60089f` only contains `.gicket` handoff and lease metadata; lack of product-code changes is not itself a PO blocker at this pre-development gate.

Split recommendations
- If PO wants to keep this ticket narrow, split or relate a prerequisite ticket that surfaces `personalData` and `encryptedPayloadAlias` into the model-first parser and metadata-first runtime/diagnostic representation first, then leave this ticket focused on advisory versus fail-closed diagnostic behavior.
- If PO wants one developer task instead, explicitly fold that prerequisite carrier work into this ticket and update scope and Definition of Done so the hidden dependency is no longer implicit.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment