<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket to a narrow follow-on for the existing optional privacy boundary: implement one explicit provider-neutral EF Core encrypted payload conversion proof inside `DCoding.Data.DVault.Privacy`, using the existing alias/key-provider opt-in seam and excluding metadata expansion, provider-native encryption, and compliance workflow scope.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already fixes the activation boundary: this work belongs in the optional `DCoding.Data.DVault.Privacy` package behind `AddDVaultPrivacy(...)`, not in default `AddDVault()` behavior.
- The visible v1 alias seam is manual registration through `DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(...)` plus `UseCallerOwnedKeyProvider(...)`; this ticket should use that seam rather than block on future `personalData` metadata ingestion.
- The ticket is the narrow v0.44 proof approved by `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md`: explicit caller-invoked encrypted payload mapping, not provider-native encryption, not automatic `SaveChanges` behavior, and not DVault-owned key lifecycle.

### Scope In
- One provider-neutral encrypted payload proof in `DCoding.Data.DVault.Privacy` using an explicit EF Core `ValueConverter` lane or functionally equivalent explicit mapping for one representative DVault payload attribute.
- Alias-driven caller-owned key-provider resolution using the existing `EncryptedPayloadAliases` and `IDataVaultPrivacyKeyProvider` opt-in registration seam.
- Success-path and fail-closed automated coverage proving explicit round-trip behavior without DVault owning keys.
- Any narrow public API, XML docs, API snapshot updates, and privacy-package tests needed to make the proof consumable.

### Scope Out
- Model-first or code-first `personalData` metadata projection, parser/importer/exporter work, or automatic alias discovery from `dvault.model.v1`.
- Provider-specific encryption optimizations, provider-native encryption features, encryption DDL, provider capability negotiation, or provider-name branching.
- Compliance claims, key management, key rotation, key destruction workflows, retention, purge, redaction suites, or background privacy orchestration.
- Implicit privacy behavior on ordinary `AddDVault()`, default `IDataVaultSaveService`, default `IDataVaultReadService`, or EF `SaveChanges`.

## Acceptance Criteria
- A consumer can opt into `DCoding.Data.DVault.Privacy`, register a caller-owned key provider plus at least one encrypted payload alias, and configure one representative payload property to persist encrypted provider values through ordinary EF Core/DVault mapping without provider-specific branching.
- The proof performs explicit round-trip conversion through application-owned crypto behavior resolved by encrypted payload alias, and DVault does not create, store, rotate, or otherwise own key material.
- If alias registration is missing, key material is unavailable, or the explicit privacy conversion cannot be approved for the requested alias, the lane fails explicitly and does not silently store plaintext, bypass privacy behavior, or substitute hashing/provider-native encryption.
- Automated tests cover the opt-in registration path, successful conversion path, and at least one fail-closed path on the existing shared provider-neutral test baseline; SQLite-backed EF proof is sufficient.
- Documentation and package-facing text continue to describe `DCoding.Data.DVault.Privacy` as an optional provider-neutral privacy seam/proof package, not as a compliance or automatic encryption feature.

## Definition of Done
- Implementation ships in `DCoding.Data.DVault.Privacy` and preserves the current dependency boundary: no core or provider project starts referencing the privacy package, and the shared proof adds no provider-specific branch logic.
- If new public surface is introduced, the privacy public API snapshot and related tests are updated and pass.
- Automated tests demonstrate explicit success-path round-trip behavior and explicit fail-closed behavior.
- Any added docs or XML comments state the feature as explicit opt-in encrypted payload conversion proof with caller-owned keys and non-goals consistent with the privacy boundary contract.

## Implementation Notes
- Current source already contains only the privacy skeleton: `AddDVaultPrivacy(...)`, alias registration, a marker `IDataVaultPrivacyKeyProvider`, API snapshot coverage, and basic registration tests. There is no existing encrypted payload converter or runtime metadata-driven privacy mapping yet.
- Current docs already approve a provider-neutral value-conversion proof and explicitly exclude provider-native encryption, automatic `SaveChanges`, and DVault-owned key lifecycle from this ticket’s scope.
- Current repo patterns already use EF Core value converters and metadata assertions elsewhere, so the proof should follow the existing translator/schema test style instead of inventing a separate ad hoc testing model.
- `IDataVaultPrivacyKeyProvider` is currently marker-only, so the implementation will likely need a narrow alias-based request/result seam or companion API for encrypt/decrypt behavior. The exact type names are implementation detail, not a PO blocker.
- Keep manual alias registration as the v1 default for this ticket. Future `personalData` metadata consumption should be handled by separate metadata-oriented follow-up work, not folded into this proof.

## Open Questions
- none

## Follow-Up Questions
- When `personalData` metadata is wired into runtime/model projection, should it validate against or populate the same encrypted-payload-alias registry instead of requiring manual registration?
- After the shared provider-neutral proof lands, which named provider, if any, should receive the first provider-specific optimization or provider-native encryption follow-up ticket?
- Should a later ticket add redaction-safe diagnostics for privacy strategy selection and failure categories beyond the minimum proof needed here?

## Risks
- Without a hard scope boundary, implementation could sprawl into provider-native encryption, compliance claims, or privacy workflow automation that the architecture docs explicitly exclude.
- Because current repository code does not yet surface `personalData` metadata into runtime mapping, any attempt to add automatic metadata-driven behavior here will turn this into a broader metadata ticket.
- If the implementation introduces public privacy conversion types, API snapshot and package-contract maintenance across both `net8.0` and `net10.0` lines will be part of the delivery cost.

## Split Recommendations
- Do not split the ticket if it stays limited to manual alias registration, one representative encrypted payload mapping lane, and bounded tests/docs.
- If implementation pressure grows toward metadata projection, broader diagnostics, read/write privacy workflow helpers, or provider-specific execution lanes, split those into follow-up tickets instead of widening this proof ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Scope: implement a narrow encrypted attribute conversion proof using EF Core value conversion or equivalent provider-neutral mapping. Acceptance: encryption behavior is explicit and testable without owning keys.