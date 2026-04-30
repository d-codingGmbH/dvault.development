[gicket-bot] PO refinement contract

Summary
- PO refinement verified the ticket, comments, relations, attachments, repository layout, Data Vault concept documents, naming policy, persistence policy, stable hashing contract, and current modeling/test source evidence. The story is bounded for PO-critic review; two existing child tickets are already linked through parentOf relations.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 concept set is hubs, links, satellites, hash keys, hash diffs, load timestamps, and record sources, matching docs/architecture/mvp-data-vault-concepts.md.
- Existing repository evidence fixes the implementation home as src/DCoding.Data.DVault with modeling APIs under DCoding.Data.DVault.Modeling where appropriate, and tests under tests/DCoding.Data.DVault.Tests.
- The current ticket already has child relations to 06EXB74XQJFKGSKVJ6THQWJY8W and 06EXB755X9TGQW2EG1G30GJG28; no additional split was materialized in this PO run.
- No ticket attachments are currently bound. Referenced repository documents are sufficient ticket context for this refinement.

Scope In
- Define provider-neutral metadata abstractions for hub, link, and satellite concepts.
- Represent business key metadata for hubs, link participant metadata for links, and satellite payload metadata for satellites.
- Represent technical metadata roles for hash keys, hash diffs, load timestamps, and record source using the closed v1 role set already visible in source evidence.
- Document public or protected APIs with XML documentation consistent with the net10.0 project baseline and CS1591 enforcement.
- Add focused unit coverage for concept shape, role coverage, naming/default behavior, and provider-neutral behavior.

Scope Out
- Schema generation, migrations, loading automation, validation tooling, and provider-specific Sqlite/Postgres behavior.
- Hash algorithm implementation or model-specific hash input normalization beyond referencing the stable hashing contract.
- PIT tables, bridge tables, multi-active satellites, and provider-specific optimizations.
- Runtime configuration APIs, custom option matrices, and advanced override hooks unless a minimal internal shape is necessary to keep the abstractions provider-neutral.
- Changing default naming semantics, persistence convention policy, repository layout, target framework, or package identity.

Open questions
- none

Follow-up questions
- Decide in later implementation or governance tickets whether the existing child tickets 06EXB74XQJFKGSKVJ6THQWJY8W and 06EXB755X9TGQW2EG1G30GJG28 should remain separate delivery slices or be completed before this parent story closes.
- Plan separate stories for schema generation, loading automation, provider adapters, PIT tables, bridge tables, and multi-active satellites when those capabilities are scheduled.
- Plan a separate API design ticket if advanced configuration hooks need a public options surface beyond the current convention-first defaults.

Risks
- The parent story spans several related modeling concepts, so implementation should keep the first pass narrow and avoid drifting into provider persistence or automation work.
- Existing source already includes technical metadata contract types; developers should preserve that baseline and extend around it rather than creating a competing concept model.
- Hash key and hash diff metadata may be confused with hash computation. This ticket should keep computation and normalization out of scope.

Split recommendations
- No new child ticket was created in this run because existing parentOf relations already show two child tickets under this story.
- If implementation proves too large, split by concept family: hub/business-key metadata, link/participant metadata, and satellite/payload metadata, while keeping the shared technical metadata role set common.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment