[gicket-bot] PO refinement contract

Summary
- Verified current ticket, comments, relations, attachments, and current branch evidence for AddDVault and the DVault test structure. Revised the delivery contract to make AddDVault(IServiceCollection) the only required startup surface, remove conditional UseDataVault/EF/DbContext assumptions, and leave no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The revised contract replaces inferred API/type claims with source-backed evidence for the only required public API: src/DVault/DVaultServiceCollectionExtensions.cs defines public static IServiceCollection AddDVault(this IServiceCollection services). The contract no longer requires, conditionally exercises, or assumes UseDataVault, EF provider integration, or a DbContext startup surface.
- critic-item-2: `answered` - The persisted delivery contract is narrowed so the only existing public startup API it relies on is AddDVault(IServiceCollection), which is visible in current source. Missing or unevidenced APIs are not treated as prerequisites, optional variants, or implicit development targets for this ticket.
- critic-item-3: `answered` - The active scope now explicitly excludes creating, requiring, or testing UseDataVault, EF provider integration, a consuming DbContext, or DbContext-specific startup behavior. The legacy draft's DbContext wording is background only and is superseded by the source-backed AddDVault smoke-test contract; any future UseDataVault or EF startup story must be planned as a separate ticket before smoke coverage is added.

Clarifications
- The authoritative v1 startup target for this ticket is the source-backed optionless AddDVault(IServiceCollection) extension in src/DVault/DVaultServiceCollectionExtensions.cs.
- The legacy Original Ticket Draft line about a small consuming DbContext is superseded; this ticket must not create or require a DbContext, EF provider integration, or UseDataVault coverage.
- tests/DVault.Tests is the existing DVault test area for this ticket; no solution, source project, package, or test scaffold creation is part of scope.
- No child tickets, outgoing relations, attachments, or planning documents were created in this PO pass; existing relation context is one incoming parentOf relation from 06EXB6Z3YMAPSRYRB8NQX3ZST4.

Scope In
- Add one self-contained smoke test under the existing tests/DVault.Tests structure for the default optionless AddDVault startup path.
- Use the branch's existing .NET test style to create a minimal ServiceCollection or equivalent startup path and call AddDVault with no DVault-specific configuration.
- Assert observable public startup success, such as building the service provider and resolving the provider-neutral defaults registered by AddDVault, without depending on private DI descriptor ordering.
- Keep the test deterministic and free of external databases, network services, and machine-specific infrastructure.

Scope Out
- Creating or renaming solution files, source projects, package metadata, repository build configuration, or test scaffolding.
- Creating the production AddDVault implementation from scratch or adding a new public startup API.
- Creating, requiring, or testing a UseDataVault API, EF provider integration, consuming DbContext, or DbContext-specific startup surface.
- Provider-matrix coverage, broad configuration regression testing, SQLite integration behavior, and external database integration tests.

Open questions
- none

Follow-up questions
- Decide separately whether DVault needs a public UseDataVault, EF provider integration, or DbContext-specific startup API and create a dedicated implementation ticket if so.
- After the AddDVault smoke test lands, decide separately whether provider-specific startup coverage or broader configuration regression tests are needed.

Risks
- A test that asserts private DI registration mechanics instead of public startup success may become brittle.
- Because the repository contains multiple DVault test layouts, the developer must place the smoke test where the branch's normal test command will actually execute it.

Split recommendations
- No prerequisite setup split is required for the current narrowed AddDVault smoke-test scope because the branch contains the source project, AddDVault entry point, and DVault test structure.
- Create a separate follow-up ticket only if UseDataVault, EF-specific startup wiring, a consuming DbContext path, or provider integration becomes part of the required public startup surface.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment