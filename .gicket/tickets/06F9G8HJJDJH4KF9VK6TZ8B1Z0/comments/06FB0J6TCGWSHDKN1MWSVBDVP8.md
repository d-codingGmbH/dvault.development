[gicket-bot] PO refinement contract

Summary
- Refined the DB2 package-verification task, updated the authoritative ticket description, removed one stale outbound blocker, and queued cleanup of one stale inbound blocker on the related ticket branch.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The current ticket description was updated on the current ticket branch and is now the authoritative handoff surface for the DB2 package-verification scope.
- A stale outbound blocks relation from 06F9G8HJJDJH4KF9VK6TZ8B1Z0 to 06F9G8HRZ72XP5Z7FNWM6MBMQC was removed on the current ticket branch.
- A stale inbound blocks relation from 06F9G8HBXS7Y42J7XFSQKZ2AZ8 to this ticket was queued for replay removal on that related ticket's owner branch.
- Repository evidence already fixes the package surface: the repo contains src/DCoding.Data.DVault.Db2 and README.md already documents DCoding.Data.DVault.Db2 install guidance for both supported package lines.
- No child tickets or planning documents were materialized because current evidence supports a bounded single-ticket verification change.

Scope In
- Update package-verification coverage so the DB2 provider artifact is counted with the coordinated provider package family.
- Assert DB2 package dependency expectations for IBM.EntityFrameworkCore per supported target framework and package-version line.
- Extend packaged README/XML documentation checks and symbol expectations to the DB2 provider artifact.
- Preserve existing verification behavior for the previously supported provider packages.

Scope Out
- Changing DB2 runtime or provider implementation behavior outside package-verification coverage.
- Changing consumer-facing package ids, package-version-line policy, or README installation guidance beyond verifying the already documented baseline.
- Broader release-governance rewrites for manual publication unless the implementation must touch shared verification fixtures.
- CI/CD, package-push automation, or arbitrary repository documentation cleanup unrelated to package verification.

Open questions
- none

Follow-up questions
- Should the broader publication and planning documents that still describe a seven-package family be updated in the same delivery stream as the DB2 packaging work, or tracked as a separate documentation follow-up?
- After DB2 packaging lands, should a coordinated release-planning ticket ratify the package-family baseline change across all publication checklists and release-note templates?

Risks
- If package-family assumptions are duplicated across tests and publication documents, stale seven-package references may remain after the DB2 verification change unless follow-up cleanup is scheduled.
- IBM.EntityFrameworkCore dependency expectations may differ between net8.0 and net10.0 package lines; if the verification matrix is not explicit, later provider updates could drift silently.

Split recommendations
- No child-ticket split is required from current evidence; this remains a bounded verification update for one new provider artifact.
- If publication-document cleanup for historical seven-package references expands beyond test and verification touchpoints, track that as a separate follow-up ticket instead of enlarging this task.

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