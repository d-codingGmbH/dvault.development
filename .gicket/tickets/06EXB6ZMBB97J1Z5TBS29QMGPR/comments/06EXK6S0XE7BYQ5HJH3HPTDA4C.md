[gicket-bot] PO refinement contract

Summary
- Verified current ticket state, comments, relations, attachments, and branch evidence. The refined contract now ties the required smoke test only to the source-backed AddDVault(IServiceCollection) extension visible in src/DVault/DVaultServiceCollectionExtensions.cs, removes the critic-triggering unsupported wording, and leaves no blocking PO questions.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated with explicit source-backed evidence: the only required startup API is public static IServiceCollection AddDVault(this IServiceCollection services) in src/DVault/DVaultServiceCollectionExtensions.cs. Unevidenced APIs such as UseDataVault, EF provider startup, and DbContext startup are excluded from this ticket rather than treated as existing prerequisites.
- critic-item-2: `answered` - The delivery contract no longer relies on any unsupported existing public API/type. It requires one smoke test against AddDVault(IServiceCollection), whose signature and behavior are visible in the current branch source. Any missing future startup API is out of scope and must be planned separately before test coverage is required.
- critic-item-3: `answered` - The prior wording that the critic parsed as an unsupported existing-API claim is replaced. The contract now says the expected work is test-only for the current branch's source-backed AddDVault(IServiceCollection) behavior; do not add repository scaffolding or a new startup API as part of this ticket. Production edits are only acceptable if needed to keep the documented current AddDVault behavior passing under the new smoke test, and any new public startup surface belongs in a separate ticket.

Clarifications
- The v1 startup surface for this ticket is the current-branch source-backed AddDVault(IServiceCollection) extension in src/DVault/DVaultServiceCollectionExtensions.cs.
- The smoke test should verify the current optionless AddDVault path; it must not require or imply a UseDataVault API, EF provider integration, a consuming DbContext, or any DbContext-specific startup surface.
- The legacy draft line about creating a small consuming DbContext is superseded by this refined contract and is background only.
- tests/DVault.Tests is the existing DVault test area for this ticket; current branch evidence shows DVault.Tests.csproj plus Modeling, Unit, Integration, and Shared test areas.
- No child tickets, outgoing relations, attachments, or planning documents were created in this PO pass. Existing relation context remains one incoming parentOf relation from 06EXB6Z3YMAPSRYRB8NQX3ZST4.

Scope In
- Add one self-contained smoke test under the existing tests/DVault.Tests structure for the default optionless AddDVault startup path.
- Use the branch's existing .NET test style to create a minimal ServiceCollection or equivalent startup path and call AddDVault with no DVault-specific configuration.
- Assert observable public startup success: AddDVault returns the same IServiceCollection, the service provider can be built, and provider-neutral defaults registered by AddDVault can be resolved.
- Keep the test deterministic and free of external databases, network services, and machine-specific infrastructure.

Scope Out
- Creating or renaming solution files, source projects, package metadata, repository build configuration, or test scaffolding.
- Adding a new public startup API as part of this ticket.
- Creating, requiring, or testing UseDataVault, EF provider integration, a consuming DbContext, or DbContext-specific startup behavior.
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
- No prerequisite setup split is required for the current narrowed AddDVault smoke-test scope because the branch contains the source project, source-backed AddDVault entry point, and DVault test structure.
- Create a separate follow-up ticket only if UseDataVault, EF-specific startup wiring, a consuming DbContext path, provider integration, or a new public startup surface becomes required.

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