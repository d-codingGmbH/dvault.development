[gicket-bot] PO refinement contract

Summary
- Verified the governance ticket against current local repository evidence. The charter epic already has the central dvault-library-guidelines.md markdown attachment with the required DCoding.Data.DVault and .slnx content, so the ticket is ready for PO critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The charter epic is 06EXB4MDREV2T51VJNJEP6R0WR, which is parent of story 06EXB6NWYVB37D7S74VB3PVTCC; that story is parent of this task 06EXB6P4ZNYA46MSYRGAJ9ZEPM.
- The guidelines source of truth belongs on the charter epic, not on each child story or task.
- Repository evidence shows the charter epic attachment manifest contains fileName dvault-library-guidelines.md with contentType text/markdown and one backing blob.
- The attachment content already states the default namespace DCoding.Data.DVault, solution format .slnx, .NET 10 target, Entity Framework/Data Vault product goal, code standards, documentation/example expectations, and NuGet publication constraint.

Scope In
- Maintain one central markdown guidelines attachment named dvault-library-guidelines.md on the charter epic.
- Keep the attachment content focused on shared DVault goals, identity requirements, implementation standards, test/documentation expectations, and package constraints.
- Ensure child backlog items can reference the charter attachment instead of duplicating shared standards.

Scope Out
- Product code, solution, project, or test implementation changes.
- Publishing NuGet packages or finalizing release/versioning policy beyond the documented constraint.
- Docker provisioning, mandatory local Postgres setup, provider-specific optimizations, PIT/bridge generation, multi-active satellites, or other advanced Data Vault automation.
- Creating duplicate guidelines attachments on every child ticket.

Open questions
- none

Follow-up questions
- Later governance can decide whether to mirror the same standards into a repository-visible docs path after the project documentation structure exists.
- When package publication becomes active scope, expand the current NuGet constraint into explicit packaging, versioning, signing, and release criteria.
- When advanced Data Vault features are scoped, extend the guidelines with provider-specific and automation-specific standards.

Risks
- Standards can drift if future tickets copy snippets instead of referencing the charter attachment.
- Because the guidelines are ticket-attached rather than in a normal docs tree, implementers must continue to retrieve the charter attachment during planning and development.

Split recommendations
- None; this remains a bounded governance attachment maintenance task.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment