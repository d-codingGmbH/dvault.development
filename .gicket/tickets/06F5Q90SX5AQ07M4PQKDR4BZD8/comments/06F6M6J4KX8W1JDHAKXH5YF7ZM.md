[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff; the delivery contract now explicitly keeps link-parent PIT support on the existing runtime path, leaves model-first PIT artifacts hub-parent-only for this story, and has no open questions.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8/description.md:5-18` and `:26-41` explicitly scope this story to runtime link-parent PIT support, keep model-first `dvault.model.v1` PIT declarations/import-export/diagnostics out of scope, and require docs to state that boundary.
- `.gicket/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8/description.md:53-54` sets `## Open Questions` to `none`, so there is no unresolved delivery-contract blocker.
- `git log --oneline -- .gicket/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8` shows the prior `019dfc324` `handoff po-critic->po` and later `e63ca88dd` `handoff po->po-critic`; `git diff 019dfc324..e63ca88dd -- .gicket/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8/description.md` adds the explicit model-first scope-out and documentation-boundary language that addressed the earlier blocker.
- Comment `.gicket/tickets/06F5Q90SX5AQ07M4PQKDR4BZD8/comments/06F6KR7V14GKDANZZ3J49X5DHC.md` records `critic-item-1` through `critic-item-5` as answered and repeats the `ready_for_po_critic` handoff.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataRegistry.cs:580-603` accepts PIT parents of kind `Hub` or `Link`, and `:565-574` validates satellites against the exact declared parent, confirming the existing runtime metadata/registry path can represent the bounded link-parent shape.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:453-457,517-521`, `src/DCoding.Data.DVault/DataVaultPitMaintenanceShapeValidator.cs:9-14`, and `src/DCoding.Data.DVault/DataVaultPitReadPipeline.cs:320-325` still reject link-based PITs today, which matches the story's stated runtime work rather than indicating missing PO scope.
- `src/DCoding.Data.DVault/DataVaultModelArtifactParser.cs:<redacted>` and `src/DCoding.Data.DVault/DataVaultModelArtifactExporter.cs:205-214` materialize/export PIT artifacts through a `hub` field only, confirming the ticket's explicit scope-out that public `dvault.model.v1` PIT artifacts remain hub-parent-only.
- `README.md:648` and `README.md:788` still describe link-based PIT support as future/unsupported, and `.gicket/tickets/06F5Q90KC6JGQPSP285XQYSPK8/ticket.json` shows the incoming related ticket is already `done`, matching the contract's documentation debt and historical-blocker clarifications.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A worked supported-shape example is still not written inline in the contract: one declared link parent with two ordered same-link non-multi-active satellites, showing that `ParentHashKey` carries the link hash key and snapshot columns stay in declaration order.

Risky assumptions
- Implementation and documentation must keep the runtime-only boundary synchronized; the repo still has hub-only public artifact behavior in `DataVaultModelArtifactParser` and `DataVaultModelArtifactExporter`, so any broader wording would recreate the ambiguity that was just resolved.

AC / test suggestions
- Keep one positive acceptance/test fixture for the bounded happy path: one link parent, ordered unique same-link satellites, deterministic `ParentHashKey` / `LoadTimestamp` / `<Satellite>LoadTimestamp` output, and explicit link hash key maintenance inputs.
- Keep negative coverage for duplicate satellites, mismatched-parent satellites, multi-active references, bridge-driven or mixed-parent shapes, and generated-model mismatches, as already called out in the contract.

Implementation watchouts
- This is a runtime-path-only story; public model-first `dvault.model.v1` PIT declaration/import-export/drift surfaces stay hub-parent-only in this ticket.
- The current repo carries hub-only assumptions in translation, maintenance validation, read validation, provider diagnostics, and public docs; partial delivery across only one of those surfaces would create inconsistent behavior or public contract drift.
- Provider-specific PIT read strategies are allowed to decline the new shape and fall back to the provider-neutral pipeline; the ticket should not be read as promising optimized-provider parity.

Non-blocking notes
- none

Split recommendations
- No split is required for the runtime story. If product later wants link-parent PIT support in public `dvault.model.v1` artifacts, keep that as a separate additive ticket across parser, exporter, and drift/diagnostic surfaces.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment