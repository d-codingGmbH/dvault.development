[gicket-bot] PO-critic review contract

Summary
- Delivery contract is specific, has no open questions, and direct repository evidence confirms the target SQLite binary-first APIs plus the current documentation mismatch in examples/README, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- gicket-read-ticket-comments returned 10 comments; the persisted PO refinement comment says decision: ready_for_po_critic, and the returned comment set adds no newer human clarification or closure-evidence amendment.
- README.md already documents coordinated consumer package lines 8.47.0 and 10.47.0, warns that 0.47.0 is only the release label, and shows AddDVault(options => options.UseBinaryFirstProfile()), AddDVaultSqlite(), and UseSqlite(...) in the quickstart.
- docs/getting-started.md documents the same SQLite binary-first registration path, ApplyDataVaultMetadataWithBinaryFirstProfile(...), and an explicit IDataVaultSaveService write boundary.
- A direct stale-version rg search across README.md, docs/getting-started.md, examples/README.md, and src/DCoding.Data.DVault.Analyzers/README.md found stale 8.45.0 and 10.45.0 consumer package guidance only in examples/README.md:29-59; README.md and src/DCoding.Data.DVault.Analyzers/README.md show aligned 8.47.0 and 10.47.0 guidance, and docs/getting-started.md produced no stale-version hit.
- Direct source hits confirm the referenced APIs exist: src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22 defines AddDVaultSqlite(...), src/DCoding.Data.DVault/DataVaultOptions.cs:87 defines UseBinaryFirstProfile(), src/DCoding.Data.DVault/DataVaultCodeFirstModelBuilderExtensions.cs:38 defines ApplyDataVaultMetadataWithBinaryFirstProfile(...), and src/DCoding.Data.DVault/IDataVaultSaveService.cs:13 plus src/DCoding.Data.DVault/IDataVaultReadService.cs:8 define the explicit save/read interfaces.
- examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs contains EnsureCreatedAsync, three explicit DataVaultRegistrySaveRequest writes, and latest/as-of IDataVaultReadService.ReadLatestSatelliteAsync(...) calls, while git log --oneline --decorate -n 5 shows HEAD 7be8c48c621b32c5c759f45afd79994cbfdce004 currently contains only PO and PO-critic workflow commits on this ticket branch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Implementation must keep one path visibly primary; current repo evidence still mixes README/getting-started Code-First onboarding with metadata-first runnable quickstarts in examples/README.md, so prominence and labeling matter.
- The ticket assumes README references to v0.47.0 will not be mistaken for consumer package guidance once examples are updated; touched docs must keep the release-label-versus-package-version distinction explicit.

AC / test suggestions
- Review the final doc diff against README.md, docs/getting-started.md, and examples/README.md to confirm one aligned SQLite mainline with 8.47.0 and 10.47.0, binary-first registration, schema creation or provisioning, one explicit save, and one explicit latest/current read.
- Use the existing quickstart flow as correctness evidence: the surfaced minimal path should remain directly traceable to examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs or an equivalent visible snippet without reintroducing SaveChanges interception as a prerequisite.

Implementation watchouts
- The current runnable quickstart is metadata-first and hides the substantive save/read steps in examples/DCoding.Data.DVault.Quickstarts.Shared/QuickstartHistoryFlow.cs; if docs keep pointing there without an inline minimal excerpt, the ticket intent is not met.
- HEAD 7be8c48c621b32c5c759f45afd79994cbfdce004 is still a pre-development branch state with workflow commits only, so the developer should treat the current repository as unchanged baseline rather than assuming any doc refresh has already landed.

Non-blocking notes
- The persisted contract already captures the richer quickstart labeling question as follow-up work instead of scope for this task.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment