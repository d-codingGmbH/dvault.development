[gicket-bot] PO refinement contract

Summary
- Verified the ticket, current comments, relations, attachments, and repository surface. No child tickets or planning documents were created; the work is bounded enough for one implementation task and is ready for PO critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 requirement is to expose public naming-policy customization while keeping the default naming behavior available without any user configuration.
- The current repository branch has no visible source or test roots yet, so this ticket should establish the first implementation in the eventual modeling surface rather than adapt an existing API.
- The ticket has one incoming parentOf relation from 06EXB75DX3YAJFMJ6TNHVPAWYG and no outgoing relations; no additional split relations were needed.
- No ticket attachments are currently bound; the referenced repository configuration documents are already available context, and the charter standard remains a DoD compliance item if present elsewhere in the project context.

Scope In
- Define a public naming policy abstraction for modeling names.
- Add an options/configuration hook that lets advanced callers provide a custom naming policy.
- Provide a default policy so callers who do not configure naming continue to get deterministic built-in names.
- Cover both the default path and a custom-policy override path with tests.

Scope Out
- Changing ticket workflow, labels, or runtime handoff metadata.
- Broad naming-strategy governance beyond the first modeling API surface.
- Multiple built-in naming policy variants beyond the default policy required for v1.
- Retrofitting downstream packages or integrations that do not yet exist in the repository surface.

Open questions
- none

Follow-up questions
- After the first modeling API is in place, decide whether additional built-in naming conventions are worth adding as separate policies.
- Once documentation structure exists, consider adding user-facing examples for custom naming policies.

Risks
- Public API shape may be hard to change later, so keep the initial naming policy contract minimal and focused on names this ticket actually needs to override.
- The referenced charter is not attached to this ticket at present; developers should use any shared project-standard context available in the repository or parent planning work.

Split recommendations
- No split recommended for this ticket; interface, options hook, default behavior, and tests form one cohesive implementation slice.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment