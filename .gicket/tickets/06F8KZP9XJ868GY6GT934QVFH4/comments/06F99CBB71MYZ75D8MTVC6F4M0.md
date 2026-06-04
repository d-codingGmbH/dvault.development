[gicket-bot] PO refinement contract

Summary
- Refined this as a contract-ratification story: the repository already establishes the support-bundle freshness/fingerprint boundary, request-bound ReadShape dependency, and raw model-first exclusion for typed helper generation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Current repository evidence already ratifies the baseline in docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md, docs/model-first-governance.md, src/DCoding.Data.DVault.Analyzers/DataVaultTypedReadModelSourceGenerator.cs, and DataVaultTypedReadModelSourceGeneratorTests.cs; this ticket should document and align that existing contract instead of inventing a new runtime mechanism.
- Typed helper generation is opt-in through DVaultGenerateTypedReadModels=true and consumes exactly one authoritative dvault.support-bundle.v1 additional file; missing, malformed, incompatible-schema, or ambiguous bundle evidence stays in the metadata-source-unavailable boundary.
- Freshness is the resolved diagnostics.explain.metadataSourceKind plus non-empty metadataSourceFingerprint; the optional DVaultTypedReadModelMetadataSourceFingerprint build property is the consumer-owned pin that turns fingerprint drift into a build failure.
- Raw dvault.model.v1 artifacts, source-visible Code-First callbacks, and literal metadata-first declarations are not direct generator inputs; model-first changes matter only after projection into EF/DVault metadata and export into the authoritative support bundle.
- PIT and bridge helpers additionally require representative request-bound readShape.pit or readShape.bridge facts supplied through consumer-owned CreateSupportBundleDiagnostics; satellite helpers do not.

Scope In
- Ratify the authoritative typed-helper input contract around dvault.support-bundle.v1, diagnostics.explain, metadataSourceKind, and metadataSourceFingerprint.
- Define how optional fingerprint pinning via DVaultTypedReadModelMetadataSourceFingerprint enforces freshness/drift for reviewed generator inputs.
- Define the boundary between projected support-bundle evidence and raw dvault.model.v1 artifacts for model-first consumers.
- Define that PIT/bridge helper emission depends on reviewed request-bound ReadShape evidence while satellite helpers rely on translated explain metadata.

Scope Out
- No new runtime freshness tracker, background bundle refresh, or automatic support-bundle routing, publication, or attachment workflow.
- No direct generator parsing of raw dvault.model.v1 files, source callbacks, or literal metadata objects.
- No provider-specific SQL generation, dynamic query compilation, unbounded traversal helpers, or runtime query-shape expansion.
- No new support-bundle schema version or change to the existing IDataVaultReadService runtime boundary.

Open questions
- none

Follow-up questions
- After this ticket is accepted, should ticket 06F8KZPN02NWFGMRC2Q1PKYKDR be re-scoped as closure or verification work given that the repository already contains the DMV1960-DMV1969 generator diagnostic baseline and tests?
- Should the historical incoming blocks relation from done ticket 06F8KZNNS76TD9Z7ESB173FZ68 be cleaned up as ticket-hygiene follow-up even though the current ticket is not marked blocked?

Risks
- The contract is currently split across architecture docs, model-first guidance, analyzer README text, source-generator code, and tests; if downstream tickets paraphrase it loosely, wording drift can recreate ambiguity about freshness versus shape compatibility.
- Ticket state may lag repository state: the blocked diagnostics story is still todo even though the current repository already shows a substantial diagnostic implementation baseline.

Split recommendations
- No new split is needed; the parent epic already separates contract definition, diagnostics implementation or verification, and documentation refresh.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment