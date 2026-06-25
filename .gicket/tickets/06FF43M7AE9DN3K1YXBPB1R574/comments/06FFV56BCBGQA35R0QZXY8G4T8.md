[gicket-bot] PO-critic review contract

Summary
- Delivery contract is source-aligned, bounded to the existing privacy seam, and has no unresolved PO questions; approve for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FF43M7AE9DN3K1YXBPB1R574/description.md` contains `PO Handoff` decision `ready_for_po_critic` and `## Open Questions` = `- none`, so the current contract is eligible for dev approval.
- Comment history under `.gicket/tickets/06FF43M7AE9DN3K1YXBPB1R574/comments/` is bot-only in the inspected files; the latest substantive content is the PO refinement comment `06FFV2ZGKERH3XA3BAFE7XCJ00.md`, and newer files are lease/handoff operational comments rather than new product requirements.
- `src/DCoding.Data.DVault.Privacy/DataVaultPrivacyOptions.cs` and `src/DCoding.Data.DVault.Privacy/IDataVaultPrivacyConfiguration.cs` expose the exact alias-registry seam the ticket targets: `RegisterEncryptedPayloadAlias(...)`, `EncryptedPayloadAliases`, and `KeyProvider`.
- `src/DCoding.Data.DVault.Privacy/DataVaultEncryptedPayloadValueConverter.cs` already enforces the three posture-relevant cases without live provider probing: missing alias, missing `IDataVaultPrivacyKeyProvider`, and marker-only provider that is not `IDataVaultEncryptedPayloadKeyProvider`.
- `tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.Privacy.approved.txt` shows the current public converter surface is only the constructor, which matches the ticket's note that any alias inspection must be added through a narrow explicit seam instead of inferred from existing public API.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultEncryptedPayloadValueConverterTests.cs` applies `HasConversion(new DataVaultEncryptedPayloadValueConverter(...))` to plain EF entity property `CustomerProfilePrivacyProofRow.EmailAddress`, which supports the contract's EF-model-based scope for ordinary EF entities as well as DVault entities.
- `docs/architecture/dvault-v1-optional-privacy-extension-boundary.md` plus `src/DCoding.Data.DVault.Privacy/DVaultPrivacyServiceCollectionExtensions.cs` keep privacy opt-in and package-local, consistent with the ticket's requirement to stay in `DCoding.Data.DVault.Privacy` and not extend default `AddDVault()` behavior.
- Live graph evidence is present in `.gicket/relations/DC/74/06FF43K0B0MJF45078STZ3H6DC--06FF43M7AE9DN3K1YXBPB1R574--parentOf.json` and the three `blocks` relation files under `.gicket/relations/74/*` for `06FF43NAAR3WXH759TVG2RS2M4`, `06FF43NJES6S8NBZVWR4FGHWGW`, and `06FF43QFBQ185N3WPRFD544H00`.
- `git rev-parse --abbrev-ref HEAD` returns `ticket/06FF43M7AE9DN3K1YXBPB1R574-task-add-privacy-key-alias-coverage-report`, HEAD is `e02d901d1e`, and `git diff --name-only 6902ed6c0a..HEAD` shows only `.gicket/tickets/06FF43M7AE9DN3K1YXBPB1R574/*` metadata changes since PO handoff.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract asks for deterministic machine-readable output but does not illustrate the exact ordering when one alias covers multiple mapped properties across multiple entity types.
- The report behavior for an empty alias registry is not illustrated; this is likely non-blocking, but one example would pin the empty-result display and machine-readable shape.
- The contract says Data Vault metadata names are additive when present, but it does not exemplify how they should appear alongside the required entity/property identifiers.

Risky assumptions
- The implementation team is expected to choose a stable ordering rule for aliases and covered mappings without additional PO direction because the contract requires determinism but does not name the exact sort key.
- The exact public entrypoint shape for creating the report is intentionally left open; approval assumes the defined inputs, outputs, and package boundary are sufficient for developer design choice.
- Parent story `06FF43K0B0MJF45078STZ3H6DC` is still `todo` with `needs-po`, but approval assumes that is not a blocker because the active relation is `parentOf` and there is no current `blocks` relation from the story to this ticket.

AC / test suggestions
- Add one explicit deterministic-order test where two aliases exist and one alias covers multiple properties across more than one entity type.
- Add one explicit empty-registry or no-covered-mappings test if the team wants the zero-result display string and machine-readable output pinned.
- Assert that additive Data Vault metadata-name context, when present, supplements rather than replaces the required entity/property identifiers.

Implementation watchouts
- Do not widen this ticket into `personalData` metadata diagnostics; sibling ticket `06FF43MQ3AXXK2S5TK65X4Y9S8` owns that mismatch-diagnostic scope.
- Do not move the surface into core diagnostics or default `AddDVault()` behavior; repo boundary evidence keeps privacy opt-in inside `DCoding.Data.DVault.Privacy`.
- Do not recover alias coverage by parsing value-converter expressions; current source and public API snapshot justify a narrow explicit alias-inspection seam instead.
- Do not emit provider/store-type details, payload values, ciphertext, or key material; the repository privacy boundary only supports redaction-safe alias and mapped-property reporting.

Non-blocking notes
- No implementation appears to have landed on the owner branch yet; the post-handoff diff observed from `6902ed6c0a..HEAD` is ticket metadata only, which is acceptable for this pre-development PO gate.
- Downstream blocked tickets remain `todo`, which is consistent with this ticket's contract being the prerequisite report surface rather than a combined docs/test rollout.

Split recommendations
- No split is needed if delivery stays limited to alias-registry inspection, EF mapping coverage, key-provider posture classification, and redaction-safe output in `DCoding.Data.DVault.Privacy`.
- Keep `personalData` mismatch diagnostics in `06FF43MQ3AXXK2S5TK65X4Y9S8` and keep quickstart/checklist expansion in the already linked downstream tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment