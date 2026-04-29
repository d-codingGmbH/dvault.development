[gicket-bot] PO-critic review contract

Summary
- PO-critic review approves the charter epic for developer handoff: the persisted contract has no open questions, the central guidelines attachment and shared standards artifact are present, and the branch/ticket history shows the PO handoff to po-critic was committed.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB4MDREV2T51VJNJEP6R0WR/description.md contains ## Open Questions with '- none'.
- .gicket/tickets/06EXB4MDREV2T51VJNJEP6R0WR/attachments/manifest.json lists attachment 06EXB6NTAJVRZB13TS9FFTD2AM as dvault-library-guidelines.md with contentType text/markdown; the corresponding blob path is tracked and readable.
- .gicket/relations/WR includes parentOf relations from 06EXB4MDREV2T51VJNJEP6R0WR to 06EXB6NWYVB37D7S74VB3PVTCC, 06EXB6PNA0VA1XTR85B6X3T7ZG, and 06EXB6QD5Y9XVVZDVZEN4M6EV8, plus relates links to the documented related tickets.
- docs/plans/shared-implementation-standards.md exists and identifies source-of-truth documents for formatting, layout, .NET baseline, Data Vault MVP concepts, default naming, stable hashing, and v1 persistence conventions.
- src/DVault/DVault.csproj directly shows TargetFramework net10.0, ImplicitUsings enable, Nullable enable, GenerateDocumentationFile true, and PackageId DCoding.Data.DVault.
- src/DVault/Modeling/DataVaultConventions.cs directly defines the default model concepts and logical object names dvault_records, dvault_record_payloads, and dvault_record_metadata; DataVaultModelConcept.cs defines Hub, Link, Satellite, HashKey, HashDiff, LoadTimestamp, and RecordSource.
- Comment 06EXKQ309PX94JZXQJ10BYXACC reports outcome po-refinement-ready and that the durable refinement contract was updated; comment 06EXKQ3MR753RS8W9WHFB5CCPC reports 3 applied child follow-up comments and 0 blocking diagnostics.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The attachment manifest records sha256 3689523bd181e246bc2d24e33351a37684aec40d2aacb4cb13c61e73fea438de and size 1714, while the checked-out blob is LF-normalized at 1686 bytes; CRLF-normalizing the blob reproduces the manifest hash. This does not block handoff because the attachment is tracked and readable, but raw-byte validators should use the gicket attachment contract deliberately.

AC / test suggestions
- Downstream implementation tickets should cite docs/plans/shared-implementation-standards.md and then the specific source-of-truth document for the area they change.

Implementation watchouts
- Keep README-reserved src/DCoding.Data.DVault versus visible src/DVault/DVault.csproj and DVault.Modeling reconciliation deferred to its owning follow-up ticket.
- Do not pull provider-specific persistence behavior, migrations, schema generation, runtime configuration APIs, PIT tables, bridges, multi-active satellites, or provider optimizations into this epic's developer scope.

Non-blocking notes
- git status --short --branch shows the target branch tracking origin at the expected branch name; the visible dirty worktree entries are unrelated .gicket/.gicket-bot or other-ticket files and not the persisted charter contract files reviewed here.

Split recommendations
- No additional split is required for this PO-critic handoff; existing child and related ticket relations are sufficient for the current charter scope.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment