[gicket-bot] PO-critic review contract

Summary
- Ticket is ready for developer handoff; the delivery contract is bounded, has no unresolved Open Questions, and repository checks support creating the initial DVault project file.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 9 comments; the PO refinement comment says decision ready_for_po_critic, and the transactional linkage comment records branch ticket/06EXB6XVWBWZGN6MA3SFWGWKM4-task-add-dconding-data-dvault-library-project-ta and handoff commit 71bce9e9379b.
- git show --name-status 71bce9e9379b shows the PO handoff changed only .gicket/tickets/06EXB6XVWBWZGN6MA3SFWGWKM4 ticket metadata files: comments, events, description.md, and ticket.json.
- find under /mnt/c/Projects/DVault/src and /mnt/c/Projects/DVault/tests found no files; find -name '*.csproj' and find -name Directory.Build.props both produced no output, so src/DVault/DVault.csproj and shared build props are not present.
- find -maxdepth 3 showed empty repository directories /mnt/c/Projects/DVault/src/DVault and /mnt/c/Projects/DVault/tests/DVault.Tests; this supports the scoped task to add the source project file without existing source project migration.
- .gicket/relations/PM/M4/06EXB6XBV95E08R2W9ZQ1PRDPM--06EXB6XVWBWZGN6MA3SFWGWKM4--parentOf.json has type parentOf, source-ticket-id 06EXB6XBV95E08R2W9ZQ1PRDPM, and target-ticket-id 06EXB6XVWBWZGN6MA3SFWGWKM4.
- rg --hidden found charter/related-ticket evidence for the namespace: .gicket/attachments/blobs/0e1aa459eab42977385326220886ff9430aca84aff7883b7cf9c27a660ce7101 says 'Default namespace: DCoding.Data.DVault', and parent ticket 06EXB6XBV95E08R2W9ZQ1PRDPM requires project names and namespaces to match DCoding.Data.DVault.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- Existing AC are sufficient: verify src/DVault/DVault.csproj targets net10.0, sets RootNamespace DCoding.Data.DVault, enables nullable and XML docs, and elevates missing XML documentation warnings to errors.
- DoD appropriately allows static project-file verification with an explicit environment note if a net10.0-capable SDK is unavailable for restore/build.

Implementation watchouts
- Keep the change limited to src/DVault/DVault.csproj and minimal source only if required for a valid class library.
- Do not create tests/DVault.Tests project scaffolding, domain APIs, packaging metadata, or repository-wide Directory.Build.props in this ticket.
- Because no Directory.Build.props was found, project-level properties should carry the target framework, nullable, XML documentation, root namespace, and documentation warning enforcement.
- Documentation enforcement should remain narrowly focused on missing public/protected API XML documentation, consistent with the ticket risk statement.

Non-blocking notes
- The contract says direct reads showed src/DVault and tests/DVault.Tests directories did not exist, but current filesystem reads show those empty directories do exist. This does not block handoff because no source/test files or csproj files exist and the requested project path is explicit.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment