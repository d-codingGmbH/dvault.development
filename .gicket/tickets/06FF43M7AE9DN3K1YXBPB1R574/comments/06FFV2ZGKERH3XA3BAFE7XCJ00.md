[gicket-bot] PO refinement contract

Summary
- Refined the ticket to a bounded provider-neutral privacy coverage-report task in `DCoding.Data.DVault.Privacy`: inspect manual alias registration plus EF model mappings, keep metadata-diagnostic and docs/test expansion in sibling tickets, and leave no blocking PO questions. No child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence shows the current runtime privacy seam is manual alias registration through `DataVaultPrivacyOptions.RegisterEncryptedPayloadAlias(...)` / `IDataVaultPrivacyConfiguration.EncryptedPayloadAliases` plus explicit `DataVaultEncryptedPayloadValueConverter` mapping; this ticket should report over that existing seam instead of inventing a new privacy metadata API.
- The report belongs in the optional `DCoding.Data.DVault.Privacy` package and stays provider-neutral, structural, and redaction-safe; it must not call providers, inspect database rows, or surface provider-native encryption details.
- Coverage is EF-model based and may include ordinary EF entities as well as DVault entities, because the shipped privacy proof applies the converter to mapped properties rather than to a dedicated Data Vault-only runtime pipeline.
- One alias entry may legitimately cover zero, one, or many mapped properties; `registered-but-unmapped` is a first-class report outcome rather than a fatal runtime error.
- Live graph evidence is coherent: this ticket is a child of story `06FF43K0B0MJF45078STZ3H6DC` and currently blocks `06FF43NAAR3WXH759TVG2RS2M4`, `06FF43NJES6S8NBZVWR4FGHWGW`, and `06FF43QFBQ185N3WPRFD544H00`; no relation cleanup is required in this refinement pass.

Scope In
- A public structured coverage-report surface in `DCoding.Data.DVault.Privacy` that reads configured encrypted-payload aliases and EF model mappings that use `DataVaultEncryptedPayloadValueConverter`.
- Deterministic alias-level status facts such as covered vs. unmapped and key-provider posture (`none`, marker-only, or encrypted-payload-capable) plus the mapped entity/property identifiers for covered aliases.
- A redaction-safe human-readable rendering in the existing report style plus machine-readable members suitable for automation and tests.
- Any narrow converter/report API additions needed to recover the alias used by a mapped privacy converter without brittle expression-tree reverse engineering.
- Minimal implementation-local tests, XML docs, and privacy API snapshot updates needed to make the report surface consumable.

Scope Out
- Automatic ingestion or validation of model-first or metadata-first `personalData` markers; that belongs to sibling ticket `06FF43MQ3AXXK2S5TK65X4Y9S8`.
- Live encryption/decryption probes, key-availability checks against caller-owned infrastructure, or any path that inspects keys, plaintext, ciphertext, or runtime policy internals.
- Provider-native encryption discovery, SQL/provider/store-type reporting, database scans, or provider-specific branch logic.
- Production checklist changes or quickstart/example expansion beyond minimal API/XML docs; those remain in `06FF43QFBQ185N3WPRFD544H00` and `06FF43NJES6S8NBZVWR4FGHWGW`.
- Compliance claims, retention/deletion workflow ownership, crypto-shredding execution, or broader privacy orchestration.

Open questions
- none

Follow-up questions
- After sibling ticket `06FF43MQ3AXXK2S5TK65X4Y9S8` lands, should the same display vocabulary be reused when `personalData` metadata and runtime alias/converter coverage disagree?
- Once the structural report exists, should a later support-bundle or design-time ticket expose it through broader diagnostics/export flows, or should it remain a privacy-package-only surface?
- When the blocked quickstart/checklist tickets resume, do adopters need one canonical example of invoking the report, or is API discovery plus checklist wording sufficient?

Risks
- Trying to fold `personalData` metadata diagnostics into this ticket will turn a bounded report task into the broader modeling ticket `06FF43MQ3AXXK2S5TK65X4Y9S8`.
- If the implementation derives alias data by inspecting value-converter expressions instead of adding an explicit seam, the report will be brittle and hard to keep deterministic.
- If the report emits provider/store-type or conversion-output details, it will violate the established privacy boundary and leak more than the story allows.
- Current downstream docs/test tasks remain blocked until both this ticket and the sibling metadata-diagnostics ticket are settled, so scope creep here delays multiple follow-on tickets.

Split recommendations
- Do not split the core report work if it stays limited to alias registry inspection, model-mapping coverage, and redaction-safe output in `DCoding.Data.DVault.Privacy`.
- Keep missing-`personalData` or missing-alias diagnostics in sibling ticket `06FF43MQ3AXXK2S5TK65X4Y9S8` instead of widening this ticket.
- If product later wants support-bundle export, build-time analyzer hooks, or provider-specific runtime validation, create separate follow-up tickets instead of extending this report surface.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment