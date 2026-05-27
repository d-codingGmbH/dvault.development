[gicket-bot] PO-critic review contract

Summary
- Contract is largely refined and has no open questions, but the developer handoff boundary is still ambiguous because current public model-first PIT declarations are hub-only and the ticket does not say whether that declaration path must change or remain excluded.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8/description.md` contains `## Open Questions` = `none` and the PO handoff decision `ready_for_po_critic`.
- `.gicket/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8/comments/06F6J8JY0QEWFM50VNJN83Q9PM.md` states the incoming `blocks` relation from `06F5Q90KC6JGQPSP285XQYSPK8` is historical; `.gicket/tickets/06F5Q90KC6JGQPSP285XQYSPK8/ticket.json` shows that related ticket is `done`.
- `git log --oneline -n 8` on branch `ticket/06F5Q90SX5AQ07M4PQKDR4BZD8-story-support-link-parent-pit-maintenance-and-re` shows only ticket-orchestration commits (`3a7352410`, `cdf25f64b`, `48b11aa5e`, `525b225ec`), and `git diff --stat develop..HEAD -- ':(exclude).gicket/**'` returned no output, so there is no implementation evidence on this branch yet.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs` already accepts `DataVaultPitMetadata` parents of kind `Hub` or `Link` and validates PIT satellites against the exact declared parent, which supports the PO claim that the direct metadata/registry path can represent link-parent PITs.
- `docs/model-first-governance.md` says model-first is part of the current public baseline, but `docs/plans/dvault-model-v1-schema-contract.md` defines PIT declarations with a required `hub` field and says referenced satellites must belong to that hub.
- `src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs` reads PIT declarations through `ReadRequiredString(..., "hub", ...)` and `ValidatePitDeclarations(...)` rejects PIT satellites unless `parent.kind == "hub"` and `parent.name == pit.Hub`, so the current model-first declaration path is still hub-only.
- `README.md`, `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs`, `src/DCoding.Data.DVault/DataVaultPitMaintenanceShapeValidator.cs`, `src/DCoding.Data.DVault/DataVaultPitReadPipeline.cs`, and current PIT tests still contain hub-only or `link-based PIT` unsupported language, matching the story's stated implementation/doc debt.

Blocking findings
- The ticket does not explicitly resolve whether link-parent PIT support must include the public model-first declaration path or whether model-first PITs remain hub-only. Current public guidance and parser/schema evidence are hub-only, so the developer handoff boundary is ambiguous.
- Because the contract asks for public documentation updates but does not name this declaration-path decision, developers could ship README/release-note wording that implies general link-parent PIT support while `dvault.model.v1` PIT artifacts still cannot express it.

Required PO actions
- Clarify in the delivery contract whether model-first `dvault.model.v1` PIT declarations/import-export/diagnostics are in scope for this story or explicitly out of scope.
- If model-first PIT support is out of scope, add explicit scope-out and documentation language that link-parent PIT support applies only to the existing `DataVaultPitMetadata`/registry-backed declaration path for this ticket.
- If model-first PIT support is in scope, add explicit acceptance criteria or definition-of-done bullets naming the model-first public contract surfaces that must be kept consistent.

Open issues ledger
- critic-item-1 [required-po-action] Clarify in the delivery contract whether model-first `dvault.model.v1` PIT declarations/import-export/diagnostics are in scope for this story or explicitly out of scope.
- critic-item-2 [required-po-action] If model-first PIT support is out of scope, add explicit scope-out and documentation language that link-parent PIT support applies only to the existing `DataVaultPitMetadata`/registry-backed declaration path for this ticket.
- critic-item-3 [required-po-action] If model-first PIT support is in scope, add explicit acceptance criteria or definition-of-done bullets naming the model-first public contract surfaces that must be kept consistent.
- critic-item-4 [blocking-finding] The ticket does not explicitly resolve whether link-parent PIT support must include the public model-first declaration path or whether model-first PITs remain hub-only. Current public guidance and parser/schema evidence are hub-only, so the developer handoff boundary is ambiguous.
- critic-item-5 [blocking-finding] Because the contract asks for public documentation updates but does not name this declaration-path decision, developers could ship README/release-note wording that implies general link-parent PIT support while `dvault.model.v1` PIT artifacts still cannot express it.

Missing examples / edge cases
- A concrete supported example of one link-parent PIT over one declared link with ordered same-link satellites would make the intended baseline easier to implement and review.
- The contract does not say whether repeated same-hub/self-link parents are included as long as they are one declared link and all PIT satellites attach to that exact link.
- The contract does not show how public docs should describe the boundary when metadata-first/code-first support diverges from model-first PIT declarations.

Risky assumptions
- Assuming the current model-first PIT schema can stay hub-only without an explicit public note is risky because `docs/model-first-governance.md` treats model-first as a current public declaration path.
- Assuming README and release-note updates alone are sufficient is risky while schema-contract and parser behavior remain hub-specific.

AC / test suggestions
- Add one acceptance-criteria sentence that explicitly states the declaration-path boundary: either model-first PIT declarations remain hub-only in this story, or they must be updated together with metadata-first/code-first behavior.
- Add one example-based test/documentation expectation for a supported link-parent PIT where `ParentHashKey` is the link hash key and satellite snapshot columns remain in declaration order.
- Keep one negative example for mismatched-parent, duplicate-satellite, and multi-active link-parent PIT shapes so the bounded support line stays crisp.

Implementation watchouts
- Current hub-only guardrails are spread across PIT translation, maintenance validation, PIT read validation, provider-read strategy diagnostics, tests, contract snapshots, and public XML docs; partial updates would leave an inconsistent public story.
- Public read contract wording still says `parent hub hash keys` in `IDataVaultReadService`, `DataVaultPitAsOfReadRequest`, and `DataVaultPitReadRecord`, so doc drift is easy if the ticket scope is not explicit.

Non-blocking notes
- The persisted contract is otherwise substantially refined: `## Open Questions` is `none`, scope boundaries are detailed, and the related incoming blocker ticket `06F5Q90KC6JGQPSP285XQYSPK8` is already `done`.
- The current owner branch contains only ticket metadata/handoff commits, which is consistent with this being a pre-development quality gate.

Split recommendations
- If product wants model-first PIT declarations to support link parents too, consider a dedicated follow-up or explicit companion scope because the current `dvault.model.v1` PIT schema and parser are hub-only today.
- If product does not want that additional declaration-path work now, no technical split is needed, but the exclusion must be written explicitly before developer handoff.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment