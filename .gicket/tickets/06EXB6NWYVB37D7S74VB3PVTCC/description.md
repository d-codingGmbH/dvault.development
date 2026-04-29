<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined 06EXB6NWYVB37D7S74VB3PVTCC using persisted ticket state, comments, attachments, relations, and repository standards evidence; no new child tickets or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Current ticket has no direct attachments; repository documents in the prompt are treated as available refinement context.
- Existing relations are part of the refined contract: this story is a child of 06EXB4MDREV2T51VJNJEP6R0WR, is parentOf 06EXB6P4ZNYA46MSYRGAJ9ZEPM and 06EXB6PDF0DSHE68B3V0656DJM, and blocks 06EXB6XBV95E08R2W9ZQ1PRDPM.
- Formatting and encoding baseline is already fixed by docs/formatting.md: two-space indentation, spaces, LF, UTF-8 without BOM, final newline, trailing whitespace rejection, tabs only for Makefile syntax, and same-line braces for brace-based source files.
- File layout baseline is already fixed by README.md and the branch snapshot: source under src/, tests under tests/, docs under docs/, examples under examples/, benchmarks under benchmarks/, and project files added to DVault.slnx when created.
- The visible .NET baseline is net10.0 with nullable, implicit usings, and generated XML documentation enabled in src/DVault/DVault.csproj; implementation may reconcile naming with README conventions in the specific child or development ticket that owns project layout.
- Default naming and deterministic implementation conventions are already documented in docs/naming/default-naming-policy.md, docs/plans/stable-hashing-contract.md, and docs/plans/dvault-v1-default-persistence-convention-policy.md; this ticket should reference and consolidate those standards rather than reopening their v1 defaults.

### Scope In
- Produce or update a shared implementation standards artifact that can be referenced by downstream tickets.
- Cover repository formatting and encoding rules, including .editorconfig, .gitattributes, and bash tools/check-format.sh as the non-mutating enforcement gate.
- Cover source, test, docs, examples, and benchmark layout expectations using the existing README layout as the v1 default.
- Cover .NET project standards visible in the current repo: net10.0 target, nullable enabled, implicit usings enabled, and generated documentation file enabled unless a later ticket documents an exception.
- Cover namespace and naming guidance by referencing the existing DVault.Modeling namespace evidence and the default naming policy document for model table and column names.
- Cover documentation expectations for governed planning and architecture docs: keep shared standards in docs, use existing docs/plans and docs/architecture patterns, and make downstream tickets reference the shared artifact instead of copying rules.

### Scope Out
- Do not implement or refactor product code as part of this PO refinement ticket.
- Do not introduce provider-specific persistence behavior, migrations, schema generation, or runtime configuration APIs.
- Do not redefine stable hashing, default persistence conventions, or default naming semantics that are already documented in existing planning documents.
- Do not require CI creation here; only document that the first CI or application build definition must run bash tools/check-format.sh as a blocking step.

## Acceptance Criteria
- A shared standards document or charter-attached planning artifact exists and is suitable for downstream tickets to reference directly.
- The standards explicitly cover formatting, encoding, line endings, tab policy, final newline, trailing whitespace, brace placement, and the required local formatting command bash tools/check-format.sh.
- The standards explicitly cover current repository layout defaults for src/, tests/, docs/, examples/, benchmarks/, root solution files, and tracked placeholder folders.
- The standards explicitly cover the current .NET implementation baseline: net10.0, nullable enabled, implicit usings enabled, XML documentation generation, and same-line C# braces.
- The standards reference existing repository decisions for Data Vault concepts, default naming policy, stable hashing contract, and v1 persistence convention policy instead of duplicating their full content.
- The standards identify which decisions are v1 defaults and which categories remain deferred follow-up work.

## Definition of Done
- The refined standards artifact is present in an approved planning or docs surface and can be attached or referenced by the charter epic.
- The artifact names the existing child-ticket relations and downstream dependency enough that related work can reference the shared source of truth.
- No unresolved PO-level architecture questions remain for formatting, encoding, file layout, documentation baseline, or current v1 naming/hash/persistence defaults.
- The final contract keeps implementation details bounded to governance documentation and does not require product-code edits.

## Implementation Notes
- Use docs/formatting.md as the canonical formatting and encoding source; do not create a parallel conflicting policy.
- Use README.md as the repository layout baseline and document any current transitional project-name mismatch as implementation cleanup owned by the relevant child or development ticket, not as a blocker for this governance story.
- Use docs/naming/default-naming-policy.md for table and column naming standards, including PascalCase model identifiers, Data Vault prefixes, technical column names, reserved-word handling, and deterministic duplicate suffixes.
- Use docs/plans/stable-hashing-contract.md for deterministic hashing rules and docs/plans/dvault-v1-default-persistence-convention-policy.md for provider-neutral persistence convention defaults.
- Because the current ticket has existing parentOf relations to 06EXB6P4ZNYA46MSYRGAJ9ZEPM and 06EXB6PDF0DSHE68B3V0656DJM, avoid creating duplicate child tickets during this refinement pass.
- If a planning document is materialized later, docs/plans/shared-implementation-standards.md is an appropriate bounded location and can then be attached to the charter epic through the approved attachment surface.

## Open Questions
- none

## Follow-Up Questions
- Should a later governance pass reconcile the README-reserved src/DCoding.Data.DVault path with the currently visible src/DVault project and namespace once the owning implementation ticket is active?
- Should future provider-specific or CI-specific standards be split into separate follow-up tickets after the first provider adapter or CI workflow exists?
- Should downstream tickets add explicit references to the shared standards artifact once it is attached to the charter epic?

## Risks
- Repository evidence shows transitional naming/layout signals between README reservations and the visible src/DVault project; this is manageable as follow-up cleanup but should not be silently duplicated in future standards.
- If downstream tickets copy standards instead of referencing the shared artifact, repository-wide governance may drift.
- CI enforcement is documented but not necessarily implemented yet, so future CI/build tickets must remember to wire bash tools/check-format.sh as a blocking step.

## Split Recommendations
- No additional split is recommended in this PO pass because two child tickets are already linked by parentOf relations from this story.
- Keep future provider-specific conventions, CI wiring, and project-layout reconciliation as separate follow-up work rather than expanding this shared-standards story.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Make repository-wide implementation standards explicit and reusable.

## Scope
- Capture formatting, encoding, namespace, documentation, and file layout rules.
- Keep the source of truth attached to the charter epic.

## Acceptance Criteria
- Standards cover all user-provided code rules.
- Tasks can reference the charter instead of copying standards.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.