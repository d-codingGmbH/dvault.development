[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a narrow follow-on for the existing optional privacy boundary: implement one explicit provider-neutral EF Core encrypted payload conversion proof inside `DCoding.Data.DVault.Privacy`, using the existing alias/key-provider opt-in seam and excluding metadata expansion, provider-native encryption, and compliance workflow scope.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the activation boundary: this work belongs in the optional `DCoding.Data.DVault.Privacy` package behind `AddDVaultPrivacy(...)`, not in default `AddDVault()` behavior.
- The visible v1 alias seam is manual registration through `DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(...)` plus `UseCallerOwnedKeyProvider(...)`; this ticket should use that seam rather than block on future `personalData` metadata ingestion.
- The ticket is the narrow v0.44 proof approved by `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md`: explicit caller-invoked encrypted payload mapping, not provider-native encryption, not automatic `SaveChanges` behavior, and not DVault-owned key lifecycle.

Scope In
- One provider-neutral encrypted payload proof in `DCoding.Data.DVault.Privacy` using an explicit EF Core `ValueConverter` lane or functionally equivalent explicit mapping for one representative DVault payload attribute.
- Alias-driven caller-owned key-provider resolution using the existing `EncryptedPayloadAliases` and `IDataVaultPrivacyKeyProvider` opt-in registration seam.
- Success-path and fail-closed automated coverage proving explicit round-trip behavior without DVault owning keys.
- Any narrow public API, XML docs, API snapshot updates, and privacy-package tests needed to make the proof consumable.

Scope Out
- Model-first or code-first `personalData` metadata projection, parser/importer/exporter work, or automatic alias discovery from `dvault.model.v1`.
- Provider-specific encryption optimizations, provider-native encryption features, encryption DDL, provider capability negotiation, or provider-name branching.
- Compliance claims, key management, key rotation, key destruction workflows, retention, purge, redaction suites, or background privacy orchestration.
- Implicit privacy behavior on ordinary `AddDVault()`, default `IDataVaultSaveService`, default `IDataVaultReadService`, or EF `SaveChanges`.

Open questions
- none

Follow-up questions
- When `personalData` metadata is wired into runtime/model projection, should it validate against or populate the same encrypted-payload-alias registry instead of requiring manual registration?
- After the shared provider-neutral proof lands, which named provider, if any, should receive the first provider-specific optimization or provider-native encryption follow-up ticket?
- Should a later ticket add redaction-safe diagnostics for privacy strategy selection and failure categories beyond the minimum proof needed here?

Risks
- Without a hard scope boundary, implementation could sprawl into provider-native encryption, compliance claims, or privacy workflow automation that the architecture docs explicitly exclude.
- Because current repository code does not yet surface `personalData` metadata into runtime mapping, any attempt to add automatic metadata-driven behavior here will turn this into a broader metadata ticket.
- If the implementation introduces public privacy conversion types, API snapshot and package-contract maintenance across both `net8.0` and `net10.0` lines will be part of the delivery cost.

Split recommendations
- Do not split the ticket if it stays limited to manual alias registration, one representative encrypted payload mapping lane, and bounded tests/docs.
- If implementation pressure grows toward metadata projection, broader diagnostics, read/write privacy workflow helpers, or provider-specific execution lanes, split those into follow-up tickets instead of widening this proof ticket.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment