<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the governance ticket against current local repository evidence. The charter epic already has the central dvault-library-guidelines.md markdown attachment with the required DCoding.Data.DVault and .slnx content, so the ticket is ready for PO critic review.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The charter epic is 06EXB4MDREV2T51VJNJEP6R0WR, which is parent of story 06EXB6NWYVB37D7S74VB3PVTCC; that story is parent of this task 06EXB6P4ZNYA46MSYRGAJ9ZEPM.
- The guidelines source of truth belongs on the charter epic, not on each child story or task.
- Repository evidence shows the charter epic attachment manifest contains fileName dvault-library-guidelines.md with contentType text/markdown and one backing blob.
- The attachment content already states the default namespace DCoding.Data.DVault, solution format .slnx, .NET 10 target, Entity Framework/Data Vault product goal, code standards, documentation/example expectations, and NuGet publication constraint.

### Scope In
- Maintain one central markdown guidelines attachment named dvault-library-guidelines.md on the charter epic.
- Keep the attachment content focused on shared DVault goals, identity requirements, implementation standards, test/documentation expectations, and package constraints.
- Ensure child backlog items can reference the charter attachment instead of duplicating shared standards.

### Scope Out
- Product code, solution, project, or test implementation changes.
- Publishing NuGet packages or finalizing release/versioning policy beyond the documented constraint.
- Docker provisioning, mandatory local Postgres setup, provider-specific optimizations, PIT/bridge generation, multi-active satellites, or other advanced Data Vault automation.
- Creating duplicate guidelines attachments on every child ticket.

## Acceptance Criteria
- The charter epic has a markdown attachment named dvault-library-guidelines.md.
- The guidelines content explicitly includes DCoding.Data.DVault and .slnx requirements.
- The guidelines content covers the project goal, .NET target, coding standards, documentation/example expectations, test expectations, and package publication constraints.
- The maintained artifact remains reusable by downstream implementation tickets as the shared standards source of truth.

## Definition of Done
- The charter epic attachment manifest references dvault-library-guidelines.md with markdown content matching the acceptance criteria.
- The guidelines are clear enough for developers and future ticket refinements to cite without copying standards into each ticket.
- No arbitrary product-code or repository files are changed as part of this governance artifact maintenance.
- No additional split is required for this bounded attachment-maintenance task.

## Implementation Notes
- Verified attachment manifest path .gicket/tickets/06EXB4MDREV2T51VJNJEP6R0WR/attachments/manifest.json and blob sha 3689523bd181e246bc2d24e33351a37684aec40d2aacb4cb13c61e73fea438de in local repository evidence.
- The parent story currently has no attachment, which is acceptable because the ticket scope says the source of truth is attached to the charter epic.
- Existing relation chain confirms this task sits under the shared implementation standards story, which sits under the charter epic.
- No child tickets, relation writes, or planning documents were created during this refinement pass because the scope is small and the required artifact already exists.

## Open Questions
- none

## Follow-Up Questions
- Later governance can decide whether to mirror the same standards into a repository-visible docs path after the project documentation structure exists.
- When package publication becomes active scope, expand the current NuGet constraint into explicit packaging, versioning, signing, and release criteria.
- When advanced Data Vault features are scoped, extend the guidelines with provider-specific and automation-specific standards.

## Risks
- Standards can drift if future tickets copy snippets instead of referencing the charter attachment.
- Because the guidelines are ticket-attached rather than in a normal docs tree, implementers must continue to retrieve the charter attachment during planning and development.

## Split Recommendations
- None; this remains a bounded governance attachment maintenance task.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Keep the shared DVault library guidelines attached to the charter epic.

## Scope
- Maintain a markdown attachment with goals, standards, test expectations, and package constraints.

## Acceptance Criteria
- Attachment name is dvault-library-guidelines.md.
- Content includes DCoding.Data.DVault and .slnx requirements.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.